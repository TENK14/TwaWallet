using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Support.V4.App;
using TwaWallet.Adapters;

namespace TwaWallet.Fragments
{
    public class HistoryFragment : Fragment
    {
        private const string TAG = "X:" + nameof(MainActivity);

        List<string> listData = new List<string>
            {
                "Line 1",
                "Line 2",
                "Line 3",
                "Line 4",
                "Line 5",
                "Line 6",
                "Line 7",
                "Line 8",
                "Line 9"
            };

        public override void OnCreate(Bundle savedInstanceState)
        {
            Log.Debug(TAG, nameof(OnCreate));

            base.OnCreate(savedInstanceState);

            // Create your fragment here
            
            //var view = inflater.Inflate(Resource.Layout.History, container, false);
            //var lv = view.FindViewById(Resources.Id.);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            Log.Debug(TAG, nameof(OnCreateView));

            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);

            //return base.OnCreateView(inflater, container, savedInstanceState);
            //return inflater.Inflate(Resource.Layout.History, container, false);

            View view = inflater.Inflate(Resource.Layout.History, container, false);

            ListView lv = view.FindViewById<ListView>(Resource.Id.history_listView);
            lv.ItemClick += OnListItemClick;
            lv.Adapter = new CusotmListAdapter(this.Activity, listData);

            return view;
        }

        void OnListItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            //string item = result.posts.ElementAt(e.Position);
            string item = listData.ElementAt(e.Position);
            // Do whatever you like here
            Toast.MakeText(this.Activity, $"You press: {item}", ToastLength.Short).Show();
        }
    }
}