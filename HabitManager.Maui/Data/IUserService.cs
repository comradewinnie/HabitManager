namespace HabitManager.Maui.Data
{
    public interface IUserService
    {
        Task<string> GetCurrentUserIdAsync();
    }
}
