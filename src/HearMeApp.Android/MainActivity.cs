using System;
using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
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

            var serviceIntent = new Intent(this, typeof(SmsService));
            StartService(serviceIntent);

            SetContentView(Resource.Layout.Main);

            PlayButton = FindViewById<Button>(Resource.Id.btn_play);

            PlayButton.Click += OnPlayButtonClick;
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