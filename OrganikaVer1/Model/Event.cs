using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OrganikaVer1.Model
{
    public class Event
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string EventName { get; set; }
        public int AssignedTime { get; set; }
        public int ScheduledHours { get; set; }
        public int Completed { get; set; }

        // helper property to format the progress text
        public string ProgressText
        {
            get { return $"{Completed}/{AssignedTime} completed"; }
        }
    }
}