using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;
using DevExpress.XtraCharts.Native;
using MyTest.Module.BusinessObjects.Product;
using MyTest.Module.BusinessObjects.Production;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTest.Module.Controllers
{
    public class ProductionOrderListViewController : ViewController<ListView>
    {
        SimpleAction createProductionTasksAction;
        public ProductionOrderListViewController()
        {
            TargetViewId = "ProductionOrder_ListView;";
            createProductionTasksAction = new(this, "createProductionTasksAction", PredefinedCategory.Edit)
            {
                Caption = "Utwórz zadania prodykcyjne",
                ImageName = "AddParagraphToTableOfContents",
                SelectionDependencyType = SelectionDependencyType.RequireSingleObject,
            };
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            createProductionTasksAction.Execute += CreateProductionTasksAction_Execute; ;
            RegisterActions(createProductionTasksAction);
        }

        private void CreateProductionTasksAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if (e.CurrentObject is ProductionOrder productionOrder)
            {
                var space = ObjectSpace.CreateNestedObjectSpace();
                foreach (var item in productionOrder.ProductionOrderItems)
                    CreateProductionTask(item, space);
                space.CommitChanges();
            }
        }
        void CreateProductionTask(ProductionOrderItem item, IObjectSpace space)
        {
            ProductionTask task = space.CreateObject<ProductionTask>();
            task.ProductionOrderItem = space.GetObject(item);
            task.Product = space.GetObject(item.Product);
            foreach (var chronologyItem in item.Product.ProductCategory.ChronologyItems)
            {
                ProductionTaskItem productionTaskItem = space.CreateObject<ProductionTaskItem>();
                productionTaskItem.ProductionTask = task;
                productionTaskItem.ProductionTaskKind = space.GetObject(chronologyItem.ProductionTaskKind);
                foreach (var element in chronologyItem.Elements)
                {
                    Element newElement = space.CreateObject<Element>();
                    newElement.ElementName = element.ElementName;
                    newElement.ProductionTaskItem = productionTaskItem;
                }
            }
        }

        protected override void OnDeactivated()
        {
            base.OnDeactivated();
            if (createProductionTasksAction != null)
                createProductionTasksAction.Execute -= CreateProductionTasksAction_Execute;
        }
    }
}
