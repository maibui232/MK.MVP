namespace MK.MVP
{
    using MK.Kernel;

    public static class UIKernel
    {
        public static void UIConfigure(this IBuilder builder)
        {
            builder.Add<ViewUIService>().AsImplementedInterface();
        }
    }
}