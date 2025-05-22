using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Tixxp.Business.Services.Abstract.Product;
using Tixxp.Entities.Product;

namespace Tixxp.WebApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        public IActionResult Index()
        {
            var result = _productService.GetAll();
            if (result.Success)
            {
                return View(result.Data);
            }
            return View(new List<ProductEntity>());
        }

        [HttpGet]
        public IActionResult GetById(long id)
        {
            var result = _productService.GetById(id);
            if (result.Success)
                return Json(result.Data);
            return NotFound();
        }

        [HttpPost]
        public IActionResult Save(ProductEntity model)
        {
            if (model.Id > 0)
            {
                model.CreatedBy = 6;
                model.Created_Date = DateTime.Now;
                model.UpdatedBy = 6;
                model.Updated_Date = DateTime.Now;
                return Json(_productService.Update(model));

            }
            else
            {
                model.CreatedBy = 6;
                model.Created_Date = DateTime.Now;
                return Json(_productService.Add(model));

            }
        }

        [HttpPost]
        public IActionResult Delete(long id)
        {
            var entity = _productService.GetById(id).Data;
            entity.IsDeleted = true;
            return Json(_productService.Update(entity));
        }
    }
}