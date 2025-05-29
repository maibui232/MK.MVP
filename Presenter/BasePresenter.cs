namespace MK.MVP
{
    using System;
    using Cysharp.Threading.Tasks;

    public abstract class BasePresenter<TView, TModel> : IPresenter where TView : BaseView, IView
    {
#region IViewModel implementation

        void IPresenter.Open() => ((IPresenter)this).OpenAsync().Forget();

        void IPresenter.Close() => ((IPresenter)this).CloseAsync().Forget();

        UniTask IPresenter.OpenAsync() => this.View.OpenAsync().ContinueWith(this.OnOpen);

        UniTask IPresenter.CloseAsync() => this.View.CloseAsync().ContinueWith(this.OnClose);

        void IPresenter.BindView(IView view) => this.BindView((TView)view);

        void IPresenter.BindModel(object model) => this.BindModel((TModel)model);

        void IDisposable.Dispose() => this.OnDispose();

#endregion

#region Abstract

        protected TView View { private set; get; }

        protected TModel Model { private set; get; }

        protected virtual void BindView(TView view) => this.View = view;

        protected virtual void BindModel(TModel model) => this.Model = model;

        protected abstract void OnOpen();

        protected abstract void OnClose();

        protected virtual void OnDispose()
        {
        }

#endregion
    }
}