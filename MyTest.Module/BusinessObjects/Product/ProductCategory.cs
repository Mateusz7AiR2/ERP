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
    public class ProductCategory : SteelCategory
    {
        public ProductCategory(Session session)
        : base(session)
        {
        }
        string _ProductCategoryName;
        public string ProductCategoryName
        {
            get => _ProductCategoryName;
            set => SetPropertyValue(nameof(ProductCategoryName), ref _ProductCategoryName, value);
        }

        [Association]
        public XPCollection<ProductionTaskItem> ChronologyItems => GetCollection<ProductionTaskItem>(nameof(ChronologyItems));


    }
}
