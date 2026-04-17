using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using OrganikaVer1.Model;
using System;
using System.Collections.Generic;

namespace OrganikaVer1.Adapters
{
    internal class EventListAdapter : BaseAdapter
    {
        Context context;
        List<Event> _events;

        public EventListAdapter(Context context, List<Event> events)
        {
            this.context = context;
            _events = events ?? new List<Event>();
        }

        public override Java.Lang.Object GetItem(int position) => position;
        public override long GetItemId(int position) => position;
        public override int Count => _events.Count;

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView;
            EventListAdapterViewHolder holder = null;

            if (view != null)
                holder = view.Tag as EventListAdapterViewHolder;

            if (holder == null)
            {
                holder = new EventListAdapterViewHolder();
                var inflater = context.GetSystemService(Context.LayoutInflaterService).JavaCast<LayoutInflater>();
                view = inflater.Inflate(Resource.Layout.trackevent_layout, parent, false);
                holder.tvName = view.FindViewById<TextView>(Resource.Id.tvName);
                holder.tvAssignedTime = view.FindViewById<TextView>(Resource.Id.tvAssignedTime);
                holder.tvScheduledHours = view.FindViewById<TextView>(Resource.Id.tvScheduledHours);
                holder.tvCompleted = view.FindViewById<TextView>(Resource.Id.tvCompleted);
                view.Tag = holder;
            }

            Event ev = _events[position];
            holder.tvName.Text = ev.EventName;
            holder.tvAssignedTime.Text = ev.AssignedTime.ToString();
            holder.tvScheduledHours.Text = $"{ev.ScheduledHours}/{ev.AssignedTime}";
            holder.tvCompleted.Text = ev.ProgressText;

            return view;
        }
    }

    internal class EventListAdapterViewHolder : Java.Lang.Object
    {
        public TextView tvName { get; set; }
        public TextView tvAssignedTime { get; set; }
        public TextView tvScheduledHours { get; set; }
        public TextView tvCompleted { get; set; }
    }
}