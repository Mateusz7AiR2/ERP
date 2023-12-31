﻿using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.StateMachine.Xpo;
using DevExpress.Xpo;
using MyTest.Module.BusinessObjects.Core;
using MyTest.Module.BusinessObjects.Core.Dictionaries;
using MyTest.Module.BusinessObjects.Core.Interfaces;
using MyTest.Module.BusinessObjects.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTest.Module.BusinessObjects.Production
{
    public class ProductionTask : BaseClass, ITransitions
    {
        public ProductionTask(Session session) : base(session)
        {
        }

        ProductionOrderItem _ProductionOrderItem;
        public ProductionOrderItem ProductionOrderItem
        {
            get => _ProductionOrderItem;
            set => SetPropertyValue(nameof(ProductionOrderItem), ref _ProductionOrderItem, value);
        }

        BaseDocumentState _DocumentState;
        public BaseDocumentState DocumentState
        {
            get => _DocumentState;
            set => SetPropertyValue(nameof(DocumentState), ref _DocumentState, value);
        }

        Product.Product _Product;
        public Product.Product Product
        {
            get => _Product;
            set => SetPropertyValue(nameof(Product), ref _Product, value);
        }

        float _Progres;
        public float Progres
        {
            get => _Progres;
            set => SetPropertyValue(nameof(Progres), ref _Progres, value);
        }

        [Association]
        public XPCollection<ProductionTaskItem> ProductionTaskItems => GetCollection<ProductionTaskItem>(nameof(ProductionTaskItems));

        BindingList<BaseClass> ITransitions.ObjectStateChangedNotifications => throw new NotImplementedException();

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            this.DocumentState = Session.FindObject<BaseDocumentState>(new BinaryOperator(nameof(BaseDocumentState.SystemDocumentState), SystemDocumentState.Created));
        }

        string ITransitions.TryExecuteTransition(ITransitions obj, CancelEventArgs e)
        {
            throw new NotImplementedException();
        }

        void ITransitions.ExecuteTransition(XpoState sourceState, XpoState targetState, View view, IObjectSpace space)
        {
            throw new NotImplementedException();
        }

        void ITransitions.SetState(object state)
        {
            throw new NotImplementedException();
        }

        bool ITransitions.CanChangeState(int sourceStateOid)
        {
            throw new NotImplementedException();
        }
    }
}
