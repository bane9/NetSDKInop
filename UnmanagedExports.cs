using System.Runtime.InteropServices;

using DWORD = System.UInt32;
using BOOL = System.Int32;
using LONG = System.Int32;
using BYTE = System.Byte;
using UINT = System.UInt32;
using WORD = System.UInt16;
using LLONG = System.Int64;

namespace NetSDKInop
{

    internal static unsafe class StructExports
    {
        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct NETSDK_INIT_PARAM
        {
            public int nThreadNum;
            public fixed BYTE bReserved[1024];
        }

        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct NET_PARAM
        {
            public int nWaittime;
            public int nConnectTime;
            public int nConnectTryNum;
            public int nSubConnectSpaceTime;
            public int nGetDevInfoTime;
            public int nConnectBufSize;
            public int nGetConnInfoTime;
            public int nSearchRecordTime;
            public int nsubDisconnetTime;
            public BYTE byNetType;
            public BYTE byPlaybackBufSize;
            public BYTE bDetectDisconnTime;
            public BYTE bKeepLifeInterval;
            public int nPicBufSize;
            public fixed BYTE bReserved[4];
        }

        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct NET_DEVICEINFO_Ex
        {
            fixed BYTE sSerialNumber[48];
            public int nAlarmInPortNum;
            public int nAlarmOutPortNum;
            public int nDiskNum;
            public int nDVRType;
            public int nChanNum;
            public BYTE byLimitLoginTime;
            public BYTE byLeftLogTimes;
            public fixed BYTE bReserved[2];
            public int nLockLeftTime;
            public fixed byte Reserved[24];
        }

        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct NET_TIME
        {
            public DWORD dwYear;
            public DWORD dwMonth;
            public DWORD dwDay;
            public DWORD dwHour;
            public DWORD dwMinute;
            public DWORD dwSecond;
        }

        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct NET_ACCESSCTLCARD_FINGERPRINT_PACKET
        {
            public DWORD dwSize;
            public int nLength;
            public int nCount;
            public byte* pPacketData;
        }

        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct NET_ACCESSCTLCARD_FINGERPRINT_PACKET_EX
        {
            public int nLength;
            public int nCount;
            public byte* pPacketData;
            public int nPacketLen;
            public int nRealPacketLen;
            public int nDuressIndex;
            public fixed BYTE byReverseed[1020];
        }

        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct NET_RECORDSET_ACCESS_CTL_CARD
        {
            public DWORD dwSize;
            public int nRecNo;
            [MarshalAs(UnmanagedType.Struct)]
            public NET_TIME stuCreateTime;
            public fixed byte szCardNo[32];
            public fixed byte szUserID[32];
            public EnumExports.NET_ACCESSCTLCARD_STATE emStatus;
            public EnumExports.NET_ACCESSCTLCARD_TYPE emType;
            public fixed byte szPsw[64];
            public int nDoorNum;
            public fixed int sznDoors[32];
            public int nTimeSectionNum;
            public fixed int sznTimeSectionNo[32];
            public int nUserTime;
            [MarshalAs(UnmanagedType.Struct)]
            public NET_TIME stuValidStartTime;
            [MarshalAs(UnmanagedType.Struct)]
            public NET_TIME stuValidEndTime;
            public BOOL bIsValid;
            [MarshalAs(UnmanagedType.Struct)]
            public NET_ACCESSCTLCARD_FINGERPRINT_PACKET stuFingerPrintInfo;
            public BOOL bFirstEnter;
            public fixed byte szCardName[32];
            public fixed byte szVTOPosition[64];
            public BOOL bHandicap;
            public BOOL bEnableExtended;
            [MarshalAs(UnmanagedType.Struct)]
            public NET_ACCESSCTLCARD_FINGERPRINT_PACKET_EX stuFingerPrintInfoEx;
            public int nFaceDataNum;
            public fixed byte szFaceData[40960];
            public fixed byte szDynamicCheckCode[16];
            public int nRepeatEnterRouteNum;
            public fixed int arRepeatEnterRoute[12];
            public int nRepeatEnterRouteTimeout;
            public BOOL bNewDoor;
            public int nNewDoorNum;
            public fixed int nNewDoors[128];
            public int nNewTimeSectionNum;
            public fixed int nNewTimeSectionNo[128];
            public fixed byte szCitizenIDNo[32];
            public int nSpecialDaysScheduleNum;
            public fixed int arSpecialDaysSchedule[128];
            public UINT nUserType;
            public int nFloorNum;
            public fixed byte szFloorNo[256];
        }

        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct NET_CTRL_RECORDSET_PARAM
        {
            public DWORD dwSize;
            public EnumExports.EM_NET_RECORD_TYPE emType;
            public void* pBuf;
            public int nBufLen;
        }

        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct NET_IN_FIND_NEXT_RECORD_PARAM
        {
            public DWORD dwSize;
            public LLONG lFindeHandle;
            public int nFileCount;
        }

        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct NET_OUT_FIND_RECORD_PARAM
        {
            public DWORD dwSize;
            public LLONG lFindeHandle;
        }

        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct NET_OUT_FIND_NEXT_RECORD_PARAM
        {
            public DWORD dwSize;
            public void* pRecordList;
            public int nMaxRecordNum;
            public int nRetRecordNum;
        }

        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct NET_IN_QUEYT_RECORD_COUNT_PARAM
        {
            public DWORD dwSize;
            public LLONG lFindeHandle;
        }

        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct NET_OUT_QUEYT_RECORD_COUNT_PARAM
        {
            public DWORD dwSize;
            public int nRecordCount;
        }

        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct DEVICE_NET_INFO_EX
        {
            public int iIPVersion;
            public fixed byte szIP[64];
            public int nPort;
            public fixed byte szSubmask[64];
            public fixed byte szGateway[64];
            public fixed byte szMac[40];
            public fixed byte szDeviceType[32];
            public BYTE byManuFactory;
            public BYTE byDefinition;
            public bool bDhcpEn;
            public BYTE byReserved1;
            public fixed byte verifyData[88];
            public fixed byte szSerialNo[48];
            public fixed byte szDevSoftVersion[128];
            public fixed byte szDetailType[32];
            public fixed byte szVendor[128];
            public fixed byte szDevName[64];
            public fixed byte szUserName[16];
            public fixed byte szPassWord[16];
            public WORD nHttpPort;
            public WORD wVideoInputCh;
            public WORD wRemoteVideoInputCh;
            public WORD wVideoOutputCh;
            public WORD wAlarmInputCh;
            public WORD wAlarmOutputCh;
            public BOOL bNewWordLen;
            public fixed byte szNewPassWord[64];
            public BYTE byInitStatus;
            public BYTE byPwdResetWay;
            public BYTE bySpecialAbility;
            public fixed byte szNewDetailType[64];
            public BOOL bNewUserName;
            public fixed byte szNewUserName[64];
            public fixed byte cReserved[41];
        }

        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct CFG_NETWORK_INTERFACE
        {
            public fixed byte szName[128];
            public fixed byte szIP[256];
            public fixed byte szSubnetMask[256];
            public fixed byte szDefGateway[256];
            public BOOL bDhcpEnable;
            public BOOL bDnsAutoGet;
            public fixed byte szDnsServers[2 * 256];
            public int nMTU;
            public fixed byte szMacAddress[256];
            public BOOL bInterfaceEnable;
            public BOOL bReservedIPEnable;
            public EnumExports.CFG_ENUM_NET_TRANSMISSION_MODE emNetTranmissionMode;
            public EnumExports.CFG_ENUM_NET_INTERFACE_TYPE emInterfaceType;
            public EnumExports.CFG_THREE_STATUS_BOOL bBond;
        }

        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct CFG_NETWORK_INFO
        {
            public fixed byte szHostName[128];
            public fixed byte szDomain[128];
            public fixed byte szDefInterface[128];
            public int nInterfaceNum;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public CFG_NETWORK_INTERFACE[] stuInterfaces;
        }

        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct NET_IN_FIND_RECORD_PARAM
        {
            public DWORD dwSize;
            public EnumExports.EM_NET_RECORD_TYPE emType;
            public void* pQueryCondition;
        }

        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct DHDEV_NETINTERFACE_INFO
        {
            public int dwSize;
            public BOOL bValid;
            public BOOL bVirtual;
            public int nSpeed;
            public int nDHCPState;
            public fixed byte szName[260];
            public fixed byte szType[260];
            public fixed byte szMAC[40];
            public fixed byte szSSID[36];
            public fixed byte szConnStatus[260];
            public int nSupportedModeNum;
            public fixed byte szSupportedModes[260];
            public BOOL bSupportLongPoE;
            public fixed byte szNetCardName[64 * 64];
        }
        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct NET_CTRL_RECORDSET_INSERT_IN
        {
            public DWORD dwSize;
            public EnumExports.EM_NET_RECORD_TYPE emType;
            public void* pBuf;
            public int nBufLen;
        }

        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct NET_CTRL_RECORDSET_INSERT_OUT
        {
            public DWORD dwSize;
            public int nRecNo;
        }

        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct NET_CTRL_RECORDSET_INSERT_PARAM
        {
            public DWORD dwSize;
            [MarshalAs(UnmanagedType.Struct)]
            public NET_CTRL_RECORDSET_INSERT_IN stuCtrlRecordSetInfo;
            [MarshalAs(UnmanagedType.Struct)]
            public NET_CTRL_RECORDSET_INSERT_OUT stuCtrlRecordSetResult;
        }

        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct USER_INFO_NEW
        {
            public DWORD dwSize;
            public DWORD dwID;
            public DWORD dwGroupID;
            public fixed byte name[128];
            public fixed byte passWord[128];
            public DWORD dwRightNum;
            public fixed DWORD rights[1024];
            public fixed byte memo[32];
            public DWORD dwFouctionMask;
            [MarshalAs(UnmanagedType.Struct)]
            public NET_TIME stuTime;
            public BYTE byIsAnonymous;
            public fixed BYTE byReserve[7];
        }

        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct NET_CTRL_ACCESS_OPEN
        {
            public DWORD dwSize;
            public int nChannelID;
            public byte* szTargetID;
            public fixed byte szUserID[32];
            public EnumExports.EM_OPEN_DOOR_TYPE emOpenDoorType;
        }

        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct ALARM_ACCESS_CTL_EVENT_INFO
        {
            public DWORD dwSize;
            public int nDoor;
            public fixed byte szDoorName[128];
            [MarshalAs(UnmanagedType.Struct)]
            public NET_TIME stuTime;
            public EnumExports.NET_ACCESS_CTL_EVENT_TYPE emEventType;
            public BOOL bStatus;
            public EnumExports.NET_ACCESSCTLCARD_TYPE emCardType;
            public EnumExports.NET_ACCESS_DOOROPEN_METHOD emOpenMethod;
            public fixed byte szCardNo[32];
            public fixed byte szPwd[64];
            public fixed byte szReaderID[32];
            public fixed byte szUserID[64];
            public fixed byte szSnapURL[256];
            public int nErrorCode;
            public int nPunchingRecNo;
            public int nNumbers;
            public EnumExports.NET_ACCESSCTLCARD_STATE emStatus;
            public fixed byte szSN[32];
            public EnumExports.NET_ATTENDANCESTATE emAttendanceState;
            public fixed byte szQRCode[512];
            public fixed byte szCallLiftFloor[16];
        }

        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct NET_RECORDSET_ACCESS_CTL_CARDREC
        {
            public DWORD dwSize;
            public int nRecNo;
            public fixed byte szCardNo[32];
            public fixed byte szPwd[64];
            [MarshalAs(UnmanagedType.Struct)]
            public NET_TIME stuTime;
            public BOOL bStatus;
            public EnumExports.NET_ACCESS_DOOROPEN_METHOD emMethod;
            public int nDoor;
            public fixed byte szUserID[32];
            public int nReaderID;
            public fixed byte szSnapFtpUrl[260];
            public fixed byte szReaderID[32];
            public EnumExports.NET_ACCESSCTLCARD_TYPE emCardType;
            public int nErrorCode;
            public fixed byte szRecordURL[128];
            public int nNumbers;
            public EnumExports.NET_ATTENDANCESTATE emAttendanceState;
            public EnumExports.NET_ENUM_DIRECTION_ACCESS_CTL emDirection;
            public fixed byte szClassNumber[32];
            public fixed byte szPhoneNumber[16];
            public fixed byte szCardName[64];
            public fixed byte szSN[32];
        }
    }

    internal static class EnumExports
    {
        public enum EM_LOGIN_SPAC_CAP_TYPE
        {
            EM_LOGIN_SPEC_CAP_TCP = 0,
            EM_LOGIN_SPEC_CAP_ANY = 1,
            EM_LOGIN_SPEC_CAP_SERVER_CONN = 2,
            EM_LOGIN_SPEC_CAP_MULTICAST = 3,
            EM_LOGIN_SPEC_CAP_UDP = 4,
            EM_LOGIN_SPEC_CAP_MAIN_CONN_ONLY = 6,
            EM_LOGIN_SPEC_CAP_SSL = 7,

            EM_LOGIN_SPEC_CAP_INTELLIGENT_BOX = 9,
            EM_LOGIN_SPEC_CAP_NO_CONFIG = 10,
            EM_LOGIN_SPEC_CAP_U_LOGIN = 11,
            EM_LOGIN_SPEC_CAP_LDAP = 12,
            EM_LOGIN_SPEC_CAP_AD = 13,
            EM_LOGIN_SPEC_CAP_RADIUS = 14,
            EM_LOGIN_SPEC_CAP_SOCKET_5 = 15,
            EM_LOGIN_SPEC_CAP_CLOUD = 16,
            EM_LOGIN_SPEC_CAP_AUTH_TWICE = 17,
            EM_LOGIN_SPEC_CAP_TS = 18,
            EM_LOGIN_SPEC_CAP_P2P = 19,
            EM_LOGIN_SPEC_CAP_MOBILE = 20,
            EM_LOGIN_SPEC_CAP_INVALID
        }

        public enum NET_ACCESSCTLCARD_STATE
        {
            NET_ACCESSCTLCARD_STATE_UNKNOWN = -1,
            NET_ACCESSCTLCARD_STATE_NORMAL = 0,
            NET_ACCESSCTLCARD_STATE_LOSE = 0x01,
            NET_ACCESSCTLCARD_STATE_LOGOFF = 0x02,
            NET_ACCESSCTLCARD_STATE_FREEZE = 0x04,
            NET_ACCESSCTLCARD_STATE_ARREARAGE = 0x08,
            NET_ACCESSCTLCARD_STATE_OVERDUE = 0x10,
            NET_ACCESSCTLCARD_STATE_PREARREARAGE = 0x20
        }

        public enum NET_ACCESSCTLCARD_TYPE
        {
            NET_ACCESSCTLCARD_TYPE_UNKNOWN = -1,
            NET_ACCESSCTLCARD_TYPE_GENERAL,
            NET_ACCESSCTLCARD_TYPE_VIP,
            NET_ACCESSCTLCARD_TYPE_GUEST,
            NET_ACCESSCTLCARD_TYPE_PATROL,
            NET_ACCESSCTLCARD_TYPE_BLACKLIST,
            NET_ACCESSCTLCARD_TYPE_CORCE,
            NET_ACCESSCTLCARD_TYPE_POLLING,
            NET_ACCESSCTLCARD_TYPE_MOTHERCARD = 0xff
        }

        public enum CtrlType
        {
            DH_CTRL_REBOOT = 0,
            DH_CTRL_SHUTDOWN,
            DH_CTRL_DISK,
            DH_KEYBOARD_POWER = 3,
            DH_KEYBOARD_ENTER,
            DH_KEYBOARD_ESC,
            DH_KEYBOARD_UP,
            DH_KEYBOARD_DOWN,
            DH_KEYBOARD_LEFT,
            DH_KEYBOARD_RIGHT,
            DH_KEYBOARD_BTN0,
            DH_KEYBOARD_BTN1,
            DH_KEYBOARD_BTN2,
            DH_KEYBOARD_BTN3,
            DH_KEYBOARD_BTN4,
            DH_KEYBOARD_BTN5,
            DH_KEYBOARD_BTN6,
            DH_KEYBOARD_BTN7,
            DH_KEYBOARD_BTN8,
            DH_KEYBOARD_BTN9,
            DH_KEYBOARD_BTN10,
            DH_KEYBOARD_BTN11,
            DH_KEYBOARD_BTN12,
            DH_KEYBOARD_BTN13,
            DH_KEYBOARD_BTN14,
            DH_KEYBOARD_BTN15,
            DH_KEYBOARD_BTN16,
            DH_KEYBOARD_SPLIT,
            DH_KEYBOARD_ONE,
            DH_KEYBOARD_NINE,
            DH_KEYBOARD_ADDR,
            DH_KEYBOARD_INFO,
            DH_KEYBOARD_REC,
            DH_KEYBOARD_FN1,
            DH_KEYBOARD_FN2,
            DH_KEYBOARD_PLAY,
            DH_KEYBOARD_STOP,
            DH_KEYBOARD_SLOW,
            DH_KEYBOARD_FAST,
            DH_KEYBOARD_PREW,
            DH_KEYBOARD_NEXT,
            DH_KEYBOARD_JMPDOWN,
            DH_KEYBOARD_JMPUP,
            DH_KEYBOARD_10PLUS,
            DH_KEYBOARD_SHIFT,
            DH_KEYBOARD_BACK,
            DH_KEYBOARD_LOGIN,
            DH_KEYBOARD_CHNNEL,
            DH_TRIGGER_ALARM_IN = 100,
            DH_TRIGGER_ALARM_OUT,
            DH_CTRL_MATRIX,
            DH_CTRL_SDCARD,
            DH_BURNING_START,
            DH_BURNING_STOP,
            DH_BURNING_ADDPWD,
            DH_BURNING_ADDHEAD,
            DH_BURNING_ADDSIGN,
            DH_BURNING_ADDCURSTOMINFO,
            DH_CTRL_RESTOREDEFAULT,
            DH_CTRL_CAPTURE_START,
            DH_CTRL_CLEARLOG,
            DH_TRIGGER_ALARM_WIRELESS = 200,
            DH_MARK_IMPORTANT_RECORD,
            DH_CTRL_DISK_SUBAREA,
            DH_BURNING_ATTACH,
            DH_BURNING_PAUSE,
            DH_BURNING_CONTINUE,
            DH_BURNING_POSTPONE,
            DH_CTRL_OEMCTRL,
            DH_BACKUP_START,
            DH_BACKUP_STOP,
            DH_VIHICLE_WIFI_ADD,
            DH_VIHICLE_WIFI_DEC,
            DH_BUZZER_START,
            DH_BUZZER_STOP,
            DH_REJECT_USER,
            DH_SHIELD_USER,
            DH_RAINBRUSH,
            DH_MANUAL_SNAP,
            DH_MANUAL_NTP_TIMEADJUST,
            DH_NAVIGATION_SMS,
            DH_CTRL_ROUTE_CROSSING,
            DH_BACKUP_FORMAT,
            DH_DEVICE_LOCALPREVIEW_SLIPT,
            DH_CTRL_INIT_RAID,
            DH_CTRL_RAID,
            DH_CTRL_SAPREDISK,
            DH_WIFI_CONNECT,
            DH_WIFI_DISCONNECT,
            DH_CTRL_ARMED,
            DH_CTRL_IP_MODIFY,
            DH_CTRL_WIFI_BY_WPS,
            DH_CTRL_FORMAT_PATITION,
            DH_CTRL_EJECT_STORAGE,
            DH_CTRL_LOAD_STORAGE,
            DH_CTRL_CLOSE_BURNER,
            DH_CTRL_EJECT_BURNER,
            DH_CTRL_CLEAR_ALARM,
            DH_CTRL_MONITORWALL_TVINFO,
            DH_CTRL_START_VIDEO_ANALYSE,
            DH_CTRL_STOP_VIDEO_ANALYSE,
            DH_CTRL_UPGRADE_DEVICE,
            DH_CTRL_MULTIPLAYBACK_CHANNALES,
            DH_CTRL_SEQPOWER_OPEN,
            DH_CTRL_SEQPOWER_CLOSE,
            DH_CTRL_SEQPOWER_OPEN_ALL,
            DH_CTRL_SEQPOWER_CLOSE_ALL,
            DH_CTRL_PROJECTOR_RISE,
            DH_CTRL_PROJECTOR_FALL,
            DH_CTRL_PROJECTOR_STOP,
            DH_CTRL_INFRARED_KEY,
            DH_CTRL_START_PLAYAUDIO,
            DH_CTRL_STOP_PLAYAUDIO,
            DH_CTRL_START_ALARMBELL,
            DH_CTRL_STOP_ALARMBELL,
            DH_CTRL_ACCESS_OPEN,
            DH_CTRL_SET_BYPASS,
            DH_CTRL_RECORDSET_INSERT,
            DH_CTRL_RECORDSET_UPDATE,
            DH_CTRL_RECORDSET_REMOVE,
            DH_CTRL_RECORDSET_CLEAR,
            DH_CTRL_ACCESS_CLOSE,
            DH_CTRL_ALARM_SUBSYSTEM_ACTIVE_SET,
            DH_CTRL_FORBID_OPEN_STROBE,
            DH_CTRL_OPEN_STROBE,
            DH_CTRL_TALKING_REFUSE,
            DH_CTRL_ARMED_EX,
            DH_CTRL_REMOTE_TALK,
            DH_CTRL_NET_KEYBOARD = 400,
            DH_CTRL_AIRCONDITION_OPEN,
            DH_CTRL_AIRCONDITION_CLOSE,
            DH_CTRL_AIRCONDITION_SET_TEMPERATURE,
            DH_CTRL_AIRCONDITION_ADJUST_TEMPERATURE,
            DH_CTRL_AIRCONDITION_SETMODE,
            DH_CTRL_AIRCONDITION_SETWINDMODE,
            DH_CTRL_RESTOREDEFAULT_EX,


            DH_CTRL_NOTIFY_EVENT,
            DH_CTRL_SILENT_ALARM_SET,
            DH_CTRL_START_PLAYAUDIOEX,
            DH_CTRL_STOP_PLAYAUDIOEX,
            DH_CTRL_CLOSE_STROBE,
            DH_CTRL_SET_ORDER_STATE,
            DH_CTRL_RECORDSET_INSERTEX,
            DH_CTRL_RECORDSET_UPDATEEX,
            DH_CTRL_CAPTURE_FINGER_PRINT,
            DH_CTRL_ECK_LED_SET,
            DH_CTRL_ECK_IC_CARD_IMPORT,
            DH_CTRL_ECK_SYNC_IC_CARD,
            DH_CTRL_LOWRATEWPAN_REMOVE,
            DH_CTRL_LOWRATEWPAN_MODIFY,
            DH_CTRL_ECK_SET_PARK_INFO,
            DH_CTRL_VTP_DISCONNECT,
            DH_CTRL_UPDATE_FILES,
            DH_CTRL_MATRIX_SAVE_SWITCH,
            DH_CTRL_MATRIX_RESTORE_SWITCH,
            DH_CTRL_VTP_DIVERTACK,
            DH_CTRL_RAINBRUSH_MOVEONCE,
            DH_CTRL_RAINBRUSH_MOVECONTINUOUSLY,
            DH_CTRL_RAINBRUSH_STOPMOVE,
            DH_CTRL_ALARM_ACK,
            DH_CTRL_RECORDSET_IMPORT,
            DH_CTRL_DELIVERY_FILE,
            DH_CTRL_FORCE_BREAKING,
            DH_CTRL_RESTORE_EXCEPT,
            DH_CTRL_SET_PARK_INFO,
            DH_CTRL_CLEAR_SECTION_STAT,
            DH_CTRL_DELIVERY_FILE_BYCAR,
            DH_CTRL_ECK_GUIDINGPANEL_CONTENT,
            DH_CTRL_SET_SAFE_LEVEL,
            DH_CTRL_VTP_INVITEACK,

            DH_CTRL_THERMO_GRAPHY_ENSHUTTER = 0x10000,
            DH_CTRL_RADIOMETRY_SETOSDMARK,
            DH_CTRL_AUDIO_REC_START_NAME,
            DH_CTRL_AUDIO_REC_STOP_NAME,
            DH_CTRL_SNAP_MNG_SNAP_SHOT,
            DH_CTRL_LOG_STOP,
            DH_CTRL_LOG_RESUME,
            DH_CTRL_POS_ADD,
            DH_CTRL_POS_REMOVE,
            DH_CTRL_POS_REMOVE_MULTI,
            DH_CTRL_POS_MODIFY,
            DH_CTRL_SET_SOUND_ALARM,
            DH_CTRL_AUDIO_MATRIX_SILENCE,
            DH_CTRL_MANUAL_UPLOAD_PICTURE,
            DH_CTRL_REBOOT_NET_DECODING_DEV,
            DH_CTRL_SET_IC_SENDER,
            DH_CTRL_SET_MEDIAKIND,
            DH_CTRL_LOWRATEWPAN_ADD,
            DH_CTRL_LOWRATEWPAN_REMOVEALL,
            DH_CTRL_SET_DOOR_WORK_MODE,
            DH_CTRL_TEST_MAIL,
            DH_CTRL_CONTROL_SMART_SWITCH,
            DH_CTRL_LOWRATEWPAN_SETWORKMODE,
            DH_CTRL_COAXIAL_CONTROL_IO,
            DH_CTRL_START_REMOTELOWRATEWPAN_ALARMBELL,
            DH_CTRL_STOP_REMOTELOWRATEWPAN_ALARMBELL,

            DH_CTRL_LOWRATEWPAN_GETWIRELESSDEVSIGNAL = 0x10100
        }

        public enum EM_NET_RECORD_TYPE
        {
            NET_RECORD_UNKNOWN,
            NET_RECORD_TRAFFICREDLIST,
            NET_RECORD_TRAFFICBLACKLIST,
            NET_RECORD_BURN_CASE,
            NET_RECORD_ACCESSCTLCARD,
            NET_RECORD_ACCESSCTLPWD,
            NET_RECORD_ACCESSCTLCARDREC,
            NET_RECORD_ACCESSCTLHOLIDAY,
            NET_RECORD_TRAFFICFLOW_STATE,
            NET_RECORD_VIDEOTALKLOG,
            NET_RECORD_REGISTERUSERSTATE,
            NET_RECORD_VIDEOTALKCONTACT,
            NET_RECORD_ANNOUNCEMENT,
            NET_RECORD_ALARMRECORD,
            NET_RECORD_COMMODITYNOTICE,
            NET_RECORD_HEALTHCARENOTICE,
            NET_RECORD_ACCESSCTLCARDREC_EX,
            NET_RECORD_GPS_LOCATION,
            NET_RECORD_RESIDENT,
            NET_RECORD_SENSORRECORD,
            NET_RECORD_ACCESSQRCODE,
            NET_RECORD_ELECTRONICSTAG,
            NET_RECORD_ACCESS_BLUETOOTH,
            NET_RECORD_ACCESS_ALARMRECORD
        }

        public enum CFG_ENUM_NET_TRANSMISSION_MODE
        {
            CFG_ENUM_NET_MODE_ADAPT,
            CFG_ENUM_NET_MODE_HALF10M,
            CFG_ENUM_NET_MODE_FULL10M,
            CFG_ENUM_NET_MODE_HALF100M,
            CFG_ENUM_NET_MODE_FULL100M
        }

        public enum CFG_ENUM_NET_INTERFACE_TYPE
        {
            CFG_ENUM_NET_INTERFACE_TYPE_UNKNOWN,
            CFG_ENUM_NET_INTERFACE_TYPE_STANDARD,
            CFG_ENUM_NET_INTERFACE_TYPE_MANAGER,
            CFG_ENUM_NET_INTERFACE_TYPE_EXTEND
        }

        public enum CFG_THREE_STATUS_BOOL
        {
            CFG_BOOL_STATUS_UNKNOWN = -1,
            CFG_BOOL_STATUS_FALSE = 0,
            CFG_BOOL_STATUS_TRUE = 1
        }

        public enum EM_OPEN_DOOR_TYPE
        {
            EM_OPEN_DOOR_TYPE_UNKNOWN = 0,
            EM_OPEN_DOOR_TYPE_REMOTE,
            EM_OPEN_DOOR_TYPE_LOCAL_PASSWORD,
            EM_OPEN_DOOR_TYPE_LOCAL_CARD,
            EM_OPEN_DOOR_TYPE_LOCAL_BUTTON
        }

        public enum NET_ACCESS_CTL_EVENT_TYPE
        {
            NET_ACCESS_CTL_EVENT_UNKNOWN = 0,
            NET_ACCESS_CTL_EVENT_ENTRY,
            NET_ACCESS_CTL_EVENT_EXIT,
        }

        public enum NET_ACCESS_DOOROPEN_METHOD
        {
            NET_ACCESS_DOOROPEN_METHOD_UNKNOWN = 0,
            NET_ACCESS_DOOROPEN_METHOD_PWD_ONLY,
            NET_ACCESS_DOOROPEN_METHOD_CARD,
            NET_ACCESS_DOOROPEN_METHOD_CARD_FIRST,
            NET_ACCESS_DOOROPEN_METHOD_PWD_FIRST,
            NET_ACCESS_DOOROPEN_METHOD_REMOTE,
            NET_ACCESS_DOOROPEN_METHOD_BUTTON,
            NET_ACCESS_DOOROPEN_METHOD_FINGERPRINT,
            NET_ACCESS_DOOROPEN_METHOD_PWD_CARD_FINGERPRINT,
            NET_ACCESS_DOOROPEN_METHOD_PWD_FINGERPRINT = 10,
            NET_ACCESS_DOOROPEN_METHOD_CARD_FINGERPRINT = 11,
            NET_ACCESS_DOOROPEN_METHOD_PERSONS = 12,
            NET_ACCESS_DOOROPEN_METHOD_KEY = 13,
            NET_ACCESS_DOOROPEN_METHOD_COERCE_PWD = 14,
            NET_ACCESS_DOOROPEN_METHOD_QRCODE = 15,
            NET_ACCESS_DOOROPEN_METHOD_FACE_RECOGNITION = 16,
            NET_ACCESS_DOOROPEN_METHOD_FACEIDCARD = 18,
            NET_ACCESS_DOOROPEN_METHOD_FACEIDCARD_AND_IDCARD = 19,
            NET_ACCESS_DOOROPEN_METHOD_BLUETOOTH = 20,
            NET_ACCESS_DOOROPEN_METHOD_CUSTOM_PASSWORD = 21,
            NET_ACCESS_DOOROPEN_METHOD_USERID_AND_PWD = 22,
            NET_ACCESS_DOOROPEN_METHOD_FACE_AND_PWD = 23,
            NET_ACCESS_DOOROPEN_METHOD_FINGERPRINT_AND_PWD = 24,
            NET_ACCESS_DOOROPEN_METHOD_FINGERPRINT_AND_FACE = 25,
            NET_ACCESS_DOOROPEN_METHOD_CARD_AND_FACE = 26,
            NET_ACCESS_DOOROPEN_METHOD_FACE_OR_PWD = 27,
            NET_ACCESS_DOOROPEN_METHOD_FINGERPRINT_OR_PWD = 28,
            NET_ACCESS_DOOROPEN_METHOD_FINGERPRINT_OR_FACE = 29,
            NET_ACCESS_DOOROPEN_METHOD_CARD_OR_FACE = 30,
            NET_ACCESS_DOOROPEN_METHOD_CARD_OR_FINGERPRINT = 31,
            NET_ACCESS_DOOROPEN_METHOD_FINGERPRINT_AND_FACE_AND_PWD = 32,
            NET_ACCESS_DOOROPEN_METHOD_CARD_AND_FACE_AND_PWD = 33,
            NET_ACCESS_DOOROPEN_METHOD_CARD_AND_FINGERPRINT_AND_PWD = 34,
            NET_ACCESS_DOOROPEN_METHOD_CARD_AND_PWD_AND_FACE = 35,
            NET_ACCESS_DOOROPEN_METHOD_FINGERPRINT_OR_FACE_OR_PWD = 36,
            NET_ACCESS_DOOROPEN_METHOD_CARD_OR_FACE_OR_PWD = 37,
            NET_ACCESS_DOOROPEN_METHOD_CARD_OR_FINGERPRINT_OR_FACE = 38,
            NET_ACCESS_DOOROPEN_METHOD_CARD_AND_FINGERPRINT_AND_FACE_AND_PWD = 39,
            NET_ACCESS_DOOROPEN_METHOD_CARD_OR_FINGERPRINT_OR_FACE_OR_PWD = 40,
            NET_ACCESS_DOOROPEN_METHOD_FACEIPCARDANDIDCARD_OR_CARD_OR_FACE = 41,
            NET_ACCESS_DOOROPEN_METHOD_FACEIDCARD_OR_CARD_OR_FACE = 42,
            NET_ACCESS_DOOROPEN_METHOD_DTMF = 43,
            NET_ACCESS_DOOROPEN_METHOD_REMOTE_QRCODE = 44,
            NET_ACCESS_DOOROPEN_METHOD_REMOTE_FACE = 45,
        }

        public enum NET_ATTENDANCESTATE
        {
            NET_ATTENDANCESTATE_UNKNOWN,
            NET_ATTENDANCESTATE_SIGNIN,
            NET_ATTENDANCESTATE_GOOUT,
            NET_ATTENDANCESTATE_GOOUT_AND_RETRUN,
            NET_ATTENDANCESTATE_SIGNOUT,
            NET_ATTENDANCESTATE_WORK_OVERTIME_SIGNIN,
            NET_ATTENDANCESTATE_WORK_OVERTIME_SIGNOUT
        }

        public enum NET_ENUM_DIRECTION_ACCESS_CTL
        {
            NET_ENUM_DIRECTION_UNKNOWN,
            NET_ENUM_DIRECTION_ENTRY,
            NET_ENUM_DIRECTION_EXIT
        }
    }

    internal static unsafe class FunctionExports
    {
        public const string dahuasdkdll = "dhnetsdk.dll";
        public const string dahuaconfigdll = "dhconfigsdk.dll";

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void fDisConnect(LLONG lLoginID, byte* pchDVRIP, LONG nDVRPort, void* dwUser);

        [DllImport(dahuasdkdll, CallingConvention = CallingConvention.StdCall)]
        public static extern BOOL CLIENT_InitEx(fDisConnect cbDisConnect, void* dwUser, StructExports.NETSDK_INIT_PARAM* lpInitParam = null);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void fHaveReConnect(LLONG lLoginID, byte* pchDVRIP, LONG nDVRPort, void* dwUser);

        [DllImport(dahuasdkdll, CallingConvention = CallingConvention.StdCall)]
        public static extern void CLIENT_SetAutoReconnect(fHaveReConnect cbAutoConnect, void* dwUser);

        [DllImport(dahuasdkdll, CallingConvention = CallingConvention.StdCall)]
        public static extern void CLIENT_SetConnectTime(int nWaitTime, int nTryTimes);

        [DllImport(dahuasdkdll, CallingConvention = CallingConvention.StdCall)]
        public static extern void CLIENT_SetNetworkParam(StructExports.NET_PARAM* pNetParam);

        [DllImport(dahuasdkdll, CallingConvention = CallingConvention.StdCall)]
        public static extern LLONG CLIENT_LoginEx2(byte* pchDVRIP, DWORD wDVRPort, byte* pchUserName, byte* pchPassword,
            EnumExports.EM_LOGIN_SPAC_CAP_TYPE emSpecCap, void* pCapParam, StructExports.NET_DEVICEINFO_Ex* lpDeviceInfo, int* error = null);

        [DllImport(dahuasdkdll, CallingConvention = CallingConvention.StdCall)]
        public static extern BOOL CLIENT_ControlDeviceEx(LLONG lLoginID, EnumExports.CtrlType emType, void* pInBuf, void* pOutBuf = null, int nWaitTime = 1000);

        [DllImport(dahuasdkdll, CallingConvention = CallingConvention.StdCall)]
        public static extern BOOL CLIENT_Logout(LLONG id);

        [DllImport(dahuasdkdll, CallingConvention = CallingConvention.StdCall)]
        public static extern DWORD CLIENT_GetSDKVersion();

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate int fMessCallBackEx1(LONG lCommand, LLONG lLoginID, byte* pBuf, DWORD dwBufLen, byte* pchDVRIP, LONG nDVRPort,
            BOOL bAlarmAckFlag, LONG nEventID, void* dwUser);

        [DllImport(dahuasdkdll, CallingConvention = CallingConvention.StdCall)]
        public static extern void CLIENT_SetDVRMessCallBackEx1(fMessCallBackEx1 cbMessage, void* dwUser);

        [DllImport(dahuasdkdll, CallingConvention = CallingConvention.StdCall)]
        public static extern BOOL CLIENT_StartListenEx(LLONG lLoginID);

        [DllImport(dahuasdkdll, CallingConvention = CallingConvention.StdCall)]
        public static extern BOOL CLIENT_StopListen(LLONG lLoginID);

        [DllImport(dahuasdkdll, CallingConvention = CallingConvention.StdCall)]
        public static extern void CLIENT_Cleanup();

        [DllImport(dahuasdkdll, CallingConvention = CallingConvention.StdCall)]
        public static extern BOOL CLIENT_QueryDeviceTime(LLONG lLoginID, StructExports.NET_TIME* pDeviceTime, int waittime = 1000);

        [DllImport(dahuasdkdll, CallingConvention = CallingConvention.StdCall)]
        public static extern BOOL CLIENT_SetupDeviceTime(LLONG lLoginID, StructExports.NET_TIME* pDeviceTime);

        [DllImport(dahuasdkdll, CallingConvention = CallingConvention.StdCall)]
        public static extern int CLIENT_FindNextRecord(StructExports.NET_IN_FIND_NEXT_RECORD_PARAM* pInParam,
            StructExports.NET_OUT_FIND_NEXT_RECORD_PARAM* pOutParam, int waittime = 1000);

        [DllImport(dahuasdkdll, CallingConvention = CallingConvention.StdCall)]
        public static extern DWORD CLIENT_GetLastError();

        [DllImport(dahuasdkdll, CallingConvention = CallingConvention.StdCall)]
        public static extern BOOL CLIENT_QueryRecordCount(StructExports.NET_IN_QUEYT_RECORD_COUNT_PARAM* pInParam,
            StructExports.NET_OUT_QUEYT_RECORD_COUNT_PARAM* pOutParam, int waittime = 1000);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void fSearchDevicesCB(void* vpDevNetInfo, void* pUserData);

        [DllImport(dahuasdkdll, CallingConvention = CallingConvention.StdCall)]
        public static extern LLONG CLIENT_StartSearchDevices(fSearchDevicesCB cbSearchDevices, void* pUserData, byte* szLocalIp = null);

        [DllImport(dahuasdkdll, CallingConvention = CallingConvention.StdCall)]
        public static extern BOOL CLIENT_StopSearchDevices(LLONG lSearchHandle);

        [DllImport(dahuasdkdll, CallingConvention = CallingConvention.StdCall)]
        public static extern BOOL CLIENT_GetNewDevConfig(LLONG lLoginID, byte* szCommand, int nChannelID, byte* szOutBuffer,
            DWORD dwOutBufferSize, int* error, int waittime = 500);

        [DllImport(dahuaconfigdll, CallingConvention = CallingConvention.StdCall)]
        public static extern BOOL CLIENT_ParseData(byte* szCommand, byte* szInBuffer, void* lpOutBuffer, DWORD dwOutBufferSize, void* pReserved);

        [DllImport(dahuaconfigdll, CallingConvention = CallingConvention.StdCall)]
        public static extern BOOL CLIENT_PacketData(byte* szCommand, void* lpInBuffer, DWORD dwInBufferSize, byte* szOutBuffer, DWORD dwOutBufferSize);

        [DllImport(dahuasdkdll, CallingConvention = CallingConvention.StdCall)]
        public static extern BOOL CLIENT_SetNewDevConfig(LLONG lLoginID, byte* szCommand, int nChannelID, byte* szInBuffer, DWORD dwInBufferSize, int* error, int* restart, int waittime = 500);

        [DllImport(dahuasdkdll, CallingConvention = CallingConvention.StdCall)]
        public static extern BOOL CLIENT_QueryDevState(LLONG lLoginID, int nType, byte* pBuf, int nBufLen, int* pRetLen, int waittime = 1000);

        [DllImport(dahuasdkdll, CallingConvention = CallingConvention.StdCall)]
        public static extern BOOL CLIENT_OperateUserInfoNew(LLONG lLoginID, int nOperateType, void* opParam, void* subParam, void* pRetParam, int waittime = 1000);

        [DllImport(dahuasdkdll, CallingConvention = CallingConvention.StdCall)]
        public static extern BOOL CLIENT_FindRecord(LLONG lLoginID, StructExports.NET_IN_FIND_RECORD_PARAM* pInParam,
            StructExports.NET_OUT_FIND_RECORD_PARAM* pOutParam, int waittime = 1000);

        [DllImport(dahuasdkdll, CallingConvention = CallingConvention.StdCall)]
        public static extern BOOL CLIENT_FindRecordClose(LLONG lFindHandle);
    }
}
