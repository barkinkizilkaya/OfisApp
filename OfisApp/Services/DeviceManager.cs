using Humanizer;
using Microsoft.Extensions.Caching.Memory;
using OfisApp.Interfaces;
using OfisApp.Models;
using panel_mx;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
            return GenerateSampleDeviceRecords();

            return  _memoryCache.GetOrCreate("DeviceData",  entry =>
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
                return  records;
            });
           
        }

        public List<DeviceRecord> GenerateSampleDeviceRecords()
        {
            List<DeviceRecord> deviceRecords = new List<DeviceRecord>();

            // Generate 100 sample records
            for (int i = 0; i < 100; i++)
            {
                DateTime enterDate = DateTime.Now.AddDays(-i); // Adjust as needed for your scenario
                DateTime exitDate = enterDate.AddHours(8); // Assuming an 8-hour workday

                // Some records can have multiple entries for the same day and same user
                int numberOfEntries = i % 5 + 1; // Vary the number of entries

                for (int j = 0; j < numberOfEntries; j++)
                {
                    DeviceRecord record = new DeviceRecord
                    {
                        Id = i * 100 + j + 1, // Unique ID for each record
                        EnterDate = enterDate,
                        ExitDate = exitDate,
                        CardNumber = (i < 5) ? 123456 : 1000 + i, // Card number 123456 for the first 5 records, others vary
                        CardOwner = $"User{i + 1}", // Sample user name
                        Year = enterDate.Year,
                        Month = enterDate.Month
                    };

                    deviceRecords.Add(record);
                }
            }

            // Duplicate some records with different hours
            foreach (var record in deviceRecords.Where(r => r.CardNumber == 123456).ToList())
            {
                for (int k = 0; k < 3; k++)
                {
                    DeviceRecord duplicateRecord = new DeviceRecord
                    {
                        Id = record.Id + 1000 + k, // Unique ID for each duplicated record
                        EnterDate = record.EnterDate.AddHours(k + 1), // Different hours for duplicates
                        ExitDate = record.ExitDate.AddHours(k + 1),
                        CardNumber = record.CardNumber,
                        CardOwner = record.CardOwner,
                        Year = record.Year,
                        Month = record.Month
                    };

                    deviceRecords.Add(duplicateRecord);
                }
            }

            return deviceRecords;
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
