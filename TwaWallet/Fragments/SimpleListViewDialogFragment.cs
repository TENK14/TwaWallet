using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Database.POCO;

namespace TwaWallet.Fragments
{
    public class SimpleListViewDialogFragment<T> : DialogFragment
    {
        // TAG can be any string of your choice.
        public static readonly string TAG = "X:" + nameof(SimpleListViewDialogFragment<T>);// typeof(MyDialogFragment).Name;//.ToUpper();

        string title = string.Empty;
        Action<T> _itemSelectedHandler = delegate { };
        List<T> _lst;


        public override void OnCreate(Bundle savedInstanceState)
        {
            Log.Debug(TAG, nameof(OnCreate));

            base.OnCreate(savedInstanceState);

            // Create your fragment here
            //this.ListAdapter = new ArrayAdapter<string>(Activity, Android.Resource.Layout.SimpleExpandableListItem1, values);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            Log.Debug(TAG, nameof(OnCreateView));

            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);

            //return base.OnCreateView(inflater, container, savedInstanceState);
            Dialog?.SetTitle(title);
            
            View view = inflater.Inflate(Resource.Layout.SimpleListView, container, false);
            ListView listView = (Android.Widget.ListView)view.FindViewById(Resource.Id.DataListView);
            listView.SetPadding(5, 5, 5, 5);
            try
            { 
                List<string> list;
                list = _lst.Select(p => p.ToString()).ToList();

                #region OLD
                /**
                //if (typeof(T) == typeof(BaseWithDescriptionAndDefault))
                if ( (typeof(T) == typeof(Category))
                    || (typeof(T) == typeof(PaymentType)) )
                //if (T.GetType() == typeof(BaseWithDescriptionAndDefault))
                {
                    list = _lst.Select(p => (p as BaseWithDescriptionAndDefault).Description).ToList();
                }
                else if (typeof(T) == typeof(Record))
                {
                    list = _lst.Select(p => (p as Record).Description).ToList();
                }
                else if (typeof(T) == typeof(RecurringPayment))
                {
                    list = _lst.Select(p => (p as RecurringPayment).Description).ToList();
                }
                else if (typeof(T) == typeof(Owner))
                {
                    list = _lst.Select(p => (p as Owner).Name).ToList();
                }
                else
                {
                    list = _lst.Select(p => p.ToString()).ToList();
                }
                /**/
                #endregion

                var adapter = new ArrayAdapter<string>(Activity, Android.Resource.Layout.SimpleExpandableListItem1, list);
                                
                listView.SetAdapter(adapter);

                listView.ChoiceMode = ChoiceMode.Single;
                
                listView.LongClickable = false;
                listView.Clickable = true;

                listView.ItemClick += OnItemClick;

                //Dialog.SetTitle(this.title);
            }
            catch (Exception ex)
            {
                Log.Error(TAG, nameof(OnCreateView), ex.GetBaseException().Message);
            }
            return view;

        }

        private void OnItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Log.Debug(TAG, nameof(OnItemClick));

            if (_itemSelectedHandler != null)
            {
                _itemSelectedHandler(this._lst[e.Position]);
                Dismiss();
            }
        }
        
        public static SimpleListViewDialogFragment<T> NewInstance(List<T> lst, Action<T> onSelectedItem, string title)
        {
            Log.Debug(TAG, nameof(SimpleListViewDialogFragment<T>));

            var result = NewInstance(lst, onSelectedItem);
            result.title = title;

            return result;
        }

        public static SimpleListViewDialogFragment<T> NewInstance(List<T> lst, Action<T> onSelectedItem)
        {
            Log.Debug(TAG, nameof(SimpleListViewDialogFragment<T>));
            
            SimpleListViewDialogFragment<T> frag = new SimpleListViewDialogFragment<T>();
            
            frag._lst = lst;
            frag._itemSelectedHandler = onSelectedItem;
            return frag;
        }
        
    }
}