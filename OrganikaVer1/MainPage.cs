using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using OrganikaVer1.Model;
using System;
using System.Collections.Generic;

using Android.Runtime;
using Android.Util;
using OrganikaVer1;
using OrganikaVer1.Adapters;
using OrganikaVer1.BusinessLogic;
using OrganikaVer1.Service;
using Firebase.Firestore;
using System.Linq;
using System.Text;

namespace OrganikaVer1
{
    [Activity(Label = "MainPage")]
    public class MainPage : Activity
    {
        RecyclerView usersRecyclerView;
        RecyclerView.LayoutManager layoutManager;
        UsersRViewAdapter userAdapter;

        TextView tvusername, tvisadmin, tvuserslist;
        Dialog mProgressDialog;
        List<User> users;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.mainpage_layout);

            InitializeViews();
            // Create your application here
        }


        private void InitializeViews()
        {
            tvusername = FindViewById<TextView>(Resource.Id.tvUsername);
            tvisadmin = FindViewById<TextView>(Resource.Id.tvIsAdmin);
            tvuserslist = FindViewById<TextView>(Resource.Id.tvUserslist);

            layoutManager = new LinearLayoutManager(this);
            usersRecyclerView = FindViewById<RecyclerView>(Resource.Id.recyclerView);

            users = new List<User>();
            //userAdapter = new UsersRViewAdapter(this, users);
            //usersRecyclerView.SetLayoutManager(layoutManager);
            //userAdapter.ItemClick += OnItemClick;
            //usersRecyclerView.SetAdapter(userAdapter);
            userAdapter = new UsersRViewAdapter(this);
            usersRecyclerView.SetLayoutManager(layoutManager);

            //usersRecyclerView.SetAdapter(userAdapter);
            //^ this line is a problem, it crashes the whole thing and doesnt let it work
        }

        void OnItemClick(object sender, int position)
        {
            Intent intent = new Intent(this, typeof(AccountActivity));
            intent.PutExtra("userID", users[position].Id);
            StartActivity(intent);

            //Toast.MakeText(this, "This is item number " + itemIndewx, ToastLength.Short).Show();
        }

        protected override void OnResume()
        {
            base.OnResume();
            tvusername.Text = ProManager.CurrentUser.FirstName;

            if (ProManager.CurrentUser.IsAdmin)
                tvisadmin.Visibility = ViewStates.Visible;

            ShowProgressBar(true);
            //GetUsersFromDB();
            FetchUsersFromDB();
        }

        protected override void OnPause()
        {
            base.OnPause();
            UsersRepository.StopUsersListener();
        }

        private void FetchUsersFromDB()
        {
            UsersRepository.FetchUsersListener();
            UsersRepository.FirestoreEventListener.getEvent += (error, args) =>
            {
                ShowProgressBar(false);
                if (users != null)
                    users.Clear(); //Clear users list
                else
                    users = new List<User>();

                try
                {
                    var snapshot = (QuerySnapshot)args.Result;
                    if (!snapshot.IsEmpty)
                    {
                        var documents = snapshot.Documents;
                        foreach (DocumentSnapshot item in documents)
                        {
                            User _user = new User()
                            {
                                Id = item.Id,
                                FirstName = item.Get("FirstName").ToString(),
                                LastName = item.Get("LastName").ToString(),
                                UserEmail = item.Get("UserEmail").ToString(),
                                UserMobile = item.Get("UserMobile").ToString(),
                                UserPass = item.Get("UserPassword").ToString(),
                                IsAdmin = bool.Parse(item.Get("IsAdmin").ToString()),
                                ImageId = Resource.Drawable.maleicon
                            };
                            users.Add(_user);
                        }

                        userAdapter.NotifyDataSetChanged();
                    }
                }
                catch (Exception ex)
                {
                    Log.Debug(ProManager.TAG, ex.Message);
                }
            };
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
    }
}