using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Telephony;
using Android.Views;
using Android.Widget;

namespace HearMeApp.Android
{
    [BroadcastReceiver(Label = "SMS Receiver")]
    [IntentFilter(new string[] { "android.provider.Telephony.SMS_RECEIVED" })]
    public class SmsReceiver : BroadcastReceiver
    {
        public static readonly string IntentAction = "android.provider.Telephony.SMS_RECEIVED";

        public override void OnReceive(Context context, Intent intent)
        {
            if (intent.HasExtra("pdus"))
            {
                var smsArray = (Java.Lang.Object[])intent.Extras.Get("pdus");
                var address = "";
                var message = "";

                foreach (var item in smsArray)
                {
                    var sms = SmsMessage.CreateFromPdu((byte[])item, "3gpp");
                    message += sms.MessageBody;
                    address = sms.OriginatingAddress;
                }
            }
        }
    }
}