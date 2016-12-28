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
using System.IO;
using Android;
using Android.Content.PM;

namespace TwaWallet.Fragments
{
    public class ExportFragment : Fragment
    {
        private const string TAG = "X:" + nameof(ExportFragment);

        private IDataContext db;

        #region GUI
        Button dateFrom_button;
        Button dateTo_button;
        Button export_button;
        CheckBox earnings_checkBox;
        CheckBox costs_checkBox;
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
            //return inflater.Inflate(Resource.Layout.Export, container, false);

            View v = inflater.Inflate(Resource.Layout.Export, container, false);

            dateFrom_button = v.FindViewById<Button>(Resource.Id.dateFrom_button);
            dateFrom_button.Click += DateFrom_button_Click;

            dateTo_button = v.FindViewById<Button>(Resource.Id.dateTo_button);
            dateTo_button.Click += DateTo_button_Click;
            export_button = v.FindViewById<Button>(Resource.Id.export_button);
            export_button.Click += Export_button_Click;

            earnings_checkBox = v.FindViewById<CheckBox>(Resource.Id.earnings_checkBox);            

            costs_checkBox = v.FindViewById<CheckBox>(Resource.Id.costs_checkBox);

            string pathToDatabase = DeviceInfo.GetFileFinallPath(Resources.GetString(Resource.String.DBfilename));
            //db = new DataContext(pathToDatabase);
            db = DataContextFactory.GetDataContext(pathToDatabase);

            InitLayout();

            return v;
        }

        public override void OnResume()
        {
            Log.Debug(TAG, nameof(OnResume));

            base.OnResume();

            //LoadData();
            InitLayout();
        }

        private void InitLayout()
        {
            Log.Debug(TAG, nameof(InitLayout));

            var date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            
            dateFrom_button.Text = date.ToString(Resources.GetString(Resource.String.DateFormat));
            dateFrom_button.Tag = new JavaLangObjectWrapper<DateTime>(date);

            date = DateTime.Now;
            dateTo_button.Text = date.ToString(Resources.GetString(Resource.String.DateFormat));
            dateTo_button.Tag = new JavaLangObjectWrapper<DateTime>(date);

            costs_checkBox.Checked = true;
            earnings_checkBox.Checked = false;
        }

        private void DateTo_button_Click(object sender, EventArgs e)
        {
            Log.Debug(TAG, nameof(DateTo_button_Click));

            DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime date)
            {
                dateTo_button.Text = date.ToString(Resources.GetString(Resource.String.DateFormat)); //.ToLongDateString();
                dateTo_button.Tag = new JavaLangObjectWrapper<DateTime>(date);
            });
            frag.Show(this.Activity.FragmentManager, DatePickerFragment.TAG);
        }

        private void DateFrom_button_Click(object sender, EventArgs e)
        {
            Log.Debug(TAG, nameof(DateFrom_button_Click));

            DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime date)
            {
                dateFrom_button.Text = date.ToString(Resources.GetString(Resource.String.DateFormat)); //.ToLongDateString();
                dateFrom_button.Tag = new JavaLangObjectWrapper<DateTime>(date);
            });
            frag.Show(this.Activity.FragmentManager, DatePickerFragment.TAG);
        }

        private void Export_button_Click(object sender, EventArgs e)
        {
            Log.Debug(TAG, nameof(Export_button_Click));

            //throw new NotImplementedException();

            DateTime dateFrom = ((JavaLangObjectWrapper<DateTime>)dateFrom_button.Tag).Value;
            DateTime dateTo = ((JavaLangObjectWrapper<DateTime>)dateTo_button.Tag).Value;

            //List<Record> lstRecord = db.Select<Record, DateTime>((o) => o.Date >= dateFrom && o.Date <= dateTo, (o) => o.Date, true).Result;

            List<Record> lstRecord = null;

            if (costs_checkBox.Checked == false && earnings_checkBox.Checked == false)
            {
                lstRecord = new List<Record>();
            }
            else if (costs_checkBox.Checked == false && earnings_checkBox.Checked == true)
            {
                lstRecord = db.Select<Record, DateTime>(o => o.Date >= dateFrom.Date && o.Date <= dateTo.Date && o.Earnings == true,o => o.Date,false).Result;
            }
            else if (costs_checkBox.Checked == true && earnings_checkBox.Checked == false)
            {
                lstRecord = db.Select<Record, DateTime>(o => o.Date >= dateFrom.Date && o.Date <= dateTo.Date && o.Earnings == false, o => o.Date, false).Result;
            }
            else if (costs_checkBox.Checked == true && earnings_checkBox.Checked == true)
            {
                lstRecord = db.Select<Record, DateTime>(o => o.Date >= dateFrom.Date && o.Date <= dateTo.Date, o => o.Date, false).Result;
            }


            //https://forums.xamarin.com/discussion/32531/permission-denied-for-the-attachment-when-creating-email-on-android
            //        Update: Found solution -
            //Need to copy file to External storage and add it as an attachment from there!
            //var externalPath = global::Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
            //            externalPath = Path.Combine(externalPath, filename);
            //            File.WriteAllBytes(externalPath, bytes);

            List<string> lstStr = new List<string>();
            foreach (var item in lstRecord)
            {
                lstStr.Add(item.IncludeObjects(db).ToString(';', Resources.GetString(Resource.String.DateFormat)));
            }

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



            //if ((int)Build.VERSION.SdkInt >= 23)
            //{
            //    if (ActivityCompat.CheckSelfPermission(this.Activity,Manifest.Permission.Camera) != Permission.Granted)
            //    {
            //        requestCameraPermission();
            //    }
            //    if (CheckSelfPermission(Manifest.Permission.AccessFineLocation) != Permission.Granted)
            //    {
            //        requestLocationPermission();
            //    }
            //}
            #endregion

            try
            {
                var externalPath = global::Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
                externalPath = Path.Combine(externalPath, Resources.GetString(Resource.String.CSVfilename));
                File.WriteAllLines(externalPath, lstStr.ToArray());
                //File.WriteAllBytes(externalPath, bytes);
                SendMail(externalPath);
            }
            catch (Exception ex)
            {
                Log.Error(TAG, nameof(Export_button_Click), ex.Message);
                Toast.MakeText(this.Activity, ex.Message, ToastLength.Short).Show();
            }

            #region OLD - doesnt function on API 23 and higher
            // TODO: make csv file
            //#region Make CSV file
            //string pathCsvFile = DeviceInfo.GetFileFinallPath(Resources.GetString(Resource.String.CSVfilename));

            //using (var streamWriter = new StreamWriter(pathCsvFile, false))
            //{
            //    foreach (var item in lstRecord)
            //    {
            //        streamWriter.WriteLine(item.IncludeObjects(db).ToString(';', Resources.GetString(Resource.String.DateFormat)));
            //    }
            //}
            //#endregion



            // TODO: send email with csv attachement
            //File filelocation = new File(Environment.GetExternalStoragePublicDirectory().getAbsolutePath(), filename);
            //string pathToDatabase = DeviceInfo.GetFileFinallPath(Resources.GetString(Resource.String.DBfilename));
            //SendMail(pathCsvFile);
            #endregion

        }

        //http://stackoverflow.com/questions/9974987/how-to-send-an-email-with-a-file-attachment-in-android
        //https://developer.xamarin.com/recipes/android/networking/email/send_an_email/
        private void SendMail(string filePath)
        {
            Log.Debug(TAG, $"{nameof(SendMail)} - {nameof(filePath)}:{filePath}");

            var file = new Java.IO.File(filePath);
            var uri = Android.Net.Uri.FromFile(file);

            file.SetReadable(true, false);
            Intent emailIntent = new Intent(Intent.ActionSend);//.ACTION_SEND);
            // set the type to 'email'
            //sendIntent.setType("text/html");
            emailIntent.SetType("vnd.android.cursor.dir/email");
            //String to[] = { "asd@gmail.com" };
            //emailIntent.putExtra(Intent.EXTRA_EMAIL, to);
            // the attachment
            emailIntent.PutExtra(Intent.ExtraStream, uri); //.EXTRA_STREAM, path);
            // the mail subject
            emailIntent.PutExtra(Intent.ExtraSubject, Resources.GetString(Resource.String.ApplicationName)); //.EXTRA_SUBJECT, "Subject");
                                                                                                             
            //StartActivity(Intent.CreateChooser(emailIntent, "Send email..."));
        
            //http://stackoverflow.com/questions/26883259/gmail-5-0-app-fails-with-permission-denied-for-the-attachment-when-it-receives
            StartActivityForResult(Intent.CreateChooser(emailIntent, "Send email..."),10);

            // http://stackoverflow.com/questions/18311597/android-attach-image-to-email-doesnt-work
        }

        public override void OnActivityResult(int requestCode, int resultCode, Intent data)
        {
            string msg = $"{nameof(OnActivityResult)} - {nameof(requestCode)}:{requestCode},{nameof(resultCode)}:{resultCode}";
            Log.Debug(TAG, msg);
            base.OnActivityResult(requestCode, resultCode, data);

            if (requestCode == 10)
            {
                Toast.MakeText(this.Activity, msg, ToastLength.Short).Show();
            }
        }

    }
}