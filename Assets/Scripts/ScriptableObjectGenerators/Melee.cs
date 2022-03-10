using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.sgagdr.BlackSky
{
    //Создаём возможность правой кнопкой мыши в папке Assets создавать прекол
    [CreateAssetMenu(fileName = "New Melee", menuName = "Melee")]
    #region Private Methods
    public class Melee : Ammunition
    {
        
        
        public float firerate = 0.5f;

        [Header("Sound Settings")]
        public AudioClip missSound;
        public AudioClip hitSound;
        public float pitch;
        [Range(0, 1f)]
        public float volume;




    }
    #endregion
}