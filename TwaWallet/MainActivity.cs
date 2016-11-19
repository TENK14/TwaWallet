﻿//using Android.App;
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
//using Org.Apache.Commons.Logging;

namespace TwaWallet
{
    [Android.App.Activity(Label = "TwaWallet_Xam", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : AppCompatActivity
    {
        private const string TAG = "X:" + nameof(MainActivity);

        Fragment[] fragments = new Fragment[]
            {
                new ReportFragment(),
                new HistoryFragment(),
                new ExportFragment(),
                new SettingsFragment(),
                new RegularPaymentsFragment()
            };


        ICharSequence[] titles = CharSequence.ArrayFromStringArray(new[]
            {
                    "REPORT",
                    "HISTORIE",
                    "EXPORT",
                    "NASTAVENI",
                    "PRAVIDELNE PLATBY"
                });


        protected override void OnCreate(Bundle bundle)
        {
            Log.Debug(TAG,nameof(OnCreate));

            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.main);

            

            // Find views
            var pager = FindViewById<ViewPager>(Resource.Id.pager);
            var tabLayout = FindViewById<TabLayout>(Resource.Id.sliding_tabs);
            //var adapter = new CustomPagerAdapter(this, SupportFragmentManager, fragments, titles);
            var toolbar = FindViewById<Toolbar>(Resource.Id.my_toolbar);

            // Setup Toolbar
            SetSupportActionBar(toolbar);
            SupportActionBar.Title = Resources.GetString(Resource.String.ApplicationName);

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

            // Adding Fragments to viewPager
            for (int i = 0; i < fragments.Length; i++)
            {
                adapter.addFragment(fragments[i], titles[i].ToString());
            }

            viewPager.Adapter = adapter;
        }
    }
}

