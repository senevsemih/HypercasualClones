using RingStack.Other;
using UnityEngine;

namespace RingStack.GameElements
{
    public class RingGraphic : MonoBehaviour
    {
        [SerializeField] private RingColorConfig _Config;
        [SerializeField] private MeshRenderer _MeshRenderer;
        
        public Material CurrentMaterial => _MeshRenderer.sharedMaterial;
        public Material GhostMaterial => _Config.GhostMaterial;

        private void Start() => _MeshRenderer.material = _Config.VisualMaterial;

        public void SetActive(bool value) => _MeshRenderer.gameObject.SetActive(value);

        public void SetGhostMaterial(Material newMaterial) => _MeshRenderer.material = newMaterial;
    }
}