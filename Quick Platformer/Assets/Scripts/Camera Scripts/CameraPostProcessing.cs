using UnityEngine;

public class CameraPostProcessing : MonoBehaviour
{
    void LateUpdate()
    {
        //currently unused script to be used for for changing post processing effects in focus mode
        /*
        if(Input.GetKey(KeyCode.F))
        {
            Camera.main.cullingMask = 1 << LayerMask.NameToLayer("Focus");
        }
        else
        {
            Camera.main.cullingMask = 1 << LayerMask.NameToLayer("Default");
        }
        */
    }
}
