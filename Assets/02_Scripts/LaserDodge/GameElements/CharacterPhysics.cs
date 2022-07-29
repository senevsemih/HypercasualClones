using System;
using UnityEngine;

namespace LaserDodge.GameElements
{
    public class CharacterPhysics : MonoBehaviour
    {
        public event Action<Collider> TriggerEnter;
        private void OnTriggerEnter(Collider other) => TriggerEnter?.Invoke(other);
    }
}