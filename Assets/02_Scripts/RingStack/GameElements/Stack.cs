using System;
using System.Collections.Generic;
using RingStack.GameElements;
using RingStack.Other;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RingStack.GameElements
{
    public class Stack : MonoBehaviour
    {
        public event Action DidStackChange;
        
        [SerializeField] private RingStackConfig _Config;
        
        [SerializeField, Range(0, 4)] private int _LevelRingCount;
        [Space]
        [SerializeField] private List<Ring> _Rings = new List<Ring>();
        [SerializeField] private List<Ring> _StaticRings = new List<Ring>();

        [ShowInInspector, ReadOnly] private bool _isInHarmony;
        
        public bool IsInHarmony => _isInHarmony;
        public Vector3 PullUpPosition { get; private set; }
        public Vector3 NewRingStackPosition => _StaticRings[RingsCount].transform.position;

        private int RingsCount => _Rings.Count;
        
        private void Start()
        {
            PullUpPosition = transform.position + _Config.StackPullUpOffset; 
            
            var inactiveRingCount = 0;
            for (var i = 0; i < _StaticRings.Count; i++)
            {
                if (i < _LevelRingCount)
                {
                    _Rings[i].Init(this, PullUpPosition);
                }
                else
                {
                    inactiveRingCount++;
                }
            }

            for (var i = 0; i < inactiveRingCount; i++)
            {
                _Rings.RemoveAt(_LevelRingCount);
            }

            CheckRingsHarmony();
        }

        public Ring TakeRing()
        {
            if (RingsCount <= 0) return null;
            var ring = _Rings[RingsCount - 1];
            ring.Taken();
            
            return ring;
        }
        
        public bool IsPlaceable(Material material)
        {
            bool value;
            
            if (RingsCount >= 4) return false;
            if (RingsCount > 0)
            {
                var topRing = _Rings[RingsCount - 1];
                value = topRing.Graphic.CurrentMaterial.Equals(material);
            }
            else
            {
                value = true;
            }

            return value;
        }

        public void AddNewRing(Ring ring)
        {
            _Rings.Add(ring);
            CheckRingsHarmony();
            
            DidStackChange?.Invoke();
        }

        public void RemoveRing(Ring ring)
        {
            _Rings.Remove(ring);
            CheckRingsHarmony();
        }

        public void SetGhostActive(bool value, Material material = null)
        {
            if (RingsCount >= 4) return;
            _StaticRings[RingsCount].Graphic.SetActive(value);
            _StaticRings[RingsCount].Graphic.SetGhostMaterial(material);
        }

        private void CheckRingsHarmony()
        {
            if (RingsCount >= 3)
            {
                var firstRingMaterial = _Rings[0].Graphic.CurrentMaterial;
                _isInHarmony = _Rings.TrueForAll(ring => ring.Graphic.CurrentMaterial == firstRingMaterial);
            }
            else if (RingsCount <= 0)
            {
                _isInHarmony = true;
            }
            else
            {
                _isInHarmony = false;
            }
        }
    }
}