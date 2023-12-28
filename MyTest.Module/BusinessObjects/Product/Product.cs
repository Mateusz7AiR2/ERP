using DevExpress.CodeParser;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.Xpo;
using MyTest.Module.BusinessObjects.Core;
using MyTest.Module.BusinessObjects.CRM;
using MyTest.Module.BusinessObjects.Production;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DevExpress.DataProcessing.InMemoryDataProcessor.AddSurrogateOperationAlgorithm;

namespace MyTest.Module.BusinessObjects.Product
{
    public class Product : BaseClass
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

        private ProductCategory _productCategory;
        public ProductCategory ProductCategory
        {
            get => _productCategory;
            set
            {
                if (SetPropertyValue(nameof(ProductCategory), ref _productCategory, value) && !IsLoading && !IsSaving && !IsDeleted)
                {
                    SteelCategory = value?.SteelsCategory.ToString();
                }

            }
        }

        private string _steelCategory;
        public string SteelCategory
        {
            get { return _steelCategory; }
            set { SetPropertyValue(nameof(SteelCategory), ref _steelCategory, value); }
        }

        private bool _linework;
        public bool Linework
        {
            get
            {
                return _linework;
            }
            set
            {
                SetPropertyValue("Linework", ref _linework, value);
            }


        }
    }
}
