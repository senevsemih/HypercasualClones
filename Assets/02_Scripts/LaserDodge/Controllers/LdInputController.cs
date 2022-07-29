using System;
using LaserDodge.GameElements;
using Sirenix.OdinInspector;
using UnityEngine;

namespace LaserDodge.Controllers
{
    public class LdInputController : MonoBehaviour
    {
        public event Action<Vector3> DidDrag;
        public event Action DidDragEnd;

        [SerializeField, Required] private LayerMask _RigLayer;

        private Camera _camera;
        private CharacterRigHandler _rigHandler;

        private Vector3? _lastInputPos;
        private const float MAX_RAY_DISTANCE = 100f;

        private void Awake() => _camera = Camera.main;

        private void Update()
        {
            var mousePosition = Input.mousePosition;
            if (Input.GetMouseButtonDown(0))
            {
                var ray = _camera.ScreenPointToRay(mousePosition);
                if (Physics.Raycast(ray, out var hitInfo, MAX_RAY_DISTANCE, _RigLayer))
                {
                    _rigHandler = hitInfo.collider.GetComponent<CharacterRigHandler>();
                    _rigHandler.SetInput(this);
                }
            }
            else if (Input.GetMouseButton(0) && _lastInputPos.HasValue)
            {
                var v = mousePosition - _lastInputPos.Value;
                var direction = v.normalized;

                DidDrag?.Invoke(direction);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                _lastInputPos = null;
                DidDragEnd?.Invoke();
            }

            _lastInputPos = mousePosition;
        }
    }
}