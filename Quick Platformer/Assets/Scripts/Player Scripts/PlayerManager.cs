using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public AudioSource playerDeathAudio;
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(hit.gameObject.CompareTag("Enemy"))
        {
            AudioManager.Instance.PlayPlayerDeath();
            Bob.Instance.ReloadScene();
            Destroy(gameObject);
        }
    }
}
