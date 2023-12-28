using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using MyTest.Module.BusinessObjects.Core;
using MyTest.Module.BusinessObjects.CRM;
using MyTest.Module.BusinessObjects.Enum;
using MyTest.Module.BusinessObjects.Product;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Person = MyTest.Module.BusinessObjects.CRM.Person;
using Product = MyTest.Module.BusinessObjects.Product.Product;

namespace MyTest.Module.BusinessObjects.Production
{
    public class ProductionOrder : BaseClass
    {
        public ProductionOrder(Session session)
            : base(session)
        {
        }
        private Entity _entity;
        public Entity Entity
        {
            get { return _entity; }
            set { SetPropertyValue(nameof(ProjectManager), ref _entity, value); }
        }

        private Person _projectManager;
        [DataSourceProperty(nameof(AvailableDesigners))]
        public Person ProjectManager
        {
            get { return _projectManager; }
            set { SetPropertyValue(nameof(ProjectManager), ref _projectManager, value); }
        }
        public IList<Person> AvailableDesigners => GetAvailableDesigners();
        private IList<Person> GetAvailableDesigners()
        {
            return new XPCollection<Person>(base.Session).Where(x => x.EmployeeRole == EmployeeRole.Designer).ToList();
        }

        private Priority _priority;
        public Priority Priority
        {
            get { return _priority; }
            set { SetPropertyValue(nameof(Priority), ref _priority, value); }
        }

        private DateTime _plannedFinishDate;
        public DateTime PlannedFinishDate
        {
            get { return _plannedFinishDate;}
            set { SetPropertyValue(nameof(PlannedFinishDate), ref _plannedFinishDate, value);}
        }

        private ProductionState _productionState;
        public ProductionState ProductionState
        {
            get { return _productionState; }
            set { SetPropertyValue(nameof(ProductionState), ref _productionState, value); }
        }

        private string _nameProductionOrder;
        public string NameProductionOrder
        {
            get { return _nameProductionOrder; }
            set { SetPropertyValue(nameof(NameProductionOrder), ref _nameProductionOrder, value); }
        }

        [Association("ProductionOrder-ProductionOrderItem")]
        public XPCollection<ProductionOrderItem> ProductionOrderItems
        {
            get
            {
                return GetCollection<ProductionOrderItem>(nameof(ProductionOrderItems));
            }
        }
    }
}
