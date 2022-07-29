using System;
using System.Collections;
using System.Collections.Generic;
using BathroomSelfie.GameElements;
using BathroomSelfie.Other;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BathroomSelfie.Controllers
{
    public class UIController : MonoBehaviour
    {
        public event Action DidOpeningEnd;

        [SerializeField] private BathroomSelfieConfig _Config;
        [Space] 
        [SerializeField] private Transform _ManChatUI;
        [SerializeField] private Transform _WomanChat0UI;
        [SerializeField] private Transform _WomanChat1UI;
        [Space] 
        [SerializeField] private Photo _Photo;
        [SerializeField] private Transform _PhotosParent;
        [SerializeField] private List<Photo> _Photos = new List<Photo>();

        private float _timer;
        private QuickTimeEventController _quickTimeEvent;

        private void Awake()
        {
            _quickTimeEvent = FindObjectOfType<QuickTimeEventController>();

            _quickTimeEvent.DidCorrect += QuickTimeEventOnDidCorrect;
            _quickTimeEvent.DidEventsCompleted += QuickTimeEventOnDidEventsCompleted;
        }

        private void Start() => StartCoroutine(OpeningRoutine());

        private IEnumerator OpeningRoutine()
        {
            yield return new WaitForSeconds(_Config.NextChatPopupWait);

            while (_timer < _Config.ChatPopupDuration)
            {
                _timer += Time.deltaTime;
                ChatAnimation(_ManChatUI);
                yield return null;
            }

            _timer = 0;

            yield return new WaitForSeconds(_Config.NextChatPopupWait);

            while (_timer < _Config.ChatPopupDuration)
            {
                _timer += Time.deltaTime;
                ChatAnimation(_WomanChat0UI);
                yield return null;
            }

            _timer = 0;

            yield return new WaitForSeconds(_Config.NextChatPopupWait);

            while (_timer < _Config.ChatPopupDuration)
            {
                _timer += Time.deltaTime;
                ChatAnimation(_WomanChat1UI);
                yield return null;
            }

            yield return new WaitForSeconds(_Config.NextChatPopupWait);

            CloseOpeningObjects();
            DidOpeningEnd?.Invoke();
        }

        private void ChatAnimation(Transform chat)
        {
            var tVal = Mathf.InverseLerp(0, _Config.ChatPopupDuration, _timer);
            var curve = _Config.ChatPopupCurve.Evaluate(tVal);
            chat.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, curve);
        }

        private void CloseOpeningObjects()
        {
            _ManChatUI.gameObject.SetActive(false);
            _WomanChat0UI.gameObject.SetActive(false);
            _WomanChat1UI.gameObject.SetActive(false);
        }

        private void QuickTimeEventOnDidCorrect(int index)
        {
            var photo = Instantiate(_Photo, _PhotosParent);
            photo.transform.localPosition = Vector3.zero + _Config.PhotoOffset * _Photos.Count;

            var randomAngle = Random.Range(_Config.PhotoRotateRange.x, _Config.PhotoRotateRange.y);
            photo.transform.rotation = Quaternion.Euler(0f, 0f, randomAngle);

            var sprite = _Config._Photos[index];
            photo.SetSprite(sprite);

            _Photos.Add(photo);
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private void QuickTimeEventOnDidEventsCompleted()
        {
            foreach (var photo in _Photos)
            {
                var randomX = Random.Range(_Config.PhotoShowcasePositionRangeX.x,
                    _Config.PhotoShowcasePositionRangeX.y);
                var randomY = Random.Range(_Config.PhotoShowcasePositionRangeY.x,
                    _Config.PhotoShowcasePositionRangeY.y);

                var showcasePosition = new Vector3(randomX, randomY);
                photo.SetShowCasePosition(showcasePosition, _Config.PhotoShowcasePositionDuration,
                    _Config.PhotoShowcasePositionCurve);
            }

            Debug.Log("Level Completed");
        }
    }
}