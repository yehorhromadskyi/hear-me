using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Runtime;

namespace HearMeApp.Android
{
    [Service]
    public class SoundNotificationService : Service, MediaPlayer.IOnCompletionListener
    {
        MediaPlayer _mediaPlayer;
        AudioManager _audioService;
        int _userVolume;

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            _mediaPlayer = MediaPlayer.Create(this, Resource.Raw.imperial);
            _mediaPlayer.SetAudioAttributes(new AudioAttributes.Builder()
                                                               .SetUsage(AudioUsageKind.Media)
                                                               .SetContentType(AudioContentType.Music)
                                                               .Build());

            _mediaPlayer.SetOnCompletionListener(this);

            _audioService = (AudioManager)GetSystemService(AudioService);

            _userVolume = _audioService.GetStreamVolume(Stream.Music);

            _audioService.SetStreamVolume(Stream.Music, _audioService.GetStreamMaxVolume(Stream.Music), VolumeNotificationFlags.PlaySound);

            _mediaPlayer.Start();

            return StartCommandResult.Sticky;
        }

        public void OnCompletion(MediaPlayer mp)
        {
            _audioService.SetStreamVolume(Stream.Music, _userVolume, VolumeNotificationFlags.PlaySound);

            StopSelf();
        }

        public override void OnDestroy()
        {
            base.OnDestroy();

            _mediaPlayer.Release();
        }
    }
}