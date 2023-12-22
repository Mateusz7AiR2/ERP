using DevExpress.Data.Filtering;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using MyTest.Module.BusinessObjects.Core;
using MyTest.Module.BusinessObjects.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTest.Module.BusinessObjects.Production
{
    public class ProductionOrder : BaseClass
    {
        public ProductionOrder(Session session)
            : base(session)
        {
        }

        private Person _projectManager;
        public Person ProjectManager
        {
            get { return _projectManager; }
            set { SetPropertyValue(nameof(ProjectManager), ref _projectManager, value); }
        }
        public XPCollection<Person> GetPersonsWithEmployeeRoleDesigner()
        {
            CriteriaOperator criteria = new BinaryOperator("EmployeeRole", EmployeeRole.Designer);
            XPCollection<Person> persons = new XPCollection<Person>(Session, criteria);
            return persons;
        }
        public void SetProjectManagerWithEmployeeRoleDesigner()
        {
            XPCollection<Person> designers = GetPersonsWithEmployeeRoleDesigner();
            ProjectManager = designers.FirstOrDefault();
        }
    }
}
