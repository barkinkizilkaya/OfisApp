using OfisApp.Interfaces;
using OfisApp.Models;
using System.Runtime.CompilerServices;

namespace OfisApp.Services
{
    public class UserService : IUserService
    {
        private readonly IDeviceManager _deviceManager;

        public UserService(IDeviceManager deviceManager)
        {
            _deviceManager = deviceManager;
        }

        public async Task<List<DeviceRecord>> GetUserData(long cardNumber)
        {
            var currentMonth = DateTime.Now.Month;
            var currentYear = DateTime.Now.Year;

           
            var allDeviceData = await  _deviceManager.ReadDeviceData();

           
            var orderedData = allDeviceData.OrderBy(record => record.EnterDate).ToList();

            var filteredData = orderedData
         .Where(record => record.CardNumber == cardNumber &&
                          record.Month == currentMonth &&
                          record.Year == currentYear)
         .GroupBy(record => new
         {
             record.CardNumber,
             Date = new DateTime(record.Year, record.Month, record.EnterDate.Day)
         })
         .Select(group => group.OrderBy(record => record.EnterDate).First())
         .ToList();

            return filteredData;
        }
    }
}
