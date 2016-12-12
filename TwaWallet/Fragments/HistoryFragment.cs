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
using Database;
using Database.POCO;

namespace TwaWallet.Fragments
{
    public class HistoryFragment : Fragment
    {
        private const string TAG = "X:" + nameof(HistoryFragment);

        private IDataContext db;

        ListView listView;

        //List<string> listData = new List<string>
        //    {
        //        "Line 1",
        //        "Line 2",
        //        "Line 3",
        //        "Line 4",
        //        "Line 5",
        //        "Line 6",
        //        "Line 7",
        //        "Line 8",
        //        "Line 9"
        //    };
        List<Record> listData;// = new List<Record>();

        public override void OnCreate(Bundle savedInstanceState)
        {
            Log.Debug(TAG, nameof(OnCreate));

            base.OnCreate(savedInstanceState);

            // Create your fragment here

            //var view = inflater.Inflate(Resource.Layout.History, container, false);
            //var lv = view.FindViewById(Resources.Id.);

            string pathToDatabase = DeviceInfo.GetFileFinallPath(Resources.GetString(Resource.String.DBfilename));
            db = new DataContext(pathToDatabase);

            //LoadData();
        }

        public override void OnResume()
        {
            Log.Debug(TAG, nameof(OnResume));

            base.OnResume();

            LoadData();
            InitLayout();
        }

        private void LoadData()
        {
            Log.Debug(TAG, nameof(LoadData));

            var r = db.Select<Record, int>((o) => o.Id > 0, (o) => o.Id, false).Result;
            listData = r.ToList();//r.Select(p => $"{p.Description}, {p.Cost}, {p.Date}").ToList();
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            Log.Debug(TAG, nameof(OnCreateView));

            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);

            //return base.OnCreateView(inflater, container, savedInstanceState);
            //return inflater.Inflate(Resource.Layout.History, container, false);

            View view = inflater.Inflate(Resource.Layout.History, container, false);

            listView = view.FindViewById<ListView>(Resource.Id.history_listView);
            listView.ItemClick += OnListItemClick;

            LoadData();
            //listView.Adapter = new CustomListAdapter(this.Activity, listData);
            InitLayout();

            return view;
        }

        private void InitLayout()
        {
            Log.Debug(TAG, nameof(InitLayout));

            listView.Adapter = new CustomListAdapter(this.Activity, listData);
        }

        void OnListItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Log.Debug(TAG, nameof(OnListItemClick));

            //string item = result.posts.ElementAt(e.Position);
            //string item = listData.ElementAt(e.Position);
            var item = listData.ElementAt(e.Position);
            // Do whatever you like here
            Toast.MakeText(this.Activity, $"You press: {item.Description}", ToastLength.Short).Show();
        }
    }
}