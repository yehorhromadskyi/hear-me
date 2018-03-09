using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using System;

namespace HearMeApp.Android
{
    [Activity(Label = "AudioActivity")]
    public class AudioActivity : Activity, MediaPlayer.IOnCompletionListener
    {
        SmsReceiver _smsReceiver;
        MediaPlayer _mediaPlayer;
        AudioManager _audioService;
        int _userVolume;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            _mediaPlayer = MediaPlayer.Create(this, Resource.Raw.sax);
            _mediaPlayer.SetAudioAttributes(new AudioAttributes.Builder()
                                                               .SetUsage(AudioUsageKind.Media)
                                                               .SetContentType(AudioContentType.Music)
                                                               .Build());

            _mediaPlayer.SetOnCompletionListener(this);

            _audioService = (AudioManager)GetSystemService(AudioService);

            _userVolume = _audioService.GetStreamVolume(Stream.Music);

            _smsReceiver = new SmsReceiver();
            _smsReceiver.Pinged += OnPinged;

            var filter = new IntentFilter();
            filter.AddAction(SmsReceiver.SmsReceivedAction);

            RegisterReceiver(_smsReceiver, filter);
        }

        private void OnPinged(object sender, EventArgs e)
        {
            Play();
        }

        private void Play()
        {
            _audioService.SetStreamVolume(Stream.Music, _audioService.GetStreamMaxVolume(Stream.Music), VolumeNotificationFlags.PlaySound);

            _mediaPlayer.Start();
        }

        public void OnCompletion(MediaPlayer mp)
        {
            _audioService.SetStreamVolume(Stream.Music, _userVolume, VolumeNotificationFlags.PlaySound);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            UnregisterReceiver(_smsReceiver);
            _mediaPlayer.Release();
            _smsReceiver.Pinged -= OnPinged;
        }
    }
}