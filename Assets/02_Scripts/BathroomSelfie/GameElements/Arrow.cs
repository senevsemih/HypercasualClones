using System;
using BathroomSelfie.Other;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace BathroomSelfie.GameElements
{
    public class Arrow : MonoBehaviour
    {
        public event Action<Arrow> DidBecomeUseless;

        [SerializeField] private BathroomSelfieConfig _Config;
        [SerializeField] private Image _Image;

        [ShowInInspector, ReadOnly] private bool _isActive;
        [ShowInInspector, ReadOnly] private bool _isInList;
        [ShowInInspector, ReadOnly] private Direction _direction;
        [ShowInInspector, ReadOnly] private Vector3 _currenPosition;
        [ShowInInspector, ReadOnly] private Vector3 _startPosition;
        [ShowInInspector, ReadOnly] private Vector3 _upwardPosition;

        public float CurrentPositionX => _currenPosition.x;
        public Direction Direction => _direction;

        private float _timer;
        private Color _imageColorDefault;
        private Color _imageColorFadeOut;
        private float _insideLimitMin;

        private const float STOP_LIMIT = -100;

        public void Init(float insideLimitMin, Direction direction)
        {
            _direction = direction;
            _insideLimitMin = insideLimitMin;
            SetRotation(direction);

            _isInList = true;
            _isActive = true;

            _imageColorDefault = _Image.color;
            _imageColorFadeOut = _imageColorDefault;
            _imageColorFadeOut.a = 0;
        }

        public void DeInit()
        {
            _isActive = false;
            _startPosition = _currenPosition;
            _upwardPosition = _currenPosition + Vector3.up * _Config.ArrowUpwardOffset;
        }

        private void Update()
        {
            _currenPosition = transform.position;

            IsInteractableCheck();

            LeftMovement();

            UpwardMovement();
        }

        private void IsInteractableCheck()
        {
            if (!(_currenPosition.x < _insideLimitMin) || !_isInList) return;

            DidBecomeUseless?.Invoke(this);
            _isInList = false;
        }

        private void LeftMovement()
        {
            if (_currenPosition.x > STOP_LIMIT && _isActive)
            {
                transform.position += Vector3.left * (Time.deltaTime * _Config.ArrowSpeed);
            }
        }

        private void UpwardMovement()
        {
            if (_isActive || !(_timer < _Config.ArrowUpwardPositionDuration)) return;

            _timer += Time.deltaTime;
            var tVal = Mathf.InverseLerp(0, _Config.ArrowUpwardPositionDuration, _timer);

            transform.position = Vector3.Lerp(_startPosition, _upwardPosition, tVal);
            _Image.color = Color.Lerp(_imageColorDefault, _imageColorFadeOut, tVal);
        }

        private void SetRotation(Direction direction)
        {
            transform.eulerAngles = direction switch
            {
                Direction.Up => Vector3.zero,
                Direction.Left => Vector3.forward * 90,
                Direction.Down => Vector3.forward * 180,
                Direction.Right => Vector3.forward * 270,
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
            };
        }
    }
}