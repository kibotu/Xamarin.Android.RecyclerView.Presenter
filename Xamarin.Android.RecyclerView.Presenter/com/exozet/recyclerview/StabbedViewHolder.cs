using Android.Support.V7.Widget;
using Android.Views;
using CheeseBind;

namespace com.exozet.recyclerview
{
    public class StabbedViewHolder : RecyclerView.ViewHolder, IViewHolder
    {
        public StabbedViewHolder(View itemView) : base(itemView) => OnBindViewHolder();

        public void OnBindViewHolder() => Cheeseknife.Bind(this, ItemView);

        public void OnViewAttachedToWindow() { }

        public void OnViewDetachedFromWindow() { }

        public void OnViewRecycled() => Cheeseknife.Reset(this);

        public void OnFailedToRecycleView() { }
    }
}