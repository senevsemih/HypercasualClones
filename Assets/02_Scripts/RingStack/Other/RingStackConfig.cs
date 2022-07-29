using Sirenix.OdinInspector;
using UnityEngine;

// ReSharper disable InconsistentNaming
namespace RingStack.Other
{
    [CreateAssetMenu(fileName = "Clones", menuName = "Clones/RingStack", order = 0)]
    public class RingStackConfig : ScriptableObject
    {
        [FoldoutGroup("Input")] public Vector3 InputFollowOffset;
        
        [FoldoutGroup("Stack")] public Vector3 StackPullUpOffset;
        
        [FoldoutGroup("Ring")] public float RingPullUpPositionDuration;
        [FoldoutGroup("Ring")] public float RingInputFollowSpeed;
        
        [FoldoutGroup("Ring")] public float RingBackToPullUpPositionDuration;
        [FoldoutGroup("Ring")] public float RingBackToStackPositionDuration;
        [FoldoutGroup("Ring")] public AnimationCurve RingBackToStackPositionCurve;
    }
}