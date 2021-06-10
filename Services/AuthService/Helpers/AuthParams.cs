namespace AuthService.Helpers
{
    public class AuthParams
    {
        public const int EXPIRE_MINUTES = 200; 
        public const int EXPIRE_MINUTES_REFRESH = 20;
        public const string PARAM_ISS = "HostAuth";
        public const string PARAM_AUD = "CashBook";
        public const string PARAM_SECRET_KEY = "Y2F0Y2hugdyydtYUEKHHU99jGhhY";
        public const int MAX_DEVICE_COUNT = 3;
    }
}
