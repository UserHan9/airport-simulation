using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiTouch : MonoBehaviour
{
    private Dictionary<int, Vector3> touchPositionOffsets = new Dictionary<int, Vector3>();
    private Dictionary<int, Transform> activeTouches = new Dictionary<int, Transform>();
    private int rotationFinger1 = -1;
    private int rotationFinger2 = -1;
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
        if (Input.touchCount > 0)
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

            // Handle two-finger rotation
            if (rotationFinger1 != -1 && rotationFinger2 != -1)
            {
                Touch touch1 = Input.GetTouch(rotationFinger1);
                Touch touch2 = Input.GetTouch(rotationFinger2);

                OnTwoFingerRotate(touch1, touch2);
            }
        }
    }

    void OnTouchDown(Touch touch)
    {
        Vector3 touchWorldPosition = GetTouchWorldPosition(touch);
        if (IsTouchingThisObject(touchWorldPosition))
        {
            touchPositionOffsets[touch.fingerId] = gameObject.transform.position - touchWorldPosition;
            activeTouches[touch.fingerId] = transform;

            // Assign fingers for rotation if two fingers touch this object
            if (rotationFinger1 == -1)
            {
                rotationFinger1 = touch.fingerId;
            }
            else if (rotationFinger2 == -1)
            {
                rotationFinger2 = touch.fingerId;
            }
        }
    }

    void OnTouchDrag(Touch touch)
    {
        if (activeTouches.ContainsKey(touch.fingerId) && touch.fingerId != rotationFinger2)
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

            // Clear rotation fingers if they are lifted
            if (touch.fingerId == rotationFinger1)
            {
                rotationFinger1 = -1;
            }
            else if (touch.fingerId == rotationFinger2)
            {
                rotationFinger2 = -1;
            }
        }
    }

    void OnTwoFingerRotate(Touch touch1, Touch touch2)
    {
        // Calculate the rotation angle between the two fingers
        Vector2 touch1PrevPos = touch1.position - touch1.deltaPosition;
        Vector2 touch2PrevPos = touch2.position - touch2.deltaPosition;

        float prevAngle = Mathf.Atan2(touch2PrevPos.y - touch1PrevPos.y, touch2PrevPos.x - touch1PrevPos.x) * Mathf.Rad2Deg;
        float currentAngle = Mathf.Atan2(touch2.position.y - touch1.position.y, touch2.position.x - touch1.position.x) * Mathf.Rad2Deg;

        float deltaAngle = currentAngle - prevAngle;

        // Apply the rotation to the object
        transform.Rotate(Vector3.forward, deltaAngle);
    }

    Vector3 GetTouchWorldPosition(Touch touch)
    {
        Vector3 touchPosition = new Vector3(touch.position.x, touch.position.y, Camera.main.nearClipPlane);
        return Camera.main.ScreenToWorldPoint(touchPosition);
    }

    bool IsTouchingThisObject(Vector3 touchWorldPosition)
    {
        Collider2D collider = GetComponent<Collider2D>();
        return collider != null && collider.OverlapPoint(touchWorldPosition);
    }
}
