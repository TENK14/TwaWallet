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
using Android.Content.PM;
using Android.Util;
using System.IO;

namespace TwaWallet
{
    public class DeviceInfo
    {
        #region Members

        private static readonly string TAG = "X:" + typeof(DeviceInfo).Name;
        protected string device;
        protected string deviceAndroidId;
        protected bool hasDatawedge;
        protected bool hasMxExtension;
        protected bool hasZxing;
        protected bool hasHardwareKeyboard;

        protected List<ApplicationInfo> installedApplication;

        protected static DeviceInfo devInfo;

        #endregion Members

        public static DeviceInfo GetDeviceInfo()
        {
            Log.Debug(TAG, nameof(GetDeviceInfo));

            if (devInfo == null)
            {
                devInfo = new DeviceInfo();
            }
            return devInfo;
        }

        protected DeviceInfo()
        {
            Log.Debug(TAG, nameof(DeviceInfo));
        }

        public string Device
        {
            get
            {
                Log.Debug(TAG, nameof(Device));

                if (device == null)
                {
                    device = GetDeviceName();
                }
                return device;
            }
        }

        public string DeviceAndroidId
        {
            get
            {
                Log.Debug(TAG, nameof(DeviceAndroidId) + ".get");

                if (deviceAndroidId == null)
                {
                    deviceAndroidId = Android.Provider.Settings.Secure.GetString(Application.Context.ContentResolver, Android.Provider.Settings.Secure.AndroidId);
                }
                return deviceAndroidId;
            }
        }
        
        public bool HasHardwareKeyboard
        {
            get
            {
                Log.Debug(TAG, nameof(HasHardwareKeyboard) + ".get");

                var kb = Android.Content.Res.Resources.System.Configuration.Keyboard;
                return kb == Android.Content.Res.KeyboardType.Qwerty || kb == Android.Content.Res.KeyboardType.Twelvekey;
            }
        }
        
        protected String GetDeviceName()
        {
            Log.Debug(TAG, nameof(GetDeviceName));

            String manufacturer = Build.Manufacturer;
            String model = Build.Model;
            if (model.StartsWith(manufacturer))
            {
                return Capitalize(model);
            }
            else
            {
                return Capitalize(manufacturer) + " " + model;
            }
        }

        protected String Capitalize(String s)
        {
            Log.Debug(TAG, nameof(Capitalize));

            if (s == null || s.Length == 0)
            {
                return "";
            }

            return s.ToUpper();
        }

        protected List<ApplicationInfo> GetInstalledApplication()
        {
            Log.Debug(TAG, nameof(GetInstalledApplication));

            if (installedApplication == null)
            {
                installedApplication = Application.Context.PackageManager.GetInstalledApplications(PackageInfoFlags.MetaData).ToList();
                installedApplication.ForEach(a => { Console.WriteLine(a.PackageName); });
            }

            return installedApplication;
        }

        public static string GetDirectoryFinallPath()
        {
            var storage = DeviceInfo.GetFilesStorage();
            var dbPath = DeviceInfo.GetFilesPath();

            return Path.Combine(storage, dbPath);
        }

        public static string GetFileFinallPath(string sqliteFilename)
        {
            //var storage = DeviceInfo.GetFilesStorage();
            //var dbPath = DeviceInfo.GetFilesPath();

            //return Path.Combine(storage, dbPath, sqliteFilename);

            string result = Path.Combine(GetDirectoryFinallPath(), sqliteFilename);
            return result;
        }
        
        private bool IntentsExistingChecker(params Intent[] lstIntent)
        {
            Log.Debug(TAG, nameof(IntentsExistingChecker));

            bool result = true;

            foreach (Intent item in lstIntent)
            {
                var lstActivities = Application.Context.PackageManager.QueryIntentActivities(item, PackageInfoFlags.Activities);
                var lstServices = Application.Context.PackageManager.QueryIntentServices(item, PackageInfoFlags.Services);

                if ((lstActivities == null || lstActivities.Count == 0) &&
                    (lstServices == null || lstServices.Count == 0))
                {
                    result = false;
                    break;
                }
            }

            return result;
        }

        // /data/user/0/TwaWallet.TwaWallet/files/
        public static string GetFilesStorage()
        {
            Log.Debug(TAG, nameof(GetFilesStorage));

            string result = string.Empty;

            // "/storage/emulated/0/TWA"
            //result = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;

            // "/data/user/0/TwaWallet.TwaWallet/files/TWA/TwaWallet/TwaWalletDB.db"
            result = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);


            Log.Debug(TAG, $"{nameof(GetFilesPath)} - {nameof(result)}:{result}");

            return result;
        }

        // /TWA/TwaWallet/
        public static string GetFilesPath()
        {
            Log.Debug(TAG, nameof(GetFilesPath));
            string result = Android.App.Application.Context.GetString(Resource.String.DBPath);
            Log.Debug(TAG, $"{nameof(GetFilesPath)} - {nameof(result)}:{result}");

            return result;
        }
    }
}