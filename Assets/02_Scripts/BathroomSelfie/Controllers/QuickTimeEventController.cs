using System;
using System.Collections;
using System.Collections.Generic;
using BathroomSelfie.GameElements;
using BathroomSelfie.Other;
using UnityEngine;
using UnityEngine.UI;

namespace BathroomSelfie.Controllers
{
    public class QuickTimeEventController : MonoBehaviour
    {
        public event Action DidEventsCompleted;
        public event Action<int> DidCorrect;

        [SerializeField] private BathroomSelfieConfig _Config;
        [Space] 
        [SerializeField] private Arrow _Arrow;
        [SerializeField] private Image _BoxIndicatorImage;
        [SerializeField] private Transform _ArrowSpawnPoint;
        [SerializeField] private GameObject _QuickTimeEventBar;
        [Space] 
        [SerializeField] private List<Arrow> _Arrows = new List<Arrow>();

        private const float INSIDE_LIMIT_MIN = 150;
        private const float INSIDE_LIMIT_MAX = 400;

        private int _totalEventCount;
        private float _timer;
        private Color _boxColorDefault;
        private Color _boxColorTarget;
        private bool _isBoxColorChangeActive;

        private UIController _ui;
        private BsInputController _input;

        private void Awake()
        {
            _ui = FindObjectOfType<UIController>();
            _input = FindObjectOfType<BsInputController>();

            _ui.DidOpeningEnd += UIOnDidOpeningEnd;
            _input.DidDragEnd += InputOnDidDragEnd;
        }

        private void Start() => _boxColorDefault = _BoxIndicatorImage.color;

        private void Update() => BoxColorChangeToEventAction();

        private void UIOnDidOpeningEnd()
        {
            _QuickTimeEventBar.SetActive(true);
            StartCoroutine(ArrowSpawnRoutine());
        }

        private void InputOnDidDragEnd(Direction direction)
        {
            var firstArrow = _Arrows[0];
            var isInside = firstArrow.CurrentPositionX < INSIDE_LIMIT_MAX &&
                           firstArrow.CurrentPositionX > INSIDE_LIMIT_MIN;

            if (isInside && direction == firstArrow.Direction)
            {
                firstArrow.DeInit();
                _Arrows.Remove(firstArrow);
                EventCountDecrease();

                _boxColorTarget = Color.green;

                var randomIndex = BsUtilities.RandomInList(_Config._Photos.Count);
                DidCorrect?.Invoke(randomIndex);
            }
            else
            {
                _boxColorTarget = Color.red;
            }

            _timer = 0;
            _isBoxColorChangeActive = true;
        }

        private IEnumerator ArrowSpawnRoutine()
        {
            var count = _Config.ArrowSpawnCount;
            _totalEventCount = count;

            for (var i = 0; i < count; i++)
            {
                var arrow = Instantiate(_Arrow, _ArrowSpawnPoint, true);
                arrow.transform.localPosition = Vector3.zero;
                arrow.Init(INSIDE_LIMIT_MIN, BsUtilities.RandomDirection());

                arrow.DidBecomeUseless += ArrowOnDidBecomeUseless;
                _Arrows.Add(arrow);
                yield return new WaitForSeconds(_Config.ArrowSpawnInterval);
            }
        }

        private void ArrowOnDidBecomeUseless(Arrow arrow)
        {
            arrow.DidBecomeUseless -= ArrowOnDidBecomeUseless;
            _Arrows.Remove(arrow);
            EventCountDecrease();
        }

        private void BoxColorChangeToEventAction()
        {
            if (_timer < _Config.BoxIndicatorColorChangeDuration && _isBoxColorChangeActive)
            {
                _timer += Time.deltaTime;
                var tVal = Mathf.InverseLerp(0, _Config.BoxIndicatorColorChangeDuration, _timer);
                var curve = _Config.BoxIndicatorColorCurve.Evaluate(tVal);
                _BoxIndicatorImage.color = Color.Lerp(_boxColorDefault, _boxColorTarget, curve);
            }
            else
            {
                _isBoxColorChangeActive = false;
            }
        }

        private void EventCountDecrease()
        {
            _totalEventCount--;
            if (_totalEventCount <= 0)
            {
                DidEventsCompleted?.Invoke();
            }
        }
    }
}