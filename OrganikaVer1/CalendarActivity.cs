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

namespace OrganikaVer1
{
    [Activity(Label = "CalendarActivity")]
    public class CalendarActivity : Activity
    {
        Button BtnPrev, BtnNext;
        TextView TvMonth, TvNum, TvDay;
        TextView TvHour00, TvHour01, TvHour02, TvHour03, TvHour04, TvHour05, TvHour06, TvHour07, TvHour08, TvHour09, TvHour10, TvHour11, TvHour12, TvHour13, TvHour14, TvHour15, TvHour16, TvHour17, TvHour18, TvHour19, TvHour20, TvHour21, TvHour22, TvHour23;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.daycalendar_layout);



            InitializeViews();
            // Create your application here
        }

        private void InitializeViews()
        {
            FindViewById<Button>(Resource.Id.btnDashboard).Click += (s, e) =>
                StartActivity(new Intent(this, typeof(UserDashboardActivity)));
            //לכתוב את השורה למעלה בצורה יותר טובה, להריץ ולבדוק זה עובד

            FindViewById<Button>(Resource.Id.btnToMainPage).Click += (s, e) =>
                StartActivity(new Intent(this, typeof(MainPage)));

            TvMonth = FindViewById<TextView>(Resource.Id.tvMonth);
            TvNum = FindViewById<TextView>(Resource.Id.tvNum);
            TvDay = FindViewById<TextView>(Resource.Id.tvDay);

            #region TvHour00 = FindView...

            TvHour00 = FindViewById<TextView>(Resource.Id.tvHour00);
            TvHour01 = FindViewById<TextView>(Resource.Id.tvHour01);
            TvHour02 = FindViewById<TextView>(Resource.Id.tvHour02);
            TvHour03 = FindViewById<TextView>(Resource.Id.tvHour03);
            TvHour04 = FindViewById<TextView>(Resource.Id.tvHour04);
            TvHour05 = FindViewById<TextView>(Resource.Id.tvHour05);
            TvHour06 = FindViewById<TextView>(Resource.Id.tvHour06);
            TvHour07 = FindViewById<TextView>(Resource.Id.tvHour07);
            TvHour08 = FindViewById<TextView>(Resource.Id.tvHour08);
            TvHour09 = FindViewById<TextView>(Resource.Id.tvHour09);
            TvHour10 = FindViewById<TextView>(Resource.Id.tvHour10);
            TvHour11 = FindViewById<TextView>(Resource.Id.tvHour11);
            TvHour12 = FindViewById<TextView>(Resource.Id.tvHour12);
            TvHour13 = FindViewById<TextView>(Resource.Id.tvHour13);
            TvHour14 = FindViewById<TextView>(Resource.Id.tvHour14);
            TvHour15 = FindViewById<TextView>(Resource.Id.tvHour15);
            TvHour16 = FindViewById<TextView>(Resource.Id.tvHour16);
            TvHour17 = FindViewById<TextView>(Resource.Id.tvHour17);
            TvHour18 = FindViewById<TextView>(Resource.Id.tvHour18);
            TvHour19 = FindViewById<TextView>(Resource.Id.tvHour19);
            TvHour20 = FindViewById<TextView>(Resource.Id.tvHour20);
            TvHour21 = FindViewById<TextView>(Resource.Id.tvHour21);
            TvHour22 = FindViewById<TextView>(Resource.Id.tvHour22);
            TvHour23 = FindViewById<TextView>(Resource.Id.tvHour23);

            #endregion

            SyncCalendarToToday();
        }


        private void SyncCalendarToToday()
        {
            //DateTime today = DateTime.Today;
            DateTime now = DateTime.Now;

            //all commands:
            //int year = now.Year;
            //int month = now.Month;
            //int day = now.Day;
            //int hour = now.Hour;
            //DayOfWeek dayOfWeek = now.DayOfWeek;

            TvMonth.Text = now.ToString("MMM");
            TvNum.Text = $" {now.ToString("dd")} ";
            TvDay.Text = now.ToString("dddd");



        }
    }
}