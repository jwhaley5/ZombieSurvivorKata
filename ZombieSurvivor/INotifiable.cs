using System;
using System.Collections.Generic;
using System.Text;

namespace ZombieSurvivor
{
    public interface INotifiable
    {
        void Notify(string eventDetail);
    }
}
