namespace MK.MVP
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Cysharp.Threading.Tasks;
    using MK.AssetsManager;
    using MK.DependencyInjection;
    using MK.Extensions;
    using MK.Log;
    using UnityEngine;
    using ILogger = MK.Log.ILogger;

    public sealed class ViewUIService : IViewUIService
    {
        private readonly IAssetsManager assetsManager;
        private readonly IResolver      resolver;
        private readonly ILogger        logger;

        public ViewUIService(IAssetsManager assetsManager, IResolver resolver, ILoggerManager logger)
        {
            this.assetsManager = assetsManager;
            this.resolver      = resolver;
            this.logger        = logger.GetLogger(this);
        }

        private readonly List<IPresenter>             viewStack       = new();
        private readonly Dictionary<Type, IView>      typeToView      = new();
        private readonly Dictionary<Type, IPresenter> typeToPresenter = new();

        private async UniTask<TView> GetViewAsync<TView>() where TView : BaseView
        {
            var type = typeof(TView);
            if (this.typeToView.TryGetValue(type, out var view))
            {
                return (TView)view;
            }

            var loadedView = (await this.assetsManager.LoadAsync<GameObject>(type.GetKey())).GetComponent<TView>();
            this.typeToView.Add(type, loadedView);

            return loadedView;
        }

        private TPresenter GetPresenter<TPresenter>() where TPresenter : IPresenter
        {
            var type = typeof(TPresenter);
            if (this.typeToPresenter.TryGetValue(type, out var presenter))
            {
                return (TPresenter)presenter;
            }

            var newPresenter = this.resolver.Instantiate<TPresenter>();
            this.typeToPresenter.Add(type, newPresenter);

            return newPresenter;
        }

        private async UniTask BindPresenter<TModel, TView, TPresenter>(TPresenter presenter, TModel model) where TPresenter : IPresenter where TView : BaseView
        {
            var view = await this.GetViewAsync<TView>();

            var presenterInterface = (IPresenter<TModel>)presenter;
            presenterInterface.BindModel(model);
            presenterInterface.BindView(view);
            this.logger.Info($"Push: {typeof(TPresenter).FullName}");
        }

        private IPresenter PopData()
        {
            var peek = ((IViewUIService)this).Peek();

            if (peek == null)
            {
                return null;
            }

            peek.Dispose();
            this.viewStack.RemoveAt(this.viewStack.Count - 1);
            this.logger.Info($"Pop: {peek.GetType().FullName}");

            return peek;
        }

        IPresenter IViewUIService.Peek() => this.viewStack.LastOrDefault();

        TPresenter IViewUIService.Push<TModel, TView, TPresenter>(TModel model)
        {
            var presenter = this.GetPresenter<TPresenter>();
            this.viewStack.Add(presenter);
            this.BindPresenter<TModel, TView, TPresenter>(presenter, model)
               .ContinueWith(() => ((IPresenter)presenter).Open())
               .Forget();

            return presenter;
        }

        void IViewUIService.Pop() => this.PopData()?.Close();

        void IViewUIService.PopAll()
        {
            while (this.viewStack.Count > 0)
            {
                ((IViewUIService)this).Pop();
            }
        }

        async UniTask<TPresenter> IViewUIService.PushAsync<TModel, TView, TPresenter>(TModel model)
        {
            var presenter = this.GetPresenter<TPresenter>();
            this.viewStack.Add(presenter);
            await this.BindPresenter<TModel, TView, TPresenter>(presenter, model);
            await ((IPresenter)presenter).OpenAsync();

            return presenter;
        }

        UniTask IViewUIService.PopAsync() => this.PopData()?.CloseAsync() ?? UniTask.CompletedTask;

        async UniTask IViewUIService.PopAllAsync()
        {
            while (this.viewStack.Count > 0)
            {
                await ((IViewUIService)this).PopAsync();
            }
        }
    }
}