namespace HabitManager.Maui.Data
{
    public class TestUserService : IUserService
    {
        private readonly string _testUserId = "demo-user-1";

        public Task<string> GetCurrentUserIdAsync()
        {
            return Task.FromResult(_testUserId);
        }
    }
}
