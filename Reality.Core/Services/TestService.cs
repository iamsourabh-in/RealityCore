namespace Reality.Core.Services
{
    public class TestService : ITestService
    {
        private int UsedCounter { set; get; }

        public TestService()
        {
            UsedCounter = 0;
        }
        public string GreetUser(string username)
        {
            IncrementUsedCountByOne();
            return $"Hello {username}  \n";
        }

        public void IncrementUsedCountByOne()
        {
            UsedCounter++;
        }
        public int GetCount()
        {
            return UsedCounter;
        }

    }
}
