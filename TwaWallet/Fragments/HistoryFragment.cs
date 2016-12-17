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
using TwaWallet.Classes;

namespace TwaWallet.Fragments
{
    public class HistoryFragment : DialogFragment //Fragment
    {
        #region Members
        private const string TAG = "X:" + nameof(HistoryFragment);

        private IDataContext db;

        ListView listView;
        
        List<Record> listData;

        #region GUI
        TextView count_value;
        TextView monthCost_value;
        TextView filterCost_value;
        Button dateFrom_button;
        Button dateTo_button;
        #endregion

        #endregion

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

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            Log.Debug(TAG, nameof(OnCreateView));

            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);

            //return base.OnCreateView(inflater, container, savedInstanceState);
            //return inflater.Inflate(Resource.Layout.History, container, false);

            View v = inflater.Inflate(Resource.Layout.History, container, false);

            listView = v.FindViewById<ListView>(Resource.Id.history_listView);
            listView.ItemClick += OnListItemClick;

            count_value = v.FindViewById<TextView>(Resource.Id.count_value);
            filterCost_value = v.FindViewById<TextView>(Resource.Id.filterCost_value);
            monthCost_value = v.FindViewById<TextView>(Resource.Id.monthCost_value);


            dateFrom_button = v.FindViewById<Button>(Resource.Id.dateFrom_button);
            dateFrom_button.Click += DateFrom_button_Click;
            dateTo_button = v.FindViewById<Button>(Resource.Id.dateTo_button);
            dateTo_button.Click += DateTo_button_Click;


            LoadData();

            InitLayout();

            return v;
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

            var r = db.Select<Record, DateTime>((o) => o.Id > 0, (o) => o.Date, false).Result;
            listData = r.ToList();//r.Select(p => $"{p.Description}, {p.Cost}, {p.Date}").ToList();
        }

        private void LoadData(DateTime dateFrom, DateTime dateTo)
        {
            Log.Debug(TAG, nameof(LoadData));

            var r = db.Select<Record, DateTime>((o) => o.Date >= dateFrom && o.Date <= dateTo, (o) => o.Date, false).Result;
            listData = r.ToList();//r.Select(p => $"{p.Description}, {p.Cost}, {p.Date}").ToList();

            if (listData != null && listData.Count > 0)
            {
                this.count_value.Text = listData.Count.ToString();
                this.filterCost_value.Text = listData.Select(p => p.Cost).Sum().ToString();
            }

            listView.Adapter = new CustomListAdapter(this.Activity, listData);
        }

        

        private void DateTo_button_Click(object sender, EventArgs e)
        {
            Log.Debug(TAG, nameof(DateTo_button_Click));

            DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime date)
            {
                dateTo_button.Text = date.ToString(Resources.GetString(Resource.String.DateFormat)); //.ToLongDateString();
                dateTo_button.Tag = new JavaLangObjectWrapper<DateTime>(date);

                LoadData(((JavaLangObjectWrapper<DateTime>)dateFrom_button.Tag).Value, ((JavaLangObjectWrapper<DateTime>)dateTo_button.Tag).Value);
            });
            frag.Show(this.Activity.FragmentManager, DatePickerFragment.TAG);

        }

        private void DateFrom_button_Click(object sender, EventArgs e)
        {
            Log.Debug(TAG, nameof(DateTo_button_Click));

            DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime date)
            {
                dateFrom_button.Text = date.ToString(Resources.GetString(Resource.String.DateFormat)); //.ToLongDateString();
                dateFrom_button.Tag = new JavaLangObjectWrapper<DateTime>(date);

                LoadData(((JavaLangObjectWrapper<DateTime>)dateFrom_button.Tag).Value, ((JavaLangObjectWrapper<DateTime>)dateTo_button.Tag).Value);
            });
            frag.Show(this.Activity.FragmentManager, DatePickerFragment.TAG);

            
        }

        private void InitLayout()
        {
            Log.Debug(TAG, nameof(InitLayout));

            listView.Adapter = new CustomListAdapter(this.Activity, listData);
            
            if (listData != null && listData.Count > 0)
            {
                var date = listData.LastOrDefault().Date;
                dateFrom_button.Text = date.ToString(Resources.GetString(Resource.String.DateFormat));
                dateFrom_button.Tag = new JavaLangObjectWrapper<DateTime>(date);

                date = listData.FirstOrDefault().Date;
                dateTo_button.Text = date.ToString(Resources.GetString(Resource.String.DateFormat));
                dateTo_button.Tag = new JavaLangObjectWrapper<DateTime>(date);

                this.count_value.Text = listData.Count.ToString();
                this.monthCost_value.Text = listData.Where(p => p.Date.Month == DateTime.Now.Month && p.Date.Year == DateTime.Now.Year).Select(p => p.Cost).Sum().ToString();
                this.filterCost_value.Text = listData.Select(p => p.Cost).Sum().ToString();
            }
        }

        private void OnListItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Log.Debug(TAG, nameof(OnListItemClick));

            //string item = result.posts.ElementAt(e.Position);
            //string item = listData.ElementAt(e.Position);
            var item = listData.ElementAt(e.Position);
            // Do whatever you like here
            Toast.MakeText(this.Activity, $"You press: {item.Description}", ToastLength.Short).Show();

            // DialogFragment.show() will take care of adding the fragment
            // in a transaction.  We also want to remove any currently showing
            // dialog, so make our own transaction and take care of that here.
            FragmentTransaction ft = FragmentManager.BeginTransaction();
            Fragment prev = FragmentManager.FindFragmentByTag("dialog");
            if (prev != null)
            {
                ft.Remove(prev);
            }
            ft.AddToBackStack(null);

            Log.Debug(TAG, $"{nameof(OnListItemClick)} - try to show ReportFragment like dialog - START");
            // Create and show the dialog.
            DialogFragment newFragment = ReportFragment.NewInstance(item.IncludeObjects(db), delegate()
            {
                LoadData(((JavaLangObjectWrapper<DateTime>)dateFrom_button.Tag).Value, ((JavaLangObjectWrapper<DateTime>)dateTo_button.Tag).Value);
            });
            

            newFragment.Show(ft, "dialog");
            Log.Debug(TAG, $"{nameof(OnListItemClick)} - try to show ReportFragment like dialog - END");

        }
    }
}