using System.Collections.Generic;
using System.Linq;

namespace ActivityLibrary1.Entities
{
    public class Cart
    {
        private List<CartLine> lineCollection = new List<CartLine>();

        public void AddItem(Product product, int qantity)
        {
            CartLine line = lineCollection.Where(x => x.Product.ProductId == product.ProductId)
                .FirstOrDefault();
            if (line == null)
            {
                lineCollection.Add(new CartLine{Product = product,Quantity = qantity});
            }
            else
            {
                line.Quantity += qantity;
            }
        }

        public void RemoveLine(Product product)
        {
            lineCollection.RemoveAll(x => x.Product.ProductId == product.ProductId);
        }

        public decimal ComputeToValue()
        {
            return lineCollection.Sum(x => x.Product.Price*x.Quantity);
        }

        public void Clear()
        {
            lineCollection.Clear();
        }
        public IEnumerable<CartLine> Lines { get { return lineCollection; } } 
    }

    public class CartLine
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }   
    }
}
