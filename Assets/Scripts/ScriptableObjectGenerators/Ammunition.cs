using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace Com.sgagdr.BlackSky
{
    public interface IActionBehaviour
    {
        public void Action();
    }

    public interface IAttackBehaviour : IActionBehaviour
    {
        public void Attack();
    }

    public class BaseAttackBehaviour : IAttackBehaviour
    {
        public virtual void Action()
        {
            Attack();
        }
        public void Attack()
        {
            Debug.Log("Make attack from BaseAttackBehaviour");
        }
    }

    public class RayAttack : BaseAttackBehaviour
    {
        public int playerLayer = 10;
        public GameObject player;
        public float weaponBloom;
        public LayerMask canBeShot;

        Transform playerCamera;

        public override void Action()
        {
            //Get player's camera
            playerCamera = player.transform.Find("Cameras/Normal Camera");
            //Calculate a direction of a ray
            Vector3 t_direction = CalculateDirection();

            GameObject attackedGameObject;
            Attack(t_direction, out attackedGameObject);
        }

        public void Attack(Vector3 p_direction, out GameObject whoIsAttacked)
        {
            //Generate Ray
            RaycastHit t_hit = new RaycastHit();
            //Check if is hit
            bool isHit = Physics.Raycast(playerCamera.position, p_direction, out t_hit, 1000f, canBeShot);

            if (isHit)
            {
                whoIsAttacked = t_hit.collider.gameObject;
            }
            else
            {
                whoIsAttacked = new GameObject();
            }

        }

        Vector3 CalculateDirection()
        {
            Vector3 newDir = playerCamera.position + playerCamera.forward * 1000f;
            //� ��� ��� ��� �������, �� ������ �����, �� � ����(sad story)
            newDir += Random.Range(-weaponBloom, weaponBloom) * playerCamera.up;
            newDir += Random.Range(-weaponBloom, weaponBloom) * playerCamera.right;
            newDir -= playerCamera.position;
            newDir.Normalize();

            return newDir;
        }
        
    }

    public class MeleeAttack : BaseAttackBehaviour
    {
        public int playerLayer = 10;
        public GameObject player;
    }

    /*
    enum AttackBehaviours : byte
    {
        Ray,
        Bullet,
        Melee
    }*/

    [CreateAssetMenu(fileName = "New Ammunition", menuName = "Ammunition")]
    public class Ammunition : ScriptableObject
    {

        //public List<IAttackBehaviour> behavioursList = new List<IAttackBehaviour>() {new RayAttack(), };

        #region ����� ���������� ��� ������ ����������

        public string gunName; //��� �����     
        public int damage;//���� �����        
        public GameObject prefab; //������ �����

        public typeOfWeapon weaponType;

        public IAttackBehaviour attackBehaviour;
        #endregion



        //������� ��� ���������(���) ��������
        public void PerformPrimaryAction()
        {
            attackBehaviour.Action();
        }
    }
}
