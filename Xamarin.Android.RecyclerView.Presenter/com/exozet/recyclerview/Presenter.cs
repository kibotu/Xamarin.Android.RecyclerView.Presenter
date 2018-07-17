using System;
using Android.Support.V7.Widget;
using Android.Support.Annotation;
using Android.Views;

namespace com.exozet.recyclerview
{
    public abstract class Presenter<T, VH> : IPresenter where VH : RecyclerView.ViewHolder
    {
        [LayoutRes]
        public abstract int Layout { get; }

        public PresenterAdapter<T> Adapter { get; set; }

        public VH OnCreateViewHolder(ViewGroup parent) => Activator.CreateInstance(typeof(VH), LayoutInflater.From(parent.Context).Inflate(Layout, parent, false)) as VH;

        public void BindViewHolderInternal(object viewHolder, T item, int position) => BindViewHolder(viewHolder as VH, item, position);

        public abstract void BindViewHolder(VH viewHolder, T item, int position);

        public T Model(int position) => Adapter.Model(position);
    }
}