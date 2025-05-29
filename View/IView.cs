namespace MK.MVP
{
    using Cysharp.Threading.Tasks;

    public interface IView
    {
        internal UniTask OpenAsync();
        internal UniTask CloseAsync();
    }
}