using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using MyTest.Module.BusinessObjects.Core;
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
        [ImmediatePostData]
        public ProductionTask ProductionTask
        {
            get => _ProductionTask;
            set
            {
                if (SetPropertyValue(nameof(ProductionTask), ref _ProductionTask, value) && !IsDeleted && !IsSaving && !IsLoading)
                    Index = ProductionTask?.ProductionTaskItems?.Count * 10 ?? 0;
            }
        }

        bool _Completed;
        public bool Completed
        {
            get => _Completed;
            set => SetPropertyValue(nameof(Completed), ref _Completed, value);
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }
    }
}
