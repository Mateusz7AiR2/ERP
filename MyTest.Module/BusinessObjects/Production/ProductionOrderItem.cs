using DevExpress.Xpo;
using MyTest.Module.BusinessObjects.Core;
using MyTest.Module.BusinessObjects.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTest.Module.BusinessObjects.Production
{
    public class ProductionOrderItem: BaseClass
    {
        public ProductionOrderItem(Session session) : base(session)
        {
        }

        private Product.Product _product;
        public Product.Product Product
        { 
            get => _product;
            set => SetPropertyValue(nameof(Product), ref _product, value);
        }

        private string _subNumberProductionOrder;
        public string SubNumberProductionOrder
        {
            get => _subNumberProductionOrder;
            set => SetPropertyValue(nameof(SubNumberProductionOrder), ref _subNumberProductionOrder, value);
        }

    
          private ProductionOrder _productionOrders;
        [Association("ProductionOrder-ProductionOrderItem")]
       public ProductionOrder ProductionOrders
       {
            get { return _productionOrders; }
            set
            {
                SetPropertyValue("ProductionOrders", ref _productionOrders, value);
            }
        }
    }
}
