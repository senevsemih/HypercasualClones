using System.Collections;
using RingStack.Controllers;
using RingStack.Other;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RingStack.GameElements
{
    public class Ring : MonoBehaviour
    {
        [SerializeField] private RingStackConfig _Config;
        [SerializeField] private RingGraphic _Graphic;
        [ShowInInspector, ReadOnly] private bool _isMovable;

        private float _timer;
        private bool _isPullUpRoutineActive;
        private bool _isBackToStackRoutineActive;

        private Vector3 _inStackPosition;
        private Vector3 _stackPullUpPosition;

        private RsInputController _input;
        private Stack _currentStack;

        public RingGraphic Graphic => _Graphic;

        public void Init(Stack stack, Vector3 stackPullUpPosition)
        {
            _currentStack = stack;
            _inStackPosition = transform.position;
            _stackPullUpPosition = stackPullUpPosition;

            _Graphic.SetActive(true);
        }

        public void Taken()
        {
            _isBackToStackRoutineActive = false;
            _isPullUpRoutineActive = true;

            StartCoroutine(TakeRoutine());
        }

        public void SetInput(RsInputController input)
        {
            _input = input;
            _input.DidDrag += InputOnDidDrag;
            _input.DidDragEnd += InputOnDidDragEnd;
        }

        private void InputOnDidDrag(Vector3 worldPosition)
        {
            if (_isMovable)
            {
                transform.position = Vector3.Lerp(
                    transform.position, worldPosition,
                    Time.deltaTime * _Config.RingInputFollowSpeed);
            }
        }

        private void InputOnDidDragEnd(bool isNewStack, Stack stack)
        {
            _input.DidDrag -= InputOnDidDrag;
            _input.DidDragEnd -= InputOnDidDragEnd;

            _isPullUpRoutineActive = false;
            _isMovable = false;
            _isBackToStackRoutineActive = true;

            var startPosition = transform.position;
            if (isNewStack)
            {
                _currentStack.RemoveRing(this);
                _stackPullUpPosition = stack.PullUpPosition;
                _inStackPosition = stack.NewRingStackPosition;

                StartCoroutine(BackToStackRoutine(startPosition, _stackPullUpPosition, _inStackPosition));

                _currentStack = stack;
                _currentStack.AddNewRing(this);
            }
            else
            {
                StartCoroutine(BackToStackRoutine(startPosition, _stackPullUpPosition, _inStackPosition));
            }
        }

        private IEnumerator BackToStackRoutine(Vector3 startPosition, Vector3 pullUpPosition, Vector3 inStackPosition)
        {
            while (_timer < _Config.RingBackToPullUpPositionDuration && _isBackToStackRoutineActive)
            {
                _timer += Time.deltaTime;
                var tVal = Mathf.InverseLerp(0, _Config.RingBackToPullUpPositionDuration, _timer);
                transform.position = Vector3.Lerp(startPosition, pullUpPosition, tVal);
                yield return null;
            }

            _timer = 0;
            while (_timer < _Config.RingBackToStackPositionDuration && _isBackToStackRoutineActive)
            {
                _timer += Time.deltaTime;
                var tVal = Mathf.InverseLerp(0, _Config.RingBackToStackPositionDuration, _timer);
                var curve = _Config.RingBackToStackPositionCurve.Evaluate(tVal);
                transform.position = Vector3.Lerp(pullUpPosition, inStackPosition, curve);
                yield return null;
            }

            _timer = 0;
            _isBackToStackRoutineActive = false;
        }

        private IEnumerator TakeRoutine()
        {
            while (_timer < _Config.RingPullUpPositionDuration && _isPullUpRoutineActive)
            {
                _timer += Time.deltaTime;
                var tVal = Mathf.InverseLerp(0, _Config.RingPullUpPositionDuration, _timer);
                transform.position = Vector3.Lerp(_inStackPosition, _stackPullUpPosition, tVal);
                yield return null;
            }

            _timer = 0;
            _isMovable = true;
        }
    }
}