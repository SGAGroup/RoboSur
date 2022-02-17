using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.sgagdr.BlackSky
{
    public class Look : MonoBehaviour
    {
        #region Variables
        public static bool cursorLocked = true;

        public Transform player;
        public Transform cams;
        public Transform weapon;

        public float xSensitivity;
        public float ySensitivity;
        public float maxAngle;

        private Quaternion camCenter;
        #endregion

        #region  Monobehavior Callbacks
        //Регион с функциями, встроенными в Юнити

                void Start()
        {
            camCenter = cams.localRotation;
        }

        void Update()
        {
            SetY();
            SetX();

            UpdateCursorLock();
        }

        #endregion

        #region Private Methods
        void SetY () 
        {
            float t_input = Input.GetAxis("Mouse Y") * ySensitivity * Time.deltaTime;
            Quaternion t_adjustment = Quaternion.AngleAxis(t_input, -Vector3.right);
            Quaternion t_delta = cams.localRotation * t_adjustment;

            if (Quaternion.Angle(camCenter, t_delta) < maxAngle)
            {
            cams.localRotation = t_delta;
            }

            weapon.rotation = cams.rotation;
        }

        void SetX () 
        {
            float t_input = Input.GetAxis("Mouse X") * xSensitivity * Time.deltaTime;
            Quaternion t_adjustment = Quaternion.AngleAxis(t_input, Vector3.up);
            Quaternion t_delta = player.localRotation * t_adjustment;
            player.localRotation = t_delta;
        }

        void UpdateCursorLock()
        {
            if(cursorLocked)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

                if(Input.GetKeyDown(KeyCode.Escape))
                {
                    cursorLocked = false;
                }
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                if(Input.GetKeyDown(KeyCode.Escape))
                {
                    cursorLocked = true;
                }
            }
        }
        #endregion
    }
}