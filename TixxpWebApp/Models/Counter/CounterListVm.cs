namespace Tixxp.WebApp.Models.Counter
{
    public class CounterListVm
    {
        public long Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; } // dil bazlı isim
        public string? OkcFiscalSerialNumber { get; set; }
        public string? IpAddress { get; set; }
        public int? Port { get; set; }
        public string? Version { get; set; }
        public bool IsOkcIntegrated { get; set; }
        public bool TsmOpen { get; set; }
        public bool GmpOpen { get; set; }
        public int OkcBrand { get; set; }
        public string OkcPassword { get; set; }
        public bool OtpVerification { get; set; }
        public bool IsDeleted { get; set; }
    }
}
