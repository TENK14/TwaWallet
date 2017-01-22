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
    class RecurringPaymentListAdapter : BaseAdapter<RecurringPayment>
    {
        private const string TAG = "X:" + nameof(RecurringPaymentListAdapter);

        Activity context;
        List<RecurringPayment> list;
        IDataContext db;

        List<Category> lstCategory;
        List<Owner> lstOwner;
        List<PaymentType> lstPaymentType;
        List<Interval> lstInterval;

        public RecurringPaymentListAdapter(Activity _context, List<RecurringPayment> _list, IDataContext db)
            : base()
        {
            Log.Debug(TAG, nameof(RecurringPaymentListAdapter));

            this.context = _context;
            this.list = _list;

            this.db = db;

            var r = db.Select<Owner, int>((o) => o.Id > 0, (o) => o.Id).Result;
            lstOwner = r.ToList();
            var r2 = db.Select<PaymentType, int>((o) => o.Id > 0, (o) => o.Id).Result;
            lstPaymentType = r2.ToList();
            var r3 = db.Select<Category, int>((o) => o.Id > 0, (o) => o.Id).Result;
            lstCategory = r3.ToList();
            var r4 = db.Select<Interval, int>((o) => o.Id > 0, (o) => o.Id).Result; ;
            lstInterval = r4.ToList();
        }

        public override int Count
        {
            get { return list.Count; }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override RecurringPayment this[int index]
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
                view = context.LayoutInflater.Inflate(Resource.Layout.RecurringPaymentListRowLayout, parent, false);
            }

            RecurringPayment item = this[position];
            //item.IncludeObjects(db);

            var img = view.FindViewById<ImageView>(Resource.Id.paymentType_imageView);
            var paymentType_textView = view.FindViewById<TextView>(Resource.Id.paymentType_textView);

            if (item.PaymentTypeId == lstPaymentType.Where(p => p.Description == PaymentTypeConst.Card).FirstOrDefault().Id)
            {
                img.Visibility = ViewStates.Visible;
                paymentType_textView.Visibility = ViewStates.Gone;

                img.SetImageResource(Resource.Drawable.credit_card);
            }
            else if (item.PaymentTypeId == lstPaymentType.Where(p => p.Description == PaymentTypeConst.Money).FirstOrDefault().Id)
            {
                img.Visibility = ViewStates.Visible;
                paymentType_textView.Visibility = ViewStates.Gone;

                img.SetImageResource(Resource.Drawable.money);
            }
            else
            {
                img.Visibility = ViewStates.Gone;
                paymentType_textView.Visibility = ViewStates.Visible;

                paymentType_textView.Text = lstPaymentType.Where(p => p.Id == item.PaymentTypeId).FirstOrDefault()?.Description ?? string.Empty;
                //paymentType_textView.Text = item.PaymentType?.Description ?? string.Empty;
            }

            view.FindViewById<TextView>(Resource.Id.description_textView).Text = item.Description;
            view.FindViewById<TextView>(Resource.Id.owner_textView).Text = lstOwner.Where(p => p.Id == item.OwnerId).FirstOrDefault()?.Name ?? string.Empty;
            //view.FindViewById<TextView>(Resource.Id.owner_textView).Text = item.Owner?.Name ?? string.Empty;
            // earnings cant have category
            view.FindViewById<TextView>(Resource.Id.category_textView).Text = lstCategory.Where(p => p.Id == item.CategoryId).FirstOrDefault()?.Description ?? string.Empty;
            //view.FindViewById<TextView>(Resource.Id.category_textView).Text = item.Category?.Description ?? string.Empty;
            view.FindViewById<TextView>(Resource.Id.cost_textView).Text = item.Cost.ToString();
            view.FindViewById<TextView>(Resource.Id.date_textView).Text = item.DateCreated.ToString(this.context.Resources.GetString(Resource.String.DateFormat));
            view.FindViewById<TextView>(Resource.Id.endDate_textView).Text = item.EndDate.ToString(this.context.Resources.GetString(Resource.String.DateFormat));
            view.FindViewById<TextView>(Resource.Id.tag_textView).Text = item.Tag;
            view.FindViewById<TextView>(Resource.Id.warranty_textView).Text = item.Warranty.ToString();
            view.FindViewById<TextView>(Resource.Id.lastUpdate_textView).Text = item?.LastUpdate.ToString(this.context.Resources.GetString(Resource.String.DateFormat)) ?? string.Empty;
            view.FindViewById<TextView>(Resource.Id.interval_textView).Text = lstInterval.Where(p => p.Id == item.IntervalId).FirstOrDefault()?.Description ?? string.Empty;
            //view.FindViewById<TextView>(Resource.Id.interval_textView).Text = item?.Interval?.Description ?? string.Empty;

            if (item.Earnings)
            {
                view.FindViewById<TextView>(Resource.Id.cost_textView).SetTextColor(Android.Graphics.Color.Green);
            }
            else
            {
                view.FindViewById<TextView>(Resource.Id.cost_textView).SetTextColor(Android.Graphics.Color.Red);
            }

            if (!item.IsActive)
            {
                view.SetBackgroundColor(context.Resources.GetColor(Resource.Color.material_grey_600));
            }
            else
            {
                //view.SetBackgroundColor(context.Resources.GetColor(Resource.Color.yellow));
                view.SetBackgroundColor(Android.Graphics.Color.Transparent);
            }

            var paymentType = lstPaymentType.Where(p => p.Id == item.PaymentTypeId).FirstOrDefault();

            return view;
        }
    }
}