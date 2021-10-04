using UnityEngine;
using System;


namespace TileMatch.Inp
{
    public class InputController : MonoBehaviour
    {
        public static event Action<Vector3> TouchBegan;
        public static event Action<Vector3> TouchDropped;

        void Update()
        {
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Began(Input.mousePosition);
            }
            else if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                Drop(Input.mousePosition);
            }
            Move(Input.mousePosition);
#elif UNITY_ANDROID || UNITY_IOS

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 touchPos = touch.position;

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    Began(touchPos);
                    break;
                case TouchPhase.Moved:
                    Move(touchPos);
                    break;
                case TouchPhase.Stationary:
                    break;
                case TouchPhase.Ended:
                    Drop(touchPos);
                    break;
                case TouchPhase.Canceled:

                    break;
                default:
                    break;
            }
        }
#endif
        }
        public void Began(Vector3 touchPosition)
        {
            TouchBegan?.Invoke(touchPosition);
        }
        public void Move(Vector3 touchPosition)
        {
        }
        public void Drop(Vector3 touchposition)
        {
            touchposition = Camera.main.ScreenToWorldPoint(touchposition);
            TouchDropped?.Invoke(touchposition);
        }
    }
}
