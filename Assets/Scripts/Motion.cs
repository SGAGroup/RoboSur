using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Com.sgagdr.BlackSky
{
    public class Motion : MonoBehaviourPun
    {
        
        #region  Variables

        public float speed;
        public float sprintModifier;
        public float jumpForce;
        public Camera normalCam;
        public GameObject cameraParent;
        public Transform groundDetector;
        public LayerMask ground;

        private Rigidbody rig;

        private float baseFOV;
        private float sprintFOVModifier = 1.4f;


        //Покачивание головы
        public Transform weaponParent; //Положение слота для оружия, используется для покачивания камеры
        private float idleCount = 0f; //Счетчик покачивания камеры для состояния стоя
        private float movementCount = 0f; //Счетчик покачивания камеры для движения
        private Vector3 weaponParentOrigin; //Изначальное положение слота для оружия
        private Vector3 targetHeadBobPos; //Положение, которое он должен занять
        #endregion

        #region  Monobehavior Callbacks

        private void Start()
        {
            if (photonView.IsMine) cameraParent.SetActive(true);
            baseFOV = normalCam.fieldOfView;
            //Включить если в сцене больше чем одна камера
            //if(Camera.main) Camera.main.enabled = false;
            rig = GetComponent<Rigidbody>();
            weaponParentOrigin = weaponParent.localPosition;
        }

        private void Update()
        {
            if (!photonView.IsMine) return;
            //"Очень крутая теория почему мы проверяем кнопки здесь, а не в FixedUpdate"

            //Впитываем оси
            float t_hmove = Input.GetAxisRaw("Horizontal");
            float t_vmove = Input.GetAxisRaw("Vertical");

            //Впитываем кнопки
            bool sprint = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
            bool jump = Input.GetKeyDown(KeyCode.Space);

            //Положения (States)
            bool isGrounded = Physics.Raycast(groundDetector.position, Vector3.down, 0.1f, ground);
            bool isJumping = jump && isGrounded;
            bool isSprinting = sprint && t_vmove > 0 && !isJumping && isGrounded;

            //Прыгаем
            if(isJumping)
            {
                rig.AddForce(Vector3.up * jumpForce);
            }


            //Покачивание головы
            if(t_hmove == 0 && t_vmove == 0)
            {
                HeadBob(idleCount, 0.025f, 0.025f);
                idleCount += Time.deltaTime;
                weaponParent.localPosition = Vector3.Lerp(weaponParent.localPosition, targetHeadBobPos, Time.deltaTime * 2f);
            }
            else if (!isSprinting)
            {
                HeadBob(movementCount, 0.035f, 0.035f);
                movementCount += Time.deltaTime*3f;
                weaponParent.localPosition = Vector3.Lerp(weaponParent.localPosition, targetHeadBobPos, Time.deltaTime * 6f);
            }
            else
            {
                HeadBob(movementCount, 0.15f, 0.07f);
                movementCount += Time.deltaTime*7f;
                weaponParent.localPosition = Vector3.Lerp(weaponParent.localPosition, targetHeadBobPos, Time.deltaTime * 10f);
            }
            
        }

        void FixedUpdate()
        {
            if (!photonView.IsMine) return;
            //Впитываем оси
            float t_hmove = Input.GetAxisRaw("Horizontal");
            float t_vmove = Input.GetAxisRaw("Vertical");

            //Впитываем кнопки
            bool sprint = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
            bool jump = Input.GetKeyDown(KeyCode.Space);

            //Положения (States)
            bool isGrounded = Physics.Raycast(groundDetector.position, Vector3.down, 0.1f, ground);
            bool isJumping = jump && isGrounded;
            bool isSprinting = sprint && t_vmove > 0 && !isJumping && isGrounded;

            //Движение
            Vector3 t_direction = new Vector3(t_hmove, 0, t_vmove);
            t_direction.Normalize();

            float t_adjustedSpeed = speed;
            if(isSprinting) t_adjustedSpeed *= sprintModifier;

            Vector3 t_targetVelocity = transform.TransformDirection(t_direction) * t_adjustedSpeed * Time.deltaTime;
            t_targetVelocity.y = rig.velocity.y;
            rig.velocity = t_targetVelocity;


            //FOV при беге и ходьбе
            if(isSprinting) { normalCam.fieldOfView = Mathf.Lerp(normalCam.fieldOfView, baseFOV * sprintFOVModifier, Time.deltaTime * 8f); }
            else { normalCam.fieldOfView = Mathf.Lerp(normalCam.fieldOfView, baseFOV, Time.deltaTime * 8f); }
        }

        #endregion

        #region Private Methods

        //Покачивание головы
        void HeadBob(float p_z, float p_xIntensity, float p_yIntensity)
        {
            targetHeadBobPos = weaponParentOrigin + new Vector3(Mathf.Cos(p_z) * p_xIntensity, Mathf.Sin(p_z*2) * p_yIntensity, 0);
        }

        #endregion
    }
}