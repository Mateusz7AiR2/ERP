using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
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
    public class ProductionTaskItem : BaseClass
    {
        public ProductionTaskItem(Session session) : base(session)
        {
        }
        int _Index;
        public int Index
        {
            get => _Index;
            set => SetPropertyValue(nameof(Index), ref _Index, value);
        }

        ProductionTaskKind _ProductionTaskKind;
        [RuleRequiredField]
        public ProductionTaskKind ProductionTaskKind
        {
            get => _ProductionTaskKind;
            set => SetPropertyValue(nameof(ProductionTaskKind), ref _ProductionTaskKind, value);
        }

        ProductionTask _ProductionTask;
        [Association]
        public ProductionTask ProductionTask
        {
            get => _ProductionTask;
            set
            {
                if (SetPropertyValue(nameof(ProductionTask), ref _ProductionTask, value) && !IsDeleted && !IsSaving && !IsLoading)
                    Index = ProductionTask?.ProductionTaskItems?.Count * 10 ?? 0;
            }
        }
        ProductCategory _ChronologyItem;
        [Association]
        public ProductCategory ChronologyItem
        {
            get => _ChronologyItem;
            set => SetPropertyValue(nameof(ChronologyItem), ref _ChronologyItem, value);
        }

        [Association]
        public XPCollection<Element> Elements => GetCollection<Element>(nameof(Elements));

        float _Progres;
        public float Progres
        {
            get => _Progres;
            set => SetPropertyValue(nameof(Progres), ref _Progres, value);
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }
    }
}
