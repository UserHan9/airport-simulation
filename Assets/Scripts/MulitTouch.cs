using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MulitTouch : MonoBehaviour
{
    private Dictionary<int, Vector3> touchPositionOffsets = new Dictionary<int, Vector3>();
    private Dictionary<int, Transform> activeTouches = new Dictionary<int, Transform>();
    private Vector3 lastPos;
    private Quaternion lastRot;

    public bool lastActive = false;

    private void Start()
    {
        lastPos = this.transform.position;
        lastRot = this.transform.rotation;
    }

    void Update()
    {
        foreach (Touch touch in Input.touches)
        {
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    OnTouchDown(touch);
                    break;
                case TouchPhase.Moved:
                    OnTouchDrag(touch);
                    break;
                case TouchPhase.Ended:
                    OnTouchUp(touch);
                    break;
            }
        }
    }

    void OnTouchDown(Touch touch)
    {
        Vector3 touchWorldPosition = GetTouchWorldPosition(touch);
        touchPositionOffsets[touch.fingerId] = gameObject.transform.position - touchWorldPosition;
        activeTouches[touch.fingerId] = transform;
    }

    void OnTouchDrag(Touch touch)
    {
        if (activeTouches.ContainsKey(touch.fingerId))
        {
            transform.position = GetTouchWorldPosition(touch) + touchPositionOffsets[touch.fingerId];
        }
    }

    void OnTouchUp(Touch touch)
    {
        if (activeTouches.ContainsKey(touch.fingerId))
        {
            if (lastActive == true)
            {
                transform.position = lastPos;
                transform.rotation = lastRot;
            }
            activeTouches.Remove(touch.fingerId);
            touchPositionOffsets.Remove(touch.fingerId);
        }
    }

    Vector3 GetTouchWorldPosition(Touch touch)
    {
        Vector3 touchPosition = new Vector3(touch.position.x, touch.position.y, Camera.main.nearClipPlane);
        return Camera.main.ScreenToWorldPoint(touchPosition);
    }
}
