using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.sgagdr.BlackSky
{
    public class Motion : MonoBehaviour
    {
        
        #region  Variables

        public float speed;
        public float sprintModifier;
        public float jumpForce;
        public Camera normalCam;
        public Transform groundDetector;
        public LayerMask ground;

        private Rigidbody rig;

        private float baseFOV;
        private float sprintFOVModifier = 1.4f;

        #endregion

        #region  Monobehavior Callbacks

        private void Start()
        {
            baseFOV = normalCam.fieldOfView;
            //Включить если в сцене больше чем одна камера
            //Camera.main.enabled = false;
            rig = GetComponent<Rigidbody>();
        }

        private void Update()
        {
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

        }

        void FixedUpdate()
        {
            
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

    }
}