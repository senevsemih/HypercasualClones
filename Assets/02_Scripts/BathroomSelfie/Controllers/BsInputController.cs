using System;
using BathroomSelfie.Other;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BathroomSelfie.Controllers
{
    public class BsInputController : MonoBehaviour
    {
        public event Action<Direction> DidDragEnd;

        [ShowInInspector, ReadOnly] private UIController _ui;
        [ShowInInspector, ReadOnly] private bool _isActive;

        private Vector3 _startPosition;
        private Vector3 _endPosition;

        private const float THRESHOLD = 0.8f;

        private void Awake()
        {
            _ui = FindObjectOfType<UIController>();
            _ui.DidOpeningEnd += UIOnDidOpeningEnd;
        }

        private void UIOnDidOpeningEnd() => _isActive = true;

        private void Update()
        {
            if (!_isActive) return;
            if (Input.GetMouseButtonDown(0))
            {
                _startPosition = Input.mousePosition;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                _endPosition = Input.mousePosition;

                var v = _endPosition - _startPosition;
                var direction = v.normalized;

                var d = DragDirection(direction);
                DidDragEnd?.Invoke(d);
            }
        }

        private Direction DragDirection(Vector3 direction)
        {
            var dir = Direction.Up;
            if (Vector2.Dot(Vector2.up, direction) > THRESHOLD)
            {
                dir = Direction.Up;
            }
            else if (Vector2.Dot(Vector2.down, direction) > THRESHOLD)
            {
                dir = Direction.Down;
            }
            else if (Vector2.Dot(Vector2.left, direction) > THRESHOLD)
            {
                dir = Direction.Left;
            }
            else if (Vector2.Dot(Vector2.right, direction) > THRESHOLD)
            {
                dir = Direction.Right;
            }

            return dir;
        }
    }
}