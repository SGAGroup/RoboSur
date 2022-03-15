using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class Pause : MonoBehaviour
{
    public static bool isPause = false;
    private bool isDisconnecting = false;

    public void TogglePause()
    {
        if (isDisconnecting) return;

        isPause = !isPause;

        transform.GetChild(0).gameObject.SetActive(isPause);
        Cursor.lockState = (isPause) ? CursorLockMode.None : CursorLockMode.Confined;
        Cursor.visible = isPause;
    }

    public void Quit()
    {
        isDisconnecting = true;
        isPause = false;
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene(1);
    }
}
