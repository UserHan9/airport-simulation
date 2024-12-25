using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MulitTouch : MonoBehaviour
{
   Vector3 touchPositionOffset;
    private Vector3 lastPos;
    private Quaternion lastRot;

    public bool lastActive = false;

    private void Start()
    {
        lastPos = this.transform.position;
        lastRot = this.transform.rotation;
    }

    private Vector3 GetTouchWorldPosition(Touch touch)
    {
        return Camera.main.ScreenToWorldPoint(touch.position);
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    OnTouchDown(touch);
                    break;
                case TouchPhase.Moved:
                    OnTouchDrag(touch);
                    break;
                case TouchPhase.Ended:
                    OnTouchUp();
                    break;
            }
        }
    }

    void OnTouchDown(Touch touch)
    {
        touchPositionOffset = gameObject.transform.position - GetTouchWorldPosition(touch);
    }

    void OnTouchDrag(Touch touch)
    {
        transform.position = GetTouchWorldPosition(touch) + touchPositionOffset;
    }

    void OnTouchUp()
    {
        if (lastActive == true)
        {
            transform.position = lastPos;
            transform.rotation = lastRot;
        }
    }
}
