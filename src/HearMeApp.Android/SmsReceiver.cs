using Android.App;
using Android.Content;
using Android.Provider;
using System;

namespace HearMeApp.Android
{
    [BroadcastReceiver]
    [IntentFilter(new string[] { "android.provider.Telephony.SMS_RECEIVED" })]
    public class SmsReceiver : BroadcastReceiver
    {
        public static readonly string SmsReceivedAction = "android.provider.Telephony.SMS_RECEIVED";

        public EventHandler<MessageReceivedEventArgs> MessageReceived;

        public override void OnReceive(Context context, Intent intent)
        {
            var smsArray = Telephony.Sms.Intents.GetMessagesFromIntent(intent);

            foreach (var sms in smsArray)
            {
                var message = new Message
                {
                    Sender = sms.DisplayOriginatingAddress,
                    Text = sms.DisplayMessageBody
                };

                MessageReceived?.Invoke(this, new MessageReceivedEventArgs { Message = message });
            }
        }
    }
}