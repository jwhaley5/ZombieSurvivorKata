using System;
using System.Collections.Generic;
using System.Text;

namespace ZombieSurvivor
{
    public class GameEvent
    {
        public DateTime EventDateTime { get; }
        public string EventDetail { get; set; }

        public GameEvent()
        {
            EventDateTime = DateTime.Now;
        }
    }
}
