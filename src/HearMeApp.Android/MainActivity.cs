﻿using System;
using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Widget;
using HockeyApp.Android;

namespace HearMeApp.Android
{
    [Activity(Label = "Hear me", MainLauncher = true)]
    public class MainActivity : Activity, MediaPlayer.IOnCompletionListener
    {
        SmsReceiver _smsReceiver;
        MediaPlayer _mediaPlayer;
        AudioManager _audioService;
        int _userVolume;

        public Button PlayButton { get; set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Main);

            PlayButton = FindViewById<Button>(Resource.Id.btn_play);

            PlayButton.Click += OnPlayButtonClick;

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

        protected override void OnResume()
        {
            base.OnResume();

            CrashManager.Register(this, Secrets.HockeyAppId);
        }

        private void OnPlayButtonClick(object sender, System.EventArgs e)
        {
            Play();
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

            _mediaPlayer.Release();
            PlayButton.Click -= OnPlayButtonClick;
            _smsReceiver.Pinged -= OnPinged;
        }
    }
}