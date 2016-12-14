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
using Database.POCO;
using Database;
using Android.Util;
using Database.Constants;

namespace TwaWallet.Adapters
{
    public class CustomListAdapter : BaseAdapter<Record>
    {
        private const string TAG = "X:" + nameof(CustomListAdapter);

        Activity context;
        List<Record> list;
        IDataContext db;

        List<Category> lstCategory;
        List<Owner> lstOwner;
        List<PaymentType> lstPaymentType;

        public CustomListAdapter(Activity _context, List<Record> _list)
            : base()
        {
            Log.Debug(TAG, nameof(CustomListAdapter));

            this.context = _context;
            this.list = _list;

            string pathToDatabase = DeviceInfo.GetFileFinallPath(this.context.Resources.GetString(Resource.String.DBfilename));
            db = new DataContext(pathToDatabase);

            var r = db.Select<Owner, int>((o) => o.Id > 0, (o) => o.Id).Result;
            lstOwner = r.ToList();
            var r2 = db.Select<PaymentType, int>((o) => o.Id > 0, (o) => o.Id).Result;
            lstPaymentType = r2.ToList();
            var r3 = db.Select<Category, int>((o) => o.Id > 0, (o) => o.Id).Result;
            lstCategory = r3.ToList();
        }

        public override int Count
        {
            get { return list.Count; }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override Record this[int index]
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

            Record item = this[position];

            

            var img = view.FindViewById<ImageView>(Resource.Id.paymentType_imageView);

            if (item.PaymentTypeId == lstPaymentType.Where(p => p.Description == PaymentTypeConst.Card).FirstOrDefault().Id)
            {
                img.SetImageResource(Resource.Drawable.credit_card);
            }
            else
            {
                img.SetImageResource(Resource.Drawable.money);
            }

            view.FindViewById<TextView>(Resource.Id.description_textView).Text = item.Description;
            view.FindViewById<TextView>(Resource.Id.owner_textView).Text = lstOwner.Where(p => p.Id == item.OwnerId).FirstOrDefault().Name;
            view.FindViewById<TextView>(Resource.Id.category_textView).Text = lstCategory.Where(p => p.Id == item.CategoryId).FirstOrDefault().Description;
            view.FindViewById<TextView>(Resource.Id.cost_textView).Text = item.Cost.ToString();
            view.FindViewById<TextView>(Resource.Id.date_textView).Text = item.Date.ToString(this.context.Resources.GetString(Resource.String.DateFormat));
            view.FindViewById<TextView>(Resource.Id.tag_textView).Text = item.Tag;
            view.FindViewById<TextView>(Resource.Id.warranty_textView).Text = item.Warranty.ToString();

            if (item.Earnings)
            {
                view.FindViewById<TextView>(Resource.Id.cost_textView).SetTextColor(Android.Graphics.Color.Green);
            }
            else
            {
                view.FindViewById<TextView>(Resource.Id.cost_textView).SetTextColor(Android.Graphics.Color.Red);
            }

            var paymentType = lstPaymentType.Where(p => p.Id == item.PaymentTypeId).FirstOrDefault();

            //if (paymentType.Description ==  constant from some dictionary .... feel seed from this source as well)
            //{
            //    // TODO: img money
            //}
            //else
            //{
            //    // TODO: img banking card
            //}

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