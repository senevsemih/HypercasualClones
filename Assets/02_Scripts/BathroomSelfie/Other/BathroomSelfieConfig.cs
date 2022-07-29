using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

// ReSharper disable InconsistentNaming
namespace BathroomSelfie.Other
{
    [CreateAssetMenu(fileName = "Clones", menuName = "Clones/BathroomConfig", order = 0)]
    public class BathroomSelfieConfig : ScriptableObject
    {
        [FoldoutGroup("UI")] public float ChatPopupDuration;
        [FoldoutGroup("UI")] public float NextChatPopupWait;
        [FoldoutGroup("UI")] public AnimationCurve ChatPopupCurve;
        [Space] 
        [FoldoutGroup("UI")] public Vector2 PhotoRotateRange;
        [FoldoutGroup("UI")] public Vector2 PhotoShowcasePositionRangeX;
        [FoldoutGroup("UI")] public Vector2 PhotoShowcasePositionRangeY;
        [FoldoutGroup("UI")] public float PhotoShowcasePositionDuration;
        [FoldoutGroup("UI")] public AnimationCurve PhotoShowcasePositionCurve;

        [FoldoutGroup("QTE")] public int ArrowSpawnCount;
        [FoldoutGroup("QTE")] public float ArrowSpawnInterval;
        [Space] 
        [FoldoutGroup("QTE")] public float BoxIndicatorColorChangeDuration;
        [FoldoutGroup("QTE")] public AnimationCurve BoxIndicatorColorCurve;

        [FoldoutGroup("Arrow")] public float ArrowSpeed;
        [FoldoutGroup("Arrow")] public float ArrowUpwardPositionDuration;
        [FoldoutGroup("Arrow")] public float ArrowUpwardOffset;

        [FoldoutGroup("Woman")] public Vector3 PhotoOffset;
        [FoldoutGroup("Woman")] public List<Sprite> _Photos = new List<Sprite>();
    }
}