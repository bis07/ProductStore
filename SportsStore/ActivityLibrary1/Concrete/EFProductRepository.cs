using System.Collections.Generic;
using ActivityLibrary1.Abstract;
using ActivityLibrary1.Entities;

namespace ActivityLibrary1.Concrete
{
    public class EFProductRepository : IProductRepository
    {
        private EFDBContext context = new EFDBContext();
        public IEnumerable<Product> Products { get { return context.Products; }  }
        public void SaveProduct(Product product)
        {
            if (product.ProductId==0)
            {
                context.Products.Add(product);
            }
            else
            {
                Product dbEntry = context.Products.Find(product.ProductId);
                if (dbEntry!= null)
                {
                    dbEntry.Name = product.Name;
                    dbEntry.Price = product.Price;
                    dbEntry.Category = product.Category;
                    dbEntry.Description = product.Description;
                    dbEntry.ImageData = product.ImageData;
                    dbEntry.ImageMimeType = product.ImageMimeType;
                }
            }
            context.SaveChanges();
        }

        public Product DeleteProduct(int productId)
        {
            Product dbEntry = context.Products.Find(productId);
            if (dbEntry!= null)
            {
                context.Products.Remove(dbEntry);
                context.SaveChanges();
            }
            return dbEntry;
        }
    }
}
