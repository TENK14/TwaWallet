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

namespace TwaWallet.Fragments
{
    public class RecurringPaymentsFragment : Fragment
    {
        #region Members
        private const string TAG = "X:" + nameof(RecurringPaymentsFragment);
        private IDataContext db;

        List<RecurringPayment> listData;
        #region GUI
        Button addRegularPayment_button;
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

            addRegularPayment_button = v.FindViewById<Button>(Resource.Id.addRegularPayment_button);
            addRegularPayment_button.Click += AddRegularPayment_button_Click;

            listView = v.FindViewById<ListView>(Resource.Id.regularPayments_listView);

            return v;
        }

        private void AddRegularPayment_button_Click(object sender, EventArgs e)
        {
            Log.Debug(TAG, nameof(AddRegularPayment_button_Click));

            FragmentTransaction ft = FragmentManager.BeginTransaction();
            Fragment prev = FragmentManager.FindFragmentByTag("dialog");
            if (prev != null)
            {
                ft.Remove(prev);
            }
            ft.AddToBackStack(null);

            Log.Debug(TAG, $"{nameof(AddRegularPayment_button_Click)} - try to show ReportFragment like dialog - START");
            // Create and show the dialog.
            DialogFragment newFragment = RecurringPaymentFragment.NewInstance(null, delegate ()
            {
                LoadData();
            });


            newFragment.Show(ft, "dialog");
            Log.Debug(TAG, $"{nameof(AddRegularPayment_button_Click)} - try to show ReportFragment like dialog - END");
        }

        public override void OnResume()
        {
            Log.Debug(TAG, nameof(OnResume));

            base.OnResume();

            LoadData();
            //InitLayout();
        }

        private void LoadData()
        {
            Log.Debug(TAG, nameof(LoadData));

            var r = db.Select<RecurringPayment, int>((o) => o.Id > 0, (o) => o.Id, false).Result;
            listData = r.ToList();
        }

        
    }
}