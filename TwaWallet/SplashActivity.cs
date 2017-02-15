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
using Database.Constants;

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

                #region Create DB
                Log.Debug(TAG, $"DB will be created!");
                IDataContext db = DataContextFactory.GetDataContext(pathToDatabase);

                var lstOwner = new Owner[]
                {
                    new Owner {Name = Resources.GetString(Resource.String.Default), IsDefault = true },
                };

                var lstCategory = new Category[]
                {
                    new Category {Description = Resources.GetString(Resource.String.Launch), IsDefault = true },
                    new Category {Description = Resources.GetString(Resource.String.Trip), IsDefault = false },
                    new Category {Description = Resources.GetString(Resource.String.Food), IsDefault = false },
                    new Category {Description = Resources.GetString(Resource.String.Miscellaneous), IsDefault = false },
                    new Category {Description = Resources.GetString(Resource.String.RestaurantBar), IsDefault = false },
                    new Category {Description = Resources.GetString(Resource.String.Household), IsDefault = false },
                    new Category {Description = Resources.GetString(Resource.String.ClothesShoes), IsDefault = false },
                    new Category {Description = Resources.GetString(Resource.String.Fuel), IsDefault = false },
                    new Category {Description = Resources.GetString(Resource.String.Services), IsDefault = false },
                    new Category {Description = Resources.GetString(Resource.String.Drugstore), IsDefault = false },
                    new Category {Description = Resources.GetString(Resource.String.Income), IsDefault = false },
                    new Category {Description = Resources.GetString(Resource.String.Medicament), IsDefault = false },
                    new Category {Description = Resources.GetString(Resource.String.Entertainment), IsDefault = false },
                    new Category {Description = Resources.GetString(Resource.String.Sport), IsDefault = false },
                    new Category {Description = Resources.GetString(Resource.String.Transportation), IsDefault = false },
                };

                var lstPaymentType = new PaymentType[]
                {
                    new PaymentType {Description = Resources.GetString(Resource.String.Cash), IsDefault = true },
                    new PaymentType {Description = Resources.GetString(Resource.String.Card), IsDefault = false },
                };

                var lstInterval = new Interval[]
                    {
                        new Interval {Description = Resources.GetString(Resource.String.Daily), IntervalCode = "000001" , IsDefault = false },
                        new Interval {Description = Resources.GetString(Resource.String.Weekly), IntervalCode = "000007" ,IsDefault = false },
                        new Interval {Description = Resources.GetString(Resource.String.Monthly), IntervalCode = "000100" ,IsDefault = true },
                        new Interval {Description = Resources.GetString(Resource.String.Quarterly), IntervalCode = "000300" ,IsDefault = false },
                        new Interval {Description = Resources.GetString(Resource.String.Yearly), IntervalCode = "010000" ,IsDefault = false },
                    };

                bool result = db.CreateDatabase().Result;
                if (result)
                {
                    if (db.Select<Category, int>(p => p.Id > 0, p => p.Id).Result.Count <= 0)
                    {
                        var r2 = (new Seed()).FillDB(db, lstOwner, lstCategory, lstPaymentType, lstInterval);
                        if (r2.Result)
                        {

                            //return true;
                        }
                        else
                        {
                            //return false;
                        }
                    }
                    else
                    {
                        //return true;
                    }
                }
                Log.Debug(TAG, $"DB was created!:: {result}");
                #endregion


                PaymentTypeConst.Cash = Resources.GetString(Resource.String.Cash);
                PaymentTypeConst.Card = Resources.GetString(Resource.String.Card);

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