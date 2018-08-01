using System;
using Android.Views;

namespace net.kibotu.recyclerview
{
    public class PresenterViewModel<T>
    {
        public T Model { get; set; }

        public Action<T, View, int> OnClick { get; set; }
    }
}
