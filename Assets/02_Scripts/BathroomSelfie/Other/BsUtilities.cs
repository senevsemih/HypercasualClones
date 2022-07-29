using System.Collections.Generic;
using UnityEngine;

namespace BathroomSelfie.Other
{
    public class BsUtilities : MonoBehaviour
    {
        private static int? _lastRandomIndex;

        public static int RandomInList(int listCount)
        {
            var indexes = new List<int>();
            for (var i = 0; i < listCount; i++)
            {
                indexes.Add(i);
            }

            indexes.Remove(_lastRandomIndex.HasValue ? _lastRandomIndex.Value : 0);
            var random = Random.Range(0, indexes.Count);

            _lastRandomIndex = indexes[random];
            return indexes[random];
        }

        public static Direction RandomDirection()
        {
            var d = Direction.Left;
            var randomIndex = Random.Range(0, 4);

            d = randomIndex switch
            {
                0 => Direction.Up,
                1 => Direction.Left,
                2 => Direction.Down,
                3 => Direction.Right,
                _ => d
            };
            return d;
        }
    }
}