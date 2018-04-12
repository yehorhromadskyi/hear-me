using Android.Support.V7.Widget;
using Android.Views;
using HearMeApp.Android.ViewHolders;
using System;
using System.Collections.Generic;

namespace HearMeApp.Android.Adapters
{
    public class ContactsAdapter : RecyclerView.Adapter
    {
        readonly List<Contact> _contacts;

        public override int ItemCount => _contacts.Count;

        public ContactsAdapter(List<Contact> contacts)
        {
            _contacts = contacts;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var viewHolder = holder as ContactViewHolder;
            var contact = _contacts[position];

            viewHolder.NameTextView.Text = contact.Name;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var id = Resource.Layout.ContactView;
            var itemView = LayoutInflater.From(parent.Context).Inflate(id, parent, false);

            return new ContactViewHolder(itemView);
        }
    }
}