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
    public class Gun : ScriptableObject
    {
        //Имя пукши
        public string gunName;
        //Модель пукши
        public GameObject prefab;
        //Урон пушки, БАХ БАХ ТРРРРРРРРРРР ПИУ ПИУ ПИУ БДЫДЫЩЩЩ!!!!!
        public int damage;
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

        public AudioClip shotClip;
        public float pitchRand;
        [Range(0f, 2f)]
        public float clipVolume; 

        private int stash; // Текущее кол-во патронов
        private int clip; // Текущее кол-во патронов в магазине 

        [Header("Visual Settings")]
        public VisualEffectAsset shotEffect;
        public GameObject trail;

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

        public int GetStash() {return stash; }
        public int GetClip() {return clip; }
    }

    #endregion

}
