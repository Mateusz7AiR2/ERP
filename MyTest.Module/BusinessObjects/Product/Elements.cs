using DevExpress.Xpo;
using MyTest.Module.BusinessObjects.Core;
using MyTest.Module.BusinessObjects.Production;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTest.Module.BusinessObjects.Product
{
    public class Elements : BaseClass
    {
        public Elements(Session session)
        : base(session)
        {
        }

        private string _elementName;
        public string ElementName
        {
            get
            {
                return _elementName;
            }
            set
            {
                SetPropertyValue(nameof(ElementName), ref _elementName, value);
            }
        }

        private ProductCategory _productCategory;
        [Association("ProductCategory-Elements")]
        public ProductCategory ProductCategory
        {
            get { return _productCategory; }
            set
            {
                SetPropertyValue(nameof(ProductCategory), ref _productCategory, value);
            }
        }
    }

}
