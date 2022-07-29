using System;
using System.Collections;
using RingStack.GameElements;
using RingStack.Other;
using Sirenix.OdinInspector;
using UnityEngine;
using Stack = RingStack.GameElements.Stack;

namespace RingStack.Controllers
{
    public class RsInputController : MonoBehaviour
    {
        public event Action<Vector3> DidDrag;
        public event Action<bool, Stack> DidDragEnd;

        [SerializeField] private RingStackConfig _Config;
        [Space] 
        [ShowInInspector, ReadOnly] private Stack _takenRingStack;
        [ShowInInspector, ReadOnly] private Stack _rayPointingStack;
        [ShowInInspector, ReadOnly] private Ring _takenRing;
        [ShowInInspector, ReadOnly] private bool _isActive = true;
        [ShowInInspector, ReadOnly] private bool _isPlaceable;

        private Camera _camera;

        private void Awake()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            if (!_isActive) return;

            var ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (Input.GetMouseButtonDown(0))
            {
                if (Physics.Raycast(ray, out var hitInfo))
                {
                    _takenRingStack = hitInfo.collider.GetComponent<Stack>();
                    _takenRing = _takenRingStack.TakeRing();
                    if (_takenRing) _takenRing.SetInput(this);
                }
            }
            else if (Input.GetMouseButton(0))
            {
                var mousePos = Input.mousePosition;
                mousePos.z = _camera.nearClipPlane - _camera.transform.position.z;
                var worldPosition = _camera.ScreenToWorldPoint(mousePos);

                if (Physics.Raycast(ray, out var hitInfo) && _takenRing)
                {
                    _rayPointingStack = hitInfo.collider.GetComponent<Stack>();
                    var isDifferentStack = _rayPointingStack != _takenRingStack;

                    _isPlaceable = _rayPointingStack.IsPlaceable(_takenRing.Graphic.CurrentMaterial);
                    if (isDifferentStack && _isPlaceable)
                    {
                        _rayPointingStack.SetGhostActive(true, _takenRing.Graphic.GhostMaterial);
                    }
                }
                else
                {
                    _isPlaceable = false;
                    if (_rayPointingStack) _rayPointingStack.SetGhostActive(false);
                }

                DidDrag?.Invoke(worldPosition + _Config.InputFollowOffset);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                if (Physics.Raycast(ray, out var hitInfo) && _takenRing)
                {
                    var stack = hitInfo.collider.GetComponent<Stack>();
                    _rayPointingStack.SetGhostActive(false);
                    DidDragEnd?.Invoke(_isPlaceable, stack);

                    StartCoroutine(DelayRoutine(_Config.RingBackToPullUpPositionDuration +
                                                _Config.RingBackToStackPositionDuration));
                }
                else
                {
                    DidDragEnd?.Invoke(false, _takenRingStack);
                    StartCoroutine(DelayRoutine(_Config.RingBackToPullUpPositionDuration +
                                                _Config.RingBackToStackPositionDuration));
                }

                _takenRingStack = null;
                _rayPointingStack = null;
                _takenRing = null;
                _isPlaceable = false;
            }
        }

        private IEnumerator DelayRoutine(float duration)
        {
            _isActive = false;
            yield return new WaitForSeconds(duration);
            _isActive = true;
        }
    }
}