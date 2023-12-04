using OfisApp.Models;

namespace OfisApp.Interfaces
{
    public interface IDeviceManager
    {
        Task<List<DeviceRecord>> ReadDeviceData();
    }
}
