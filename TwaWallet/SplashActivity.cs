using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V7.App;
using Android.Util;
using Database.POCO;
using Database;
using Android.Support.V4.App;
using System.IO;

namespace TwaWallet
{
    [Activity(Theme = "@style/SplashTheme", MainLauncher = true, NoHistory = true)]
    public class SplashActivity : AppCompatActivity
    {
        private static readonly string TAG = "X:" + typeof(SplashActivity).Name;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            Log.Debug(TAG, nameof(OnCreate));

            base.OnCreate(savedInstanceState);

            // Create your application here

            #region Show Version
            /**/
            //string version = System.Reflection.Assembly.GetExecutingAssembly()
            //                               .GetName()
            //                               .Version
            //                               .ToString();

            //string versionCompatibility = System.Reflection.Assembly.GetExecutingAssembly()
            //                               .GetName()
            //                               .VersionCompatibility
            //                               .ToString();

            //var code = ApplicationContext.PackageManager.GetPackageInfo(ApplicationContext.PackageName, 0).VersionCode;
            //var name = this.ApplicationContext.PackageManager.GetPackageInfo(ApplicationContext.PackageName, 0).VersionName;


            //RunOnUiThread(() =>
            //{
            //    string s = "test";
            //    Android.Widget.Toast.MakeText(this, $"{version}-{versionCompatibility}", Android.Widget.ToastLength.Short).Show();
            //    Android.Widget.Toast.MakeText(this, $"{name}-{code}", Android.Widget.ToastLength.Short).Show();
            //});
            /**/

            var code = ApplicationContext.PackageManager.GetPackageInfo(ApplicationContext.PackageName, 0).VersionCode;
            var name = this.ApplicationContext.PackageManager.GetPackageInfo(ApplicationContext.PackageName, 0).VersionName;

            var str = $"{Resources.GetString(Resource.String.ApplicationName)} ({code}-{name})";
            #endregion Show Version

            try
            {

                // create DB path
                string directoryPath = DeviceInfo.GetDirectoryFinallPath();
                Directory.CreateDirectory(directoryPath);

                #region DB
                string pathToDatabase = DeviceInfo.GetFileFinallPath(Resources.GetString(Resource.String.DBfilename));
                #endregion

                //Android.Widget.Toast.MakeText(this, pathToDatabase, Android.Widget.ToastLength.Short).Show();
                Log.Debug(TAG, $"directory: {directoryPath}, pathToDB: {pathToDatabase}");

                #region Ask for permission
                //https://github.com/xamarin/monodroid-samples/blob/master/android-m/RuntimePermissions/MainActivity.cs
                const int REQUEST_CODE_ASK_PERMISSIONS = 123;
                const string permission = Android.Manifest.Permission.WriteExternalStorage;

                var hasWriteContactsPermission = ActivityCompat.CheckSelfPermission(this, permission);

                if (hasWriteContactsPermission != Android.Content.PM.Permission.Granted)
                {
                    RequestPermissions(new string[] { Android.Manifest.Permission.WriteExternalStorage },
                            REQUEST_CODE_ASK_PERMISSIONS);
                    return;
                }
                #endregion

                Log.Debug(TAG, $"DB will be created!");
                IDataContext db = DataContextFactory.GetDataContext(pathToDatabase);
                var result = db.CreateDatabase().Result;
                Log.Debug(TAG, $"DB was created!:: {result}");


                #region RecurringPayments
                /**/
                var lstRecurringPayment = db.Select<RecurringPayment, int>(p => p.IsActive, p => p.Id, false).Result;

                if (lstRecurringPayment != null && lstRecurringPayment.Count > 0)
                {
                    var dtNow = DateTime.Now.Date;
                    foreach (var item in lstRecurringPayment)
                    {
                        if (item.IsActive)
                        {
                            var i = item.IncludeObjects(db);

                            DateTime dt = i.Interval.NextDateTime(i.LastUpdate).Date;

                            while (dt <= dtNow)
                            {
                                Record record = new Record()
                                {
                                    CategoryId = i.CategoryId,
                                    Cost = i.Cost,
                                    Date = dt, //dtNow,
                                    Description = i.Description,
                                    Earnings = i.Earnings,
                                    OwnerId = i.OwnerId,
                                    PaymentTypeId = i.PaymentTypeId,
                                    Tag = i.Tag,
                                    Warranty = i.Warranty,

                                };
                                db.Insert(record);

                                i.LastUpdate = dt;
                                db.Update(i);

                                dt = i.Interval.NextDateTime(i.LastUpdate).Date;
                                
                            }
                        }
                    }
                }
                /**/
                #endregion
            }
            catch (Exception ex)
            {
                Log.Error(TAG, ex.Message);
            }


            Intent intent = new Intent(this, typeof(MainActivity));
            StartActivity(intent);
            Finish();
        }
    }
}