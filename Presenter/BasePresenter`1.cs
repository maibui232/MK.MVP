namespace MK.MVP
{
    using System;
    using Cysharp.Threading.Tasks;

    public abstract class BasePresenter<TView> : BasePresenter, IPresenter where TView : IView
    {
        void IPresenter.Open() => ((IPresenter)this).OpenAsync().Forget();

        void IPresenter.Close() => ((IPresenter)this).CloseAsync().Forget();

        UniTask IPresenter.OpenAsync() => this.View.OpenAsync().ContinueWith(this.OnOpen);

        UniTask IPresenter.CloseAsync() => this.View.CloseAsync().ContinueWith(this.OnClose);

        void IPresenter.BindView(IView view)
        {
            this.View = (TView)view;
            this.OnBindView();
        }

        void IDisposable.Dispose() => this.OnDispose();

#region Abstract

        protected TView View { get; private set; }

        protected virtual void OnBindView()
        {
        }

#endregion
    }
}