using Android.App;
using Android.Content;
using Android.Provider;
using Android.Widget;
using System;

namespace HearMeApp.Android
{
    [BroadcastReceiver]
    [IntentFilter(new string[] { "android.provider.Telephony.SMS_RECEIVED" })]
    public class SmsReceiver : BroadcastReceiver
    {
        public static readonly string SmsReceivedAction = "android.provider.Telephony.SMS_RECEIVED";

        public override void OnReceive(Context context, Intent intent)
        {
            if (intent.Action != SmsReceivedAction)
            {
                return;
            }

            var smsArray = Telephony.Sms.Intents.GetMessagesFromIntent(intent);

            foreach (var sms in smsArray)
            {
                Toast.MakeText(context, string.Format("{0} - {1}", sms.DisplayOriginatingAddress, sms.DisplayMessageBody), ToastLength.Short).Show();
            }

            var soundNotificationIntent = new Intent(Application.Context, typeof(SoundNotificationService));
            Application.Context.StartService(soundNotificationIntent);
        }
    }
}