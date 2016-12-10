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

namespace TwaWallet.Adapters
{
    public class CustomListAdapter : BaseAdapter<string>
    {
        Activity context;
        List<string> list;

        public CustomListAdapter(Activity _context, List<string> _list)
            : base()
        {
            this.context = _context;
            this.list = _list;
        }

        public override int Count
        {
            get { return list.Count; }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override string this[int index]
        {
            get { return list[index]; }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView;

            // re-use an existing view, if one is available
            // otherwise create a new one
            if (view == null)
            {
                view = context.LayoutInflater.Inflate(Resource.Layout.ListRowLayout, parent, false);
            }

            string item = this[position];
            view.FindViewById<TextView>(Resource.Id.Title).Text = item;
            //view.FindViewById<TextView>(Resource.Id.Title).Text = item.title;
            //view.FindViewById<TextView>(Resource.Id.Description).Text = item.description;

            //using (var imageView = view.FindViewById<ImageView>(Resource.Id.Thumbnail))
            //{
            //    string url = Android.Text.Html.FromHtml(item.thumbnail).ToString();

            //    //Download and display image
            //    Koush.UrlImageViewHelper.SetUrlDrawable(imageView,
            //        url, Resource.Drawable.Placeholder);
            //}
            return view;
        }
    }
}