using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;

namespace OrganikaVer1
{
    [Activity(Label = "UserDashboardActivity")]
    public class UserDashboardActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.userdashboard_layout);

            FindViewById<Button>(Resource.Id.btnTrackerList).Click += (s, e) =>
                StartActivity(new Intent(this, typeof(TrackerActivity)));

            FindViewById<Button>(Resource.Id.btnCalendar).Click += (s, e) =>
                StartActivity(new Intent(this, typeof(CalendarActivity)));

            FindViewById<Button>(Resource.Id.btnAccountPanel).Click += (s, e) =>
                StartActivity(new Intent(this, typeof(AccountActivity)));
        }
    }
}