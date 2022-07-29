using System;
using LaserDodge.Controllers;
using LaserDodge.Other;
using UnityEngine;

namespace LaserDodge.GameElements
{
    public class CharacterRigHandler : MonoBehaviour
    {
        public event Action<bool> DidTriggerWithLaser;

        [SerializeField] private LaserDodgeConfig _Config;
        [SerializeField] private Collider _Collider;
        [SerializeField] private MeshRenderer _IndicatorMesh;
        [Space] [SerializeField] private Transform _Rig;
        [Space] [SerializeField] private float _Angle;
        [SerializeField] private Vector3 _Axis;

        private float _initialAngle;
        private Quaternion _initialRotation;
        private LdInputController _input;
        public bool IsInLaser { get; private set; }

        private void Start()
        {
            _initialAngle = _Angle;
            _initialRotation = _Rig.localRotation;
        }

        public void SetActive(bool value)
        {
            _Collider.enabled = value;
            _IndicatorMesh.gameObject.SetActive(value);
        }

        public void BackToOriginalPose()
        {
            _Angle = _initialAngle;
            _Rig.localRotation = _initialRotation;
        }

        public void SetInput(LdInputController input)
        {
            _input = input;
            _input.DidDrag += InputOnDidDrag;
            _input.DidDragEnd += InputOnDidDragEnd;
        }

        private void InputOnDidDrag(Vector3 deltaPos)
        {
            _Angle += deltaPos.x * _Config.RigRotationSpeed * Time.deltaTime;
            _Rig.localRotation = Quaternion.AngleAxis(_Angle, _Axis);
        }

        private void InputOnDidDragEnd()
        {
            _input.DidDrag -= InputOnDidDrag;
            _input.DidDragEnd -= InputOnDidDragEnd;
        }

        private void OnTriggerEnter(Collider other)
        {
            IsInLaser = true;
            _IndicatorMesh.material = _Config.RedMaterial;
            DidTriggerWithLaser?.Invoke(IsInLaser);
        }

        private void OnTriggerExit(Collider other)
        {
            IsInLaser = false;
            _IndicatorMesh.material = _Config.GreenMaterial;
            DidTriggerWithLaser?.Invoke(IsInLaser);
        }
    }
}