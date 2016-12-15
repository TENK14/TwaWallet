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
using TwaWallet.Extensions;
using TwaWallet.Classes;

namespace TwaWallet.Fragments
{
    public class ReportFragment : Fragment
    {
        #region Members
        private const string TAG = "X:" + nameof(ReportFragment);

        private IDataContext db;

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
        //Button warranty_button;
        EditText warranty_editText;
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
            //category_button.Text = lstCategory?.First()?.Description ?? "nic";
            category_button.Click += Category_button_Click;

            paymentType_button = v.FindViewById<Button>(Resource.Id.paymentType_button);
            //paymentType_button.Text = lstPaymentType?.First()?.Description ?? "nic";
            paymentType_button.Click += PaymentType_button_Click;

            owner_button = v.FindViewById<Button>(Resource.Id.owner_button);
            //owner_button.Text = lstOwner?.First()?.Name ?? "nic";
            owner_button.Click += Owner_button_Click;

            cost_editText = v.FindViewById<EditText>(Resource.Id.cost_editText);
            earnings_checkBox = v.FindViewById<CheckBox>(Resource.Id.earnings_checkBox);

            description_editText = v.FindViewById<EditText>(Resource.Id.description_editText);

            date_button = v.FindViewById<Button>(Resource.Id.date_button);
            date_button.Click += Date_button_Click;

            //warranty_button = v.FindViewById<Button>(Resource.Id.warranty_button);
            warranty_editText = v.FindViewById<EditText>(Resource.Id.warranty_editText);

            tags_editText = v.FindViewById<EditText>(Resource.Id.tags_editText);

            save_button = v.FindViewById<Button>(Resource.Id.save_button);
            save_button.Click += Save_button_Click;

            InitLayout();

            return v; 
        }

        private void InitLayout()
        {
            Log.Debug(TAG, nameof(InitLayout));

            this.category_button.Text = lstCategory.FirstOrDefault().Description;
            this.category_button.Tag = new JavaLangObjectWrapper<Category>(lstCategory.FirstOrDefault());
            this.paymentType_button.Text = lstPaymentType.FirstOrDefault().Description;
            this.paymentType_button.Tag = new JavaLangObjectWrapper<PaymentType>(lstPaymentType.FirstOrDefault());
            this.owner_button.Text = lstOwner.FirstOrDefault().Name;
            this.owner_button.Tag = new JavaLangObjectWrapper<Owner>(lstOwner.FirstOrDefault());

            this.earnings_checkBox.Checked = false;

            this.cost_editText.Text = string.Empty;
            this.description_editText.Text = string.Empty;
            this.warranty_editText.Text = string.Empty;
            this.tags_editText.Text = string.Empty;

            var date = DateTime.Now;
            this.date_button.Text = date.ToString(Resources.GetString(Resource.String.DateFormat));
            this.date_button.Tag = new JavaLangObjectWrapper<DateTime>(date);
        }


        #region Button click

        private void Owner_button_Click(object sender, EventArgs e)
        {
            Log.Debug(TAG, nameof(Owner_button_Click));

            var fr = SimpleListViewDialogFragment<Owner>.NewInstance(lstOwner, delegate (Owner selectedItem)
            {
                owner_button.Text = selectedItem.Name;
                owner_button.Tag = new JavaLangObjectWrapper<Owner>(selectedItem);
            });
            fr.Show(this.Activity.FragmentManager, SimpleListViewDialogFragment<Owner>.TAG);
        }

        private void PaymentType_button_Click(object sender, EventArgs e)
        {
            Log.Debug(TAG, nameof(PaymentType_button_Click));

            var fr = SimpleListViewDialogFragment<PaymentType>.NewInstance(lstPaymentType, delegate (PaymentType selectedItem)
            {
                paymentType_button.Text = selectedItem.Description;
                paymentType_button.Tag = new JavaLangObjectWrapper<PaymentType>(selectedItem);
            });
            fr.Show(this.Activity.FragmentManager, SimpleListViewDialogFragment<PaymentType>.TAG);
        }

        private void Category_button_Click(object sender, EventArgs e)
        {
            Log.Debug(TAG, nameof(Category_button_Click));

            var fr = SimpleListViewDialogFragment<Category>.NewInstance(lstCategory, delegate (Category selectedItem)
             {
                 category_button.Text = selectedItem.Description;
                //category_button.SetFlags();
                //category_button.Tag = /*"str";//*/ // (Java.Lang.Object)c;
                category_button.Tag = new JavaLangObjectWrapper<Category>(selectedItem);
             });
            fr.Show(this.Activity.FragmentManager, SimpleListViewDialogFragment<Category>.TAG);

        }

        private void Date_button_Click(object sender, EventArgs e)
        {
            Log.Debug(TAG, nameof(Date_button_Click));

            DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime date)
            {
                date_button.Text = date.ToString(Resources.GetString(Resource.String.DateFormat)); //.ToLongDateString();
                date_button.Tag = new JavaLangObjectWrapper<DateTime>(date);
            });
            frag.Show(this.Activity.FragmentManager, DatePickerFragment.TAG);
        }

        private void Save_button_Click(object sender, EventArgs e)
        {
            Log.Debug(TAG, nameof(Save_button_Click));
            
            try
            {
                int cId = ((JavaLangObjectWrapper<Category>)category_button.Tag).Value.Id;
                int oId = ((JavaLangObjectWrapper<Owner>)owner_button.Tag).Value.Id;
                int pId = ((JavaLangObjectWrapper<PaymentType>)paymentType_button.Tag).Value.Id;

                int warranty;
                if (!int.TryParse(warranty_editText.Text, out warranty))
                {
                    warranty = 0;
                }

                float cost = float.Parse(this.cost_editText.Text);

                var record = new Record
                {
                    CategoryId = cId, // lstCategory?.First()?.Id ?? 0, //this.category_button.Text,
                    Cost = this.earnings_checkBox.Checked ? cost : 0f - cost,
                    Date = ((JavaLangObjectWrapper<DateTime>)this.date_button.Tag).Value, // DateTime.Now,
                    Description = this.description_editText.Text,
                    OwnerId = oId, //lstOwner?.First()?.Id ?? 0, //this.owner_button.Text,
                    PaymentTypeId = pId, //lstPaymentType?.First()?.Id ?? 0, //this.paymentType_button.Text,
                    Tag = this.tags_editText.Text,
                    Warranty = warranty, // 0, //int.Parse(this.warranty_button.Text),
                    Earnings = this.earnings_checkBox.Checked
                };

                #region Ask for permission
                //const int REQUEST_CODE_ASK_PERMISSIONS = 123;
                //const string permission = Android.Manifest.Permission.WriteExternalStorage;
                //var hasWriteContactsPermission = Android.Content.ContextWrapper.CheckSelfPermission(permission);

                //if (hasWriteContactsPermission != Android.Content.PM.Permission.Granted)
                //{
                //    RequestPermissions(new string[] { Android.Manifest.Permission.WriteExternalStorage },
                //            REQUEST_CODE_ASK_PERMISSIONS);
                //    return;
                //} 
                #endregion

                if (db.Insert(record).Result)
                {
                    Toast.MakeText(this.Activity, record.ToString(), ToastLength.Short).Show();
                    InitLayout();
                }
                else
                {
                    Toast.MakeText(this.Activity, "Wasnt saved", ToastLength.Short).Show();
                }
                               
            }
            catch (Exception ex)
            {
                Log.Error(TAG, nameof(Save_button_Click), ex.Message);
                Toast.MakeText(this.Activity, ex.Message, ToastLength.Short).Show();
            }

        }

        
        #endregion
    }
}