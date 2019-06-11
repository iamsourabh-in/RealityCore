namespace AWS.Lambda.Environment
{
    public class EnvironmentService : IEnvironmentService
    {
        public EnvironmentService()
        {
            EnvironmentName = System.Environment.GetEnvironmentVariable("LAMBDA_ENV") ?? "Production";  // Constants.Environments.Production;
        }

        public string EnvironmentName { get; set; }
    }
}
