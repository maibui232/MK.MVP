namespace MK.MVP
{
    using Cysharp.Threading.Tasks;

    public interface IViewUIService
    {
        IPresenter Peek();

        TPresenter Push<TModel, TView, TPresenter>(TModel model) where TPresenter : BasePresenter<TView, TModel> where TView : BaseView;

        void Pop();

        void PopAll();

        UniTask<TPresenter> PushAsync<TModel, TView, TPresenter>(TModel model) where TPresenter : BasePresenter<TView, TModel> where TView : BaseView;

        UniTask PopAsync();

        UniTask PopAllAsync();
    }
}