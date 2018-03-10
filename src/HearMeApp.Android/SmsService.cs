using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using System;

namespace HearMeApp.Android
{
    [Service]
    [IntentFilter(new String[] { "com.hearme.SmsService" })]
    public class SmsService : Service, MediaPlayer.IOnCompletionListener
    {
        SmsReceiver _smsReceiver;
        MediaPlayer _mediaPlayer;
        AudioManager _audioService;
        int _userVolume;

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        public override void OnCreate()
        {
            base.OnCreate();

            _mediaPlayer = MediaPlayer.Create(this, Resource.Raw.sax);
            _mediaPlayer.SetAudioAttributes(new AudioAttributes.Builder()
                                                               .SetUsage(AudioUsageKind.Media)
                                                               .SetContentType(AudioContentType.Music)
                                                               .Build());

            _mediaPlayer.SetOnCompletionListener(this);

            _audioService = (AudioManager)GetSystemService(AudioService);

            _userVolume = _audioService.GetStreamVolume(Stream.Music);

            _smsReceiver = new SmsReceiver();
            _smsReceiver.MessageReceived += OnMessageReceived;

            var filter = new IntentFilter();
            filter.AddAction(SmsReceiver.SmsReceivedAction);

            RegisterReceiver(_smsReceiver, filter);
        }

        private void OnMessageReceived(object sender, MessageReceivedEventArgs args)
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

        public override void OnDestroy()
        {
            base.OnDestroy();

            UnregisterReceiver(_smsReceiver);
            _mediaPlayer.Release();
            _smsReceiver.MessageReceived -= OnMessageReceived;
        }
    }
}