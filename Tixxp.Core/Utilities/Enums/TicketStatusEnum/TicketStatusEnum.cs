namespace Tixxp.Core.Utilities.Enums.TicketStatusEnum;
public enum TicketStatusEnum
{
    Active = 1,    // Ödendi, geçerlilik süresi başladı (henüz kullanılmadı)
    CheckedIn = 2, // Kullanıcı giriş yaptı (turnike vs.)
    CheckedOut = 3,// Kullanıcı çıkış yaptı
    Expired = 4,   // Geçerlilik süresi doldu, kullanılmadı
    Cancelled = 5, // Kullanıcı ya da sistem tarafından iptal edildi
    Refunded = 6 // İade edildi
}
