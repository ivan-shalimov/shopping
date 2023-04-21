namespace Shopping.SeriGraylog
{
    public class LoggerSettings
    {
        public string MinimumLevel { get; init; }

        public string HostnameOrAddress { get; init; }

        public int Port { get; init; }

        public string TransportType { get; set; }
    }
}