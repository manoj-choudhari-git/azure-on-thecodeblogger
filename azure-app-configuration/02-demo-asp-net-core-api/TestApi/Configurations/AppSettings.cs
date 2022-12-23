namespace TestApi.Configurations
{
    public class GreetingMessage
    {
        public string Message { get; set; } = null!;
    }

    public class AppSettings
    {
        public GreetingMessage GetEndpoint { get; set; } = null!;
        public GreetingMessage PostEndpoint { get; set; } = null!;
    }
}
