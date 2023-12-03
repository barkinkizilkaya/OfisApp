using Microsoft.Extensions.Caching.Memory;
using OfisApp.Interfaces;
using OfisApp.Models;
using panel_mx;
using System.Text;

namespace OfisApp.Services
{
    public class DeviceManager : IDeviceManager
    {
        const string DEVICE_IP = "172.16.128.201";
        const string PORT_NUMBER = "9747";
        List<long> EXCLUDED_CARD_NUMBERS = new List<long>()
         {
            538976288
         };

        private readonly IMemoryCache _memoryCache;

        public DeviceManager(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public async Task<List<DeviceRecord>> ReadDeviceData()
        {
            return _memoryCache.GetOrCreate("DeviceData", entry =>
            {
                entry.AbsoluteExpiration = DateTime.Now.AddHours(4);
                entry.SlidingExpiration = TimeSpan.FromDays(4);
                Class1 deviceSdk = new Class1();
                List<DeviceRecord> records = new List<DeviceRecord>();
                int dataSize = 17;
                var deviceData = deviceSdk.dataal(DEVICE_IP, PORT_NUMBER);

                for (int i = 0; i < deviceData.Length; i += dataSize)
                {
                    DeviceRecord record = new DeviceRecord();
                    record.CardNumber = BitConverter.ToInt32(deviceData, i + 6);
                    if (EXCLUDED_CARD_NUMBERS.Contains(record.CardNumber))
                    {
                        continue;
                    }
                    record.CardOwner = GetUserNameFromDeviceOrCache(record.CardNumber);
                    record.EnterDate = ExtractDateTime(deviceData, i);
                    record.Month = record.EnterDate.Month;
                    record.Year = record.EnterDate.Year;
                    records.Add(record);
                }
                return records;
            });
           
        }

        private static DateTime ExtractDateTime(byte[] deviceData, int i)
        {
            int year = deviceData[i + 2] + 2000;
            int month = deviceData[i + 1];
            int day = deviceData[i];
            int hour = deviceData[i + 3];
            int minute = deviceData[i + 4];
            int second = 0;
            return new DateTime(year, month, day, hour, minute, second);
        }

        private string GetUserNameFromDeviceOrCache(long cardNumber)
        {
            return _memoryCache.GetOrCreate($"User_{cardNumber}", entry =>
            {
                entry.AbsoluteExpiration = DateTime.Now.AddDays(1);
                entry.SlidingExpiration= TimeSpan.FromDays(1);

                Class1 deviceSdk = new Class1();
                string nameSurname = "";
                var deviceData = deviceSdk.kartoku(DEVICE_IP, PORT_NUMBER, cardNumber);
                int dataSize = 38;
                for (int i = 0; i < deviceData.Length; i += dataSize)
                {
                    long card = BitConverter.ToInt32(deviceData, i);
                    if (EXCLUDED_CARD_NUMBERS.Contains(card))
                    {
                        continue;
                    }
                    if (card == cardNumber)
                    {
                        nameSurname = Encoding.UTF8.GetString(deviceData, i + 4, 10) + Encoding.UTF8.GetString(deviceData, i + 14, 10);
                        break;
                    }
                }
                return nameSurname;
            });
           
        }
    }
}
