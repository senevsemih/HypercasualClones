using System.Collections.Generic;
using RingStack.GameElements;
using UnityEngine;

namespace RingStack.Controllers
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private List<Stack> _Stacks = new List<Stack>();

        private void Awake()
        {
            foreach (var stack in _Stacks)
            {
                stack.DidStackChange += StackOnDidStackChange;
            }
        }

        private void StackOnDidStackChange() => Debug.Log(_Stacks.TrueForAll(stack => stack.IsInHarmony)
            ? "Level Completed"
            : "Level not Completed");
    }
}