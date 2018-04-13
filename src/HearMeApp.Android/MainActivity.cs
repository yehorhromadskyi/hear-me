using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.App.Job;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Support.V7.Widget.Helper;
using Android.Widget;
using HearMeApp.Android.Adapters;
using HearMeApp.Android.Helpers;
using HockeyApp.Android;
using XamarinAndroid = Android;

namespace HearMeApp.Android
{
    [Activity(Label = "Hear me", MainLauncher = true)]
    public class MainActivity : Activity
    {
        ContactsAdapter _contactListAdapter;
        SimpleSwipeHelperCallback _swipeCallback;
        DatabaseProvider _databaseProvider = new DatabaseProvider();

        public RecyclerView ContactsRecyclerView { get; set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Main);

            ContactsRecyclerView = FindViewById<RecyclerView>(Resource.Id.recycler_contacts);

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

            // TODO:
            contactList.Add("John Doe");
            contactList.Add("Mark Doe");
            contactList.Add("Jane Doe");

            _contactListAdapter = new ContactsAdapter(
                contactList.Select(c => new Contact { Name = c }).ToList());
            ContactsRecyclerView.SetAdapter(_contactListAdapter);
            ContactsRecyclerView.SetLayoutManager(
                new LinearLayoutManager(this, LinearLayoutManager.Vertical, false));

            _swipeCallback = new SimpleSwipeHelperCallback();
            var swipeHelper = new ItemTouchHelper(_swipeCallback);
            swipeHelper.AttachToRecyclerView(ContactsRecyclerView);

            _swipeCallback.Swiped += OnContactSwiped;

            var intent = new Intent(this, typeof(SmsReceiver));
            var pending = PendingIntent.GetBroadcast(this, 0, intent, PendingIntentFlags.UpdateCurrent);
            var alarmManager = GetSystemService(AlarmService).JavaCast<AlarmManager>();
            alarmManager.Set(AlarmType.ElapsedRealtimeWakeup, SystemClock.ElapsedRealtime() + 5 * 1000, pending);
        }

        private void OnContactSwiped(object sender, int position)
        {
            _contactListAdapter.NotifyItemChanged(position);
        }

        private void OnItemClicked(object sender, AdapterView.ItemClickEventArgs e)
        {
            //var contact = _contactListAdapter.GetItem(e.Position);
            //_databaseProvider.Add(new Contact { Name = contact });
        }

        protected override void OnResume()
        {
            base.OnResume();

            CrashManager.Register(this, Secrets.HockeyAppId);
        }

        protected override void OnDestroy()
        {
            _swipeCallback.Swiped -= OnContactSwiped;

            base.OnDestroy();
        }
    }
}