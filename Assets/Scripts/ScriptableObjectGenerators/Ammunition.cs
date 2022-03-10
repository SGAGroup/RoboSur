using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace Com.sgagdr.BlackSky
{
    [CreateAssetMenu(fileName = "New Ammunition", menuName = "Ammunition")]
    public class Ammunition : ScriptableObject
    {
        //��� �����
        public string gunName;
        //���� �����
        public int damage;
        //������ �����
        public GameObject prefab;

        public typeOfWeapon weaponType;
    }
}
