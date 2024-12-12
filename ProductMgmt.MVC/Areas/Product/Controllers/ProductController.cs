using Microsoft.AspNetCore.Mvc;
using ProductMgmt.MVC.ViewModels;

namespace ProductMgmt.MVC.Areas.Product.Controllers
{
    [Area("Product")]
    public class ProductController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ProductController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient("ProductMgmtApiConnection");
            var productList = await client.GetFromJsonAsync<List<ProductVM>>("Product/GetAll");
            return View(productList);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ProductVM product = new();
            return PartialView("_ProductModalPartial", product);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductVM product)
        {
            var client = _httpClientFactory.CreateClient("ProductMgmtApiConnection");
            await client.PostAsJsonAsync<ProductVM>("Product/Create", product);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int Id)
        {
            if (Id == 0) return View();
            var client = _httpClientFactory.CreateClient("ProductMgmtApiConnection");
            var product = await client.GetFromJsonAsync<ProductVM>("Product/GetById/?Id=" + Id);
            return PartialView("_EditProductModalPartial", product);
        }

        [HttpPost]
        public async Task<IActionResult> Update(ProductVM product)
        {
            if (product.Id == 0) return View();
            var client = _httpClientFactory.CreateClient("ProductMgmtApiConnection");
            await client.PutAsJsonAsync<ProductVM>("Product/Update/", product);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int Id)
        {
            if (Id == 0) return View();
            var client = _httpClientFactory.CreateClient("ProductMgmtApiConnection");
            var product = await client.GetFromJsonAsync<ProductVM>("Product/GetById/?Id=" + Id);
            return PartialView("_DeleteProductModalPartial", product);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(ProductVM product)
        {
            if (product.Id == 0) return View();
            var client = _httpClientFactory.CreateClient("ProductMgmtApiConnection");
            var productList = await client.DeleteAsync("Product/Delete?Id=" + product.Id);
            return RedirectToAction("Index");
        }

    }
}
