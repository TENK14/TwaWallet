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
using Android.Support.Design.Widget;
using com.refractored.fab;

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
        CheckBox earnings_checkBox;
        CheckBox costs_checkBox;
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
            //db = new DataContext(pathToDatabase);
            db = DataContextFactory.GetDataContext(pathToDatabase);

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
            listView.ItemLongClick += OnListItemLongClick;

            count_value = v.FindViewById<TextView>(Resource.Id.count_value);
            filterCost_value = v.FindViewById<TextView>(Resource.Id.filterCost_value);
            monthCost_value = v.FindViewById<TextView>(Resource.Id.monthCost_value);
            earnings_checkBox = v.FindViewById<CheckBox>(Resource.Id.earnings_checkBox);
            earnings_checkBox.CheckedChange += Earnings_checkBox_CheckedChange;
            earnings_checkBox.Checked = false;

            costs_checkBox = v.FindViewById<CheckBox>(Resource.Id.costs_checkBox);
            costs_checkBox.CheckedChange += Costs_checkBox_CheckedChange;
            costs_checkBox.Checked = true;


            dateFrom_button = v.FindViewById<Button>(Resource.Id.dateFrom_button);
            dateFrom_button.Click += DateFrom_button_Click;
            dateTo_button = v.FindViewById<Button>(Resource.Id.dateTo_button);
            dateTo_button.Click += DateTo_button_Click;

            var fab = v.FindViewById<com.refractored.fab.FloatingActionButton>(Resource.Id.fab);
            //fab.AttachToListView(listView, this, this);
            fab.AttachToListView(listView);
            //fab.Click += (sender, args) =>
            //{
            //    Toast.MakeText(Activity, "FAB Clicked!", ToastLength.Short).Show();                
            //};
            fab.Click += Fab_Click;

            LoadData();

            InitLayout();

            return v;
        }

        private void Fab_Click(object sender, EventArgs e)
        {
            Log.Debug(TAG, $"{nameof(Fab_Click)} - try to show ReportFragment like dialog - START");

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

            
            // Create and show the dialog.
            DialogFragment newFragment = ReportFragment.NewInstance(null, delegate ()
            {
                LoadData(((JavaLangObjectWrapper<DateTime>)dateFrom_button.Tag).Value, ((JavaLangObjectWrapper<DateTime>)dateTo_button.Tag).Value, costs_checkBox.Checked, earnings_checkBox.Checked);
            });


            newFragment.Show(ft, "dialog");

            Log.Debug(TAG, $"{nameof(Fab_Click)} - try to show ReportFragment like dialog - END");
        }

        private void Costs_checkBox_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            Log.Debug(TAG, nameof(Costs_checkBox_CheckedChange));

            if (dateFrom_button?.Tag != null)
            {
                LoadData(((JavaLangObjectWrapper<DateTime>)dateFrom_button.Tag).Value, ((JavaLangObjectWrapper<DateTime>)dateTo_button.Tag).Value, costs_checkBox.Checked, earnings_checkBox.Checked);
            }
            else
            {
                LoadData();
            }
        }

        private void Earnings_checkBox_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            Log.Debug(TAG, nameof(Earnings_checkBox_CheckedChange));

            
            if (dateFrom_button?.Tag != null)
            {
                LoadData(((JavaLangObjectWrapper<DateTime>)dateFrom_button.Tag).Value, ((JavaLangObjectWrapper<DateTime>)dateTo_button.Tag).Value, costs_checkBox.Checked, earnings_checkBox.Checked);
            }
            else
            {
                LoadData();
            }

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

            //var r = db.Select<Record, DateTime>((o) => o.Id > 0, (o) => o.Date, false).Result;
            var r = db.Select<Record, DateTime>((o) => o.Id > 0 && o.Earnings == false, (o) => o.Date, false).Result;
            listData = r.ToList();//r.Select(p => $"{p.Description}, {p.Cost}, {p.Date}").ToList();

            
        }

        private void LoadData(DateTime dateFrom, DateTime dateTo, bool includeCosts, bool includeEarnings)
        {
            Log.Debug(TAG, nameof(LoadData));

            // Refresh review
            var rAll = db.Select<Record, DateTime>((o) => o.Id > 0, (o) => o.Date, false).Result;
            var listAll = rAll.ToList();

            //var r = db.Select<Record, DateTime>((o) => o.Date >= dateFrom && o.Date <= dateTo, (o) => o.Date, false).Result;
            //listData = r.ToList();//r.Select(p => $"{p.Description}, {p.Cost}, {p.Date}").ToList();

            if ( listAll != null && listAll.Count > 0)
            {
                

                // Filtered values
                if (includeCosts == false && includeEarnings == false)
                {
                    listData = new List<Record>();
                    // Month values
                    this.monthCost_value.Text = Convert.ToString(0);
                }
                else if (includeCosts == false && includeEarnings == true)
                {
                    listData = listAll.Where((o) => o.Date.Date >= dateFrom.Date && o.Date.Date <= dateTo.Date && o.Earnings == true).OrderByDescending(o => o.Date).ToList();
                    // Month values
                    this.monthCost_value.Text = listAll.Where(p => p.Date.Month == DateTime.Now.Month && p.Date.Year == DateTime.Now.Year && p.Earnings == true).Select(p => p.Cost).Sum().ToString();
                }
                else if (includeCosts == true && includeEarnings == false)
                {
                    listData = listAll.Where((o) => o.Date.Date >= dateFrom.Date && o.Date.Date <= dateTo.Date && o.Earnings == false).OrderByDescending(o => o.Date).ToList();
                    // Month values
                    this.monthCost_value.Text = listAll.Where(p => p.Date.Month == DateTime.Now.Month && p.Date.Year == DateTime.Now.Year && p.Earnings == false).Select(p => p.Cost).Sum().ToString();
                }
                else if (includeCosts == true && includeEarnings == true)
                {
                    listData = listAll.Where((o) => o.Date.Date >= dateFrom.Date && o.Date.Date <= dateTo.Date).OrderByDescending(o => o.Date).ToList();
                    // Month values
                    this.monthCost_value.Text = listAll.Where(p => p.Date.Month == DateTime.Now.Month && p.Date.Year == DateTime.Now.Year).Select(p => p.Cost).Sum().ToString();
                }

                if (listData != null && listData.Count > 0)
                {
                    this.count_value.Text = listData.Count.ToString();
                    this.filterCost_value.Text = listData.Select(p => p.Cost).Sum().ToString();
                }
            }
            else
            {
                this.monthCost_value.Text = Convert.ToString(0);
                listData = new List<Record>();
            }

            // Refresh review
            this.count_value.Text = listData.Count.ToString();
            this.filterCost_value.Text = listData.Select(p => p.Cost).Sum().ToString();

            listView.Adapter = new RecordListAdapter(this.Activity, listData, db);
        }

        

        private void DateTo_button_Click(object sender, EventArgs e)
        {
            Log.Debug(TAG, nameof(DateTo_button_Click));

            DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime date)
            {
                dateTo_button.Text = date.ToString(Resources.GetString(Resource.String.DateFormat)); //.ToLongDateString();
                dateTo_button.Tag = new JavaLangObjectWrapper<DateTime>(date);

                LoadData(((JavaLangObjectWrapper<DateTime>)dateFrom_button.Tag).Value, ((JavaLangObjectWrapper<DateTime>)dateTo_button.Tag).Value, costs_checkBox.Checked, earnings_checkBox.Checked);
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

                LoadData(((JavaLangObjectWrapper<DateTime>)dateFrom_button.Tag).Value, ((JavaLangObjectWrapper<DateTime>)dateTo_button.Tag).Value, costs_checkBox.Checked, earnings_checkBox.Checked);
            });
            frag.Show(this.Activity.FragmentManager, DatePickerFragment.TAG);

            
        }

        private void InitLayout()
        {
            Log.Debug(TAG, nameof(InitLayout));

            listView.Adapter = new RecordListAdapter(this.Activity, listData, this.db);
            
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
            //Toast.MakeText(this.Activity, $"You press: {item.Description}", ToastLength.Short).Show();

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
                LoadData(((JavaLangObjectWrapper<DateTime>)dateFrom_button.Tag).Value, ((JavaLangObjectWrapper<DateTime>)dateTo_button.Tag).Value, costs_checkBox.Checked, earnings_checkBox.Checked);
            });
            

            newFragment.Show(ft, "dialog");
            Log.Debug(TAG, $"{nameof(OnListItemClick)} - try to show ReportFragment like dialog - END");

        }

        private void OnListItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            Log.Debug(TAG, nameof(OnListItemLongClick));

            var item = listData.ElementAt(e.Position);

            Android.App.AlertDialog.Builder builder = new Android.App.AlertDialog.Builder(this.Activity);
            builder.SetMessage(item.ToString())
                //.SetPositiveButton("Yes", dialogClickListener)
                .SetPositiveButton(this.Activity.Resources.GetString(Resource.String.Edit), (s, args) =>
                {
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

                    Log.Debug(TAG, $"{nameof(OnListItemLongClick)} - try to show ReportFragment like dialog - START");
                    // Create and show the dialog.
                    DialogFragment newFragment = ReportFragment.NewInstance(item.IncludeObjects(db), delegate ()
                    {
                        LoadData(((JavaLangObjectWrapper<DateTime>)dateFrom_button.Tag).Value, ((JavaLangObjectWrapper<DateTime>)dateTo_button.Tag).Value, costs_checkBox.Checked, earnings_checkBox.Checked);
                    });


                    newFragment.Show(ft, "dialog");
                    Log.Debug(TAG, $"{nameof(OnListItemLongClick)} - try to show ReportFragment like dialog - END");
                })
                //.SetNegativeButton("No", dialogClickListener)
                .SetNegativeButton(this.Activity.Resources.GetString(Resource.String.Delete), (s, args) =>
                {
                    if (db.Delete(item).Result)
                    {
                        Toast.MakeText(this.Activity, Resources.GetString(Resource.String.Deleted), ToastLength.Short).Show();

                        LoadData(((JavaLangObjectWrapper<DateTime>)dateFrom_button.Tag).Value, ((JavaLangObjectWrapper<DateTime>)dateTo_button.Tag).Value, costs_checkBox.Checked, earnings_checkBox.Checked);
                    }
                    else
                    {
                        Toast.MakeText(this.Activity, Resources.GetString(Resource.String.WasntDeleted), ToastLength.Short).Show();
                    }
                    
                })
                .Show();

        }
    }
}