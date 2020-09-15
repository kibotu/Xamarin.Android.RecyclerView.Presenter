using System;
using Android.Views;
using System.Collections.Generic;
using System.Linq;
using Java.Util;
using AndroidX.RecyclerView.Widget;

namespace net.kibotu.recyclerview
{
    public class PresenterAdapter<T> : RecyclerView.Adapter
    {
        public List<Item<T>> Data;

        protected List<dynamic> Presenter;

        protected WeakReference<RecyclerView> RecyclerView { get; set; }

        public bool Debug { get; set; } = false;

        protected Lazy<string> uuid = new Lazy<string>(() => UUID.RandomUUID().ToString().Substring(0, 8));

        public PresenterAdapter()
        {
            Data = new List<Item<T>>();
            Presenter = new List<dynamic>();
        }

        public override int ItemCount => Data.Count;

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType) => PresenterByViewType(viewType).OnCreateViewHolder(parent);

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            (holder as IViewHolder)?.OnBindViewHolder();
            PresenterAt(position).BindViewHolderInternal(holder, Model(position), position);
        }

        public T Model(int position) => Data[position].Model;

        public IQueryable<T> AsQueryable() => Data.Select(item => item.Model).AsQueryable();

        public void Add<P>(T t) where P : IPresenter
        {
            Data.Add(new Item<T> {Model = t, PresenterType = typeof(P)});
            AddIfNotExists<P>();
        }

        public void Insert<P>(int index, T t, bool notify = false) where P : IPresenter
        {
            Data.Insert(index, new Item<T> {Model = t, PresenterType = typeof(P)});
            AddIfNotExists<P>();

            if (notify)
                NotifyItemInserted(index);
        }

        public void Append<P>(T t, bool notify = false) where P : IPresenter
        {
            Data.Append(new Item<T> {Model = t, PresenterType = typeof(P)});
            AddIfNotExists<P>();

            if (notify)
                NotifyItemInserted(ItemCount - 1);
        }

        public void Prepend<P>(T t, bool notify = false) where P : IPresenter
        {
            Data.Prepend(new Item<T> {Model = t, PresenterType = typeof(P)});
            AddIfNotExists<P>();

            if (notify)
                NotifyItemInserted(0);
        }

        public void Update(int position, T item, bool notify = false)
        {
            Data[position].Model = item;

            if (notify)
                NotifyItemChanged(position);
        }

        public void Remove(int position, bool notify = false)
        {
            Data.RemoveAt(position);

            if (notify)
                NotifyItemRemoved(position);
        }

        protected void AddIfNotExists<P>() where P : IPresenter
        {
            var presenter = Presenter.FirstOrDefault(p => p.GetType() == typeof(P));
            if (presenter != null)
                return;

            dynamic x = Activator.CreateInstance(typeof(P));

            Presenter.Add(x);
        }

        public override int GetItemViewType(int position) => Presenter.FindIndex(p => p.GetType() == Data[position].PresenterType);

        protected dynamic PresenterByViewType(int viewType) => Presenter[viewType];

        protected dynamic PresenterAt(int position) => Presenter[GetItemViewType(position)];

        public void Clear()
        {
            Data.Clear();
            Presenter = new List<dynamic>();
        }

        public void RemoveAllViews()
        {
            RecyclerView list = null;
            RecyclerView?.TryGetTarget(out list);
            list?.RemoveAllViews();
        }

        public override void OnAttachedToRecyclerView(RecyclerView recyclerView) => RecyclerView = new WeakReference<RecyclerView>(recyclerView);

        public override void OnDetachedFromRecyclerView(RecyclerView recyclerView) => new WeakReference<RecyclerView>(recyclerView);

        public override bool OnFailedToRecycleView(Java.Lang.Object holder)
        {
            var result = base.OnFailedToRecycleView(holder);
            (holder as IViewHolder)?.OnFailedToRecycleView();
            return result;
        }

        public override void OnViewAttachedToWindow(Java.Lang.Object holder)
        {
            base.OnViewAttachedToWindow(holder);
            (holder as IViewHolder)?.OnViewAttachedToWindow();
        }

        public override void OnViewDetachedFromWindow(Java.Lang.Object holder)
        {
            base.OnViewDetachedFromWindow(holder);
            (holder as IViewHolder)?.OnViewDetachedFromWindow();
        }

        public override void OnViewRecycled(Java.Lang.Object holder)
        {
            base.OnViewRecycled(holder);
            (holder as IViewHolder)?.OnViewRecycled();
        }

        internal void Log(string message)
        {
            if (Debug)
                Android.Util.Log.Verbose($"{GetType().Name}[{uuid}]", message);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                Clear();
            base.Dispose(disposing);
        }
    }
}