﻿using DevExpress.Xpo;
using MyTest.Module.BusinessObjects.Core;
using MyTest.Module.BusinessObjects.CRM;
using MyTest.Module.BusinessObjects.Production;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTest.Module.BusinessObjects.Product
{
    public class ElementToProductionTask : BaseClass
    {
        public ElementToProductionTask(Session session)
        : base(session)
        {
        }

        private Element _elementName;
        public Element ElementName
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

        Person _Employee;
        public Person Employee
        {
            get => _Employee;
            set => SetPropertyValue(nameof(Employee), ref _Employee, value);
        }

        bool _Completed;
        public bool Completed
        {
            get => _Completed;
            set => SetPropertyValue(nameof(Completed), ref _Completed, value);
        }
        public void Complete() => _Completed = true; 

        ProductionTaskItem _ProductionTaskItem;
        [Association]
        public ProductionTaskItem ProductionTaskItem
        {
            get => _ProductionTaskItem;
            set => SetPropertyValue(nameof(ProductionTaskItem), ref _ProductionTaskItem, value);
        }
    }

}
