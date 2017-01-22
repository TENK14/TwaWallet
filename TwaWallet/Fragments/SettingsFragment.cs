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

            var category_add_button = v.FindViewById<Button>(Resource.Id.category_add_button);
            category_add_button.Click += Category_add_button_Click;
            var category_rem_button = v.FindViewById<Button>(Resource.Id.category_rem_button);
            category_rem_button.Click += Category_rem_button_Click;

            var paymentType_add_button = v.FindViewById<Button>(Resource.Id.paymentType_add_button);
            paymentType_add_button.Click += PaymentType_add_button_Click;
            var paymentType_rem_button = v.FindViewById<Button>(Resource.Id.paymentType_rem_button);
            paymentType_rem_button.Click += PaymentType_rem_button_Click;

            var owner_add_button = v.FindViewById<Button>(Resource.Id.owner_add_button);
            owner_add_button.Click += Owner_add_button_Click;
            var owner_rem_button = v.FindViewById<Button>(Resource.Id.owner_rem_button);
            owner_rem_button.Click += Owner_rem_button_Click;

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

        
        #region Owner
        private void Owner_button_Click(object sender, EventArgs e)
        {
            Log.Debug(TAG, nameof(Owner_button_Click));

            var fr = SimpleListViewDialogFragment<Owner>.NewInstance(lstOwner, delegate (Owner selectedItem)
            {
                owner_button.Text = selectedItem.Name;
                owner_button.Tag = new JavaLangObjectWrapper<Owner>(selectedItem);

                var r = db.SetAllDefault<Owner>(false).Result;

                selectedItem.IsDefault = true;

                if (db.Update(selectedItem).Result)
                {
                    Toast.MakeText(this.Activity, Resource.String.Saved, ToastLength.Short).Show();
                }
                else
                {
                    Toast.MakeText(this.Activity, Resource.String.WasntSaved, ToastLength.Short).Show();
                }

            }, Resources.GetString(Resource.String.Owner));
            fr.Dialog?.SetTitle(Resource.String.Owner);
            fr.Dialog?.SetTitle("Nejaky nazev");
            fr.Show(this.Activity.FragmentManager, SimpleListViewDialogFragment<Owner>.TAG);
        }

        private void Owner_add_button_Click(object sender, EventArgs e)
        {
            Log.Debug(TAG, nameof(Owner_add_button_Click));

            var fr = InputDialogFragment.NewInstance(Resources.GetString(Resource.String.AddOwner), delegate (string input)
            {

                if (!string.IsNullOrWhiteSpace(input))
                {
                    if (db.Insert<Owner>(new Owner { Name = input, IsDefault = false }).Result)
                    {
                        Toast.MakeText(this.Activity, Resource.String.Saved, ToastLength.Short).Show();
                    }
                    else
                    {
                        Toast.MakeText(this.Activity, Resource.String.WasntSaved, ToastLength.Short).Show();
                    }

                    LoadData();
                    InitLayout();
                }
            });
            fr.Show(this.Activity.FragmentManager, SimpleListViewDialogFragment<Owner>.TAG);
        }

        private void Owner_rem_button_Click(object sender, EventArgs e)
        {
            Log.Debug(TAG, nameof(Owner_rem_button_Click));

            var fr = SimpleListViewDialogFragment<Owner>.NewInstance(lstOwner, delegate (Owner selectedItem)
            {
                //owner_button.Text = selectedItem.Description;
                //owner_button.Tag = new JavaLangObjectWrapper<Owner>(selectedItem);

                bool r = db.Delete<Owner>(selectedItem).Result;

                if (selectedItem.IsDefault)
                {
                    var item = db.Select<Owner, int>((o) => o.Id > 0, (o) => o.Id).Result.First();
                    if (item != null)
                    {
                        item.IsDefault = true;

                        if (db.Update(item).Result)
                        {
                            Toast.MakeText(this.Activity, Resource.String.Saved, ToastLength.Short).Show();
                        }
                        else
                        {
                            Toast.MakeText(this.Activity, Resource.String.WasntSaved, ToastLength.Short).Show();
                        }

                        //owner_button.Text = item.Description;
                        //owner_button.Tag = new JavaLangObjectWrapper<Category>(item);
                    }
                    else
                    {
                        //owner_button.Text = string.Empty;
                        //owner_button.Tag = null;// new JavaLangObjectWrapper<Category>(item);
                    }
                }

                LoadData();
                InitLayout();

            }, Resources.GetString(Resource.String.Owner));
            fr.Show(this.Activity.FragmentManager, SimpleListViewDialogFragment<Owner>.TAG);
        }
        #endregion

        #region PaymentType
        private void PaymentType_add_button_Click(object sender, EventArgs e)
        {
            Log.Debug(TAG, nameof(PaymentType_add_button_Click));

            var fr = InputDialogFragment.NewInstance(Resources.GetString(Resource.String.AddPaymentType), delegate (string input)
            {

                if (!string.IsNullOrWhiteSpace(input))
                {
                    if (db.Insert<PaymentType>(new PaymentType { Description = input, IsDefault = false }).Result)
                    {
                        Toast.MakeText(this.Activity, Resource.String.Saved, ToastLength.Short).Show();
                    }
                    else
                    {
                        Toast.MakeText(this.Activity, Resource.String.WasntSaved, ToastLength.Short).Show();
                    }

                    LoadData();
                    InitLayout();
                }
            });
            fr.Show(this.Activity.FragmentManager, SimpleListViewDialogFragment<PaymentType>.TAG);
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

            }, Resources.GetString(Resource.String.PaymentType));

            fr.Dialog?.SetTitle(Resource.String.PaymentType);
            fr.Show(this.Activity.FragmentManager, SimpleListViewDialogFragment<PaymentType>.TAG);
        }


        private void PaymentType_rem_button_Click(object sender, EventArgs e)
        {
            Log.Debug(TAG, nameof(Category_rem_button_Click));

            var fr = SimpleListViewDialogFragment<PaymentType>.NewInstance(lstPaymentType, delegate (PaymentType selectedItem)
            {
                //paymentType_button.Text = selectedItem.Description;
                //paymentType_button.Tag = new JavaLangObjectWrapper<PaymentType>(selectedItem);

                bool r = db.Delete<PaymentType>(selectedItem).Result;

                if (selectedItem.IsDefault)
                {
                    var item = db.Select<PaymentType, int>((o) => o.Id > 0, (o) => o.Id).Result.First();
                    if (item != null)
                    {
                        item.IsDefault = true;

                        if (db.Update(item).Result)
                        {
                            Toast.MakeText(this.Activity, Resource.String.Saved, ToastLength.Short).Show();
                        }
                        else
                        {
                            Toast.MakeText(this.Activity, Resource.String.WasntSaved, ToastLength.Short).Show();
                        }

                        //paymentType_button.Text = item.Description;
                        //paymentType_button.Tag = new JavaLangObjectWrapper<PaymentType>(item);
                    }
                    else
                    {
                        //paymentType_button.Text = string.Empty;
                        //paymentType_button.Tag = null;// new JavaLangObjectWrapper<PaymentType>(item);
                    }
                }

                LoadData();
                InitLayout();

            }, Resources.GetString(Resource.String.PaymentType));
            fr.Show(this.Activity.FragmentManager, SimpleListViewDialogFragment<PaymentType>.TAG);
        }
        #endregion



        #region Category
        private void Category_button_Click(object sender, EventArgs e)
        {
            Log.Debug(TAG, nameof(Category_button_Click));

            var fr = SimpleListViewDialogFragment<Category>.NewInstance(title:Resources.GetString(Resource.String.Category), 
                lst:lstCategory, 
                onSelectedItem:delegate (Category selectedItem)
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

            });
            
            fr.Show(this.Activity.FragmentManager, SimpleListViewDialogFragment<Category>.TAG);
        }

        private void Category_add_button_Click(object sender, EventArgs e)
        {
            Log.Debug(TAG, nameof(Category_add_button_Click));

            var fr = InputDialogFragment.NewInstance(Resources.GetString(Resource.String.AddCategory), delegate (string input)
            {

                if (!string.IsNullOrWhiteSpace(input))
                {
                    if (db.Insert<Category>(new Category { Description = input, IsDefault = false }).Result)
                    { 
                        Toast.MakeText(this.Activity, Resource.String.Saved, ToastLength.Short).Show();
                    }
                    else
                    {
                        Toast.MakeText(this.Activity, Resource.String.WasntSaved, ToastLength.Short).Show();
                    }

                    LoadData();
                    InitLayout();
                }
            });
            fr.Show(this.Activity.FragmentManager, SimpleListViewDialogFragment<Category>.TAG);
        }

        private void Category_rem_button_Click(object sender, EventArgs e)
        {
            Log.Debug(TAG, nameof(Category_rem_button_Click));

            var fr = SimpleListViewDialogFragment<Category>.NewInstance(lstCategory, delegate (Category selectedItem)
            {
                //category_button.Text = selectedItem.Description;
                //category_button.Tag = new JavaLangObjectWrapper<Category>(selectedItem);

                bool r = db.Delete<Category>(selectedItem).Result;

                if (selectedItem.IsDefault)
                {
                    var item = db.Select<Category, int>((o) => o.Id > 0, (o) => o.Id).Result.First();
                    if (item != null)
                    {
                        item.IsDefault = true;

                        if (db.Update(item).Result)
                        {
                            Toast.MakeText(this.Activity, Resource.String.Saved, ToastLength.Short).Show();
                        }
                        else
                        {
                            Toast.MakeText(this.Activity, Resource.String.WasntSaved, ToastLength.Short).Show();
                        }

                        //category_button.Text = item.Description;
                        //category_button.Tag = new JavaLangObjectWrapper<Category>(item);
                    }
                    else
                    {
                        //category_button.Text = string.Empty;
                        //category_button.Tag = null;// new JavaLangObjectWrapper<Category>(item);
                    }
                }

                LoadData();
                InitLayout();
                
            }, Resources.GetString(Resource.String.Category));
            fr.Show(this.Activity.FragmentManager, SimpleListViewDialogFragment<Category>.TAG);
        }
        #endregion


    }
}