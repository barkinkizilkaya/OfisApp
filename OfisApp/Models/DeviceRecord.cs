namespace OfisApp.Models
{
    public class DeviceRecord
    {
        public int Id { get; set; }
        public DateTime EnterDate { get; set; }
        public DateTime ExitDate { get; set; }
        public long CardNumber { get; set; }
        public string? CardOwner { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
    }
}
