using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Effects : MonoBehaviour
{

    #region Variables
    public VisualEffect shotExplosion;
    public AudioSource shotSound;


    public bool isSoundPlaying = false;
    #endregion

    #region Public Methods
    public void PlayEffects()
    {
        if (shotExplosion)
        {
            shotExplosion.Play();
        }
        if (shotSound && !isSoundPlaying)
        {
            shotSound.Play();
            isSoundPlaying = true;
        }
    }

    public void StopSound()
    {
        if (shotSound)
        {
            shotSound.Stop();
            isSoundPlaying = false;
        }
    }


    #endregion 

}
