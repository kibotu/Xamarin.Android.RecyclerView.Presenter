using System;
using Android.Views;

namespace net.kibotu.recyclerview
{
    public class OnItemClickListener<T> : Java.Lang.Object, View.IOnClickListener
    {
        public Action<T, View, int> Action;

        public T Model { get; set; }

        public View View { get; set; }

        public int Position { get; set; }

        public void OnClick(View v) => Action?.Invoke(Model, View, Position);
    }
}