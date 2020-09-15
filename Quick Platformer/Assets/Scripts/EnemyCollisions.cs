using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyCollisions : MonoBehaviour
{
    public AudioSource enemyDeathAudio;
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Pie"))
        {
            AudioManager.Instance.PlayEnemyDeath();
            Destroy(gameObject);
        }
    }
}
