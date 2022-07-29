using System.Collections;
using BathroomSelfie.Controllers;
using UnityEngine;

namespace BathroomSelfie.GameElements
{
    public class WomanCharacter : MonoBehaviour
    {
        private static readonly int Pose0 = Animator.StringToHash("Pose_0");
        private static readonly int Pose1 = Animator.StringToHash("Pose_1");
        private static readonly int Pose2 = Animator.StringToHash("Pose_2");
        private static readonly int Pose3 = Animator.StringToHash("Pose_3");

        [SerializeField] private Animator _Animator;
        [SerializeField] private GameObject _Flash;

        private QuickTimeEventController _quickTimeEvent;
        private const float FLASH_DURATION = 0.1f;

        private void Awake()
        {
            _quickTimeEvent = FindObjectOfType<QuickTimeEventController>();
            _quickTimeEvent.DidCorrect += QuickTimeEventOnDidCorrect;
        }

        private void QuickTimeEventOnDidCorrect(int index)
        {
            AnimationChangeTo(index);
            StartCoroutine(FlashRoutine());
        }

        private IEnumerator FlashRoutine()
        {
            _Flash.SetActive(true);
            yield return new WaitForSeconds(FLASH_DURATION);
            _Flash.SetActive(false);
        }

        private void AnimationChangeTo(int index)
        {
            switch (index)
            {
                case 0:
                    _Animator.SetTrigger(Pose0);
                    break;
                case 1:
                    _Animator.SetTrigger(Pose1);
                    break;
                case 2:
                    _Animator.SetTrigger(Pose2);
                    break;
                case 3:
                    _Animator.SetTrigger(Pose3);
                    break;
            }
        }
    }
}