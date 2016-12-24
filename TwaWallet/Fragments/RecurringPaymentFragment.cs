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
using TwaWallet.Classes;

namespace TwaWallet.Fragments
{
    public class RecurringPaymentFragment : DialogFragment
    {

        #region Members
        private const string TAG = "X:" + nameof(RecurringPaymentFragment);
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
        Button frequency_button;
        Button endDate_button;
        EditText warranty_editText;
        //EditText tags_editText;
        CheckBox isActive_checkBox;
        Button save_button;
        #endregion
        #endregion

        #region Properties
        public RecurringPayment SelectedItem { get; set; } = null;
        #endregion

        #region DialogFragment
        // Initialize this value to prevent NullReferenceExceptions.
        Action onContinueWithHandler = delegate { };

        public static RecurringPaymentFragment NewInstance(RecurringPayment selectedItem, Action onContinueWith)
        {
            Log.Debug(TAG, $"{nameof(NewInstance)} - {nameof(selectedItem)}:{selectedItem.ToString()}");

            var frag = new RecurringPaymentFragment();
            frag.SelectedItem = selectedItem;
            frag.onContinueWithHandler = onContinueWith;
            return frag;
        }
        #endregion

        public override void OnCreate(Bundle savedInstanceState)
        {
            Log.Debug(TAG, nameof(OnCreate));

            base.OnCreate(savedInstanceState);

            // Create your fragment here
            string pathToDatabase = DeviceInfo.GetFileFinallPath(Resources.GetString(Resource.String.DBfilename));
            db = DataContextFactory.GetDataContext(pathToDatabase);

            LoadData();
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);

            View v = inflater.Inflate(Resource.Layout.RecurringPayment, container, false);

            category_button = v.FindViewById<Button>(Resource.Id.category_button);
            category_button.Click += Category_button_Click;

            paymentType_button = v.FindViewById<Button>(Resource.Id.paymentType_button);
            paymentType_button.Click += PaymentType_button_Click;

            owner_button = v.FindViewById<Button>(Resource.Id.owner_button);
            owner_button.Click += Owner_button_Click;

            cost_editText = v.FindViewById<EditText>(Resource.Id.cost_editText);

            earnings_checkBox = v.FindViewById<CheckBox>(Resource.Id.earnings_checkBox);

            description_editText = v.FindViewById<EditText>(Resource.Id.description_editText);

            this.frequency_button = v.FindViewById<Button>(Resource.Id.frequency_button);
            this.frequency_button.Click += Frequency_button_Click;

            endDate_button = v.FindViewById<Button>(Resource.Id.date_button);
            endDate_button.Click += EndDate_button_Click;

            //warranty_button = v.FindViewById<Button>(Resource.Id.warranty_button);
            warranty_editText = v.FindViewById<EditText>(Resource.Id.warranty_editText);

            //tags_editText = v.FindViewById<EditText>(Resource.Id.tags_editText);

            isActive_checkBox = v.FindViewById<CheckBox>(Resource.Id.isActive_checkBox);

            save_button = v.FindViewById<Button>(Resource.Id.save_button);
            save_button.Click += Save_button_Click;

            InitLayout();

            return v;


        }

        private void Frequency_button_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Save_button_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void EndDate_button_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Owner_button_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void PaymentType_button_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Category_button_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
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

            var r = db.Select<Owner, int>((o) => o.Id > 0, (o) => o.Id).Result;
            lstOwner = r.ToList();

            var r2 = db.Select<PaymentType, int>((o) => o.Id > 0, (o) => o.Id).Result;
            lstPaymentType = r2.ToList();

            var r3 = db.Select<Category, int>((o) => o.Id > 0, (o) => o.Id).Result;
            lstCategory = r3.ToList();

        }

        private void InitLayout()
        {
            Log.Debug(TAG, nameof(InitLayout));

            if (SelectedItem == null) // Show Empty ReportFragment
            {
                var category = lstCategory.Where(p => p.IsDefault).FirstOrDefault();
                this.category_button.Text = category.Description;
                this.category_button.Tag = new JavaLangObjectWrapper<Category>(category);

                var paymentType = lstPaymentType.Where(p => p.IsDefault).FirstOrDefault();
                this.paymentType_button.Text = paymentType.Description;
                this.paymentType_button.Tag = new JavaLangObjectWrapper<PaymentType>(paymentType);

                var owner = lstOwner.Where(p => p.IsDefault).FirstOrDefault();
                this.owner_button.Text = owner.Name;
                this.owner_button.Tag = new JavaLangObjectWrapper<Owner>(owner);

                this.earnings_checkBox.Checked = false;

                this.cost_editText.Text = string.Empty;
                this.description_editText.Text = string.Empty;
                this.warranty_editText.Text = string.Empty;
                //this.tags_editText.Text = string.Empty;

                // TODO: vyres frequency button 
                //this.frequency_button = 

                var date = DateTime.Now;
                this.endDate_button.Text = date.ToString(Resources.GetString(Resource.String.DateFormat));
                this.endDate_button.Tag = new JavaLangObjectWrapper<DateTime>(date);

                this.isActive_checkBox.Checked = true; 
            }
            else // Show details about some history item
            {
                this.category_button.Text = SelectedItem?.Category?.Description ?? string.Empty;
                this.category_button.Tag = SelectedItem != null ? new JavaLangObjectWrapper<Category>(SelectedItem.Category) : null;
                this.paymentType_button.Text = SelectedItem?.PaymentType?.Description ?? string.Empty;
                this.paymentType_button.Tag = SelectedItem != null ? new JavaLangObjectWrapper<PaymentType>(SelectedItem.PaymentType) : null;
                this.owner_button.Text = SelectedItem?.Owner?.Name ?? string.Empty;
                this.owner_button.Tag = SelectedItem != null ? new JavaLangObjectWrapper<Owner>(SelectedItem.Owner) : null;


                this.earnings_checkBox.Checked = SelectedItem?.Earnings ?? false;

                if (this.earnings_checkBox.Checked)
                {
                    this.cost_editText.Text = (SelectedItem?.Cost).ToString() ?? string.Empty;
                }
                else
                {
                    this.cost_editText.Text = (0 - SelectedItem?.Cost).ToString() ?? string.Empty;
                }
                this.description_editText.Text = SelectedItem?.Description ?? string.Empty;
                this.warranty_editText.Text = SelectedItem?.Warranty.ToString() ?? string.Empty;
                //this.tags_editText.Text = SelectedItem?.Tag ?? string.Empty;

                // TODO: vyres frequency button 
                //this.frequency_button = 

                this.endDate_button.Text = SelectedItem?.EndDate.ToString(Resources.GetString(Resource.String.DateFormat)) ?? string.Empty;
                this.endDate_button.Tag = SelectedItem != null ? new JavaLangObjectWrapper<DateTime>(SelectedItem.EndDate) : null;

                this.isActive_checkBox.Checked = SelectedItem?.IsActive ?? true;
            }
        }
    }
}