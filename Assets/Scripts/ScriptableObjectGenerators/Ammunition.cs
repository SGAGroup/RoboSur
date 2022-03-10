using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace Com.sgagdr.BlackSky
{
    [CreateAssetMenu(fileName = "New Ammunition", menuName = "Ammunition")]
    public class Ammunition : ScriptableObject
    {
        //Имя пушки
        public string gunName;
        //Урон пушки
        public int damage;
        //Модель пушки
        public GameObject prefab;

        public typeOfWeapon weaponType;
    }
}
