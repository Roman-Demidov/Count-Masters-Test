using System;
using UnityEngine;

namespace countMastersTest.infrastructure.input
{
    public class SwipeDetector : MonoBehaviour, IInputService
    {
        [SerializeField] private bool _detectSwipeOnlyAfterRelease = false;
        [SerializeField] private float _minDistanceForSwipe = 20f;

        private Vector2 _fingerDownPosition;
        private Vector2 _fingerUpPosition;

        public event Action<SwipeData> onSwipe;

        private void Update()
        {
#if UNITY_EDITOR
            updateForMouse();
#elif UNITY_ANDROID || UNITY_IOS
            updateForTouch();
#endif
        }

        private void updateForMouse()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _fingerUpPosition = Input.mousePosition;
                _fingerDownPosition = Input.mousePosition;
            }

            if (!_detectSwipeOnlyAfterRelease && Input.GetMouseButton(0))
            {
                _fingerDownPosition = Input.mousePosition;
                DetectSwipe();
            }

            if (Input.GetMouseButtonUp(0))
            {
                _fingerDownPosition = Input.mousePosition;
                DetectSwipe();
            }
        }

        
        private void updateForTouch()
        {
            if (Input.touchCount == 0)
            {
                return;
            }

            Touch touch = Input.touches[0];

            if (touch.phase == TouchPhase.Began)
            {
                _fingerUpPosition = touch.position;
                _fingerDownPosition = touch.position;
            }

            if (!_detectSwipeOnlyAfterRelease && touch.phase == TouchPhase.Moved)
            {
                _fingerDownPosition = touch.position;
                DetectSwipe();
            }

            if (touch.phase == TouchPhase.Ended)
            {
                _fingerDownPosition = touch.position;
                DetectSwipe();
            }
        }

        private void DetectSwipe()
        {
            if (SwipeDistanceCheckMet())
            {
                Vector2 direction;
                direction.y = _fingerDownPosition.y - _fingerUpPosition.y > 0 ? 1 : -1;
                direction.x = _fingerDownPosition.x - _fingerUpPosition.x > 0 ? 1 : -1;

                SendSwipe(direction, 1);

                _fingerUpPosition = _fingerDownPosition;
            }
        }

        private bool SwipeDistanceCheckMet()
        {
            return VerticalMovementDistance() > _minDistanceForSwipe || HorizontalMovementDistance() > _minDistanceForSwipe;
        }

        private float VerticalMovementDistance()
        {
            return Mathf.Abs(_fingerDownPosition.y - _fingerUpPosition.y);
        }

        private float HorizontalMovementDistance()
        {
            return Mathf.Abs(_fingerDownPosition.x - _fingerUpPosition.x);
        }

        private void SendSwipe(Vector3 direction, float speed)
        {
            SwipeData swipeData = new SwipeData()
            {
                direction = direction,
                speed = speed
            };

            onSwipe?.Invoke(swipeData);
        }
    }

    public struct SwipeData
    {
        public Vector3 direction;
        public float speed;
    }
}