using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Com.sgagdr.BlackSky
{
    public class HealthBox : PickableItem
    {
        //Amount of hp to add
        public int health = 50;
        override public void Action(GameObject other)
        {
            other.GetComponent<Player>().AddHealth(health);
          
            base.Action(other);
        }
    }
}
