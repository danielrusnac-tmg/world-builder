using UnityEngine;

namespace Autotiling
{
    public class Prototype : MonoBehaviour
    {
        [SerializeField] private Vector3 _size = Vector3.one;
        [SerializeField] private Corner[] _corners = new Corner[4];

        public Vector3 Size => _size;
        public Corner[] Corners => _corners;
    }
}