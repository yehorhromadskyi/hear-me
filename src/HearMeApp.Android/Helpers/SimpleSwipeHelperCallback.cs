using Android.Support.V7.Widget;
using Android.Support.V7.Widget.Helper;
using System;

namespace HearMeApp.Android.Helpers
{
    public class SimpleSwipeHelperCallback : ItemTouchHelper.SimpleCallback
    {
        public event EventHandler<int> Swiped;

        public SimpleSwipeHelperCallback()
            : base(0, ItemTouchHelper.Right)
        {
        }

        public override bool OnMove(RecyclerView recyclerView, RecyclerView.ViewHolder viewHolder, RecyclerView.ViewHolder target)
        {
            return false;
        }

        public override void OnSwiped(RecyclerView.ViewHolder viewHolder, int direction)
        {
            Swiped?.Invoke(this, viewHolder.AdapterPosition);
        }
    }
}