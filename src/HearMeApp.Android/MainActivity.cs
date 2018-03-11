using System;
using Android.App;
using Android.App.Job;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using HockeyApp.Android;

namespace HearMeApp.Android
{
    [Activity(Label = "Hear me", MainLauncher = true)]
    public class MainActivity : Activity
    {
        public Button PlayButton { get; set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Main);

            PlayButton = FindViewById<Button>(Resource.Id.btn_play);

            PlayButton.Click += OnPlayButtonClick;

            var intent = new Intent(this, typeof(SmsReceiver));
            var pending = PendingIntent.GetBroadcast(this, 0, intent, PendingIntentFlags.UpdateCurrent);
            var alarmManager = GetSystemService(AlarmService).JavaCast<AlarmManager>();
            alarmManager.Set(AlarmType.ElapsedRealtimeWakeup, SystemClock.ElapsedRealtime() + 5 * 1000, pending);
        }

        protected override void OnResume()
        {
            base.OnResume();

            CrashManager.Register(this, Secrets.HockeyAppId);
        }

        private void OnPlayButtonClick(object sender, System.EventArgs e)
        {
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            PlayButton.Click -= OnPlayButtonClick;
        }
    }
}