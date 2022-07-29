using System.Collections;
using System.Collections.Generic;
using LaserDodge.Other;
using UnityEngine;

namespace LaserDodge.GameElements
{
    public class Character : MonoBehaviour
    {
        [SerializeField] private LaserDodgeConfig _Config;
        [SerializeField] private CharacterPhysics _Physics;
        [SerializeField] private List<CharacterRigHandler> _RigHandlers = new List<CharacterRigHandler>();

        private bool _isInTrap;
        private float _timer;

        private float Speed => _isInTrap ? _Config.SlowMotionSpeed : _Config.FallSpeed;

        private void Awake()
        {
            _Physics.TriggerEnter += PhysicsOnTriggerEnter;
            foreach (var rigHandler in _RigHandlers)
            {
                rigHandler.DidTriggerWithLaser += RigHandlerOnDidTriggerWithLaser;
            }
        }

        private void Start() => SetAllRigs(false);

        private void Update()
        {
            transform.position += Vector3.down * (Speed * Time.deltaTime);

            TrapZoneTimer();
        }

        private void PhysicsOnTriggerEnter(Collider other)
        {
            _isInTrap = true;
            SetAllRigs(true);
        }

        private void RigHandlerOnDidTriggerWithLaser(bool isInLaser)
        {
            var isAllRigsSafe = _RigHandlers.TrueForAll(rig => !rig.IsInLaser);
            if (isAllRigsSafe)
            {
                _isInTrap = false;
                _timer = 0;
                SetAllRigs(false);

                StartCoroutine(Delay());
            }
        }

        private void SetAllRigs(bool value)
        {
            foreach (var rigHandler in _RigHandlers)
            {
                rigHandler.SetActive(value);
            }
        }

        private void TrapZoneTimer()
        {
            if (!_isInTrap) return;
            if (_timer < _Config.TrapZoneDuration)
            {
                _timer += Time.deltaTime;
            }
            else
            {
                Debug.Log("Level Fail");
            }
        }

        private IEnumerator Delay()
        {
            yield return new WaitForSeconds(_Config.RigsBackToOriginalPoseDelay);
            foreach (var rigHandler in _RigHandlers)
            {
                rigHandler.BackToOriginalPose();
            }
        }
    }
}