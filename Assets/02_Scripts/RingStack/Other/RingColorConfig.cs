using UnityEngine;

// ReSharper disable InconsistentNaming
namespace RingStack.Other
{
    [CreateAssetMenu(fileName = "Clones", menuName = "Clones/RingStack/Color", order = 0)]
    public class RingColorConfig : ScriptableObject
    {
        public Material VisualMaterial;
        public Material GhostMaterial;
    }
}