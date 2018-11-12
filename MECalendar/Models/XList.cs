using System;
using System.Collections.Generic;

namespace MECalendar.Models
{
    public class XList<T> : List<T>
    {
        public event EventHandler OnAdd;
        public event EventHandler OnClear;
        public void Add(T item)
        {
            OnAdd?.Invoke(this, null);
            base.Add(item);
        }

        public void Clear()
        {
            OnClear?.Invoke(this, null);
            base.Clear();
        }
    }
}
