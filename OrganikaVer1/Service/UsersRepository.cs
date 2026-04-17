using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Gms.Common.Apis;
using Android.Gms.Extensions;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Firebase.Auth;
using Firebase.Firestore;
using Java.Util;
using OrganikaVer1.BusinessLogic;

namespace OrganikaVer1.Service
{
    public class UsersRepository
    {
        public static IListenerRegistration Registration;
        public static FirestoreEventListener FirestoreEventListener;

        public static async Task DeleteAsync(object userToDelete)
        {
            try
            {
                // Create the credential for the account being deleted
                AuthCredential credential = EmailAuthProvider.GetCredential(
                    (userToDelete as Model.User).UserEmail, 
                    (userToDelete as Model.User).UserPass);

                // Reauthenticate the ACTIVE user session specifically
                await FirebaseAuth.Instance.SignInWithCredential(credential);

                // Now delete the Firestore data
                await FirebaseFirestore.Instance.Collection("users").
                                                 Document((userToDelete as Model.User).Id).Delete();

                // Now delete the Auth record
                await FirebaseAuth.Instance.CurrentUser.DeleteAsync();

                // Reauthenticate the ACTIVE user session to Current User CredentiaLS
                credential = EmailAuthProvider.GetCredential(ProManager.CurrentUser.UserEmail, ProManager.CurrentUser.UserPass);
                await FirebaseAuth.Instance.SignInWithCredential(credential);

                //delete all user related events
                DeleteAllUserEvents((userToDelete as Model.User).Id);
            }
            catch (Exception ex)
            {
                Log.Debug(ProManager.TAG, $"Delete user failed! " + ex.Message);
                throw new Exception("Delete user failed!");
            }
        }

        private static async void DeleteAllUserEvents(string userId)
        {
            try
            {
                var userEvents = await FirebaseFirestore.Instance.Collection("events").WhereEqualTo("UserId", userId).Get();

                foreach (var userEvent in ((QuerySnapshot)userEvents).Documents)
                {
                    await FirebaseFirestore.Instance.Collection("events").Document(userEvent.Id).Delete();
                }
            }
            catch (Exception ex)
            {
                Log.Debug(ProManager.TAG, $"Delete all user events failed! " + ex.Message);
                throw new Exception("Delete all user events failed!");
            }

        }

        public static async Task<object> GetByIdAsync(string userId)
        {
            Model.User newuser = null;
            try
            {
                DocumentReference userRef = FirebaseFirestore.Instance
                .Collection("users")
                .Document(userId);

                var userObject = await userRef.Get();

                newuser = new Model.User()
                {
                    Id = userId,
                    FirstName = ((DocumentSnapshot)userObject).Get("FirstName").ToString(),
                    LastName = ((DocumentSnapshot)userObject).Get("LastName").ToString(),
                    UserEmail = ((DocumentSnapshot)userObject).Get("UserEmail").ToString(),
                    UserMobile = ((DocumentSnapshot)userObject).Get("UserMobile").ToString(),
                    UserPass = ((DocumentSnapshot)userObject).Get("UserPassword").ToString(),
                    IsAdmin = bool.Parse(((DocumentSnapshot)userObject).Get("IsAdmin").ToString())
                };
                Log.Debug(ProManager.TAG, $"GetUserById: Get User from Firestore DB success");
                return newuser;
            }
            catch (FirebaseFirestoreException ex)
            {
                Log.Debug(ProManager.TAG, $"GetUserByID: Get User from Firestore failed: {ex.Message}");
                return null; // Indicate failure
            }
            catch (System.Exception ex)
            {
                Log.Debug(ProManager.TAG, $"GetUserByID general error: {ex.Message}");
                return null;
            }
        }
        public static async Task<string> InsertAsync(object item)
        {
            Model.User tmpUser = item as Model.User;
            try
            {
                //Add user account to Firebase Auth Module
                tmpUser.Id = await RegisterUserForAuth(tmpUser);
                await AddUserToFirestore(tmpUser);
                return tmpUser.Id;
            }
            catch (Exception ex)
            {
                Log.Error(ProManager.TAG, $"Insert user failed: {ex.Message}");
                throw new Exception("Insert user failed");
            }
        }
        public static async Task UpdateAsync(object item)
        {
            Model.User tmpUser = item as Model.User;
            try
            {
                DocumentReference userRef = FirebaseFirestore.Instance
                                            .Collection("users").Document(tmpUser.Id);

                await userRef.Update("FirstName", tmpUser.FirstName);
                await userRef.Update("LastName", tmpUser.LastName);
                await userRef.Update("UserMobile", tmpUser.UserMobile);

                Log.Debug(ProManager.TAG, $"FirebaseHelper: Update {tmpUser.UserEmail} success");
            }
            catch (System.Exception ex)
            {
                Log.Debug(ProManager.TAG, $"FirebaseHelper: Update {tmpUser.UserEmail} failed " + ex.Message);
                throw new Exception($"Update {tmpUser.UserEmail} failed");
                //    }
            }
        }
        public static async Task<List<object>> GetAllAsync()
        {
            List<object> users = new List<object> ();

            try
            {
                var documents = await FirebaseFirestore.Instance.Collection("users").Get();
                var FirestoreUsersCollection = (QuerySnapshot)documents;

                if (!FirestoreUsersCollection.IsEmpty)
                {
                    var usersCollection = FirestoreUsersCollection.Documents;
                    foreach (DocumentSnapshot item in usersCollection)
                    {
                        Model.User user = new Model.User()
                        {
                            Id = item.Id,
                            FirstName = item.Get("FirstName").ToString(),
                            LastName = item.Get("LastName").ToString(),
                            UserEmail = item.Get("UserEmail").ToString(),
                            UserMobile = item.Get("UserMobile").ToString(),
                            UserPass = item.Get("UserPassword").ToString(),
                            IsAdmin = bool.Parse(item.Get("IsAdmin").ToString())
                        };
                        users.Add(user);
                    }
                    Log.Debug(ProManager.TAG, $"GetUsersCollection: loaded successfully! " +
                                              $"Count: {users.Count}");
                }
                return users;
            }
            catch (FirebaseFirestoreException ex)
            {
                Log.Debug(ProManager.TAG, $"GetUsersCollection failed: {ex.Message}");
                return users; // Indicate failure
            }
            catch (System.Exception ex)
            {
                Log.Debug(ProManager.TAG, $"GetUsersCollection general error: {ex.Message}");
                return users;
            }
        }
        public static async Task<string> SignInUserAsync(string uemail, string upass)
        {
            try
            {
                FirebaseAuth mAuth = FirebaseAuth.Instance;
                //using Android.Gms.Extensions;
                await mAuth.SignInWithEmailAndPassword(uemail, upass);
                Log.Debug(ProManager.TAG, $"MyApp: User Auth {uemail} SignIn success");
                return mAuth.CurrentUser.Uid; // Indicate success
            }
            catch (FirebaseAuthException ex)
            {
                Log.Error(ProManager.TAG, $"SignInUserAsync: User Auth SignIn failed: {ex.Message}");
                return null; // Indicate failure
            }
            catch (System.Exception ex)
            {
                Log.Error(ProManager.TAG, $"SignInUserAsync: User Auth SignIn failed, general error: {ex.Message}");
                return null; // Indicate failure
            }
        }
        public static async Task<string> RegisterUserForAuth(Model.User user)
        {
            //Add user account to Firebase Auth Module
            try
            {
                FirebaseAuth mAuth = FirebaseAuth.Instance;
                //using Android.Gms.Extensions;
                await mAuth.CreateUserWithEmailAndPasswordAsync(user.UserEmail, user.UserPass);
                Log.Debug(ProManager.TAG, $"RegisterUserForAuth: User Auth {user.UserEmail} SignIn success");

                return mAuth?.CurrentUser.Uid;
            }
            catch (FirebaseAuthException ex)
            {
                Log.Error(ProManager.TAG, $"RegisterUserForAuth: {ex.Message}");
                throw new Exception("RegisterUserForAuth Failed!");
            }
            catch (System.Exception ex)
            {
                Log.Error(ProManager.TAG, $"RegisterUserForAuth general error: {ex.Message}");
                throw new Exception("RegisterUserForAuth Failed!");
            }
        }
        public static async Task AddUserToFirestore(Model.User user)
        {
            try
            {
                //Insert user to FireStore database
                HashMap userMap = new HashMap(); //using Java.Util;
                userMap.Put("FirstName", user.FirstName);
                userMap.Put("IsAdmin", user.IsAdmin);
                userMap.Put("LastName", user.LastName);
                userMap.Put("UserEmail", user.UserEmail);
                userMap.Put("UserMobile", user.UserMobile);
                userMap.Put("UserPassword", user.UserPass);


                DocumentReference userReference = FirebaseFirestore.Instance
                                                                        .Collection("users")
                                                                        .Document(user.Id);
                await userReference.Set(userMap);
                Log.Debug(ProManager.TAG, $"Add User to Firestore complited");
            }
            catch (FirebaseFirestoreException ex)
            {
                Log.Error(ProManager.TAG, $"Add User to Firestore failed: {ex.Message}");
                throw new Exception("Add User to Firestore failed");
            }
            catch (System.Exception ex)
            {
                Log.Error(ProManager.TAG, $"Add User to Firestore failed: {ex.Message}");
                throw new Exception("Add User to Firestore failed");
            }
        }
             
        public static void FetchUsersListener()
        {
            FirestoreEventListener = new FirestoreEventListener();
            Registration = FirebaseFirestore.Instance
                .Collection("users")
                .AddSnapshotListener(FirestoreEventListener);
        }
        public static void StopUsersListener()
        {
            Registration?.Remove();
            Registration = null;
            FirestoreEventListener = null;
        }
    }
}