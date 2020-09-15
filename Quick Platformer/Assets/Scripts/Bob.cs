using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bob : MonoBehaviour
{
    public static float loadTime = 1;
    public static Bob Instance;
    private void Start()
    {
        Instance = this;
    }
    public void ReloadScene()
    {
        StartCoroutine(Wait());
    }
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(loadTime);
        SceneManager.LoadScene(0);
    }
}
