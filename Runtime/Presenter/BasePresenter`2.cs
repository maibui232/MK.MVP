namespace MK.MVP
{
    using System;

    public abstract class BasePresenter<TView, TModel> : BasePresenter<TView>, IPresenter<TModel> where TView : BaseView, IView
    {
#region IViewModel implementation

        void IPresenter<TModel>.BindModel(TModel model)
        {
            this.Model = model;
            this.OnBindModel();
        }

        void IDisposable.Dispose() => this.OnDispose();

#endregion

#region Abstract

        protected TModel Model { private set; get; }

        protected virtual void OnBindModel()
        {
        }

#endregion
    }
}