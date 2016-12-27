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
using Database;
using Database.POCO;
using TwaWallet.Adapters;

namespace TwaWallet.Fragments
{
    public class RecurringPaymentsFragment : DialogFragment
    {
        #region Members
        private const string TAG = "X:" + nameof(RecurringPaymentsFragment);
        private IDataContext db;

        List<RecurringPayment> listData;
        #region GUI
        Button addRecurringPayment_button;
        ListView listView;
        #endregion
        #endregion



        public override void OnCreate(Bundle savedInstanceState)
        {
            Log.Debug(TAG, nameof(OnCreate));

            base.OnCreate(savedInstanceState);

            // Create your fragment here
            string pathToDatabase = DeviceInfo.GetFileFinallPath(Resources.GetString(Resource.String.DBfilename));
            db = DataContextFactory.GetDataContext(pathToDatabase);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            Log.Debug(TAG, nameof(OnCreateView));

            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);

            //return base.OnCreateView(inflater, container, savedInstanceState);
            View v = inflater.Inflate(Resource.Layout.RecurringPayments, container, false);

            addRecurringPayment_button = v.FindViewById<Button>(Resource.Id.addRecurringPayment_button);
            addRecurringPayment_button.Click += AddRecurringPayment_button_Click;

            listView = v.FindViewById<ListView>(Resource.Id.regularPayments_listView);
            listView.ItemLongClick += OnListItemLongClick;

            return v;
        }

        private void AddRecurringPayment_button_Click(object sender, EventArgs e)
        {
            Log.Debug(TAG, nameof(AddRecurringPayment_button_Click));

            FragmentTransaction ft = FragmentManager.BeginTransaction();
            Fragment prev = FragmentManager.FindFragmentByTag("dialog");
            if (prev != null)
            {
                ft.Remove(prev);
            }
            ft.AddToBackStack(null);

            Log.Debug(TAG, $"{nameof(AddRecurringPayment_button_Click)} - try to show RecurringPaymentFragment like dialog - START");
            // Create and show the dialog.
            DialogFragment newFragment = RecurringPaymentFragment.NewInstance(null, delegate ()
            {
                LoadData();
            });
            Log.Debug(TAG, $"{nameof(AddRecurringPayment_button_Click)} - 1");

            newFragment.Show(ft, "dialog");
            Log.Debug(TAG, $"{nameof(AddRecurringPayment_button_Click)} - try to show ReportFragment like dialog - END");
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

            var r = db.Select<RecurringPayment, int>((o) => o.Id > 0, (o) => o.Id, false).Result;
            listData = r.ToList();

            listView.Adapter = new RecurringPaymentListAdapter(this.Activity, listData, this.db);
        }

        private void InitLayout()
        {
            Log.Debug(TAG, nameof(InitLayout));

            listView.Adapter = new RecurringPaymentListAdapter(this.Activity, listData, this.db);

            //if (listData != null && listData.Count > 0)
            //{
            //    var date = listData.LastOrDefault().Date;
            //    dateFrom_button.Text = date.ToString(Resources.GetString(Resource.String.DateFormat));
            //    dateFrom_button.Tag = new JavaLangObjectWrapper<DateTime>(date);

            //    date = listData.FirstOrDefault().Date;
            //    dateTo_button.Text = date.ToString(Resources.GetString(Resource.String.DateFormat));
            //    dateTo_button.Tag = new JavaLangObjectWrapper<DateTime>(date);

            //    this.count_value.Text = listData.Count.ToString();
            //    this.monthCost_value.Text = listData.Where(p => p.Date.Month == DateTime.Now.Month && p.Date.Year == DateTime.Now.Year).Select(p => p.Cost).Sum().ToString();
            //    this.filterCost_value.Text = listData.Select(p => p.Cost).Sum().ToString();
            //}
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
                    DialogFragment newFragment = RecurringPaymentFragment.NewInstance(item.IncludeObjects(db), delegate ()
                    {
                        LoadData();
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

                        LoadData();
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