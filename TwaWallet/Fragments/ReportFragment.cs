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

namespace TwaWallet.Fragments
{
    public class ReportFragment : Fragment
    {
        private const string TAG = "X:" + nameof(ReportFragment);

        #region GUI
        Button category_button;
        Button paymentType_button;
        Button owner_button;
        EditText cost_editText;
        CheckBox earnings_checkBox;
        EditText description_editText;
        Button date_button;
        Button warranty_button;
        Button save_button;
        #endregion


        public override void OnCreate(Bundle savedInstanceState)
        {
            Log.Debug(TAG, nameof(OnCreate));

            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            Log.Debug(TAG, nameof(OnCreateView));

            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);

            //return base.OnCreateView(inflater, container, savedInstanceState);
            //return inflater.Inflate(Resource.Layout.Report, container, false);

            View v = inflater.Inflate(Resource.Layout.Report, container, false);

            category_button = v.FindViewById<Button>(Resource.Id.category_button);
            paymentType_button = v.FindViewById<Button>(Resource.Id.paymentType_button);
            owner_button = v.FindViewById<Button>(Resource.Id.owner_button);

            cost_editText = v.FindViewById<EditText>(Resource.Id.cost_editText);
            earnings_checkBox = v.FindViewById<CheckBox>(Resource.Id.earnings_checkBox);

            description_editText = v.FindViewById<EditText>(Resource.Id.description_editText);

            date_button = v.FindViewById<Button>(Resource.Id.date_button);
            warranty_button = v.FindViewById<Button>(Resource.Id.warranty_button);
            save_button = v.FindViewById<Button>(Resource.Id.save_button);
            save_button.Click += Save_button_Click;

            return v;
        }

        private void Save_button_Click(object sender, EventArgs e)
        {

            //this.Activity.RunOnUiThread(() =>
            //{
               Toast.MakeText(this.Activity, "You clicked on me!", ToastLength.Short).Show();
            //});
        }
    }
}