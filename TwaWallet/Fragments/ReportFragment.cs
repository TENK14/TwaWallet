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
using Database.POCO;
using Database;
using System.Threading.Tasks;

namespace TwaWallet.Fragments
{
    public class ReportFragment : Fragment
    {
        #region Members
        private const string TAG = "X:" + nameof(ReportFragment);

        private DataContext db;

        List<Owner> lstOwner;
        List<PaymentType> lstPaymentType;
        List<Category> lstCategory;

        #region GUI
        Button category_button;
        Button paymentType_button;
        Button owner_button;
        EditText cost_editText;
        CheckBox earnings_checkBox;
        EditText description_editText;
        Button date_button;
        Button warranty_button;
        EditText tags_editText;
        Button save_button;
        #endregion
        #endregion

        public override void OnCreate(Bundle savedInstanceState)
        {
            Log.Debug(TAG, nameof(OnCreate));

            base.OnCreate(savedInstanceState);

            //var docsFolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
            //var dbPath = Resources.GetString(Resource.String.DBPath);
            //var pathToDatabase = System.IO.Path.Combine(dbPath, Resources.GetString(Resource.String.DBfilename));// "db_sqlcompnet.db");

            string pathToDatabase = DeviceInfo.GetFileFinallPath(Resources.GetString(Resource.String.DBfilename));
            db = new DataContext(pathToDatabase);

            LoadData();

            // Create your fragment here
        }

        private void LoadData()
        {
            Log.Debug(TAG, nameof(LoadData));

            //throw new NotImplementedException();

            //Task.Run(() =>
            //{
            Log.Debug(TAG, $"{nameof(LoadData)}-1");
            //var r = db.Select<Owner>($"{nameof(Owner.Id)} > 0", $"{nameof(Owner.Id)}").Result;
            var r = db.Select<Owner,int>((o) => o.Id > 0, (o) => o.Id).Result;
            lstOwner = r.ToList();
            Log.Debug(TAG, $"{nameof(LoadData)}-2");
            //lstPaymentType = db.Select<PaymentType>($"{nameof(PaymentType.Id)} > 0", $"{nameof(PaymentType.Id)}").Result.ToList();
            var r2 = db.Select<PaymentType, int>((o) => o.Id > 0, (o) => o.Id).Result;
            lstPaymentType = r2.ToList();
            Log.Debug(TAG, $"{nameof(LoadData)}-3");
            //lstCategory = db.Select<Category>($"{nameof(Category.Id)} > 0", $"{nameof(Category.Id)}").Result.ToList();
            var r3 = db.Select<Category, int>((o) => o.Id > 0, (o) => o.Id).Result;
            lstCategory = r3.ToList();
            Log.Debug(TAG, $"{nameof(LoadData)}-4");

            this.Activity.RunOnUiThread(() =>
                {
                    Toast.MakeText(this.Activity, $"owners: {lstOwner.Count}, paymentTypes: {lstPaymentType.Count}, categories: {lstCategory.Count}", ToastLength.Short);
                });
            //});
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
            category_button.Text = lstCategory?.First()?.Description ?? "nic";

            paymentType_button = v.FindViewById<Button>(Resource.Id.paymentType_button);
            paymentType_button.Text = lstPaymentType?.First()?.Description ?? "nic";

            owner_button = v.FindViewById<Button>(Resource.Id.owner_button);
            owner_button.Text = lstOwner?.First()?.Name ?? "nic";

            cost_editText = v.FindViewById<EditText>(Resource.Id.cost_editText);
            earnings_checkBox = v.FindViewById<CheckBox>(Resource.Id.earnings_checkBox);

            description_editText = v.FindViewById<EditText>(Resource.Id.description_editText);

            date_button = v.FindViewById<Button>(Resource.Id.date_button);
            warranty_button = v.FindViewById<Button>(Resource.Id.warranty_button);
            tags_editText = v.FindViewById<EditText>(Resource.Id.tags_editText);
            save_button = v.FindViewById<Button>(Resource.Id.save_button);
            save_button.Click += Save_button_Click;

            return v; 
        }

        private void Save_button_Click(object sender, EventArgs e)
        {
            Log.Debug(TAG, nameof(Save_button_Click));

            //this.Activity.RunOnUiThread(() =>
            //{
            Toast.MakeText(this.Activity, "You clicked on me!", ToastLength.Short).Show();
            //});

            var record = new Record
            {
                CategoryId = lstCategory?.First()?.Id ?? 0, //this.category_button.Text,
                Cost = int.Parse(this.cost_editText.Text),
                DateCreated = DateTime.Now,
                Description = this.description_editText.Text,
                OwnerId = lstOwner?.First()?.Id ?? 0, //this.owner_button.Text,
                PaymantTypeId = lstPaymentType?.First()?.Id ?? 0, //this.paymentType_button.Text,
                Tag = this.tags_editText.Text,
                Warranty = 0, //int.Parse(this.warranty_button.Text),
            };

            if (db.Insert(record).Result)
            {
                Toast.MakeText(this.Activity, record.ToString(), ToastLength.Short).Show();
            }
            else
            {
                Toast.MakeText(this.Activity, "Wasnt saved", ToastLength.Short).Show();
            }

            //Toast.MakeText(this.Activity, db.Insert<Record>(record).Result,ToastLength.Short); //.RunSynchronously();


        }
    }
}