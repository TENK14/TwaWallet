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

namespace TwaWallet.Fragments
{
    public class ExportFragment : Fragment
    {
        private const string TAG = "X:" + nameof(ExportFragment);

        private DataContext db;

        #region GUI
        Button dateFrom_button;
        Button dateTo_button;
        Button export_button;
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

            string pathToDatabase = DeviceInfo.GetFileFinallPath(Resources.GetString(Resource.String.DBfilename));
            db = new DataContext(pathToDatabase);

            InitLayout();

            return v;
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

            var lstRecord = db.Select<Record, int>((o) => o.Date >= dateFrom && o.Date <= dateTo, (o) => o.Id, false).Result;

            // TODO: make csv file

            // TODO: send email with csv attachement
            //http://stackoverflow.com/questions/9974987/how-to-send-an-email-with-a-file-attachment-in-android
            //https://developer.xamarin.com/recipes/android/networking/email/send_an_email/
            String filename = "contacts_sid.vcf";
            //File filelocation = new File(Environment.GetExternalStoragePublicDirectory().getAbsolutePath(), filename);
            string pathToDatabase = DeviceInfo.GetFileFinallPath(Resources.GetString(Resource.String.DBfilename));
            
            //File filelocation = new File(pathToDatabase, filename);
            //Uri path = Uri.fromFile(filelocation);
            Intent emailIntent = new Intent(Intent.ActionSend);//.ACTION_SEND);
            // set the type to 'email'
            //sendIntent.setType("text/html");
            emailIntent.SetType("vnd.android.cursor.dir/email");
            //String to[] = { "asd@gmail.com" };
            //emailIntent.putExtra(Intent.EXTRA_EMAIL, to);
            // the attachment
            //emailIntent.PutExtra(Intent.ExtraStream, path); //.EXTRA_STREAM, path);
            // the mail subject
            emailIntent.PutExtra(Intent.ExtraSubject, Resources.GetString(Resource.String.ApplicationName)); //.EXTRA_SUBJECT, "Subject");
            StartActivity(Intent.CreateChooser(emailIntent, "Send email..."));
        }
    }
}