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
using static AndroidX.RecyclerView.Widget.RecyclerView;
using OrganikaVer1.Adapters;
using OrganikaVer1.BusinessLogic;
using OrganikaVer1.Service;

namespace OrganikaVer1
{
    [Activity(Label = "TrackerActivity")]
    public class TrackerActivity : Activity
    {
        ListView lvTasks;
        //private List<Task> _myTasks;
        //EventListAdapter TLAdapter;
        Button btnAddEvent;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.trackerlist_layout);

            InitilizeViews();
            // 1. Find the ListView on the screen
            //lvTasks = FindViewById<ListView>(Resource.Id.listViewTasks);

            // 2. Generate some test data (matching your image)
            // _myTasks = new List<Task>
            // {
            // new Task { ActivityName = "Crochet", AssignedTime = 6.5, ScheduledHours = 6.5, CompletedHours = 2 },
            // new Task { ActivityName = "Study french", AssignedTime = 3, ScheduledHours = 3, CompletedHours = 1.5 },
            // new Task { ActivityName = "Swim", AssignedTime = 5, ScheduledHours = 4, CompletedHours = 2 },
            // new Task { ActivityName = "Journal", AssignedTime = 4, ScheduledHours = 4, CompletedHours = 2 },
            // new Task { ActivityName = "Clean fridge", AssignedTime = 1, ScheduledHours = 1, CompletedHours = 0 }
            //};

            // 3. Create the adapter and attach it to the ListView
            //TaskListAdapter adapter = new TaskListAdapter(this, _myTasks);
            //_listViewTasks.Adapter = adapter;




            // Only show the Add Event button if the logged-in user is admin
            //btnAddEvent.Visibility = ProManager.CurrentUser?.IsAdmin == true
            //    ? Android.Views.ViewStates.Visible
            //    : Android.Views.ViewStates.Gone;



        }
        protected override void OnResume()
        {
            base.OnResume();
            ShowTrackerList();
        }

        private void InitilizeViews()
        {
            lvTasks = FindViewById<ListView>(Resource.Id.lvTasks);
            //lvTasks.OnItemClickListener = this;

            btnAddEvent.Click += (s, e) =>
            {
                StartActivity(new Intent(this, typeof(AddEventActivity)));
            };

            ShowTrackerList();

        }

        private async void ShowTrackerList()
        {
            var allEvents = await EventsRepository.GetAllAsync();
            var events = allEvents.Cast<Model.Event>().ToList();
            lvTasks.Adapter = new EventListAdapter(this, events);
        }

    }  //מה לעשות כשאני ממשיכה לעבוד על זה?
       // בדיוק העתקתי דברים מהעבודה שלי של סוך שנה שעברה כדי לעשות ליסט וויו. אני מעתיקה הכל אבל משנה כדי שזה יהיה לטראקר. להשיך לעשות את זה עד שאיון טעויות
       //ובתקווה דיי העלמתי את כל הai
       // עשיתי את זה רק לעמוד הזה כרגע אבל אני צריכה לשנות גם את כל הלייאאוטים ואת האדפטר. פשוט להשוות הכל לעבודה הקודמת
       //מומלץ אולי לפתוח את הפיירבייס קודם
       //פשוט להשקיע זמן ולהבין את הכל כדי להעתיק בצורה ממש דומה את מה שעשיתי ליוזרז

}