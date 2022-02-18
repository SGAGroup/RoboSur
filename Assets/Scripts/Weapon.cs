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
        public GameObject bulletholePrefab;
        public LayerMask canBeShot;

        public float bulletholeDuration = 5f;

        private int currentIndex;
        private GameObject currentWeapon;

        #endregion

        #region  MonoBehaviour Callbacks

        void Update()
        {
            if(Input.GetKeyDown(KeyCode.Alpha1)) Equip(0);

            if(currentWeapon !=null)
            {
            //Если нажата ЛКМ, то внутри функции Aim переменная p_isAiming будет равна истине
            Aim(Input.GetMouseButton(1));

            if(Input.GetMouseButtonDown(0))
            {
                Shoot();
            }
            }  
        }

        #endregion

        #region Private Methods

        //Когда хотим взять определённое оружие вызываем эту функцию. p_ind номер оружия которое хотим достать
        void Equip(int p_ind)
        {
            if(currentWeapon != null) Destroy(currentWeapon);

            currentIndex = p_ind;

            GameObject t_newWeapon = Instantiate (loadout[p_ind].prefab, weaponParent.position, weaponParent.rotation, weaponParent) as GameObject;
            //Создаём объёкт с номером p_ind на позиции weaponParent с тем же поворотом, и зачем то даём Transform WeaponParenta'a
            t_newWeapon.transform.localPosition = Vector3.zero;
            //Убеждаемся что новый объёкт находится в локальных нулях
            t_newWeapon.transform.localEulerAngles = Vector3.zero;
            //Тоже самое с поворотом

            currentWeapon = t_newWeapon;
        }

        void Aim(bool p_isAiming)
        {
            //Ищет положение определённого объекта, который принадлежит родительскому объекту (род. объект - curentWeapon)
            Transform t_anchor = currentWeapon.transform.Find("Anchor");
            //Ищем положение объекта Aiming (В префабе оружия есть такой)
            Transform t_state_aiming = currentWeapon.transform.Find("States/Aiming");
            //Ищем положение объекта Hip (В префабе оружия есть такой)
            Transform t_state_hip = currentWeapon.transform.Find("States/Hip");

            if(p_isAiming)
            {
                //Если да, то целимся
                t_anchor.position = Vector3.Lerp(t_anchor.position, t_state_aiming.position, Time.deltaTime * loadout[currentIndex].aimSpeed);
            }
            else
            {
                //Если нет, то от бедра
                t_anchor.position = Vector3.Lerp(t_anchor.position, t_state_hip.position, Time.deltaTime * loadout[currentIndex].aimSpeed);
            }
        }

        void Shoot ()
        {
            //Получаем положение нашей камеры
            Transform t_spawn = transform.Find("Cameras/Normal Camera");

            RaycastHit t_hit = new RaycastHit();
            //Пальнули невидимый лучом из камеры (t_spawn.position) в направлении синей стрелки (t_spawn.forward) записали положение попадания в t_hit
            if(Physics.Raycast(t_spawn.position, t_spawn.forward, out t_hit, 1000f, canBeShot))
            {
                GameObject t_newHole = Instantiate(bulletholePrefab, t_hit.point + t_hit.normal * 0.002f, Quaternion.identity) as GameObject;
                t_newHole.transform.LookAt(t_hit.point + t_hit.normal);
                Destroy(t_newHole, bulletholeDuration);
            }
        }

        #endregion
        
    }
}

