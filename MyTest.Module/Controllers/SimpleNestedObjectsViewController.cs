using DevExpress.ExpressApp;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTest.Module.Controllers
{
    public class SimpleNestedObjectsViewController : ViewController<ListView>
    {
        private ListViewProcessCurrentObjectController listViewProcessCurrentObjectController;
        private NewObjectViewController newObjectViewController;
        public event EventHandler<ObjectCreatedEventArgs> ObjectCreated;

        public SimpleNestedObjectsViewController()
        {
            base.TargetViewId = TargetViewIds;
        }
        protected virtual string TargetViewIds => "ProductionTask_ProductionTaskItems_ListView;" +
            "ProductionOrder_ProductionOrderItems_ListView;" +
            "ProductCategory_Elements_ListView;";

        protected override void OnActivated()
        {
            base.OnActivated();
            listViewProcessCurrentObjectController = base.Frame.GetController<ListViewProcessCurrentObjectController>();
            if (listViewProcessCurrentObjectController != null)
            {
                listViewProcessCurrentObjectController.CustomHandleProcessSelectedItem += ListViewProcessCurrentObjectController_CustomHandleProcessSelectedItem;
            }

            newObjectViewController = base.Frame.GetController<NewObjectViewController>();
            if (newObjectViewController != null)
            {
                newObjectViewController.ObjectCreating += NewObjectViewController_ObjectCreating;
            }
        }
        private void ListViewProcessCurrentObjectController_CustomHandleProcessSelectedItem(object sender, HandledEventArgs e)
        {
            e.Handled = true;
        }

        protected virtual void NewObjectViewController_ObjectCreating(object sender, ObjectCreatingEventArgs e)
        {
            object obj = base.ObjectSpace.CreateObject(e.ObjectType);
            e.Cancel = true;
            e.ShowDetailView = false;
            base.View.CollectionSource.Add(obj);
            base.View.CurrentObject = obj;
            ObjectCreatedEventArgs e2 = new ObjectCreatedEventArgs(obj, base.ObjectSpace);
            this.ObjectCreated?.Invoke(this, e2);
            if (obj is PermissionPolicyNavigationPermissionObject permissionPolicyNavigationPermissionObject)
            {
                permissionPolicyNavigationPermissionObject.NavigateState = SecurityPermissionState.Allow;
            }
        }
    }
}
