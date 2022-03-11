using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.sgagdr.BlackSky
{

    public class AmmoBox : PickableItem
    {
        //Amount of ammo to add
        public int amount = 30;
        //Enum type of ammo
        public typeOfAmmo type;

        public override void Action(GameObject other)
        {
            base.Action(other);
            Gun[] arr = other.gameObject.GetComponent<Weapon>().loadout;
            foreach (Gun gun in arr)
            {
                if (gun.ammoType == type)
                {
                    gun.AddStash(amount);
                    sfx.Play();
                    StartCoroutine(DestroyObj());
                }
            }
        }

    }
}
