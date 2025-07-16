namespace Tixxp.Core.Utilities.Enums.ReservationStatusEnum;

public enum ReservationStatusEnum
{
    Pending = 1,             // Yeni rezervasyon, henüz işlem görmedi
    Processing = 2,          // Ödeme bekliyor ya da işleniyor
    Confirmed = 3,           // Ödeme alındı ve onaylandı
    TicketsPrinted = 4,      // Biletler basıldı (fiziksel satışlarda kullanışlı)
    Completed = 5,           // Rezervasyon kullanıldı (etkinliğe katıldı)
    CancelledByUser = 6,     // Kullanıcı iptal etti
    CancelledBySystem = 7,   // Sistemsel veya manuel iptal (yetkili)
    OnHold = 8,              // Geçici beklemede (belge/ödeme vs. bekleniyor)
    Expired = 9              // Süresi doldu, işleme alınmadı
}
