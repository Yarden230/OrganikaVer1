using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using OrganikaVer1.BusinessLogic;
using OrganikaVer1.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Android.Graphics.ColorSpace;


namespace OrganikaVer1
{
    [Activity(Label = "AccountActivity")]
    public class AccountActivity : Activity
    {
        EditText _firstName, _lastName, _userEmail, _userMobile;
        ImageButton _btnDelete;
        Button _btnUpdate;
        Dialog mProgressDialog;
        string _userId;
        Model.User _user;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.account_layout);
            InitializeViews();
            // Create your application here
        }

        private async void InitializeViews()
        {
            _firstName = FindViewById<EditText>(Resource.Id.et_account_first_name);
            _lastName = FindViewById<EditText>(Resource.Id.et_account_last_name);
            _userEmail = FindViewById<EditText>(Resource.Id.et_account_email);
            _userMobile = FindViewById<EditText>(Resource.Id.et_account_mobile);
            _btnUpdate = FindViewById<Button>(Resource.Id.btn_account_update);
            _btnDelete = FindViewById<ImageButton>(Resource.Id.ibAccountDelete);
            _btnUpdate.Click += Update_Click;
            _btnDelete.Click += BtnDelete_Click;

            _userId = Intent.GetStringExtra("userID");

            //if(!string.IsNullOrEmpty(_userId)) //Arrived from Users List (Admin)
            if (_userId != ProManager.CurrentUser.Id) //Show delete button if not current user
            {
                ShowProgressBar(true);
                try
                {
                    //_user = await FireBaseHelper.GetUserById(_userId);
                    _userId = Intent.GetStringExtra("userID");
                    FillUserDetails();
                    _btnDelete.Visibility = ViewStates.Visible;
                    ShowProgressBar(false);
                }
                catch (Exception ex)
                {
                    ShowProgressBar(false);
                    Toast.MakeText(this, ex.Message, ToastLength.Short).Show();
                }

            }
            else //Arrived from Current User
            {
                _user = ProManager.CurrentUser;
                FillUserDetails();
            }
        }



        private async void BtnDelete_Click(object sender, EventArgs e)
        {
            //Alert YES NO 
            try
            {
                ShowProgressBar(true);
                await UsersRepository.DeleteAsync(_user);
                //await FireBaseHelper.ReauthenticateAndRemove(_user);

                ShowProgressBar(false);
                Toast.MakeText(this, "User deleted successfuly!", ToastLength.Short).Show();
                Finish();
            }
            catch (Exception ex)
            {
                ShowProgressBar(false);
                Toast.MakeText(this, ex.Message, ToastLength.Short).Show();
            }
        }

        private void FillUserDetails()
        {
            _firstName.Text = _user.FirstName;
            _lastName.Text = _user.LastName;
            _userEmail.Text = _user.UserEmail;
            _userMobile.Text = _user.UserMobile;
        }

        private async void Update_Click(object sender, EventArgs e)
        {
            _user.FirstName = _firstName.Text;
            _user.LastName = _lastName.Text;
            _user.UserMobile = _userMobile.Text;

            try
            {
                ShowProgressBar(true);
                await UsersRepository.UpdateAsync(_user);

                ShowProgressBar(false);
                Toast.MakeText(this, "Update success", ToastLength.Short).Show();

                if (string.IsNullOrEmpty(_userId)) //Update Current user
                {
                    ProManager.CurrentUser = _user;
                }
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, ex.Message, ToastLength.Short).Show();
            }
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