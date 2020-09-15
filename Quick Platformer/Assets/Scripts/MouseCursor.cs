using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCursor : MonoBehaviour
{
    private static bool cursorLock = false;
    private static Vector3 lockPosition;

    public float sensitivity = 5000;
    public RectTransform canvasRect;

    RectTransform m_ImageRect;
    Vector3 m_Movement;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        m_ImageRect = GetComponent<RectTransform>();
    }

    void Update()
    {
        //large cursor position bugfix
        if(m_ImageRect.localPosition.x > 500 || m_ImageRect.localPosition.x < -500 || m_ImageRect.localPosition.y > 250 || m_ImageRect.localPosition.y < -250)
        {
            m_ImageRect.localPosition = Vector2.zero;
        }
        
        if (!cursorLock)
        {
            //move watermelon cursor with mouse movement
            m_Movement = Vector3.zero;
            if ((m_ImageRect.localPosition.x < 400 || Input.GetAxis("Mouse X") < 0) && (m_ImageRect.localPosition.x > -400 || Input.GetAxis("Mouse X") > 0))
            {
                m_Movement.x = Input.GetAxis("Mouse X");
            }
            if ((m_ImageRect.localPosition.y < 180 || Input.GetAxis("Mouse Y") < 0) && (m_ImageRect.localPosition.y > -180 || Input.GetAxis("Mouse Y") > 0))
            {
                m_Movement.y = Input.GetAxis("Mouse Y");
            }
            m_ImageRect.localPosition += m_Movement * sensitivity * Time.deltaTime;
        }
        else
        {
            //lock cursor to world position
            Vector2 lockCanvasPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, Camera.main.WorldToScreenPoint(lockPosition), Camera.main, out lockCanvasPoint);
            m_ImageRect.localPosition = lockCanvasPoint;
        }
    }

    public static void LockCursor(Vector3 pos)
    {
        if(!cursorLock)
        {
            cursorLock = true;
            lockPosition = pos;
        }
    }

    public static void UnlockCursor()
    {
        cursorLock = false;
    }
}
