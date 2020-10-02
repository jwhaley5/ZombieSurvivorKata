using System;
using System.Collections.Generic;
using System.Text;

namespace ZombieSurvivor
{
    public class GameNotifier
    {
        public INotifiable ToNotify { get; }

        public GameNotifier(INotifiable toNotify)
        {
            ToNotify = toNotify;
        }

        public void Notify(string eventDetail)
        {
            ToNotify.Notify(eventDetail);
        }
    }
}
