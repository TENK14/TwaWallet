using System;
using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using Java.Lang;
using Android.Util;

namespace TwaWallet
{
    public class PageFragment : Fragment
    {
        private const string TAG = "X:" + nameof(PageFragment);
        
        const string ARG_PAGE = "ARG_PAGE";
        private int mPage;



        public static PageFragment newInstance(int page)
        {
            Log.Debug(TAG, $"{nameof(newInstance)} - page:{page}");

            var args = new Bundle();
            args.PutInt(ARG_PAGE, page);
            var fragment = new PageFragment();
            fragment.Arguments = args;
            return fragment;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            Log.Debug(TAG, nameof(OnCreate));

            base.OnCreate(savedInstanceState);
            mPage = Arguments.GetInt(ARG_PAGE);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            Log.Debug(TAG, nameof(OnCreateView));

            var view = inflater.Inflate(Resource.Layout.fragment_page, container, false);
            var textView = (TextView)view;
            textView.Text = "Fragment #" + mPage;
            return view;
        }
    }
}