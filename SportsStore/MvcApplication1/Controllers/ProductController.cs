using System.Linq;
using System.Web.Mvc;
using ActivityLibrary1.Abstract;
using ActivityLibrary1.Entities;
using SportsStore.Models;

namespace SportsStore.Controllers
{
    public class ProductController : Controller
    {
        private IProductRepository repository;
        public int pageSize = 4;

        public ProductController(IProductRepository productRepository)
        {
            repository = productRepository;
        }

        public ViewResult List(string category, int page = 1)
        {
            ProductsListViewModel model = new ProductsListViewModel
            {
                Products = repository.Products
                .Where(x=> category==null || x.Category==category)
                .OrderBy(x => x.ProductId)
                .Skip((page - 1)*pageSize)
                .Take(pageSize),
                PagingInfo =
                    new PagingInfo
                    {
                        CurrentPage = page,
                        ItemsPerPage = pageSize,
                        TotalItems =category ==null ? repository.Products.Count()
                        :repository.Products.Where(x=>x.Category==category).Count()
                    },
                    CurrentCategory = category
            };
            return View(model);
        }

        public FileContentResult GetImage(int productId)
        {
            Product prod = repository.Products.FirstOrDefault(x => x.ProductId == productId);
            if (prod!=null)
            {
                return File(prod.ImageData, prod.ImageMimeType);
            }
            else
            {
                return null;
            }
        }

    }
}
