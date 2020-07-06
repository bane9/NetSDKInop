using System;

namespace NetSDKInop
{
    public class DeviceConnectionFailedException : Exception
    {
        public DeviceConnectionFailedException()
        {

        }

        public DeviceConnectionFailedException(string message)
            : base(message)
        {

        }

        public DeviceConnectionFailedException(string message, DeviceUtil.LoginError loginError)
            : base(message)
        {
            this.loginError = loginError;
        }

        public DeviceUtil.LoginError loginError { get; private set; }
    }

    public class DeviceNotConnectedExpeption : Exception
    {
        public DeviceNotConnectedExpeption()
        {

        }
    }

    public class SdkNotInitalizedException : Exception
    {
        public SdkNotInitalizedException()
        {

        }
    }
}
