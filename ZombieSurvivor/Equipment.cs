using System;

namespace ZombieSurvivor
{
    public class Equipment
    {
        public string Name { get; }
        public bool InHand { get; set; }

        internal Equipment(string name)
        {
            Name = name;
        }
    }
}
