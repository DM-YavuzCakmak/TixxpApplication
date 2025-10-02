namespace Tixxp.Core.Utilities.Enums.ProductSaleStatusEnum
{
    public enum ProductSaleStatusEnum
    {
        /// <summary>
        /// Sepet oluşturuldu, henüz ödeme alınmadı.
        /// </summary>
        Pending = 1,

        /// <summary>
        /// Ödeme bekleniyor (3D Secure yönlendirmesi vs. olabilir).
        /// </summary>
        AwaitingPayment = 2,

        /// <summary>
        /// Ödeme başarıyla tamamlandı.
        /// </summary>
        Paid = 3,

        /// <summary>
        /// Satış tamamlandı ve kapatıldı (ürün teslim edildi / süreç bitti).
        /// </summary>
        Completed = 4,

        /// <summary>
        /// Kullanıcı ya da sistem tarafından iptal edildi.
        /// </summary>
        Cancelled = 5,

        /// <summary>
        /// Ödeme başarısız oldu.
        /// </summary>
        Failed = 6,

        /// <summary>
        /// İade / geri ödeme yapıldı.
        /// </summary>
        Refunded = 7
    }
}

