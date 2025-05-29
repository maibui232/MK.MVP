namespace MK.MVP
{
    using Cysharp.Threading.Tasks;
    using MK.Extensions;
    using UnityEngine;

    public abstract class BaseView : MonoBehaviour, IView
    {
        [SerializeField] private Animate openAnimate;
        [SerializeField] private Animate closeAnimate;

        UniTask IView.OpenAsync()
        {
            return this.openAnimate.PlayAsync();
        }

        UniTask IView.CloseAsync()
        {
            return this.closeAnimate.PlayAsync();
        }
    }
}