using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ActivityLibrary1.Entities;

namespace ActivityLibrary1.Abstract
{
    public interface IOrderProcessor
    {
        void ProcessorOrder(Cart cart, ShippingDetails shippingDetails);
    }
}
