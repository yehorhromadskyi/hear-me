using Android.App;
using Android.Widget;
using Android.OS;
using Android.Media;

namespace HearMeApp.Android
{
    [Activity(Label = "Hear me", MainLauncher = true)]
    public class MainActivity : Activity
    {
        MediaPlayer _mediaPlayer;

        public Button PlayButton { get; set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Main);

            PlayButton = FindViewById<Button>(Resource.Id.btn_play);

            PlayButton.Click += OnPlayButtonClick;

            _mediaPlayer = MediaPlayer.Create(this, Resource.Raw.sax);
        }

        private void OnPlayButtonClick(object sender, System.EventArgs e)
        {
            _mediaPlayer.Start();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            _mediaPlayer.Release();
            PlayButton.Click -= OnPlayButtonClick;
        }
    }
}