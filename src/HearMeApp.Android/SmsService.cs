using Android.App;
using Android.App.Job;
using Android.Content;
using Android.Media;
using Android.OS;
using System;

namespace HearMeApp.Android
{
    [Service(Name = "HearMe.SmsService", Permission = "android.permission.BIND_JOB_SERVICE")]
    public class SmsService : JobService, MediaPlayer.IOnCompletionListener
    {
        SmsReceiver _smsReceiver;
        MediaPlayer _mediaPlayer;
        AudioManager _audioService;
        int _userVolume;

        public override bool OnStartJob(JobParameters @params)
        {
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

            return true;
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

        public override bool OnStopJob(JobParameters @params)
        {
            UnregisterReceiver(_smsReceiver);
            _mediaPlayer.Release();
            _smsReceiver.MessageReceived -= OnMessageReceived;

            return true;
        }
    }
}