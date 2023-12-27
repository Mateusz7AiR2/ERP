using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DevExpress.DataProcessing.InMemoryDataProcessor.AddSurrogateOperationAlgorithm;

namespace MyTest.Module.BusinessObjects.Product
{
    public class Product : ProductCategory
    {
        public Product(Session session)
        : base(session)
        {
        }
        private string _productSizeName;
        public string ProductSizeName
        {
            get
            {
                return _productSizeName;
            }
            set
            {
                SetPropertyValue("ProductSizeName", ref _productSizeName, value);
            }
        }
        private string _description;
        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                SetPropertyValue("Description", ref _description, value);
            }
        }



    }
}
