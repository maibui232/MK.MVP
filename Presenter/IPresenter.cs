namespace MK.MVP
{
    using System;
    using Cysharp.Threading.Tasks;

    public interface IPresenter : IDisposable
    {
        internal void Open();
        internal void Close();

        internal UniTask OpenAsync();
        internal UniTask CloseAsync();

        internal void BindView(IView   view);
        internal void BindModel(object model);
    }
}