using System.ComponentModel.DataAnnotations.Schema;
using Tixxp.Entities.Base;

namespace Tixxp.Entities.Counter;

[Table("Counter")]
public class CounterEntity : BaseEntity
{
    [Column("IsOKCIntegrated")]
    public bool IsOkcIntegrated { get; set; }

    [Column("OkcBrand")]
    public int OkcBrand { get; set; }

    [Column("OkcFiscalSerialNumber")]
    public string OkcFiscalSerialNumber { get; set; }

    [Column("OkcPassword")]
    public string OkcPassword { get; set; }

    [Column("TsmOpen")]
    public bool TsmOpen { get; set; }

    [Column("GmpOpen")]
    public bool GmpOpen { get; set; }

    [Column("IpAddress")]
    public string IpAddress { get; set; }

    [Column("Port")]
    public int Port { get; set; }

    [Column("Version")]
    public string Version { get; set; }

    [Column("OtpVerification")]
    public bool OtpVerification { get; set; }
}
