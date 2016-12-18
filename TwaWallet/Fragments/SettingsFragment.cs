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
    public class SettingsFragment : Fragment
    {
        #region Members
        private const string TAG = "X:" + nameof(SettingsFragment);

        private IDataContext db = null;

        private List<Category> lstCategory = null;
        private List<PaymentType> lstPaymentType = null;
        private List<Owner> lstOwner = null;

        #region GUI
        Button category_button;
        Button paymentType_button;
        Button owner_button; 
        #endregion

        #endregion

        public override void OnCreate(Bundle savedInstanceState)
        {
            Log.Debug(TAG, nameof(OnCreate));

            base.OnCreate(savedInstanceState);

            // Create your fragment here
            string pathToDatabase = DeviceInfo.GetFileFinallPath(Resources.GetString(Resource.String.DBfilename));
            //db = new DataContext(pathToDatabase);
            db = DataContextFactory.GetDataContext(pathToDatabase);

            
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            Log.Debug(TAG, nameof(OnCreateView));

            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);

            //return base.OnCreateView(inflater, container, savedInstanceState);
            View v = inflater.Inflate(Resource.Layout.Settings, container, false);

            category_button = v.FindViewById<Button>(Resource.Id.category_button);
            category_button.Click += Category_button_Click;
            paymentType_button = v.FindViewById<Button>(Resource.Id.paymentType_button);
            paymentType_button.Click += PaymentType_button_Click;
            owner_button = v.FindViewById<Button>(Resource.Id.owner_button);
            owner_button.Click += Owner_button_Click;

            LoadData();
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

            var category = lstCategory.Where(p => p.IsDefault).FirstOrDefault();
            this.category_button.Text = category.Description;
            this.category_button.Tag = new JavaLangObjectWrapper<Category>(category);

            var paymentType = lstPaymentType.Where(p => p.IsDefault).FirstOrDefault();
            this.paymentType_button.Text = paymentType.Description;
            this.paymentType_button.Tag = new JavaLangObjectWrapper<PaymentType>(paymentType);

            var owner = lstOwner.Where(p => p.IsDefault).FirstOrDefault();
            this.owner_button.Text = owner.Name;
            this.owner_button.Tag = new JavaLangObjectWrapper<Owner>(owner);
        }

        private void Category_button_Click(object sender, EventArgs e)
        {
            Log.Debug(TAG, nameof(Category_button_Click));

            var fr = SimpleListViewDialogFragment<Category>.NewInstance(lstCategory, delegate (Category selectedItem)
            {
                category_button.Text = selectedItem.Description;
                category_button.Tag = new JavaLangObjectWrapper<Category>(selectedItem);

                var r = db.SetAllDefault<Category>(false).Result;

                selectedItem.IsDefault = true;

                if (db.Update(selectedItem).Result)
                {
                    Toast.MakeText(this.Activity, Resource.String.Saved, ToastLength.Short).Show();
                }
                else
                {
                    Toast.MakeText(this.Activity, Resource.String.WasntSaved, ToastLength.Short).Show();
                }

                //var item = db.Select<Category, int>(p => p.IsDefault, p => p.Id).Result.FirstOrDefault();

                //Toast.MakeText(this.Activity, $"default: {item.Description}", ToastLength.Short).Show();

            });
            fr.Show(this.Activity.FragmentManager, SimpleListViewDialogFragment<Category>.TAG);
        }

        private void Owner_button_Click(object sender, EventArgs e)
        {
            Log.Debug(TAG, nameof(Owner_button_Click));

            var fr = SimpleListViewDialogFragment<Owner>.NewInstance(lstOwner, delegate (Owner selectedItem)
            {
                owner_button.Text = selectedItem.Name;
                owner_button.Tag = new JavaLangObjectWrapper<Owner>(selectedItem);

                var r = db.SetAllDefault<Owner>(false).Result;
                //if (db.SetAllDefault<Owner>(false).Result)
                //{
                //    Toast.MakeText(this.Activity, "Inicializovano", ToastLength.Short).Show();
                //}
                //else
                //{
                //    Toast.MakeText(this.Activity, "Neinicializovano", ToastLength.Short).Show();
                //}

                selectedItem.IsDefault = true;

                if (db.Update(selectedItem).Result)
                {
                    Toast.MakeText(this.Activity, Resource.String.Saved, ToastLength.Short).Show();
                }
                else
                {
                    Toast.MakeText(this.Activity, Resource.String.WasntSaved, ToastLength.Short).Show();
                }

                //var item = db.Select<Owner, int>(p => p.IsDefault, p => p.Id).Result.FirstOrDefault();

                //Toast.MakeText(this.Activity, $"default: {item.Name}", ToastLength.Short).Show();

                //item = db.Select<Owner, int>(p => p.IsDefault, p => p.Id).Result.LastOrDefault();

                //Toast.MakeText(this.Activity, $"default: {item.Name}", ToastLength.Short).Show();

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

                var r = db.SetAllDefault<PaymentType>(false).Result;

                selectedItem.IsDefault = true;
                if (db.Update(selectedItem).Result)
                {
                    Toast.MakeText(this.Activity, Resource.String.Saved, ToastLength.Short).Show();
                }
                else
                {
                    Toast.MakeText(this.Activity, Resource.String.WasntSaved, ToastLength.Short).Show();
                }

                //var item = db.Select<PaymentType, int>(p => p.IsDefault, p => p.Id).Result.FirstOrDefault();

                //Toast.MakeText(this.Activity, $"default: {item.Description}", ToastLength.Short).Show();

            });
            fr.Show(this.Activity.FragmentManager, SimpleListViewDialogFragment<PaymentType>.TAG);
        }

        

        
    }
}