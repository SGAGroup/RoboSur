using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace Com.sgagdr.BlackSky
{

    //Здесь короче база для всех пушек будет

    //Создаём возможность правой кнопкой мыши в папке Assets создавать прекол
    [CreateAssetMenu(fileName = "New Gun", menuName = "Gun")]

    #region Private Methods
    
    //Сюда можно добавить что душе угодно
    public class Gun : Ammunition
    {
        public typeOfAmmo ammoType;
        
        //Разброс (хз почему блум блять)
        public int ammo;
        //Патроны
        public int clipsize;
        //Магазин
        public float reload;
        //Время на перезарядку
        public float bloom;
        //Отдача по вертикали
        public float recoil;
        //Отбрасывание пушки назад при выстреле
        public float kickback;
        //Темп огня
        public float firerate;
        //Скорость прицеливания
        public float aimSpeed;

        [Header("Audio Settings")]
        public AudioClip shotClip;
        public AudioClip reloadClip;
        public float pitchRand;
        [Range(0f, 1f)]
        public float clipVolume;


        [Header("Visual Settings")]
        public VisualEffectAsset shotEffect;
        public GameObject trail;
        
        
        private int stash; // Текущее кол-во патронов
        private int clip; // Текущее кол-во патронов в магазине 

        //Позиция в списке оружия
        public int posInLoadOut;

        public void Initialize()
        {
            stash = ammo;
            clip = clipsize;
        }

        public bool FireBullet()
        {
            if(clip > 0)
            {
                clip -= 1;
                return true;
            }
            else return false;
        }

        public void Reload()
        {
            stash += clip;
            clip = Mathf.Min(clipsize, stash);
            stash -= clip;
        }

        //Returns a count of ammo in stash
        public int GetStash() {return stash; }
        //Returns a count of ammo in current clip
        public int GetClip() {return clip; }

        public void AddStash(int val) { stash += val; }
    }

    #endregion

}
