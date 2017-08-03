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
using TwaWallet.Adapters;
using Database;
using Database.POCO;
using TwaWallet.Classes;
//using Android.Support.Design.Widget;
using com.refractored.fab;
using TwaWallet.Extensions;
using System.Threading.Tasks;
//using Android.App;

namespace TwaWallet.Fragments
{
    public class HistoryFragment : DialogFragment //Fragment
    {
        #region Members
        private const string TAG = "X:" + nameof(HistoryFragment);

        private IDataContext db;

        ListView listView;
        
        List<Record> listData;

        #region GUI
        protected Android.App.ProgressDialog dialog = null;
        //TextView count_label;
        TextView count_value;
        //TextView monthCost_label;
        TextView monthCost_value;
        //TextView filterCost_label;
        TextView filterCost_value;
        CheckBox earnings_checkBox;
        CheckBox costs_checkBox;
        Button dateFrom_button;
        Button dateTo_button;
        EditText find_editText;
        ImageButton find_imageButton;
        #endregion

        #endregion

        public override void OnCreate(Bundle savedInstanceState)
        {
            Log.Debug(TAG, nameof(OnCreate));

            base.OnCreate(savedInstanceState);

            // Create your fragment here

            //var view = inflater.Inflate(Resource.Layout.History, container, false);
            //var lv = view.FindViewById(Resources.Id.);

            string pathToDatabase = DeviceInfo.GetFileFinallPath(Resources.GetString(Resource.String.DBfilename));
            //db = new DataContext(pathToDatabase);
            db = DataContextFactory.GetDataContext(pathToDatabase);

            //LoadData();
        }

        public override /*async*/ View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            Log.Debug(TAG, nameof(OnCreateView));
            
            View v = inflater.Inflate(Resource.Layout.History, container, false);

            listView = v.FindViewById<ListView>(Resource.Id.history_listView);
            listView.ItemClick += OnListItemClick;
            listView.ItemLongClick += OnListItemLongClick;

            //count_label = v.FindViewById<TextView>(Resource.Id.count_label);
            count_value = v.FindViewById<TextView>(Resource.Id.count_value);
            //filterCost_label = v.FindViewById<TextView>(Resource.Id.filterCost_label);
            //filterCost_label.Text = Resources.GetString(Resource.String.);
            filterCost_value = v.FindViewById<TextView>(Resource.Id.filterCost_value);
            //monthCost_label = v.FindViewById<TextView>(Resource.Id.monthCost_label);
            monthCost_value = v.FindViewById<TextView>(Resource.Id.monthCost_value);
            earnings_checkBox = v.FindViewById<CheckBox>(Resource.Id.earnings_checkBox);
            earnings_checkBox.CheckedChange += Earnings_checkBox_CheckedChange;
            earnings_checkBox.Checked = false;

            costs_checkBox = v.FindViewById<CheckBox>(Resource.Id.costs_checkBox);
            costs_checkBox.CheckedChange += Costs_checkBox_CheckedChange;
            costs_checkBox.Checked = true;


            dateFrom_button = v.FindViewById<Button>(Resource.Id.dateFrom_button);
            dateFrom_button.Click += DateFrom_button_Click;
            dateTo_button = v.FindViewById<Button>(Resource.Id.dateTo_button);
            dateTo_button.Click += DateTo_button_Click;

            find_editText = v.FindViewById<EditText>(Resource.Id.find_editText);
            find_imageButton = v.FindViewById<ImageButton>(Resource.Id.find_imageButton);
            find_imageButton.Click += Find_imageButton_Click;


            var fab = v.FindViewById<com.refractored.fab.FloatingActionButton>(Resource.Id.fab);
            //fab.AttachToListView(listView, this, this);
            fab.AttachToListView(listView);
            //fab.Click += (sender, args) =>
            //{
            //    Toast.MakeText(Activity, "FAB Clicked!", ToastLength.Short).Show();                
            //};
            fab.Click += Fab_Click;


            Task.Run(() =>
            {
                /*await*/
                var r = LoadData();//.Result;

                this.Activity.RunOnUiThread(() =>
                {
                    InitLayout(true,false);
                });
            });
            return v;
        }

        private void Find_imageButton_Click(object sender, EventArgs e)
        {
            //if (!string.IsNullOrWhiteSpace(find_editText.Text))
            //{
                LoadData(((JavaLangObjectWrapper<DateTime>)dateFrom_button.Tag).Value, ((JavaLangObjectWrapper<DateTime>)dateTo_button.Tag).Value, costs_checkBox.Checked, earnings_checkBox.Checked, find_editText.Text);
            //}
            
        }

        private void Fab_Click(object sender, EventArgs e)
        {
            Log.Debug(TAG, $"{nameof(Fab_Click)} - try to show ReportFragment like dialog - START");

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

            
            // Create and show the dialog.
            DialogFragment newFragment = ReportFragment.NewInstance(null, delegate ()
            {
                //var r = LoadData(((JavaLangObjectWrapper<DateTime>)dateFrom_button.Tag).Value, ((JavaLangObjectWrapper<DateTime>)dateTo_button.Tag).Value, costs_checkBox.Checked, earnings_checkBox.Checked);//.Result;
                var r = LoadData();

                this.Activity.RunOnUiThread(() =>
                {
                    InitLayout(true, false);
                });
            });

            //newFragment.SetStyle()
            //Dialog.requestWindowFeature(Window.FEATURE_NO_TITLE);
            newFragment.Show(ft, "dialog");

            Log.Debug(TAG, $"{nameof(Fab_Click)} - try to show ReportFragment like dialog - END");
        }

        private void Costs_checkBox_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            Log.Debug(TAG, nameof(Costs_checkBox_CheckedChange));

            if (dateFrom_button?.Tag != null)
            {
                var r = LoadData(((JavaLangObjectWrapper<DateTime>)dateFrom_button.Tag).Value, ((JavaLangObjectWrapper<DateTime>)dateTo_button.Tag).Value, costs_checkBox.Checked, earnings_checkBox.Checked);//.Result;
                this.Activity.RunOnUiThread(() =>
                {
                    InitLayout(costs_checkBox.Checked, earnings_checkBox.Checked);
                });
            }
            else
            {
                var r = LoadData();//.Result;
                
            }
        }

        private void Earnings_checkBox_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            Log.Debug(TAG, nameof(Earnings_checkBox_CheckedChange));

            
            if (dateFrom_button?.Tag != null)
            {
                var r = LoadData(((JavaLangObjectWrapper<DateTime>)dateFrom_button.Tag).Value, ((JavaLangObjectWrapper<DateTime>)dateTo_button.Tag).Value, costs_checkBox.Checked, earnings_checkBox.Checked);//.Result;
                this.Activity.RunOnUiThread(() =>
                {
                    InitLayout(costs_checkBox.Checked, earnings_checkBox.Checked);
                });
            }
            else
            {
                var r = LoadData();//.Result;
            }

        }

        public override /*async*/ void OnResume()
        {
            Log.Debug(TAG, nameof(OnResume));

            base.OnResume();

            Task.Run(() =>
            {
                /*await*/
                var r = LoadData();//.Result;

                this.Activity.RunOnUiThread(() =>
                {
                    InitLayout(true, false);
                });
            });
        }

        private /*async Task<bool>*/ bool LoadData()
        {
            Log.Debug(TAG, nameof(LoadData));

            this.Activity.RunOnUiThread(() =>
            {
                Log.Debug(TAG, "[1] Starting dialog.");
                dialog = this.Activity.ProgressDialogShow(dialog);
                Log.Debug(TAG, "[2] Dialog started.");
            });

            //Task.Run(async () =>
            //await Task.Run(() =>
            //{
            // http://www.gregshackles.com/using-background-threads-in-mono-for-android-applications/
            //Task.Factory
            //.StartNew(() =>
            //{
                try
                {
                    //Java.Lang.Thread.Sleep(2000);
                    //var r = db.Select<Record, DateTime>((o) => o.Id > 0, (o) => o.Date, false).Result;
                    var r = db.Select<Record, DateTime>((o) => o.Earnings == false, (o) => o.Date, false).Result;
                    //var r = await db.Select<Record, DateTime>((o) => o.Id > 0 && o.Earnings == false, (o) => o.Date, false); //.Result;
                    listData = r.ToList();//r.Select(p => $"{p.Description}, {p.Cost}, {p.Date}").ToList();

                    // Month values
                    this.monthCost_value.Text = listData.Where(p => p.Date.Month == DateTime.Now.Month && p.Date.Year == DateTime.Now.Year && p.Earnings == false).Select(p => p.Cost).Sum().ToString();

                    return true;
                }
                catch (Exception ex)
                {
                    Log.Error(TAG, ex.ToString());
                    throw;
                }
                finally
                {
                    this.Activity.RunOnUiThread(() =>
                    {
                        this.Activity.ProgressDialogDismiss(dialog);
                    });
                }
            //})
            //.ContinueWith(task =>
            //this.Activity.RunOnUiThread(() =>
            //{
            //    this.Activity.ProgressDialogDismiss(dialog);
            //}));

            //});

            return false;
        }
        
        private /*async Task<bool>*/ bool LoadData(DateTime dateFrom, DateTime dateTo, bool includeCosts, bool includeEarnings, string find = "")
        {
            Log.Debug(TAG, $"{nameof(LoadData)} - {nameof(dateFrom)}:{dateFrom.ToString()}, {nameof(dateTo)}:{dateTo.ToString()}, {nameof(includeCosts)}:{includeCosts.ToString()},{nameof(includeEarnings)}:{includeEarnings}");

            this.Activity.RunOnUiThread(() =>
            {
                Log.Debug(TAG, "[1] Starting dialog.");
                dialog = this.Activity.ProgressDialogShow(dialog);
                Log.Debug(TAG, "[2] Dialog started.");
            });

            //await Task.Run(() =>
            //{

            //Task.Factory
            //.StartNew(() =>
            //{
            try
            {
                // Refresh review
                var rAll = db.Select<Record, DateTime>((o) => o.Id > 0, (o) => o.Date, false).Result;
                //var rAll = await db.Select<Record, DateTime>((o) => o.Id > 0, (o) => o.Date, false);//.Result;
                var listAll = rAll.ToList();

                //var r = db.Select<Record, DateTime>((o) => o.Date >= dateFrom && o.Date <= dateTo, (o) => o.Date, false).Result;
                //listData = r.ToList();//r.Select(p => $"{p.Description}, {p.Cost}, {p.Date}").ToList();

                if (listAll != null && listAll.Count > 0)
                {


                    // Filtered values
                    if (includeCosts == false && includeEarnings == false)
                    {
                        listData = new List<Record>();
                        // Month values
                        this.monthCost_value.Text = Convert.ToString(0);
                    }
                    else if (includeCosts == false && includeEarnings == true)
                    {
                        listData = listAll.Where((o) => o.Date.Date >= dateFrom.Date && o.Date.Date <= dateTo.Date && o.Earnings == true).OrderByDescending(o => o.Date).ToList();
                        // Month values
                        this.monthCost_value.Text = listAll.Where(p => p.Date.Month == DateTime.Now.Month && p.Date.Year == DateTime.Now.Year && p.Earnings == true).Select(p => p.Cost).Sum().ToString();
                    }
                    else if (includeCosts == true && includeEarnings == false)
                    {
                        listData = listAll.Where((o) => o.Date.Date >= dateFrom.Date && o.Date.Date <= dateTo.Date && o.Earnings == false).OrderByDescending(o => o.Date).ToList();
                        // Month values
                        this.monthCost_value.Text = listAll.Where(p => p.Date.Month == DateTime.Now.Month && p.Date.Year == DateTime.Now.Year && p.Earnings == false).Select(p => p.Cost).Sum().ToString();
                    }
                    else if (includeCosts == true && includeEarnings == true)
                    {
                        listData = listAll.Where((o) => o.Date.Date >= dateFrom.Date && o.Date.Date <= dateTo.Date).OrderByDescending(o => o.Date).ToList();
                        // Month values
                        this.monthCost_value.Text = listAll.Where(p => p.Date.Month == DateTime.Now.Month && p.Date.Year == DateTime.Now.Year).Select(p => p.Cost).Sum().ToString();
                    }

                    if (listData != null && listData.Count > 0)
                    {
                        this.count_value.Text = listData.Count.ToString();
                        this.filterCost_value.Text = listData.Select(p => p.Cost).Sum().ToString();
                    }

                    // Find filter
                    if (!string.IsNullOrWhiteSpace(find) && listData != null && listData.Count > 0)
                    {
                        listData = listData.Select(a => a.IncludeObjects(db)).ToList();

                        string findTmp = find.ToLower();

                        var tmp = listData.Where(a => a.Tag.Contains(findTmp)
                        || (!string.IsNullOrWhiteSpace(a.Description) ? a.Description.ToLower().Contains(findTmp) : false)
                        || ((a.Category != null && !string.IsNullOrWhiteSpace(a.Category.Description)) ? a.Category.Description.ToLower().Contains(findTmp) : false)
                        || ((a.Owner != null && !string.IsNullOrWhiteSpace(a.Owner.Name)) ? a.Owner.Name.ToLower().Contains(findTmp) : false));
                        //);

                        if (tmp != null && tmp.Count() > 0)
                        {
                            listData = tmp.ToList();
                        }
                        else
                        {
                            this.monthCost_value.Text = Convert.ToString(0);
                            listData = new List<Record>();
                        }

                    }
                }
                else
                {
                    this.monthCost_value.Text = Convert.ToString(0);
                    listData = new List<Record>();
                }

                // Refresh review
                this.count_value.Text = listData.Count.ToString();
                this.filterCost_value.Text = listData.Select(p => p.Cost).Sum().ToString();

                listView.Adapter = new RecordListAdapter(this.Activity, listData, db);

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
            //})
            //.ContinueWith(task =>
            //this.Activity.RunOnUiThread(() =>
            //{
            //    this.Activity.ProgressDialogDismiss(dialog);
            //}));

            //});

            return false;
        }
        
        private void DateTo_button_Click(object sender, EventArgs e)
        {
            Log.Debug(TAG, nameof(DateTo_button_Click));

            DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime date)
            {
                dateTo_button.Text = date.ToString(Resources.GetString(Resource.String.DateFormat)); //.ToLongDateString();
                dateTo_button.Tag = new JavaLangObjectWrapper<DateTime>(date);

                var r = LoadData(((JavaLangObjectWrapper<DateTime>)dateFrom_button.Tag).Value, ((JavaLangObjectWrapper<DateTime>)dateTo_button.Tag).Value, costs_checkBox.Checked, earnings_checkBox.Checked);//.Result;
            });
            frag.Show(this.Activity.FragmentManager, DatePickerFragment.TAG);

        }

        private void DateFrom_button_Click(object sender, EventArgs e)
        {
            Log.Debug(TAG, nameof(DateTo_button_Click));

            DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime date)
            {
                dateFrom_button.Text = date.ToString(Resources.GetString(Resource.String.DateFormat)); //.ToLongDateString();
                dateFrom_button.Tag = new JavaLangObjectWrapper<DateTime>(date);

                var r = LoadData(((JavaLangObjectWrapper<DateTime>)dateFrom_button.Tag).Value, ((JavaLangObjectWrapper<DateTime>)dateTo_button.Tag).Value, costs_checkBox.Checked, earnings_checkBox.Checked);//.Result;
            });
            frag.Show(this.Activity.FragmentManager, DatePickerFragment.TAG);

            
        }

        private void InitLayout(bool costsIsChecked, bool earningsIsChecked)
        {
            Log.Debug(TAG, nameof(InitLayout));

            this.Activity.RunOnUiThread(() =>
            {
                Log.Debug(TAG, "[1] Starting dialog.");
                dialog = this.Activity.ProgressDialogShow(dialog);
                Log.Debug(TAG, "[2] Dialog started.");
            });

            try
            {
                costs_checkBox.Checked = costsIsChecked;
                earnings_checkBox.Checked = earningsIsChecked;

                listView.Adapter = new RecordListAdapter(this.Activity, listData, this.db);

                if (listData != null && listData.Count > 0)
                {
                    var date = listData.LastOrDefault().Date;
                    dateFrom_button.Text = date.ToString(Resources.GetString(Resource.String.DateFormat));
                    dateFrom_button.Tag = new JavaLangObjectWrapper<DateTime>(date);

                    date = listData.FirstOrDefault().Date;
                    dateTo_button.Text = date.ToString(Resources.GetString(Resource.String.DateFormat));
                    dateTo_button.Tag = new JavaLangObjectWrapper<DateTime>(date);

                    this.count_value.Text = listData.Count.ToString();
                    this.monthCost_value.Text = listData.Where(p => p.Date.Month == DateTime.Now.Month && p.Date.Year == DateTime.Now.Year).Select(p => p.Cost).Sum().ToString();
                    this.filterCost_value.Text = listData.Select(p => p.Cost).Sum().ToString();
                }
                else
                {
                    var date = DateTime.Now;

                    dateFrom_button.Text = date.ToString(Resources.GetString(Resource.String.DateFormat));
                    dateFrom_button.Tag = new JavaLangObjectWrapper<DateTime>(date);

                    dateTo_button.Text = date.ToString(Resources.GetString(Resource.String.DateFormat));
                    dateTo_button.Tag = new JavaLangObjectWrapper<DateTime>(date);

                    this.count_value.Text = 0.ToString();
                    this.monthCost_value.Text = 0.ToString();
                    this.filterCost_value.Text = 0.ToString();
                }
            }
            catch (Exception ex)
            {
                Log.Error(TAG, ex.ToString());
                throw;
            }
            finally
            {
                this.Activity.RunOnUiThread(() =>
                {
                    this.Activity.ProgressDialogDismiss(dialog);
                });
            }        
        }

        private void OnListItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Log.Debug(TAG, nameof(OnListItemClick));

            //string item = result.posts.ElementAt(e.Position);
            //string item = listData.ElementAt(e.Position);
            var item = listData.ElementAt(e.Position);
            // Do whatever you like here
            //Toast.MakeText(this.Activity, $"You press: {item.Description}", ToastLength.Short).Show();

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

            Log.Debug(TAG, $"{nameof(OnListItemClick)} - try to show ReportFragment like dialog - START");
            // Create and show the dialog.
            DialogFragment newFragment = ReportFragment.NewInstance(item.IncludeObjects(db), delegate()
            {
                var r = LoadData(((JavaLangObjectWrapper<DateTime>)dateFrom_button.Tag).Value, ((JavaLangObjectWrapper<DateTime>)dateTo_button.Tag).Value, costs_checkBox.Checked, earnings_checkBox.Checked);//.Result;
            });
            

            newFragment.Show(ft, "dialog");
            Log.Debug(TAG, $"{nameof(OnListItemClick)} - try to show ReportFragment like dialog - END");

        }

        private void OnListItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            Log.Debug(TAG, nameof(OnListItemLongClick));

            var item = listData.ElementAt(e.Position);

            Android.App.AlertDialog.Builder builder = new Android.App.AlertDialog.Builder(this.Activity);
            builder.SetMessage(item.ToString())
                //.SetPositiveButton("Yes", dialogClickListener)
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
                    DialogFragment newFragment = ReportFragment.NewInstance(item.IncludeObjects(db), delegate ()
                    {
                        var r = LoadData(((JavaLangObjectWrapper<DateTime>)dateFrom_button.Tag).Value, ((JavaLangObjectWrapper<DateTime>)dateTo_button.Tag).Value, costs_checkBox.Checked, earnings_checkBox.Checked);//.Result;
                    });


                    newFragment.Show(ft, "dialog");
                    Log.Debug(TAG, $"{nameof(OnListItemLongClick)} - try to show ReportFragment like dialog - END");
                })
                //.SetNegativeButton("No", dialogClickListener)
                .SetNegativeButton(this.Activity.Resources.GetString(Resource.String.Delete), (s, args) =>
                {
                    if (db.Delete(item).Result)
                    {
                        Toast.MakeText(this.Activity, Resources.GetString(Resource.String.Deleted), ToastLength.Short).Show();

                        var r = LoadData(((JavaLangObjectWrapper<DateTime>)dateFrom_button.Tag).Value, ((JavaLangObjectWrapper<DateTime>)dateTo_button.Tag).Value, costs_checkBox.Checked, earnings_checkBox.Checked);//.Result;
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