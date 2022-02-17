using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.sgagdr.BlackSky
{
    public class Weapon : MonoBehaviour
    {

        #region Variables

        public Gun[] loadout;
        //Из-за [] можно хранить несколько предметов в одном gameobject'e
        //Как массив, но не массив, ебать!
        public Transform weaponParent;
        //Получаем позицию родительского объекта в иерархии

        private GameObject currentWeapon;

        #endregion

        #region  MonoBehaviour Callbacks

        void Update()
        {
            if(Input.GetKeyDown(KeyCode.Alpha1)) Equip(0);        
        }

        #endregion

        #region Private Methods

        //Когда хотим взять определённое оружие вызываем эту функцию. p_ind номер оружия которое хотим достать
        void Equip(int p_ind)
        {
            if(currentWeapon != null) Destroy(currentWeapon);
            GameObject t_newWeapon = Instantiate (loadout[p_ind].prefab, weaponParent.position, weaponParent.rotation, weaponParent) as GameObject;
            //Создаём объёкт с номером p_ind на позиции weaponParent с тем же поворотом, и зачем то даём Transform WeaponParenta'a
            t_newWeapon.transform.localPosition = Vector3.zero;
            //Убеждаемся что новый объёкт находится в локальных нулях
            t_newWeapon.transform.localEulerAngles = Vector3.zero;
            //Тоже самое с поворотом

            currentWeapon = t_newWeapon;
        }

        #endregion
        
    }
}

