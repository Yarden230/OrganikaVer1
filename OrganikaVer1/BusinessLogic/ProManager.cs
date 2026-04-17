using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using OrganikaVer1.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
    

namespace OrganikaVer1.BusinessLogic
{
    public class ProManager
    {
        public static bool DebugMode = false; //if true- will start immediatly w/o needing to fill in details
        public static readonly string TAG = "YARDENAPP";

        public static User CurrentUser { get; set; }

        public static List<Model.Event> GetAllTasks()
        {
            return new List<Model.Event>();
        }
    }
}