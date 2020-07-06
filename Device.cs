using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading;

using LLONG = System.Int64;

namespace NetSDKInop
{

    public unsafe class Device
    {
        public string Ip { get; private set; }
        public int Port { get; private set; }
        public string Username { get; private set; }
        public string Password { get; private set; }
        public bool IsConnected { get; internal set; } = false;
        internal LLONG devId = 0;
        public DeviceUtil.LoginError LoginError { get; private set; }
        public string Mac { get; private set; }
        private KeyValuePair<string, int> devKvp;

        public class Card
        {
            public string password;
            public string cardNo;
            public int id;
            public DateTime validStartTime;
            public DateTime validEndTime;
        }

        public class Entry
        {
            public string cardNo;
            public DateTime time;
            public int direction;
            public bool valid;
            public string password;
        }

        public Device(string ip, string username, string password, int port = 37777)
        {
            DeviceUtil.CheckSDKInit();

            Ip = ip;
            Port = port;
            Username = username;
            Password = password;

            devKvp = new KeyValuePair<string, int>(Ip, Port);

            ReConnect();

            IsConnected = true;
        }

        ~Device()
        {
            LogOut();
        }

        public bool LogOut()
        {
            IsConnected = false;
            DeviceUtil.ActiveDevices.TryRemove(devKvp, out _);
            return FunctionExports.CLIENT_Logout(devId) != 0;
        }

        public void ReConnect()
        {
            var byteIp = Encoding.ASCII.GetBytes(Ip);
            var byteUsername = Encoding.ASCII.GetBytes(Username);
            var bytePassword = Encoding.ASCII.GetBytes(Password);

            fixed (byte* ipPtr = byteIp, usernamePtr = byteUsername, passwordPtr = bytePassword)
            {
                int error = 0;
                devId = FunctionExports.CLIENT_LoginEx2(ipPtr, (uint)Port, usernamePtr,
                    passwordPtr, EnumExports.EM_LOGIN_SPAC_CAP_TYPE.EM_LOGIN_SPEC_CAP_NO_CONFIG, null, null, &error);
                LoginError = (DeviceUtil.LoginError)error;
            }

            if (devId == 0) throw new DeviceConnectionFailedException($"Connecting to the device failed, error: {LoginError}",
                LoginError);

            var stuInfo = new StructExports.DHDEV_NETINTERFACE_INFO[64];
            for (int i = 0; i < stuInfo.Length; i++) stuInfo[i].dwSize = sizeof(StructExports.DHDEV_NETINTERFACE_INFO);

            fixed (void* infoPtr = stuInfo)
            {
                int err;
                if (FunctionExports.CLIENT_QueryDevState(devId, 0x0045, (byte*)infoPtr, sizeof(StructExports.DHDEV_NETINTERFACE_INFO)
                    * stuInfo.Length, &err) == 0)
                {
                    LoginError = DeviceUtil.LoginError.mac_fetch_failed;
                    throw new DeviceConnectionFailedException($"{LoginError}", LoginError);

                }
            }

            fixed (byte* macPtr = stuInfo[0].szMAC)
            {
                Mac = Marshal.PtrToStringAnsi((IntPtr)macPtr);
            }

            DeviceUtil.ActiveDevices[devKvp] = this;
        }

        private void CheckConnection()
        {
            if (!IsConnected) throw new DeviceNotConnectedExpeption();
        }

        private static DateTime NetTimeToDateTime(StructExports.NET_TIME netTime)
        {
            try
            {
                return new DateTime((int)netTime.dwYear, (int)netTime.dwMonth, (int)netTime.dwDay,
                    (int)netTime.dwHour, (int)netTime.dwMinute, (int)netTime.dwSecond);
            }
            catch
            {
                return default;
            }
        }

        private static StructExports.NET_TIME DateTimeToNetTime(DateTime dateTime)
        {
            var netTime = new StructExports.NET_TIME();
            netTime.dwYear = (uint)dateTime.Year;
            netTime.dwMonth = (uint)dateTime.Month;
            netTime.dwDay = (uint)dateTime.Day;

            netTime.dwHour = (uint)dateTime.Hour;
            netTime.dwMinute = (uint)dateTime.Minute;
            netTime.dwSecond = (uint)dateTime.Second;

            return netTime;
        }

        private static StructExports.NET_RECORDSET_ACCESS_CTL_CARD CardToNetCard(Card card)
        {
            var netCard = new StructExports.NET_RECORDSET_ACCESS_CTL_CARD();
            netCard.dwSize = (uint)sizeof(StructExports.NET_RECORDSET_ACCESS_CTL_CARD);

            fixed (char* c = card.cardNo)
            {
                byte* b = netCard.szCardNo;
                Encoding.ASCII.GetBytes(c, card.cardNo.Length, b, 32);
            }

            fixed (char* c = card.password)
            {
                byte* b = netCard.szPsw;
                Encoding.ASCII.GetBytes(c, card.cardNo.Length, b, 32);
            }

            fixed (char* c = card.id.ToString())
            {
                byte* b = netCard.szUserID;
                Encoding.ASCII.GetBytes(c, card.cardNo.Length, b, 32);
            }

            netCard.stuValidStartTime = DateTimeToNetTime(card.validStartTime);
            netCard.stuValidEndTime = DateTimeToNetTime(card.validEndTime);

            return netCard;
        }

        private static Card NetCardToCard(StructExports.NET_RECORDSET_ACCESS_CTL_CARD netCard)
        {
            Card card = new Card();
            card.validStartTime = NetTimeToDateTime(netCard.stuValidStartTime);
            card.validEndTime = NetTimeToDateTime(netCard.stuValidEndTime);
            card.id = netCard.nRecNo;
            card.cardNo = Marshal.PtrToStringAnsi((IntPtr)netCard.szCardNo);
            card.password = Marshal.PtrToStringAnsi((IntPtr)netCard.szPsw);

            return card;
        }

        public bool AddCard(Card card)
        {
            CheckConnection();

            var netCard = CardToNetCard(card);

            netCard.nRecNo = card.id;
            netCard.nUserTime = 255;
            netCard.sznTimeSectionNo[0] = 255;
            netCard.nDoorNum = 1;
            netCard.nTimeSectionNum = 1;

            var stuParam = new StructExports.NET_CTRL_RECORDSET_INSERT_PARAM();
            stuParam.dwSize = (uint)sizeof(StructExports.NET_CTRL_RECORDSET_INSERT_PARAM);
            stuParam.stuCtrlRecordSetInfo.dwSize = (uint)sizeof(StructExports.NET_CTRL_RECORDSET_INSERT_IN);
            stuParam.stuCtrlRecordSetInfo.emType = EnumExports.EM_NET_RECORD_TYPE.NET_RECORD_ACCESSCTLCARD;
            stuParam.stuCtrlRecordSetInfo.pBuf = &netCard;
            stuParam.stuCtrlRecordSetInfo.nBufLen = sizeof(StructExports.NET_RECORDSET_ACCESS_CTL_CARD);
            stuParam.stuCtrlRecordSetResult.dwSize = (uint)sizeof(StructExports.NET_CTRL_RECORDSET_INSERT_OUT);

            return FunctionExports.CLIENT_ControlDeviceEx(devId, EnumExports.CtrlType.DH_CTRL_RECORDSET_INSERT, &stuParam) != 0;
        }

        public bool RemoveCard(int cardNo)
        {
            CheckConnection();

            var stuCard = new StructExports.NET_RECORDSET_ACCESS_CTL_CARD();
            stuCard.dwSize = (uint)sizeof(StructExports.NET_RECORDSET_ACCESS_CTL_CARD);
            stuCard.nRecNo = cardNo;

            var stuParam = new StructExports.NET_CTRL_RECORDSET_PARAM();
            stuParam.dwSize = (uint)sizeof(StructExports.NET_CTRL_RECORDSET_PARAM);
            stuParam.emType = EnumExports.EM_NET_RECORD_TYPE.NET_RECORD_ACCESSCTLCARD;
            stuParam.pBuf = &stuCard.nRecNo;
            stuParam.nBufLen = sizeof(int);

            return FunctionExports.CLIENT_ControlDeviceEx(devId, EnumExports.CtrlType.DH_CTRL_RECORDSET_REMOVE, &stuParam) != 0;
        }

        public bool ModifyCard(Card card)
        {
            CheckConnection();

            var stuCard = CardToNetCard(card);

            var stuParam = new StructExports.NET_CTRL_RECORDSET_PARAM();
            stuParam.dwSize = (uint)sizeof(StructExports.NET_CTRL_RECORDSET_PARAM);
            stuParam.emType = EnumExports.EM_NET_RECORD_TYPE.NET_RECORD_ACCESSCTLCARD;
            stuParam.pBuf = &stuCard;
            stuParam.nBufLen = sizeof(StructExports.NET_RECORDSET_ACCESS_CTL_CARD);

            return FunctionExports.CLIENT_ControlDeviceEx(devId, EnumExports.CtrlType.DH_CTRL_RECORDSET_UPDATE, &stuParam) != 0;
        }

        public Card GetCard(int cardNo)
        {
            CheckConnection();

            var stuCard = new StructExports.NET_RECORDSET_ACCESS_CTL_CARD();
            stuCard.dwSize = (uint)sizeof(StructExports.NET_RECORDSET_ACCESS_CTL_CARD);
            stuCard.nRecNo = cardNo;

            var stuParam = new StructExports.NET_CTRL_RECORDSET_PARAM();
            stuParam.dwSize = (uint)sizeof(StructExports.NET_CTRL_RECORDSET_PARAM);
            stuParam.emType = EnumExports.EM_NET_RECORD_TYPE.NET_RECORD_ACCESSCTLCARD;
            stuParam.pBuf = &stuCard;

            int ret;
            if (FunctionExports.CLIENT_QueryDevState(devId, 0x0158, (byte*)&stuParam,
                sizeof(StructExports.NET_CTRL_RECORDSET_PARAM), &ret) == 0) return null;

            return NetCardToCard(stuCard);

        }

        private static readonly byte[] CFG_CMD_NETWORK = Encoding.ASCII.GetBytes("Network");

        public bool ChangeIp(string ip, string subnet, string gateway)
        {
            CheckConnection();

            byte[] szJsonBuf = new byte[1024];
            fixed (byte* jsonPtr = szJsonBuf, cmdNetPtr = CFG_CMD_NETWORK)
            {
                if (FunctionExports.CLIENT_GetNewDevConfig(devId, cmdNetPtr,
                    -1, jsonPtr, (uint)szJsonBuf.Length, null, 2000) == 0) return false;
            }

            var stuNetParam = new StructExports.CFG_NETWORK_INFO();
            IntPtr temp = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(StructExports.CFG_NETWORK_INFO)));
            Marshal.StructureToPtr(stuNetParam, temp, true);

            fixed (byte* jsonPtr = szJsonBuf, cmdNetPtr = CFG_CMD_NETWORK)
            {
                if (FunctionExports.CLIENT_ParseData(cmdNetPtr, jsonPtr, (void*)temp,
                (uint)Marshal.SizeOf(typeof(StructExports.CFG_NETWORK_INFO)), null) == 0) return false;
            }


            stuNetParam = (StructExports.CFG_NETWORK_INFO)Marshal.PtrToStructure(temp, typeof(StructExports.CFG_NETWORK_INFO));
            Marshal.FreeHGlobal(temp);

            fixed (char* c = ip)
            fixed (byte* b = stuNetParam.stuInterfaces[0].szIP)
            {
                Encoding.ASCII.GetBytes(c, ip.Length, b, 256);
            }

            fixed (char* c = subnet)
            fixed (byte* b = stuNetParam.stuInterfaces[0].szSubnetMask)
            {
                Encoding.ASCII.GetBytes(c, subnet.Length, b, 256);
            }

            fixed (char* c = gateway)
            fixed (byte* b = stuNetParam.stuInterfaces[0].szDefGateway)
            {
                Encoding.ASCII.GetBytes(c, gateway.Length, b, 256);
            }

            temp = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(StructExports.CFG_NETWORK_INFO)));
            Marshal.StructureToPtr(stuNetParam, temp, true);

            bool result = false;
            fixed (byte* jsonPtr = szJsonBuf, cmdNetPtr = CFG_CMD_NETWORK)
            {
                if (FunctionExports.CLIENT_PacketData(cmdNetPtr, (void*)temp, (uint)Marshal.SizeOf(typeof(StructExports.CFG_NETWORK_INFO)),
                    jsonPtr, (uint)szJsonBuf.Length) != 0)
                {
                    result = FunctionExports.CLIENT_SetNewDevConfig(devId, cmdNetPtr,
                        -1, jsonPtr, (uint)DeviceUtil.InternalStrLen(jsonPtr), null, null, 2000) != 0;
                }
            }

            Marshal.FreeHGlobal(temp);

            return result;

        }

        public bool ChangePassword(string newPassword)
        {
            CheckConnection();

            var stuOldInfo = new StructExports.USER_INFO_NEW();

            fixed (char* c = Username)
            {
                byte* b = stuOldInfo.name;
                Encoding.ASCII.GetBytes(c, Username.Length, b, 256);
            }

            fixed (char* c = Password)
            {
                byte* b = stuOldInfo.passWord;
                Encoding.ASCII.GetBytes(c, Password.Length, b, 256);
            }

            var stuModifiedInfo = new StructExports.USER_INFO_NEW();

            fixed (char* c = newPassword)
            {
                byte* b = stuModifiedInfo.passWord;
                Encoding.ASCII.GetBytes(c, Password.Length, b, 256);
            }

            bool result = FunctionExports.CLIENT_OperateUserInfoNew(devId, 6, &stuModifiedInfo, &stuOldInfo, null, 3000) != 0;

            if (result) Password = newPassword;

            return result;
        }

        public bool OpenDoor(int door = 0)
        {
            CheckConnection();

            var stuParam = new StructExports.NET_CTRL_ACCESS_OPEN();
            stuParam.dwSize = (uint)sizeof(StructExports.NET_CTRL_ACCESS_OPEN);
            stuParam.nChannelID = door;
            return FunctionExports.CLIENT_ControlDeviceEx(devId, EnumExports.CtrlType.DH_CTRL_ACCESS_OPEN, &stuParam, null, 3000) != 0;
        }

        public bool CloseDoor(int door = 0)
        {
            CheckConnection();

            var stuParam = new StructExports.NET_CTRL_ACCESS_OPEN();
            stuParam.dwSize = (uint)sizeof(StructExports.NET_CTRL_ACCESS_OPEN);
            stuParam.nChannelID = door;
            return FunctionExports.CLIENT_ControlDeviceEx(devId, EnumExports.CtrlType.DH_CTRL_ACCESS_CLOSE, &stuParam, null, 3000) != 0;
        }

        internal string readCard;
        public string ReadCard(int timeout = 60)
        {
            CheckConnection();

            readCard = "";
            FunctionExports.CLIENT_StartListenEx(devId);
            int timeSpent = 0;
            while (readCard.Length == 0 && timeSpent++ < timeout) Thread.Sleep(1000);
            FunctionExports.CLIENT_StopListen(devId);
            return readCard;
        }

        public bool RestartDevice()
        {
            CheckConnection();

            bool result = FunctionExports.CLIENT_ControlDeviceEx(devId, EnumExports.CtrlType.DH_CTRL_REBOOT, null, null, 3000) != 0;
            if (result) IsConnected = false;
            return result;
        }

        public bool ClearRecords()
        {
            CheckConnection();

            var rem = new StructExports.NET_CTRL_RECORDSET_PARAM();
            rem.dwSize = (uint)sizeof(StructExports.NET_CTRL_RECORDSET_PARAM);
            rem.emType = EnumExports.EM_NET_RECORD_TYPE.NET_RECORD_ACCESSCTLCARDREC;
            return FunctionExports.CLIENT_ControlDeviceEx(devId, EnumExports.CtrlType.DH_CTRL_RECORDSET_CLEAR, &rem) != 0;
        }

        public DateTime? GetDeviceDateTime()
        {
            CheckConnection();

            StructExports.NET_TIME netTime;
            FunctionExports.CLIENT_QueryDeviceTime(devId, &netTime);
            if (FunctionExports.CLIENT_QueryDeviceTime(devId, &netTime) == 0) return null;
            return NetTimeToDateTime(netTime);
        }

        public bool SetDeviceTime(DateTime datetime)
        {
            CheckConnection();

            var netTime = DateTimeToNetTime(datetime);
            return FunctionExports.CLIENT_SetupDeviceTime(devId, &netTime) != 0;
        }

        public bool RemoveAllCards()
        {
            CheckConnection();

            var stuParam = new StructExports.NET_CTRL_RECORDSET_PARAM();
            stuParam.dwSize = (uint)sizeof(StructExports.NET_CTRL_RECORDSET_PARAM);
            stuParam.emType = EnumExports.EM_NET_RECORD_TYPE.NET_RECORD_ACCESSCTLCARD;
            return FunctionExports.CLIENT_ControlDeviceEx(devId, EnumExports.CtrlType.DH_CTRL_RECORDSET_CLEAR, &stuParam) != 0;
        }

        private LLONG GetFileHandle(EnumExports.EM_NET_RECORD_TYPE handleType)
        {
            StructExports.NET_IN_FIND_RECORD_PARAM stuIn;
            stuIn.dwSize = (uint)sizeof(StructExports.NET_IN_FIND_RECORD_PARAM);
            stuIn.emType = handleType;

            StructExports.NET_OUT_FIND_RECORD_PARAM stuOut;
            stuOut.dwSize = (uint)sizeof(StructExports.NET_OUT_FIND_RECORD_PARAM);

            FunctionExports.CLIENT_FindRecord(devId, &stuIn, &stuOut);

            return stuOut.lFindeHandle;
        }

        const int bufferSize = 10;

        private List<Entry> FetchEntries(StructExports.NET_IN_FIND_NEXT_RECORD_PARAM inParam,
            StructExports.NET_OUT_FIND_NEXT_RECORD_PARAM outParam, int bufferSize)
        {
            var entries = new List<Entry>();

            var entryBuffer = new StructExports.NET_RECORDSET_ACCESS_CTL_CARDREC[bufferSize];

            for (int i = 0; i < entryBuffer.Length; i++) entryBuffer[i].dwSize =
                    (uint)sizeof(StructExports.NET_RECORDSET_ACCESS_CTL_CARDREC);

            fixed (StructExports.NET_RECORDSET_ACCESS_CTL_CARDREC* entryBufferPtr = entryBuffer)
            {
                outParam.pRecordList = entryBufferPtr;

                if (FunctionExports.CLIENT_FindNextRecord(&inParam, &outParam) > 0)
                {
                    for (int i = 0; i < outParam.nRetRecordNum; i++)
                    {
                        var entry = new Entry();

                        entry.cardNo = Marshal.PtrToStringAnsi((IntPtr)entryBufferPtr[i].szCardNo);
                        entry.password = Marshal.PtrToStringAnsi((IntPtr)entryBufferPtr[i].szPwd);

                        entry.time = NetTimeToDateTime(entryBufferPtr[i].stuTime);

                        entry.direction = (int)entryBufferPtr[i].emDirection;
                        entry.valid = entryBufferPtr[i].bStatus != 0;
                        entries.Add(entry);
                    }
                }

            }

            return entries;
        }

        public int GetEntryAmount()
        {
            LLONG handle = GetFileHandle(EnumExports.EM_NET_RECORD_TYPE.NET_RECORD_ACCESSCTLCARDREC);
            var inParam = new StructExports.NET_IN_QUEYT_RECORD_COUNT_PARAM();
            inParam.dwSize = (uint) sizeof(StructExports.NET_IN_QUEYT_RECORD_COUNT_PARAM);
            inParam.lFindeHandle = handle;
            var outParam = new StructExports.NET_OUT_QUEYT_RECORD_COUNT_PARAM();
            outParam.dwSize = (uint) sizeof(StructExports.NET_OUT_QUEYT_RECORD_COUNT_PARAM);
            int result = FunctionExports.CLIENT_QueryRecordCount(&inParam, &outParam);
            FunctionExports.CLIENT_FindRecordClose(handle);
            if (result == 0) return -1;
            else return outParam.nRecordCount;
        }

        public int GetCardAmount()
        {
            LLONG handle = GetFileHandle(EnumExports.EM_NET_RECORD_TYPE.NET_RECORD_ACCESSCTLCARD);
            var inParam = new StructExports.NET_IN_QUEYT_RECORD_COUNT_PARAM();
            inParam.dwSize = (uint)sizeof(StructExports.NET_IN_QUEYT_RECORD_COUNT_PARAM);
            inParam.lFindeHandle = handle;
            var outParam = new StructExports.NET_OUT_QUEYT_RECORD_COUNT_PARAM();
            outParam.dwSize = (uint)sizeof(StructExports.NET_OUT_QUEYT_RECORD_COUNT_PARAM);
            int result = FunctionExports.CLIENT_QueryRecordCount(&inParam, &outParam);
            FunctionExports.CLIENT_FindRecordClose(handle);
            if (result == 0) return -1;
            else return outParam.nRecordCount;
        }

        public IEnumerable<Entry> GetEntryIterator()
        {
            CheckConnection();

            LLONG handle = GetFileHandle(EnumExports.EM_NET_RECORD_TYPE.NET_RECORD_ACCESSCTLCARDREC);

            var inParam = new StructExports.NET_IN_FIND_NEXT_RECORD_PARAM();
            inParam.dwSize = (uint)Marshal.SizeOf(typeof(StructExports.NET_IN_FIND_NEXT_RECORD_PARAM));
            inParam.lFindeHandle = handle;
            inParam.nFileCount = bufferSize;

            var outParam = new StructExports.NET_OUT_FIND_NEXT_RECORD_PARAM();
            outParam.dwSize = (uint)Marshal.SizeOf(typeof(StructExports.NET_OUT_FIND_NEXT_RECORD_PARAM));
            outParam.nMaxRecordNum = bufferSize;

            int i = -1;
            List<Entry> entries = null;

            while (true)
            {

                if (i == -1 || i == entries.Count)
                {

                    CheckConnection();

                    entries = FetchEntries(inParam, outParam, bufferSize);

                    if (entries.Count == 0)
                    {
                        FunctionExports.CLIENT_FindRecordClose(handle);
                        yield break;
                    }

                    i = 0;
                }

                yield return entries[i++];
            }
        }

        private List<Card> FetchCards(StructExports.NET_IN_FIND_NEXT_RECORD_PARAM inParam,
            StructExports.NET_OUT_FIND_NEXT_RECORD_PARAM outParam, int bufferSize)
        {
            var cards = new List<Card>();

            var cardBuffer = new StructExports.NET_RECORDSET_ACCESS_CTL_CARD[bufferSize];

            for (int i = 0; i < cardBuffer.Length; i++) cardBuffer[i].dwSize = (uint)sizeof(StructExports.NET_RECORDSET_ACCESS_CTL_CARD);

            fixed (StructExports.NET_RECORDSET_ACCESS_CTL_CARD* cardBufferPtr = cardBuffer)
            {
                outParam.pRecordList = cardBufferPtr;

                if (FunctionExports.CLIENT_FindNextRecord(&inParam, &outParam) > 0)
                {
                    for (int i = 0; i < outParam.nRetRecordNum; i++)
                    {
                        cards.Add(NetCardToCard(cardBufferPtr[i]));
                    }
                }

            }

            return cards;
        }

        public IEnumerable<Card> GetCardIterator()
        {
            CheckConnection();

            LLONG handle = GetFileHandle(EnumExports.EM_NET_RECORD_TYPE.NET_RECORD_ACCESSCTLCARD);

            var inParam = new StructExports.NET_IN_FIND_NEXT_RECORD_PARAM();
            inParam.dwSize = (uint)Marshal.SizeOf(typeof(StructExports.NET_IN_FIND_NEXT_RECORD_PARAM));
            inParam.lFindeHandle = handle;
            inParam.nFileCount = bufferSize;

            var outParam = new StructExports.NET_OUT_FIND_NEXT_RECORD_PARAM();
            outParam.dwSize = (uint)Marshal.SizeOf(typeof(StructExports.NET_OUT_FIND_NEXT_RECORD_PARAM));
            outParam.nMaxRecordNum = bufferSize;

            int i = -1;
            List<Card> cards = null;

            while (true)
            {
                if (i == -1 || i == cards.Count)
                {

                    CheckConnection();

                    cards = FetchCards(inParam, outParam, bufferSize);

                    if (cards.Count == 0)
                    {
                        FunctionExports.CLIENT_FindRecordClose(handle);
                        yield break;
                    }

                    i = 0;
                }

                yield return cards[i++];
            }

        }

    }
}
