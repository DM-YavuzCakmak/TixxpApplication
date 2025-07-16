using Tixxp.Entities.Product;
using Tixxp.Entities.ProductTranslation;

namespace Tixxp.WebApp.Models.Product
{
    public class ProductWithTranslationViewModel
    {
        public ProductEntity Product { get; set; }
        public List<ProductTranslationEntity> Translations { get; set; }
    }
}
