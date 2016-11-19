using System;
using Android.Content;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using Java.Lang;
using Android.Util;
using TwaWallet.Fragments;
using System.Collections.Generic;

namespace TwaWallet
{
    public class CustomPagerAdapter : FragmentPagerAdapter
    {
        //const int PAGE_COUNT = 2;
        //private string[] tabTitles = { "Tab1", "Tab2" };
        //private readonly Fragment[] fragments;
        //private readonly ICharSequence[] titles;

        readonly Context context;

        private List<Android.Support.V4.App.Fragment> mFragmentList = new List<Android.Support.V4.App.Fragment>();
        private List<string> mFragmentTitleList = new List<string>();

        private const string TAG = "X:" + nameof(CustomPagerAdapter);
        

        public CustomPagerAdapter(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
            Log.Debug(TAG, $"{nameof(CustomPagerAdapter)}1");


        }

        public CustomPagerAdapter(Context context, FragmentManager fm/*, Fragment[] fragments, ICharSequence[] titles*/) : base(fm)
        {
            Log.Debug(TAG, $"{nameof(CustomPagerAdapter)}2");
            
            this.context = context;
            //this.fragments = Fragments;
            //this.titles = titles;
        }

        public override int Count
        {
            get
            {
                //Log.Debug(TAG, $"{nameof(Count)}.get - {(fragments != null ? fragments.Length.ToString() : "null")}");
                Log.Debug(TAG, $"{nameof(Count)}.get - {(mFragmentList != null ? mFragmentList.Count.ToString() : "null")}");

                //return PAGE_COUNT;
                //return fragments.Length;
                return mFragmentList.Count;
            }
        }

        public override Fragment GetItem(int position)
        {
            Log.Debug(TAG, $"{nameof(GetItem)} - position:{position}");

            //return PageFragment.newInstance(position + 1);
            //return fragments[position];
            return mFragmentList[position];
        }

        public override ICharSequence GetPageTitleFormatted(int position)
        {
            Log.Debug(TAG, $"{nameof(GetPageTitleFormatted)} - position:{position}");

            // Generate title based on item position
            //return CharSequence.ArrayFromStringArray(tabTitles)[position];
            //return titles[position];
            return new Java.Lang.String(mFragmentTitleList[position].ToLower());// display the title

        }

        //public View GetTabView(int position)
        //{
        //    Log.Debug(TAG, $"{nameof(GetTabView)} - position:{position}");

        //    //// Given you have a custom layout in `res/layout/custom_tab.xml` with a TextView
        //    //var tv = (TextView)LayoutInflater.From(context).Inflate(Resource.Layout.custom_tab, null);
        //    //tv.Text = titles[position];
        //    //return tv;

        //    return fragments[position].OnCreateView(LayoutInflater.From(context), null, null);
        //}

        public void addFragment(Android.Support.V4.App.Fragment fragment, string title)
        {
            Log.Debug(TAG, $"{nameof(GetPageTitleFormatted)} - fragment:{fragment.ToString()}, title:{title}");

            mFragmentList.Add(fragment);
            mFragmentTitleList.Add(title);
        }
    }
}