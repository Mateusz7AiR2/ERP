using DevExpress.Xpo;
using MyTest.Module.BusinessObjects.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTest.Module.BusinessObjects.Product
{
    public class ProductCategory : SteelCategory
    {
        public ProductCategory(Session session)
        : base(session)
        {
        }
        private string _productCategoryName;
        public string ProductCategoryName
        {
            get
            {
                return _productCategoryName;
            }
            set
            {
                SetPropertyValue("ProductsCategoryName", ref _productCategoryName, value);
            }
        }



    }
}
