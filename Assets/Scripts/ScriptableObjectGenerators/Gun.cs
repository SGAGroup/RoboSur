using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        //Темп огня
        public float firerate;
        //Скорость прицеливания
        public float aimSpeed;
    }

    #endregion

}