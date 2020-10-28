using System;
using System.Collections.Generic;
using System.Text;

namespace FunK
{
    public static class FunK
    {
        public static R Using<TDisp, R>(TDisp disposable, Func<TDisp, R> f) where TDisp : IDisposable
        {
            using (disposable) return f(disposable);
        }
    }
}
