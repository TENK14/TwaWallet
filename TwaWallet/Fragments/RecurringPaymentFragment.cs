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
        List<Interval> lstInterval;

        #region GUI
        Button category_button;
        Button paymentType_button;
        Button owner_button;
        EditText cost_editText;
        CheckBox earnings_checkBox;
        EditText description_editText;
        Button interval_button;
        Button startDate_button;
        Button endDate_button;
        EditText warranty_editText;
        EditText tags_editText;
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
            Log.Debug(TAG, $"{nameof(NewInstance)} - {nameof(selectedItem)}:{selectedItem?.ToString() ?? "is null"}");

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
            Log.Debug(TAG, nameof(OnCreateView));

            Dialog.Window.RequestFeature(WindowFeatures.NoTitle);

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

            this.interval_button = v.FindViewById<Button>(Resource.Id.interval_button);
            this.interval_button.Click += Interval_button_Click;

            startDate_button = v.FindViewById<Button>(Resource.Id.startDate_button);
            startDate_button.Click += StartDate_button_Click;

            endDate_button = v.FindViewById<Button>(Resource.Id.endDate_button);
            endDate_button.Click += EndDate_button_Click;

            //warranty_button = v.FindViewById<Button>(Resource.Id.warranty_button);
            warranty_editText = v.FindViewById<EditText>(Resource.Id.warranty_editText);

            tags_editText = v.FindViewById<EditText>(Resource.Id.tags_editText);

            isActive_checkBox = v.FindViewById<CheckBox>(Resource.Id.isActive_checkBox);

            save_button = v.FindViewById<Button>(Resource.Id.save_button);
            save_button.Click += Save_button_Click;

            InitLayout();

            return v;


        }

        private void StartDate_button_Click(object sender, EventArgs e)
        {
            Log.Debug(TAG, nameof(StartDate_button_Click));

            DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime date)
            {
                startDate_button.Text = date.ToString(Resources.GetString(Resource.String.DateFormat)); //.ToLongDateString();
                startDate_button.Tag = new JavaLangObjectWrapper<DateTime>(date);
            });
            frag.Show(this.Activity.FragmentManager, DatePickerFragment.TAG);
        }

        #region Button click

        private void Owner_button_Click(object sender, EventArgs e)
        {
            Log.Debug(TAG, nameof(Owner_button_Click));

            var fr = SimpleListViewDialogFragment<Owner>.NewInstance(lstOwner, delegate (Owner selectedItem)
            {
                owner_button.Text = selectedItem.Name;
                owner_button.Tag = new JavaLangObjectWrapper<Owner>(selectedItem);
            },
             Resources.GetString(Resource.String.Owner));
            fr.Show(this.Activity.FragmentManager, SimpleListViewDialogFragment<Owner>.TAG);
        }

        private void PaymentType_button_Click(object sender, EventArgs e)
        {
            Log.Debug(TAG, nameof(PaymentType_button_Click));

            var fr = SimpleListViewDialogFragment<PaymentType>.NewInstance(lstPaymentType, delegate (PaymentType selectedItem)
            {
                paymentType_button.Text = selectedItem.Description;
                paymentType_button.Tag = new JavaLangObjectWrapper<PaymentType>(selectedItem);
            },
             Resources.GetString(Resource.String.PaymentType));
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
            },
             Resources.GetString(Resource.String.Category));
            fr.Show(this.Activity.FragmentManager, SimpleListViewDialogFragment<Category>.TAG);

        }

        private void EndDate_button_Click(object sender, EventArgs e)
        {
            Log.Debug(TAG, nameof(EndDate_button_Click));

            DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime date)
            {
                endDate_button.Text = date.ToString(Resources.GetString(Resource.String.DateFormat)); //.ToLongDateString();
                endDate_button.Tag = new JavaLangObjectWrapper<DateTime>(date);
            });
            frag.Show(this.Activity.FragmentManager, DatePickerFragment.TAG);
        }

        private void Interval_button_Click(object sender, EventArgs e)
        {
            Log.Debug(TAG, nameof(Interval_button_Click));

            var fr = SimpleListViewDialogFragment<Interval>.NewInstance(lstInterval, delegate (Interval selectedItem)
            {
                interval_button.Text = selectedItem.Description;
                interval_button.Tag = new JavaLangObjectWrapper<Interval>(selectedItem);
            },
            Resources.GetString(Resource.String.Interval));
            fr.Show(this.Activity.FragmentManager, SimpleListViewDialogFragment<Interval>.TAG);
        }

        private void Save_button_Click(object sender, EventArgs e)
        {
            Log.Debug(TAG, nameof(Save_button_Click));

            try
            {
                // earnings cant have category
                //int cId = this.earnings_checkBox.Checked ? 0 : ((JavaLangObjectWrapper<Category>)category_button.Tag).Value.Id;
                int cId = ((JavaLangObjectWrapper<Category>)category_button.Tag).Value.Id;
                int oId = ((JavaLangObjectWrapper<Owner>)owner_button.Tag).Value.Id;
                int pId = ((JavaLangObjectWrapper<PaymentType>)paymentType_button.Tag).Value.Id;
                int iId = ((JavaLangObjectWrapper<Interval>)interval_button.Tag).Value.Id;

                int warranty;
                if (!int.TryParse(warranty_editText.Text, out warranty))
                {
                    warranty = 0;
                }

                float cost = float.Parse(this.cost_editText.Text);

                RecurringPayment item = new RecurringPayment()
                {
                    //TODO: napln RecurringPayment obsah ....
                    CategoryId = cId, // lstCategory?.First()?.Id ?? 0, //this.category_button.Text,
                    Cost = this.earnings_checkBox.Checked ? cost : 0f - cost,
                    EndDate = ((JavaLangObjectWrapper<DateTime>)this.endDate_button.Tag).Value, // DateTime.Now,
                    LastUpdate = ((JavaLangObjectWrapper<Interval>)interval_button.Tag).Value.BeforeDateTime(((JavaLangObjectWrapper<DateTime>)this.startDate_button.Tag).Value),
                    Description = this.description_editText.Text,
                    OwnerId = oId, //lstOwner?.First()?.Id ?? 0, //this.owner_button.Text,
                    PaymentTypeId = pId, //lstPaymentType?.First()?.Id ?? 0, //this.paymentType_button.Text,
                    IntervalId = iId,
                    //Interval = ((JavaLangObjectWrapper<Interval>)interval_button.Tag).Value,                    
                    //Tag = this.tags_editText.Text,
                    Warranty = warranty, // 0, //int.Parse(this.warranty_button.Text),
                    Earnings = this.earnings_checkBox.Checked,
                    Tag = this.tags_editText.Text,
                    //DateCreated = new Java.Sql.Timestamp(Tools.ConvertToTimestamp(DateTime.Now))
                    IsActive = this.isActive_checkBox.Checked,
                };

                #region Ask for permission
                //https://github.com/xamarin/monodroid-samples/blob/master/android-m/RuntimePermissions/MainActivity.cs
                const int REQUEST_CODE_ASK_PERMISSIONS = 123;
                const string permission = Android.Manifest.Permission.WriteExternalStorage;

                var hasWriteContactsPermission = ActivityCompat.CheckSelfPermission(this.Activity, permission);

                if (hasWriteContactsPermission != Android.Content.PM.Permission.Granted)
                {
                    RequestPermissions(new string[] { Android.Manifest.Permission.WriteExternalStorage },
                            REQUEST_CODE_ASK_PERMISSIONS);
                    return;
                }
                #endregion

                if (SelectedItem != null) // update existing item
                {
                    item.Id = SelectedItem.Id;
                    if (db.Update(item).Result)
                    {
                        //Toast.MakeText(this.Activity, item.ToString(), ToastLength.Short).Show();
                        Toast.MakeText(this.Activity, Resources.GetString(Resource.String.Saved), ToastLength.Short).Show();

                        if (onContinueWithHandler != null)
                        {
                            onContinueWithHandler();
                        }
                        Dismiss();

                        //SelectedItem = null;
                        //InitLayout();
                    }
                }
                else if (db.Insert(item).Result) // inser new item
                {
                    //Toast.MakeText(this.Activity, item.ToString(), ToastLength.Short).Show();
                    Toast.MakeText(this.Activity, Resources.GetString(Resource.String.Saved), ToastLength.Short).Show();
                    //InitLayout();

                    if (onContinueWithHandler != null)
                    {
                        onContinueWithHandler();
                    }
                    Dismiss();
                }
                else
                {
                    Toast.MakeText(this.Activity, Resources.GetString(Resource.String.WasntSaved), ToastLength.Short).Show();
                }

            }
            catch (Exception ex)
            {
                Log.Error(TAG, nameof(Save_button_Click),ex.Message);
            }
        }

        #endregion

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

            var r4 = db.Select<Interval, int>((o) => o.Id > 0, (o) => o.Id).Result;
            lstInterval = r4.ToList();

            Log.Debug(TAG, $"{nameof(LoadData)} - END");
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

                var interval = lstInterval.Where(p => p.IsDefault).FirstOrDefault();
                this.interval_button.Text = interval.Description;
                this.interval_button.Tag = new JavaLangObjectWrapper<Interval>(interval);

                this.earnings_checkBox.Checked = false;

                this.cost_editText.Text = string.Empty;
                this.description_editText.Text = string.Empty;
                this.warranty_editText.Text = string.Empty;
                //this.tags_editText.Text = string.Empty;

                // TODO: vyres interval button 
                //this.interval_button = 

                var date = DateTime.Now.Date;

                this.startDate_button.Text = date.ToString(Resources.GetString(Resource.String.DateFormat));
                this.startDate_button.Tag = new JavaLangObjectWrapper<DateTime>(date);

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
                this.tags_editText.Text = SelectedItem?.Tag ?? string.Empty;

                // TODO: vyres interval button 
                this.interval_button.Text = SelectedItem?.Interval.Description;
                this.interval_button.Tag = SelectedItem != null ? new JavaLangObjectWrapper<Interval>(SelectedItem.Interval) : null;

                this.startDate_button.Text = SelectedItem?.Interval?.NextDateTime(SelectedItem.LastUpdate).ToString(Resources.GetString(Resource.String.DateFormat)) ?? string.Empty;
                this.startDate_button.Tag = SelectedItem != null ? new JavaLangObjectWrapper<DateTime>(SelectedItem.Interval.NextDateTime(SelectedItem.LastUpdate)) : null;

                this.endDate_button.Text = SelectedItem?.EndDate.ToString(Resources.GetString(Resource.String.DateFormat)) ?? string.Empty;
                this.endDate_button.Tag = SelectedItem != null ? new JavaLangObjectWrapper<DateTime>(SelectedItem.EndDate) : null;

                this.isActive_checkBox.Checked = SelectedItem?.IsActive ?? true;
            }
        }
    }
}