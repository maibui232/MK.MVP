namespace MK.MVP
{
    using UnityEngine;

    [AddComponentMenu(nameof(MK) + "/" + nameof(MVP) + "/" + nameof(RootViewUI))]
    internal sealed class RootViewUI : MonoBehaviour
    {
        [SerializeField] private Transform showContainer;
        [SerializeField] private Transform hideContainer;

        public Transform ShowContainer => this.showContainer;
        public Transform HideContainer => this.hideContainer;
    }
}