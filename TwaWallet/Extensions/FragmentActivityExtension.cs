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
using Android.Support.V4.App;

namespace TwaWallet.Extensions
{
    public static class FragmentActivityExtension
    {
        private const string TAG = "X:" + nameof(FragmentActivityExtension);

        public static ProgressDialog ProgressDialogShow(this FragmentActivity fragmentActivity, ProgressDialog dialog)
        {
            Log.Debug(TAG, nameof(ProgressDialogShow));

            if (dialog == null || dialog.IsShowing == false)
            {
                if (dialog == null)
                {
                    Log.Debug(TAG, $"--{nameof(ProgressDialogShow)}: Create new dialog (dialog is NULL)");
                }
                else if (dialog.IsShowing == false)
                {
                    Log.Debug(TAG, $"--{nameof(ProgressDialogShow)}: Create new dialog (dialog is not showing)");
                }

                dialog = ProgressDialog.Show(fragmentActivity, "", fragmentActivity.Resources.GetString(Resource.String.IsBussy), true);
            }
            else
            {
                Log.Debug(TAG, $"--{nameof(ProgressDialogShow)} - IsShowing");
            }

            return dialog;
        }

        public static void ProgressDialogDismiss(this FragmentActivity fragmentActivity, Android.App.ProgressDialog dialog)
        {
            Log.Debug(TAG, nameof(ProgressDialogDismiss));

            fragmentActivity.RunOnUiThread(() =>
            {
                Log.Debug(TAG, "[3] Closing dialog.");
                if (dialog != null)
                {
                    if (dialog.IsShowing)
                    {
                        Log.Debug(TAG, string.Format($"--{nameof(ProgressDialogDismiss)}: dialog - IsShowing"));
                        dialog.Dismiss();
                    }
                    else
                    {
                        Log.Debug(TAG, string.Format($"--{nameof(ProgressDialogDismiss)}: dialog - NOT IsShowing"));
                    }
                }
                else
                {
                    Log.Debug(TAG, string.Format($"--{nameof(ProgressDialogDismiss)}: dialog is NULL"));
                }
                Log.Debug(TAG, "[4] Dialog closed.");
            });
        }
    }
}