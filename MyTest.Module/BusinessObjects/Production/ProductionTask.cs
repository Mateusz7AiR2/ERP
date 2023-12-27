using DevExpress.Xpo;
using MyTest.Module.BusinessObjects.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTest.Module.BusinessObjects.Production
{
    [DefaultProperty(nameof(Symbol))]
    public class ProductionTask : BaseClass
    {
        public ProductionTask(Session session) : base(session)
        {
        }

        ProductionOrder _ProductionOrder;
        public ProductionOrder ProductionOrder
        {
            get => _ProductionOrder;
            set => SetPropertyValue(nameof(ProductionOrder), ref _ProductionOrder, value);
        }
        string _Symbol;
        public string Symbol
        {
            get => _Symbol;
            set => SetPropertyValue(nameof(Symbol), ref _Symbol, value);
        }

        string _State;
        public string State
        {
            get => _State;
            set => SetPropertyValue(nameof(State), ref _State, value);
        }

        float _Progres;
        public float Progres
        {
            get => _Progres;
            set => SetPropertyValue(nameof(Progres), ref _Progres, value);
        }

        [Association]
        public XPCollection<ProductionTaskItem> ProductionTaskItems => GetCollection<ProductionTaskItem>(nameof(ProductionTaskItems));

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            State = "Nie rozpoczęto";
        }
    }
}
