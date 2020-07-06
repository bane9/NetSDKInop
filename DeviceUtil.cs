using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Runtime.InteropServices;
using System.Threading;

using DWORD = System.UInt32;
using BOOL = System.Int32;
using LONG = System.Int32;
using LLONG = System.Int64;

namespace NetSDKInop
{
    public unsafe static class DeviceUtil
    {
        public static bool IsInit { get; private set; } = false;

        public struct DeviceInfo
        {
            public string ip;
            public string mac;
            public int port;
        }

        public enum LoginError
        {
            login_ok,
            incorrect_password,
            user_does_not_exist,
            login_timeout,
            account_logged_in,
            account_is_locked,
            account_on_black_list,
            system_busy,
            sub_connection_failed,
            main_connection_failed,
            over_max_user_connection,
            only_support_third_agreement,
            no_usb_in_device,
            clients_ip_doesnt_have_power_login,
            account_not_initialized,
            mac_fetch_failed
        }

        private static readonly FunctionExports.fDisConnect fDisconnect = DevDisconnect;
        private static readonly FunctionExports.fHaveReConnect fRecconect = DevReconnect;
        private static readonly FunctionExports.fMessCallBackEx1 fDevAlarm = DevAlarm;

        public static bool Init(int WaitTime, int TryTimes, bool Reconncect = false)
        {
            if (FunctionExports.CLIENT_InitEx(fDisconnect, null) == 0) return false;

            if (Reconncect) FunctionExports.CLIENT_SetAutoReconnect(fRecconect, null);
            FunctionExports.CLIENT_SetConnectTime(WaitTime, TryTimes);

            var param = new StructExports.NET_PARAM();
            param.nConnectTime = WaitTime;
            FunctionExports.CLIENT_SetNetworkParam(&param);

            FunctionExports.CLIENT_SetDVRMessCallBackEx1(fDevAlarm, null);

            return IsInit = true;
        }

        public static void CleanUp()
        {
            FunctionExports.CLIENT_Cleanup();
            IsInit = false;
        }

        internal static void CheckSDKInit()
        {
            if (!IsInit) throw new SdkNotInitalizedException();
        }

        public static uint GetLastError()
        {
            CheckSDKInit();

            return FunctionExports.CLIENT_GetLastError() & 0x7FFFFFFFU;
        }

        internal static int InternalStrLen(byte* bytes)
        {
            int count = 0;
            while (*bytes++ != 0) ++count;
            return count;
        }

        private static List<DeviceInfo> LanDevices = new List<DeviceInfo>();

        private static readonly FunctionExports.fSearchDevicesCB fDeviceReadCB = DeviceReadCallback;

        static public List<DeviceInfo> GetLanDevices(int scanTime = 2500)
        {
            CheckSDKInit();

            LanDevices.Clear();
            LLONG handle = FunctionExports.CLIENT_StartSearchDevices(fDeviceReadCB, null);
            Thread.Sleep(scanTime);
            FunctionExports.CLIENT_StopSearchDevices(handle);
            return new List<DeviceInfo>(LanDevices);
        }

        internal static ConcurrentDictionary<KeyValuePair<string, int>, Device> ActiveDevices =
            new ConcurrentDictionary<KeyValuePair<string, int>, Device>();

        internal static void DevDisconnect(LLONG lLoginID, byte* pchDVRIP, LONG nDVRPort, void* dwUser)
        {
            var devKvp = new KeyValuePair<string, int>(Marshal.PtrToStringAnsi((IntPtr)pchDVRIP), nDVRPort);
            if (ActiveDevices.ContainsKey(devKvp)) ActiveDevices[devKvp].IsConnected = false;
        }

        internal static void DevReconnect(LLONG lLoginID, byte* pchDVRIP, LONG nDVRPort, void* dwUser)
        {
            var devKvp = new KeyValuePair<string, int>(Marshal.PtrToStringAnsi((IntPtr)pchDVRIP), nDVRPort);
            if (ActiveDevices.ContainsKey(devKvp))
            {
                var device = ActiveDevices[devKvp];
                device.IsConnected = true;
                device.devId = lLoginID;
            }
        }

        internal static void DeviceReadCallback(void* vpDevNetInfo, void* pUserData)
        {
            var pDevNetInfo = (StructExports.DEVICE_NET_INFO_EX*)vpDevNetInfo;
            var deviceInfo = new DeviceInfo();
            deviceInfo.ip = Marshal.PtrToStringAnsi((IntPtr)pDevNetInfo->szIP);
            deviceInfo.mac = Marshal.PtrToStringAnsi((IntPtr)pDevNetInfo->szMac);
            deviceInfo.port = pDevNetInfo->nPort;

            LanDevices.Add(deviceInfo);
        }

        internal static int DevAlarm(LONG lCommand, LLONG lLoginID, byte* pBuf, DWORD dwBufLen, byte* pchDVRIP, LONG nDVRPort,
            BOOL bAlarmAckFlag, LONG nEventID, void* dwUser)
        {
            if (lCommand == 0x3181)
            {
                var devKvp = new KeyValuePair<string, int>(Marshal.PtrToStringAnsi((IntPtr)pchDVRIP), nDVRPort);
                StructExports.ALARM_ACCESS_CTL_EVENT_INFO* pstAccessInfo = (StructExports.ALARM_ACCESS_CTL_EVENT_INFO*)pBuf;
                if (ActiveDevices.ContainsKey(devKvp))
                {
                    ActiveDevices[devKvp].readCard = Marshal.PtrToStringAnsi((IntPtr)pstAccessInfo->szCardNo);
                }
            }

            return 0;
        }
    }
}
