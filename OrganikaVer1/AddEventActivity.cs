using Android.App;
using Android.OS;
using Android.Widget;
using OrganikaVer1.Model;
using OrganikaVer1.Service;

namespace OrganikaVer1
{
    [Activity(Label = "Add Event")]
    public class AddEventActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.AddEvent);

            var etName = FindViewById<EditText>(Resource.Id.et_event_name);
            var etHours = FindViewById<EditText>(Resource.Id.et_a_hours);
            var btnAdd = FindViewById<Button>(Resource.Id.btn_addevent);

            btnAdd.Click += async (s, e) =>
            {
                string name = etName.Text.Trim();
                string hoursText = etHours.Text.Trim();

                if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(hoursText))
                {
                    Toast.MakeText(this, "Please fill in all fields", ToastLength.Short).Show();
                    return;
                }

                var newEvent = new Event
                {
                    EventName = name,
                    AssignedTime = int.Parse(hoursText)
                };

                await EventsRepository.InsertAsync(newEvent);
                Toast.MakeText(this, "Event added!", ToastLength.Short).Show();
                Finish(); // go back to TrackerActivity
            };
        }
    }
}