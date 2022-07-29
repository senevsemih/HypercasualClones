using UnityEngine;
using UnityEngine.UI;

namespace BathroomSelfie.GameElements
{
    public class Photo : MonoBehaviour
    {
        [SerializeField] private Image _Image;

        private bool _isShowcasePositionActive;
        private Vector3 _startPosition;
        private Vector3 _showcasePosition;

        private float _timer;
        private float _duration;
        private AnimationCurve _curve;

        public void SetSprite(Sprite sprite) => _Image.sprite = sprite;

        public void SetShowCasePosition(Vector3 showcasePosition, float duration, AnimationCurve curve)
        {
            _startPosition = transform.position;
            _showcasePosition = showcasePosition;
            _duration = duration;
            _curve = curve;

            _isShowcasePositionActive = true;
        }

        private void Update()
        {
            if (!_isShowcasePositionActive || !(_timer < _duration)) return;

            _timer += Time.deltaTime;
            var tVal = Mathf.InverseLerp(0, _duration, _timer);
            var curve = _curve.Evaluate(tVal);
            transform.position = Vector3.Lerp(_startPosition, _showcasePosition, curve);
        }
    }
}