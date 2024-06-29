using System;
using System.Collections.Generic;
using UnityEngine;

namespace HUDUber
{
    public struct AutoDispose<T> : IDisposable where T : class
    {
        private T _value;
        private Action<T> _disposeAction;

        public AutoDispose(T value, Action<T> disposeAction)
        {
            _value = value;
            _disposeAction = disposeAction;
        }

        public void Dispose()
        {
            if (_value != null)
            {
                _disposeAction(_value);
                _value = null;
            }
        }
    }
}
