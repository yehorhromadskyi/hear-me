using Android.Content;
using Android.Provider;
using System;

namespace HearMeApp.Android
{
    [BroadcastReceiver(Label = "SMS Receiver")]
    public class SmsReceiver : BroadcastReceiver
    {
        public static readonly string SmsReceivedAction = "android.provider.Telephony.SMS_RECEIVED";

        public EventHandler Pinged;

        public override void OnReceive(Context context, Intent intent)
        {
            if (intent.Action == SmsReceivedAction)
            {
                var messages = Telephony.Sms.Intents.GetMessagesFromIntent(intent);

                foreach (var message in messages)
                {
                    if (message.DisplayOriginatingAddress.Contains("000 00 00")
                        && message.DisplayMessageBody.Length == 1
                        && message.DisplayMessageBody.Contains("."))
                    {
                        Pinged?.Invoke(this, EventArgs.Empty);
                    }
                }
            }
        }
    }
}