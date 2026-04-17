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
        List<Model.Event> _myEvents;
        ListView lvTasks;
        TextView tvTestTextView;
        //private List<Task> _myTasks;
        //EventListAdapter TLAdapter;
        Button btnAddEvent;
        Dialog mProgressDialog;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.trackerlist_layout);

            InitilizeViews();
            GetEventsListfromDB();

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

        private async void GetEventsListfromDB()
        {
            try
            {
                ShowProgressBar(true);
                var allEvents = await EventsRepository.GetAllAsync();

                _myEvents.Clear(); //clear the list before adding new items
                foreach (var item in allEvents) 
                {
                    _myEvents.Add((Model.Event)item); //add each event to the list
                }
                ShowProgressBar(false);
            }
            catch (Exception ex )
            {

                tvTestTextView.Text = $"Error fetching events: {ex.Message}";
                ShowProgressBar(false);
            }
            ShowEvents(); //diaplay all the events in the textview (debugging)
        }

        private void ShowEvents() //זמנייייייייייייייי
        {
            if (_myEvents.Count != 0)
            {
                tvTestTextView.Text = "Events List: \n\n";
                foreach (var ev in _myEvents)
                {
                    tvTestTextView.Text += $"Event: {ev.EventName}" + $"Assigend time: {ev.AssignedTime}" + $"Scheduled hours: {ev.ScheduledHours}" + $"Completed: {ev.Completed}\n";
                }
            }
            else
            {
                tvTestTextView.Text = "no events found";
                return;
            }
        }

        protected override void OnResume()
        {
            base.OnResume();
            //ShowTrackerList();
        }

        private void InitilizeViews()
        {
            _myEvents = new List<Model.Event>();
            tvTestTextView = FindViewById<TextView>(Resource.Id.tvTasksListTitle);

            lvTasks = FindViewById<ListView>(Resource.Id.lvTasks);
            //lvTasks.OnItemClickListener = this;
            btnAddEvent = FindViewById<Button>(Resource.Id.btnAddEvent);

            btnAddEvent.Click += (s, e) =>
            {
                //1.Add event to firestore
                InsertEvent();

                //StartActivity(new Intent(this, typeof(AddEventActivity)));
            };

            //ShowTrackerList();

        }

        private async void InsertEvent()
        { //להוסיף את הקוד לאפליציה של להוסיף איוונט
            Model.Event tmpEvent = new Model.Event()
            {
                EventName = "new Event",
                AssignedTime = 5,
                ScheduledHours = 0,
                Completed = 0,
                UserId = ProManager.CurrentUser?.Id ?? ""
            };

            try
            { 
                ShowProgressBar(true);
                string EventId = await EventsRepository.InsertAsync(tmpEvent);
                tvTestTextView.Text = $"Inserted event with id: {EventId}";
                ShowProgressBar(false);
            }
            catch (Exception ex)
            {
                ShowProgressBar(false);
                tvTestTextView.Text = $"Error inserting event: {ex.Message}";
            }

        }

        private async void ShowTrackerList()
        {
            var allEvents = await EventsRepository.GetAllAsync();
            var events = allEvents.Cast<Model.Event>().ToList();
            lvTasks.Adapter = new EventListAdapter(this, events);
        }

        private void ShowProgressBar(bool show)
        {
            //android:background="@android:color/transparent"

            if (show)
            {
                mProgressDialog = new Dialog(this, Android.Resource.Style.ThemeNoTitleBar);
                View view = LayoutInflater.From(this).Inflate(Resource.Layout.fb_progressbar, null);
                //var mProgressMessage = (TextView)view.FindViewById(Resource.Id.;
                //mProgressMessage.Text = "Loading...";
                mProgressDialog.Window.SetBackgroundDrawableResource(Resource.Color.mtrl_btn_transparent_bg_color);
                mProgressDialog.SetContentView(view);
                mProgressDialog.SetCancelable(false);
                mProgressDialog.Show();
            }
            else
            {
                mProgressDialog.Dismiss();
            }
        }

    }  //מה לעשות כשאני ממשיכה לעבוד על זה?
       // בדיוק העתקתי דברים מהעבודה שלי של סוך שנה שעברה כדי לעשות ליסט וויו. אני מעתיקה הכל אבל משנה כדי שזה יהיה לטראקר. להשיך לעשות את זה עד שאיון טעויות
       //ובתקווה דיי העלמתי את כל הai
       // עשיתי את זה רק לעמוד הזה כרגע אבל אני צריכה לשנות גם את כל הלייאאוטים ואת האדפטר. פשוט להשוות הכל לעבודה הקודמת
       //מומלץ אולי לפתוח את הפיירבייס קודם
       //פשוט להשקיע זמן ולהבין את הכל כדי להעתיק בצורה ממש דומה את מה שעשיתי ליוזרז


    //    <LinearLayout
    //    android:layout_width="match_parent"
    //    android:layout_height="wrap_content"
    //    android:orientation="horizontal"
    //    android:background="#b06969"
    //    android:padding="10dp"
    //    android:layout_marginHorizontal="15dp">

    //    <TextView android:layout_width="0dp" android:layout_height="wrap_content" android:layout_weight="2" android:text="Activity / Task" android:textColor="#FFFFFF" android:textStyle="bold" />
    //    <TextView android:layout_width="0dp" android:layout_height="wrap_content" android:layout_weight="1.5" android:text="Assigned time\n(hours)" android:textColor="#FFFFFF" android:textStyle="bold" />
    //    <TextView android:layout_width="0dp" android:layout_height="wrap_content" android:layout_weight="1.5" android:text="Scheduled hours\n(SH / AT)" android:textColor="#FFFFFF" android:textStyle="bold" />
    //    <TextView android:layout_width="0dp" android:layout_height="wrap_content" android:layout_weight="2" android:text="Progress" android:textColor="#FFFFFF" android:textStyle="bold" />
    //    <TextView android:layout_width="0dp" android:layout_height="wrap_content" android:layout_weight="0.8" android:text="..." android:textColor="#FFFFFF" android:textStyle="bold" android:gravity="center" />
    //</LinearLayout>

}