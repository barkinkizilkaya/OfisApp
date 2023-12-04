using OfisApp.Models;

namespace OfisApp.Interfaces
{
    public interface IUserService
    {
        Task<List<DeviceRecord>> GetUserData(long cardNumber);

    }
}
