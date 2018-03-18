using System;
using System.Collections.Generic;
using Android.App;
using Android.App.Job;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Widget;
using HockeyApp.Android;
using XamarinAndroid = Android;

namespace HearMeApp.Android
{
    [Activity(Label = "Hear me", MainLauncher = true)]
    public class MainActivity : Activity
    {
        ArrayAdapter<string> _contactListAdapter;
        DatabaseProvider _databaseProvider = new DatabaseProvider();

        public ListView ContactsListView { get; set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Main);

            ContactsListView = FindViewById<ListView>(Resource.Id.listview_contacts);

            var uri = ContactsContract.Contacts.ContentUri;

            string[] projection = 
            {
                ContactsContract.Contacts.InterfaceConsts.Id,
                ContactsContract.Contacts.InterfaceConsts.DisplayName
            };

            var cursor = ContentResolver.Query(uri, projection, null, null, null);

            var contactList = new List<string>();

            if (cursor.MoveToFirst())
            {
                do
                {
                    contactList.Add(cursor.GetString(cursor.GetColumnIndex(projection[1])));
                }
                while (cursor.MoveToNext());
            }

            _contactListAdapter = new ArrayAdapter<string>(this, XamarinAndroid.Resource.Layout.SimpleListItem1, contactList);
            ContactsListView.Adapter = _contactListAdapter;

            ContactsListView.ItemClick += OnItemClicked;

            var intent = new Intent(this, typeof(SmsReceiver));
            var pending = PendingIntent.GetBroadcast(this, 0, intent, PendingIntentFlags.UpdateCurrent);
            var alarmManager = GetSystemService(AlarmService).JavaCast<AlarmManager>();
            alarmManager.Set(AlarmType.ElapsedRealtimeWakeup, SystemClock.ElapsedRealtime() + 5 * 1000, pending);
        }

        private void OnItemClicked(object sender, AdapterView.ItemClickEventArgs e)
        {
            var contact = _contactListAdapter.GetItem(e.Position);
            _databaseProvider.Add(new Contact { Name = contact });
        }

        protected override void OnResume()
        {
            base.OnResume();

            CrashManager.Register(this, Secrets.HockeyAppId);
        }

        protected override void OnDestroy()
        {
            ContactsListView.ItemClick -= OnItemClicked;

            base.OnDestroy();
        }
    }
}