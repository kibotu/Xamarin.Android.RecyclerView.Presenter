using System;
namespace net.kibotu.recyclerview
{
    public class PresenterViewModel<T>
    {
        public T Model { get; set; }

        public Action<T> OnClick { get; set; }
    }
}
