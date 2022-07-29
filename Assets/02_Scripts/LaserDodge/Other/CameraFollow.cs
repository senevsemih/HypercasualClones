using LaserDodge.GameElements;
using UnityEngine;

namespace LaserDodge.Other
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private LaserDodgeConfig _Config;
        private Character _character;

        private Vector3 TargetPosition => _character.transform.position + _Config.CameraFollowOffset;

        private void Awake() => _character = FindObjectOfType<Character>();

        private void LateUpdate()
        {
            transform.position = Vector3.Lerp(transform.position, TargetPosition,
                _Config.CameraFollowSpeed * Time.deltaTime);
        }
    }
}