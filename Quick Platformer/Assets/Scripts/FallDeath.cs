using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallDeath : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            AudioManager.Instance.PlayPlayerDeath();
            Destroy(other.gameObject);
            Bob.Instance.ReloadScene();
        }
    }
}
