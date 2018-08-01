namespace net.kibotu.recyclerview
{
    public class PresenterViewModel<T>
    {
        public T Model { get; set; }

        public OnItemClickListener<T> OnClick { get; set; }
    }
}