// <copyright file="ProviderProbe.cs" company="Math.NET">
// Math.NET Numerics, part of the Math.NET Project
// https://numerics.mathdotnet.com
// https://github.com/mathnet/mathnet-numerics
//
// Copyright (c) 2009-2021 Math.NET
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
// </copyright>

using System;
using System.Threading;
using MathNet.Numerics;
using MathNet.Numerics.Providers;

namespace Mercury.Language.Math.Providers
{
    /// <summary>
    /// ProviderProbe Description
    /// </summary>
    public class ProviderProbe<T> where T : class
    {
        readonly bool _disabled;
        readonly Lazy<IProviderCreator<T>> _creator;

        public ProviderProbe(string typeName, bool disabled = false)
        {
            _disabled = disabled;
            _creator = new Lazy<IProviderCreator<T>>(() =>
            {
                var type = Type.GetType(typeName);
                return type is null ? null : Activator.CreateInstance(type) as IProviderCreator<T>;
            }, LazyThreadSafetyMode.ExecutionAndPublication);
        }

        public T Create()
        {
            if (_disabled)
            {
                throw new NotSupportedException(LocalizedResources.Instance().SPECIFIC_NATIVE_PROVIDER_DISABLED_BY_AN_APPLICATION_SWITCH);
            }

            if (AppSwitches.DisableNativeProviders)
            {
                throw new NotSupportedException(LocalizedResources.Instance().NATIVE_PROVIDER_DISABLED_BY_AN_APPLICATION_SWITCH);
            }

            if (AppSwitches.DisableNativeProviderProbing)
            {
                throw new NotSupportedException(LocalizedResources.Instance().NATIVE_PROVIDER_PROBING_IS_DISABLED_BY_AN_APPLICATION_SWITCH);
            }

            var creator = _creator.Value;
            if (creator is null)
            {
                throw new NotSupportedException(LocalizedResources.Instance().NATIVE_PROVIDER_PROBING_FAILED_TO_RESOLVE_CREATOR);
            }

            return creator.CreateProvider();
        }

        public T TryCreate()
        {
            if (_disabled || AppSwitches.DisableNativeProviderProbing || AppSwitches.DisableNativeProviders)
            {
                return null;
            }

            try
            {
                return _creator.Value?.CreateProvider();
            }
            catch
            {
                // intentionally swallow exceptions here - use the explicit variants if you're interested in why
                return null;
            }
        }
    }
}
