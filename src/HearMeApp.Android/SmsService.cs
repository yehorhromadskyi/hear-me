using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using System;

namespace HearMeApp.Android
{
    [Service]
    [IntentFilter(new String[] { "com.hearme.SmsService" })]
    public class SmsService : Service
    {
        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            var audioIntent = new Intent(this, typeof(AudioActivity));
            audioIntent.SetFlags(ActivityFlags.NewTask);
            StartActivity(audioIntent);

            return base.OnStartCommand(intent, flags, startId);
        }
    }
}