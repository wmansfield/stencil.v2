using System;

namespace Stencil.Native
{
    public static class _CoreExtensions
    {
        public static T DisposeSafe<T>(this T disposable)
            where T : class, IDisposable
        {
            disposable?.Dispose();
            return null;
        }
    }
}
