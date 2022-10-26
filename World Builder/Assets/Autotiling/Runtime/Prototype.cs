using UnityEngine;

namespace Autotiling
{
    public class Prototype : MonoBehaviour
    {
        [SerializeField] private Vector3 _size = Vector3.one;
        [SerializeField] private Corner[] _corners = new Corner[4];

        public Vector3 Size => _size;
        public Corner[] Corners => _corners;

        private void Reset()
        {
            MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>();

            Bounds bounds = new Bounds(transform.position, Vector3.one);

            foreach (MeshRenderer childRenderer in renderers)
                bounds.Encapsulate(childRenderer.bounds);

            Vector3 scale = transform.lossyScale;
            _size = new Vector3(bounds.size.x / scale.x, bounds.size.y / scale.y, bounds.size.z / scale.z);
        }
    }
}