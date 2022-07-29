using Sirenix.OdinInspector;
using UnityEngine;

// ReSharper disable InconsistentNaming
namespace LaserDodge.Other
{
    [CreateAssetMenu(fileName = "Clones", menuName = "Clones/LaserDodge", order = 0)]
    public class LaserDodgeConfig : ScriptableObject
    {
        [FoldoutGroup("Character")] public float FallSpeed;
        [FoldoutGroup("Character")] public float SlowMotionSpeed;
        [FoldoutGroup("Character")] public float TrapZoneDuration;
        [Space] 
        [FoldoutGroup("Character")] public float RigRotationSpeed;
        [FoldoutGroup("Character")] public float RigsBackToOriginalPoseDelay;

        [FoldoutGroup("Material")] public Material RedMaterial;
        [FoldoutGroup("Material")] public Material GreenMaterial;

        [FoldoutGroup("Camera")] public Vector3 CameraFollowOffset;
        [FoldoutGroup("Camera")] public float CameraFollowSpeed;
    }
}