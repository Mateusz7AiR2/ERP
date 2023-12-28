using DevExpress.ExpressApp.StateMachine.Xpo;
using DevExpress.ExpressApp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.DashboardCommon.Native;

namespace MyTest.Module.BusinessObjects.Core.Interfaces
{
    public interface ITransitions
    {
        int Oid { get; }

        BindingList<BaseClass> ObjectStateChangedNotifications { get; }

        string TryExecuteTransition(ITransitions obj, CancelEventArgs e);

        void ExecuteTransition(XpoState sourceState, XpoState targetState, View view, IObjectSpace space);

        void SetState(object state);

        bool GetShowWaitForm()
        {
            return false;
        }

        string GetWaitFormDescription(XpoState targetState)
        {
            return "Proszę czekać";
        }

        string GetWaitFormCaption(XpoState targetState)
        {
            return "...";
        }

        bool CanChangeState(int sourceStateOid);

        void OnObjectSaving()
        {
        }
    }
}
