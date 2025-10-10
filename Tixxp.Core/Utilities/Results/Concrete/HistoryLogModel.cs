namespace Tixxp.Core.Utilities.Results.Concrete;
public class HistoryLogModel
{
    public string? EntityId { get; set; } // İlgili entity'nin ID'si
    public Guid? CorrelationId { get; set; } // İşlem takip kimliği
    public string? ProjectName { get; set; } // Proje adı
    public string? MethodName { get; set; } // Çağrılan metod adı
    public string? EntityName { get; set; } // Entity adı
    public bool IsFirst { get; set; } = false; // İlk kayıt kontrolü
    public string? OldData { get; set; } // Eski veriler
    public string? NewData { get; set; } // Yeni veriler
    public string? UserInformation { get; set; } // Kullanıcı bilgisi
    public List<ColumnChange>? ColumnChanges { get; set; } = new(); // Kolon değişiklikleri listesi
    public DateTime? CreatedDate { get; set; } // Kayıt oluşturulma tarihi
    public DateTime? UpdateDate { get; set; } // Güncellenme tarihi
    public DateTime? RequestDate { get; set; } // İstek tarihi
}
