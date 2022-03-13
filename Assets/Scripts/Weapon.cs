using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.Experimental.VFX;
using Photon.Pun;

namespace Com.sgagdr.BlackSky
{
    public enum typeOfAmmo : int
    {
        none = -1,
        revolver = 0,
        rpk = 1
    }
    public enum typeOfWeapon : int
    {
        Gun = 0,
        Melee = 1
    };



    public class Weapon : MonoBehaviourPun
    {

        #region Variables
        private Player player;


        public Gun[] loadout;
        private Gun currentGunData;
        //Из-за [] можно хранить несколько предметов в одном gameobject'e
        //Как массив, но не массив, ебать!
        public Transform weaponParent;
        //Получаем позицию родительского объекта в иерархии
        public GameObject bulletholePrefab;
        public LayerMask canBeShot;

        public float bulletholeDuration = 5f;

        private float currentCooldown;
        private int currentIndex;
        private GameObject currentWeapon;

        private bool isReloading;

        public AudioSource sfx;

        private int equipID = 0;
        private int lastCountOfPlayers = 0;


        private bool isAbleToReload = false;

        #endregion

        #region  MonoBehaviour Callbacks

        private void Start()
        {
            player = gameObject.GetComponent<Player>();
            lastCountOfPlayers = PhotonNetwork.CountOfPlayers;

            for (int i = 0; i < loadout.Length; i++)
            {
                Gun curGun = loadout[i];
                //if (curGun.weaponType == typeOfWeapon.Gun) 
                    curGun.Initialize();
            }
            Equip(0);
        }

        void Update()
        {
            if (!photonView.IsMine) return;

            if (Input.GetKeyDown(KeyCode.P))
            {
                photonView.RPC("TakeDamage", RpcTarget.All, 20);
            }

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                equipID = 0;
                photonView.RPC("Equip", RpcTarget.All, equipID);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                equipID = 1;
                photonView.RPC("Equip", RpcTarget.All, equipID);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                equipID = 2;
                photonView.RPC("Equip", RpcTarget.All, equipID);
            }

            if (currentWeapon != null)
            {
                isAbleToReload = (currentGunData.GetStash() > 0 && currentGunData.weaponType == typeOfWeapon.Gun);

                //Если нажата ЛКМ, то внутри функции Aim переменная p_isAiming будет равна истине
                Aim(Input.GetMouseButton(1));


                if (Input.GetMouseButton(0) && currentCooldown <= 0 && !isReloading)
                {
                    if (loadout[currentIndex].weaponType == typeOfWeapon.Gun)
                    {

                        if (loadout[currentIndex].FireBullet()) photonView.RPC("Shoot", RpcTarget.All);
                        else if (isAbleToReload)
                        {
                            StartCoroutine(Reload(loadout[currentIndex].reload));
                        }
                    }
                    else
                    {
                        photonView.RPC("Shoot", RpcTarget.All);
                    }
                }


                if (Input.GetKeyDown(KeyCode.R) && isAbleToReload)
                {
                    currentCooldown = currentGunData.reload - currentGunData.firerate;
                    StartCoroutine(Reload(loadout[currentIndex].reload));
                }

                //weapon position elasticity (Ну чтобы тут хранилась позиция пукши, к которой мы можем вернуться, после отдачи например)
                currentWeapon.transform.localPosition = Vector3.Lerp(currentWeapon.transform.localPosition, Vector3.zero, Time.deltaTime * 4f);

                //cooldown
                if (currentCooldown > 0) currentCooldown -= Time.deltaTime;
            }

            if (lastCountOfPlayers != PhotonNetwork.CountOfPlayers)
            {
                photonView.RPC("Equip", RpcTarget.All, equipID);
                lastCountOfPlayers = PhotonNetwork.CountOfPlayers;
            }
        }

        #endregion

        #region Photon Callbacks



        #endregion




        #region Private Methods

        //Когда хотим взять определённое оружие вызываем эту функцию. p_ind номер оружия которое хотим достать

        IEnumerator Reload(float p_wait)
        {
            //Пошла перезарядка
            isReloading = true;
            currentWeapon.SetActive(false);

            //SFX
            sfx.Stop();
            sfx.clip = currentGunData.reloadClip;
            sfx.Play();


            //Ждём её окончания p_wait секунд (yield, кстати корутина)
            yield return new WaitForSeconds(p_wait);

            //Заканчиваем перезарядку
            loadout[currentIndex].Reload();
            currentWeapon.SetActive(true);
            isReloading = false;
        }

        [PunRPC]
        void Equip(int p_ind)
        {
            if (currentWeapon != null)
            {
                if (isReloading) StopCoroutine("Reload");
                Destroy(currentWeapon);
            }

            currentIndex = p_ind;
            currentGunData = loadout[p_ind];

            GameObject t_newWeapon = Instantiate(currentGunData.prefab, weaponParent.position, weaponParent.rotation, weaponParent) as GameObject;
            //Создаём объёкт с номером p_ind на позиции weaponParent с тем же поворотом, и зачем то даём Transform WeaponParenta'a
            t_newWeapon.transform.localPosition = Vector3.zero;
            //Убеждаемся что новый объёкт находится в локальных нулях
            t_newWeapon.transform.localEulerAngles = Vector3.zero;
            //Тоже самое с поворотом
            t_newWeapon.GetComponent<Sway>().isMine = photonView.IsMine;

            currentWeapon = t_newWeapon;
            currentWeapon.GetComponentInChildren<VisualEffect>().visualEffectAsset = currentGunData.shotEffect;
        }

        void Aim(bool p_isAiming)
        {
            player.isAim = p_isAiming;
            Debug.Log("Player is aiming:" + player.isAim);

            //Ищет положение определённого объекта, который принадлежит родительскому объекту (род. объект - curentWeapon)
            Transform t_anchor = currentWeapon.transform.Find("Anchor");
            //Ищем положение объекта Aiming (В префабе оружия есть такой)
            Transform t_state_aiming = currentWeapon.transform.Find("States/Aiming");
            //Ищем положение объекта Hip (В префабе оружия есть такой)
            Transform t_state_hip = currentWeapon.transform.Find("States/Hip");

            if (p_isAiming)
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

        [PunRPC]
        void Shoot()
        {


            //Получаем положение нашей камеры
            Transform t_spawn = transform.Find("Cameras/Normal Camera");

            //Если пушка огнестрельная - то там всякая фигня с лучами и т.д.
            if (currentGunData.weaponType == typeOfWeapon.Gun)
            {

                //Разброс (хз почему этот уёбок говорит, что bloom - это разброс)            
                //Создали точку, которая находится на расстоянии 1000 юнитивских единиц от игрока
                Vector3 t_bloom = t_spawn.position + t_spawn.forward * 1000f;




                //А вот тут чет сложное, не совсем понял, че к чему(sad story)
                t_bloom += Random.Range(-loadout[currentIndex].bloom, loadout[currentIndex].bloom) * t_spawn.up;
                t_bloom += Random.Range(-loadout[currentIndex].bloom, loadout[currentIndex].bloom) * t_spawn.right;
                t_bloom -= t_spawn.position;
                t_bloom.Normalize();
                //raycast
                RaycastHit t_hit = new RaycastHit();
                //Пальнули невидимый лучом из камеры (t_spawn.position) в направлении синей стрелки (t_spawn.forward) записали положение попадания в t_hit
                if (Physics.Raycast(t_spawn.position, t_bloom, out t_hit, 1000f, canBeShot))
                {
                    GameObject t_newHole = Instantiate(bulletholePrefab, t_hit.point + t_hit.normal * 0.002f, Quaternion.identity) as GameObject;
                    t_newHole.transform.LookAt(t_hit.point + t_hit.normal);
                    Destroy(t_newHole, bulletholeDuration);

                    
                    Transform t_barell = currentWeapon.GetComponentInChildren<VisualEffect>().gameObject.transform;
                    if (!t_barell) t_barell = currentWeapon.transform;
                    GameObject t_newTrail = Instantiate(currentGunData.trail, t_barell.position, Quaternion.identity);
                    t_newTrail.GetComponent<TrailScript>().finalPoint = t_hit.point + t_hit.normal * 0.002f;

                    //Штото с тем, что мы это мы
                    if (photonView.IsMine)
                    {
                        //Что-то с тем, что мы попали в игрока
                        if (t_hit.collider.gameObject.layer == 10)
                        {
                            //А тут должен быть эффект от урона
                            //И теперь тут есть урон
                            //Наносим урон равный урону текущего оружия, сообщаем всем
                            t_hit.collider.gameObject.GetPhotonView().RPC("TakeDamage", RpcTarget.All, loadout[currentIndex].damage, PhotonNetwork.LocalPlayer.ActorNumber);
                        }
                        //Отдача по вертикали
                        currentWeapon.transform.Rotate(-loadout[currentIndex].recoil, 0, 0);
                        //Отбрасыванию пушки назад при выстреле (Ну, отдача по горизонтали, получается)
                        currentWeapon.transform.position -= currentWeapon.transform.forward * loadout[currentIndex].kickback;
                    }
                }
            }

            //cooldown
            currentCooldown = loadout[currentIndex].firerate;

            //Gun SFX
            PlaySFX(currentGunData);

            //Gun VFX
            PlayVFX(currentGunData, currentWeapon);

            //gun fx


        }

        [PunRPC]
        private void TakeDamage(int p_damage)
        {
            //TODO:
            Player pl = GetComponent<Player>();
            pl.TakeDamage(p_damage, pl.manager.myind);
        }

        private void PlaySFX(Gun curGunDat)
        {
            sfx.Stop();
            sfx.volume = curGunDat.clipVolume;
            sfx.clip = curGunDat.shotClip;
            sfx.pitch = 1 - curGunDat.pitchRand + Random.Range(-curGunDat.pitchRand, curGunDat.pitchRand);
            sfx.Play();
        }

        private void PlayVFX(Gun curGunDat, GameObject curWeap)
        {
            VisualEffect vfx = currentWeapon.GetComponentInChildren<VisualEffect>();
            Animation anim = currentWeapon.GetComponentInChildren<Animation>();

            if (vfx) vfx.Play();
            if (anim) anim.Play();

        }

        #endregion

        #region Public Methods

        public void RefreshAmmo(Text p_text)
        {
            int t_clipsize = loadout[currentIndex].clipsize;
            int t_clip = loadout[currentIndex].GetClip();
            int t_stash = loadout[currentIndex].GetStash();

            p_text.text = t_clip.ToString("D2") + " / " + t_stash.ToString("D2");
            if (1f * t_clip / t_clipsize <= 0.25f) p_text.color = Color.red;
            else p_text.color = Color.white;
        }

        #endregion
    }
}

