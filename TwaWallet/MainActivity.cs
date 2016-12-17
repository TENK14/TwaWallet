//using Android.App;
using Android.OS;
using Android.Support.V4.View;
using Android.Support.V4.App;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Runtime;
using TwaWallet.Fragments;
using Android.Util;
using Java.Lang;
using System.IO;
using Database;
using Android.Widget;
//using Org.Apache.Commons.Logging;

namespace TwaWallet
{
    [Android.App.Activity(Label = "TwaWallet_Xam", 
        MainLauncher = true, 
        Icon = "@drawable/icon",
        ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait,
        WindowSoftInputMode = Android.Views.SoftInput.AdjustPan)]
    public class MainActivity : AppCompatActivity
    {
        private const int REQUEST_CODE_ASK_PERMISSIONS = 123;


        private const string TAG = "X:" + nameof(MainActivity);

        Fragment[] fragments = new Fragment[]
            {
                new ReportFragment(),
                new HistoryFragment(),
                new ExportFragment(),
                new SettingsFragment(),
                new RegularPaymentsFragment()
            };


        //ICharSequence[] titles = CharSequence.ArrayFromStringArray(new[]
        //    {
        //            Resources.GetString(Resource.String.Report),//"REPORT",
        //            "HISTORIE",
        //            "EXPORT",
        //            "NASTAVENI",
        //            "PRAVIDELNE PLATBY"
        //        });


        protected override void OnCreate(Bundle bundle)
        {
            Log.Debug(TAG,nameof(OnCreate));

            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.main);


            int minWorker, minIOC;
            // Get the current settings.
            System.Threading.ThreadPool.GetMinThreads(out minWorker, out minIOC);
            // Change the minimum number of worker threads to four, but
            // keep the old setting for minimum asynchronous I/O 
            // completion threads.
            if (System.Threading.ThreadPool.SetMinThreads(8, minIOC))
            {
                Log.Debug(TAG, $"{nameof(OnCreate)} - The minimum number of threads was set successfully.");
            }
            else
            {
                Log.Debug(TAG, $"{nameof(OnCreate)} - The minimum number of threads was not changed.");
            }


            // Find views
            var pager = FindViewById<ViewPager>(Resource.Id.pager);
            var tabLayout = FindViewById<TabLayout>(Resource.Id.sliding_tabs);
            //var adapter = new CustomPagerAdapter(this, SupportFragmentManager, fragments, titles);
            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.my_toolbar);

            var code = ApplicationContext.PackageManager.GetPackageInfo(ApplicationContext.PackageName, 0).VersionCode;
            var name = this.ApplicationContext.PackageManager.GetPackageInfo(ApplicationContext.PackageName, 0).VersionName;

            // Setup Toolbar
            SetSupportActionBar(toolbar);
            SupportActionBar.Title = $"{Resources.GetString(Resource.String.ApplicationName)} ({code}-{name})";

            // Set adapter to view pager
            //pager.Adapter = adapter;
            //viewPager.Adapter = new TabsFragmentPagerAdapter(SupportFragmentManager, fragments, titles);
            SetupViewPager(pager);

            // Setup tablayout with view pager
            tabLayout.SetupWithViewPager(pager);
            tabLayout.HorizontalScrollBarEnabled = true;

            //// Iterate over all tabs and set the custom view
            //for (int i = 0; i < tabLayout.TabCount; i++)
            //{
            //    TabLayout.Tab tab = tabLayout.GetTabAt(i);
            //    tab.SetCustomView(adapter.GetTabView(i));
            //}

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
            #endregion Show Version


            // create DB path
            //var docsFolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
            //var dbPath = Resources.GetString(Resource.String.DBPath);
            //var pathToDatabase = System.IO.Path.Combine(dbPath, Resources.GetString(Resource.String.DBfilename));// "db_sqlcompnet.db");

            //Directory.CreateDirectory(directoryPath);            
            //Directory.CreateDirectory(dbPath);

            string directoryPath = DeviceInfo.GetDirectoryFinallPath();
            Directory.CreateDirectory(directoryPath);

            #region DB
            string pathToDatabase = DeviceInfo.GetFileFinallPath(Resources.GetString(Resource.String.DBfilename));
            //var dbAdapter = new kCollect2.Data.Database.StateMachineDataAdapter(new SQLite_Android(new LogWrapper()), dbPath);
            //dbAdapter.CreateOrUpdateDatabase();

            #endregion

            Android.Widget.Toast.MakeText(this, pathToDatabase, Android.Widget.ToastLength.Short).Show();
            Log.Debug(TAG, $"directory: {directoryPath}, pathToDB: {pathToDatabase}");

            //#region DB
            //string dbPath = DeviceInfo.GetFileFinallPath(Resources.GetString(Resource.String.DBfilename));
            //var dbAdapter = new kCollect2.Data.Database.StateMachineDataAdapter(new SQLite_Android(new LogWrapper()), dbPath);
            //dbAdapter.CreateOrUpdateDatabase();
            //#endregion

            if (!File.Exists(pathToDatabase))
            {
                #region Ask for permission
                //const string permission = Android.Manifest.Permission.WriteExternalStorage;
                //var hasWriteContactsPermission = CheckSelfPermission(permission);

                //if (hasWriteContactsPermission != Android.Content.PM.Permission.Granted)
                //{
                //    RequestPermissions(new string[] { Android.Manifest.Permission.WriteExternalStorage },
                //            REQUEST_CODE_ASK_PERMISSIONS);
                //    return;
                //} 
                #endregion

                Log.Debug(TAG, $"DB will be created!");
                IDataContext db = new DataContext(pathToDatabase);
                var result = db.CreateDatabase().Result;
                //Toast.MakeText(this,result,ToastLength.Short).Show();// (pathToDatabase);
                Log.Debug(TAG, $"DB was created!:: {result}");
            }
            else
            {
                //Toast.MakeText(this, "DB soubor již existuje.", ToastLength.Short).Show();
                Log.Debug(TAG, "DB exists!");
            }


        }

        private void InitialFragment()
        {
            Log.Debug(TAG, nameof(InitialFragment));

            //exploreFrg = new Explore();
            //featuredFrg = new Featured();
            //moreFrg = new More();
            //todoFrag = new Todo();
        }

        public void SetupViewPager(ViewPager viewPager)
        {
            Log.Debug(TAG, nameof(SetupViewPager));

            InitialFragment();
            //ViewPagerAdapter adapter = new ViewPagerAdapter(SupportFragmentManager);
            var adapter = new CustomPagerAdapter(this, SupportFragmentManager);
            //adapter.addFragment(exploreFrg, "Explore");
            //adapter.addFragment(featuredFrg, "Featured");


            var titles = new[]
            {
                Resources.GetString(Resource.String.Report),
                Resources.GetString(Resource.String.History),
                Resources.GetString(Resource.String.Export),
                Resources.GetString(Resource.String.Settings),
                Resources.GetString(Resource.String.RegularPayments)
            };

            // Adding Fragments to viewPager
            for (int i = 0; i < fragments.Length; i++)
            {
                adapter.addFragment(fragments[i], titles[i].ToString());
            }

            viewPager.Adapter = adapter;

            viewPager.PageSelected += ViewPager_PageSelected;
        }

        private void ViewPager_PageSelected(object sender, ViewPager.PageSelectedEventArgs e)
        {
            Log.Debug(TAG, nameof(ViewPager_PageSelected));

            var fr = ((FragmentPagerAdapter)((ViewPager)sender).Adapter).GetItem(e.Position);
            fr.OnResume();

        }
    }
}

