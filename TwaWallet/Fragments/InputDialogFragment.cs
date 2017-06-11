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
using Android.Util;

namespace TwaWallet.Fragments
{
    public class InputDialogFragment : DialogFragment
    {
        // TAG can be any string of your choice.
        public static readonly string TAG = "X:" + nameof(InputDialogFragment);

        // Initialize this value to prevent NullReferenceExceptions.
        Action<string> _itemEnteredHandler = delegate { };
        string title = string.Empty;
        EditText input;

        public override void OnCreate(Bundle savedInstanceState)
        {
            Log.Debug(TAG, nameof(OnCreate));

            base.OnCreate(savedInstanceState);

        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            Log.Debug(TAG, nameof(OnCreateView));

            Dialog?.SetTitle(title);

            View view = inflater.Inflate(Resource.Layout.Input, container, false);

            input = (Android.Widget.EditText)view.FindViewById(Resource.Id.input_editText);
            input.KeyPress += Input_KeyPress;

            var btnOK = (Android.Widget.Button)view.FindViewById(Resource.Id.ok_button);
            btnOK.Click += BtnOK_Click;

            return view;

        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            Log.Debug(TAG, nameof(BtnOK_Click));

            if (_itemEnteredHandler != null)
            {
                _itemEnteredHandler(input.Text);
                Dismiss();
            }
        }

        private void Input_KeyPress(object sender, View.KeyEventArgs e)
        {
            Log.Debug(TAG, nameof(Input_KeyPress));

            if (_itemEnteredHandler != null)
            {
                _itemEnteredHandler(input.Text);
                Dismiss();
            }
        }

        public static InputDialogFragment NewInstance(string title, Action<string> onItemEntered)
        {
            Log.Debug(TAG, nameof(InputDialogFragment));

            InputDialogFragment frag = new InputDialogFragment();
            frag.title = title;
            frag._itemEnteredHandler = onItemEntered;
            return frag;
        }
    }
}