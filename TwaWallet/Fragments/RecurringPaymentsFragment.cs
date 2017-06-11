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
using com.refractored.fab;
using System.Threading.Tasks;
using TwaWallet.Extensions;

namespace TwaWallet.Fragments
{
    public class RecurringPaymentsFragment : DialogFragment
    {
        #region Members
        private const string TAG = "X:" + nameof(RecurringPaymentsFragment);
        private IDataContext db;

        List<RecurringPayment> listData;
        #region GUI
        protected Android.App.ProgressDialog dialog = null;
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
            View v = inflater.Inflate(Resource.Layout.RecurringPayments, container, false);

            listView = v.FindViewById<ListView>(Resource.Id.recurringPayments_listView);
            listView.ItemLongClick += OnListItemLongClick;

            var fab = v.FindViewById<com.refractored.fab.FloatingActionButton>(Resource.Id.fab);
            fab.AttachToListView(listView);
            fab.Click += AddRecurringPayment_button_Click;

            return v;
        }
        
        public override void OnResume()
        {
            Log.Debug(TAG, nameof(OnResume));

            base.OnResume();
            
            var r = LoadData();
            InitLayout();
        }

        private bool LoadData()
        {
            Log.Debug(TAG, nameof(LoadData));

            this.Activity.RunOnUiThread(() =>
            {
                Log.Debug(TAG, "[1] Starting dialog.");
                dialog = this.Activity.ProgressDialogShow(dialog);
                Log.Debug(TAG, "[2] Dialog started.");
            });

            try
            {
                var r = db.Select<RecurringPayment, int>((o) => o.Id > 0, (o) => o.Id, false).Result;
                listData = r.ToList();

                listView.Adapter = new RecurringPaymentListAdapter(this.Activity, listData, this.db);

                return true;
            }
            catch (Exception ex)
            {
                Log.Error(TAG, ex.ToString());
                throw;
            }
            finally
            {
                this.Activity.ProgressDialogDismiss(dialog);
            }

            return false;
        }

        private void InitLayout()
        {
            Log.Debug(TAG, nameof(InitLayout));

            listView.Adapter = new RecurringPaymentListAdapter(this.Activity, listData, this.db);
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
                var r = LoadData();
            });
            Log.Debug(TAG, $"{nameof(AddRecurringPayment_button_Click)} - 1");

            newFragment.Show(ft, "dialog");
            Log.Debug(TAG, $"{nameof(AddRecurringPayment_button_Click)} - try to show ReportFragment like dialog - END");
        }

        private void OnListItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            Log.Debug(TAG, nameof(OnListItemLongClick));

            var item = listData.ElementAt(e.Position);

            Android.App.AlertDialog.Builder builder = new Android.App.AlertDialog.Builder(this.Activity);
            builder.SetMessage(item.ToString())
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
                        var r = LoadData();
                    });


                    newFragment.Show(ft, "dialog");
                    Log.Debug(TAG, $"{nameof(OnListItemLongClick)} - try to show ReportFragment like dialog - END");
                })
                .SetNegativeButton(this.Activity.Resources.GetString(Resource.String.Delete), (s, args) =>
                {
                    if (db.Delete(item).Result)
                    {
                        Toast.MakeText(this.Activity, Resources.GetString(Resource.String.Deleted), ToastLength.Short).Show();

                        var r = LoadData();
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