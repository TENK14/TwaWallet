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
    public class ReportFragment : DialogFragment
    {
        #region Members
        private const string TAG = "X:" + nameof(ReportFragment);

        private IDataContext db;

        List<Owner> lstOwner;
        List<PaymentType> lstPaymentType;
        List<Category> lstCategory;

        // Initialize this value to prevent NullReferenceExceptions.
        //Action<Record> itemSelectedHandler = delegate { };

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

        #region Properties
        public Record SelectedItem { get; set; } = null;
        #endregion
        
        #region DialogFragment
        // Initialize this value to prevent NullReferenceExceptions.
        Action onContinueWithHandler = delegate { };

        public static ReportFragment NewInstance(Record selectedItem, Action onContinueWith)
        {
            Log.Debug(TAG, $"{nameof(NewInstance)} - {nameof(selectedItem)}:{selectedItem?.ToString()}");

            var frag = new ReportFragment();
            //frag.itemSelectedHandler = onItemSelected;
            frag.SelectedItem = selectedItem;
            frag.onContinueWithHandler = onContinueWith;
            return frag;
        }
        
        #endregion

        public override void OnCreate(Bundle savedInstanceState)
        {
            Log.Debug(TAG, nameof(OnCreate));

            base.OnCreate(savedInstanceState);
            
            string pathToDatabase = DeviceInfo.GetFileFinallPath(Resources.GetString(Resource.String.DBfilename));
            db = DataContextFactory.GetDataContext(pathToDatabase);

            LoadData();
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            Log.Debug(TAG, nameof(OnCreateView));

            Dialog.Window.RequestFeature(WindowFeatures.NoTitle);
            
            View v = inflater.Inflate(Resource.Layout.Report, container, false);

            category_button = v.FindViewById<Button>(Resource.Id.category_button);
            category_button.Click += Category_button_Click;

            paymentType_button = v.FindViewById<Button>(Resource.Id.paymentType_button);
            paymentType_button.Click += PaymentType_button_Click;

            owner_button = v.FindViewById<Button>(Resource.Id.owner_button);
            owner_button.Click += Owner_button_Click;

            cost_editText = v.FindViewById<EditText>(Resource.Id.cost_editText);
            
            earnings_checkBox = v.FindViewById<CheckBox>(Resource.Id.earnings_checkBox);
            
            description_editText = v.FindViewById<EditText>(Resource.Id.description_editText);
            
            date_button = v.FindViewById<Button>(Resource.Id.date_button);            
            date_button.Click += Date_button_Click;

            warranty_editText = v.FindViewById<EditText>(Resource.Id.warranty_editText);

            tags_editText = v.FindViewById<EditText>(Resource.Id.tags_editText);

            save_button = v.FindViewById<Button>(Resource.Id.save_button);
            save_button.Click += Save_button_Click;

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
            
            var r = db.Select<Owner,int>((o) => o.Id > 0, (o) => o.Id).Result;
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
                this.category_button.Text = category?.Description ?? string.Empty;
                this.category_button.Tag = category != null ? new JavaLangObjectWrapper<Category>(category) : null;

                var paymentType = lstPaymentType.Where(p => p.IsDefault).FirstOrDefault();
                this.paymentType_button.Text = paymentType?.Description ?? string.Empty;
                this.paymentType_button.Tag = paymentType != null ? new JavaLangObjectWrapper<PaymentType>(paymentType) : null;

                var owner = lstOwner.Where(p => p.IsDefault).FirstOrDefault();
                this.owner_button.Text = owner?.Name ?? string.Empty;
                this.owner_button.Tag = owner != null ? new JavaLangObjectWrapper<Owner>(owner) : null;
                
                this.earnings_checkBox.Checked = false;

                this.cost_editText.Text = string.Empty;
                this.description_editText.Text = string.Empty;
                this.warranty_editText.Text = string.Empty;
                this.tags_editText.Text = string.Empty;

                var date = DateTime.Now.Date;
                this.date_button.Text = date.ToString(Resources.GetString(Resource.String.DateFormat));
                this.date_button.Tag = new JavaLangObjectWrapper<DateTime>(date);
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

                var date = DateTime.Now;
                this.date_button.Text = SelectedItem?.Date.ToString(Resources.GetString(Resource.String.DateFormat)) ?? string.Empty;
                this.date_button.Tag = SelectedItem != null ? new JavaLangObjectWrapper<DateTime>(SelectedItem.Date) : null;
                
            }
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
                category_button.Tag = new JavaLangObjectWrapper<Category>(selectedItem);
             },
             Resources.GetString(Resource.String.Category));
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
                // earnings cant have category
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
                    CategoryId = cId,
                    Cost = this.earnings_checkBox.Checked ? cost : 0f - cost,
                    Date = ((JavaLangObjectWrapper<DateTime>)this.date_button.Tag).Value,
                    Description = this.description_editText.Text,
                    OwnerId = oId, 
                    PaymentTypeId = pId,
                    Tag = this.tags_editText.Text,
                    Warranty = warranty,
                    Earnings = this.earnings_checkBox.Checked,
                    //DateCreated = new Java.Sql.Timestamp(Tools.ConvertToTimestamp(DateTime.Now))
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
                    record.Id = SelectedItem.Id;
                    if (db.Update(record).Result)
                    {
                        Toast.MakeText(this.Activity, Resources.GetString(Resource.String.Saved), ToastLength.Short).Show();

                        if (onContinueWithHandler != null)
                        {
                            onContinueWithHandler();
                        }
                        Dismiss();
                    }
                }
                else if (db.Insert(record).Result) // inser new item
                {
                    Toast.MakeText(this.Activity,Resources.GetString(Resource.String.Saved), ToastLength.Short).Show();

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
            catch (NullReferenceException ex)
            {
                Log.Error(TAG, nameof(Save_button_Click), ex.Message);
                Toast.MakeText(this.Activity, Resources.GetString(Resource.String.SomethingMissing), ToastLength.Short).Show();
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