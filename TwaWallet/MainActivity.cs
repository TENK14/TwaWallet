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
//using Java.Lang;
using System.IO;
using Database;
using Android.Widget;
using Database.POCO;
using System;
using Android.Support.V4.Widget;
//using Org.Apache.Commons.Logging;

namespace TwaWallet
{
    [Android.App.Activity(Label = "TwaWallet", 
        //MainLauncher = true, 
        Icon = "@drawable/icon",
        ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait,
        WindowSoftInputMode = Android.Views.SoftInput.AdjustPan)]
    public class MainActivity : AppCompatActivity
    {
        private const int REQUEST_CODE_ASK_PERMISSIONS = 123;

        private DrawerLayout mDrawerLayout;
        private ListView mDrawerList;
        
        private const string TAG = "X:" + nameof(MainActivity);

        Fragment[] fragments = new Fragment[]
            {
                new HistoryFragment(),
                new ExportFragment(),
                new SettingsFragment(),
                new RecurringPaymentsFragment()
            };
        
        protected override void OnCreate(Bundle bundle)
        {
            Log.Debug(TAG,nameof(OnCreate));

            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.main);

            // finally change the color            
            //window.setStatusBarColor(activity.getResources().getColor(R.color.my_statusbar_color));

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
            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.my_toolbar);

            var code = ApplicationContext.PackageManager.GetPackageInfo(ApplicationContext.PackageName, 0).VersionCode;
            var name = this.ApplicationContext.PackageManager.GetPackageInfo(ApplicationContext.PackageName, 0).VersionName;

            // Setup Toolbar
            SetSupportActionBar(toolbar);
            //SupportActionBar.Title = $"{Resources.GetString(Resource.String.ApplicationName)} ({code}-{name})";
            SupportActionBar.Title = Resources.GetString(Resource.String.ApplicationName);

            // Set adapter to view pager
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

            var adapter = new CustomPagerAdapter(this, SupportFragmentManager);

            var titles = new[]
            {
                Resources.GetString(Resource.String.History),
                Resources.GetString(Resource.String.Export),
                Resources.GetString(Resource.String.Settings),
                Resources.GetString(Resource.String.RecurringPayments)
            };

            // Adding Fragments to viewPager
            for (int i = 0; i < fragments.Length; i++)
            {
                adapter.AddFragment(fragments[i], titles[i].ToString());
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

