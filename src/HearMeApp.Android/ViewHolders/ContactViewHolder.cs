using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

namespace HearMeApp.Android.ViewHolders
{
    public class ContactViewHolder : RecyclerView.ViewHolder
    {
        public TextView NameTextView { get; set; }

        public ContactViewHolder(View itemView) : base(itemView)
        {
            NameTextView = itemView.FindViewById<TextView>(Resource.Id.text_contact_name);
        }
    }
}