namespace hefesto.admin.Services
{
    public interface IChangePasswordService
    {
        bool ValidatePassword(string login, string senha);
    }
}