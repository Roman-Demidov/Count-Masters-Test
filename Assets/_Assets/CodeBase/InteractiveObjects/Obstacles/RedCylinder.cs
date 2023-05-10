using UnityEngine;

namespace countMastersTest.interactiveObjects.obstacles
{
    public class RedCylinder : MonoBehaviour, IObstacle
    {
        [SerializeField, Range(0, 2)] private float _moveSpeed;
        [SerializeField] private Vector3 _point1;
        [SerializeField] private Vector3 _point2;

        Vector3 _pos1;
        Vector3 _pos2;

        private void Start()
        {
            _pos1 = _point1 + transform.position;
            _pos2 = _point2 + transform.position;
        }

        private void Update()
        {
            transform.position = Vector3.Lerp(_pos1, _pos2, Mathf.PingPong(Time.time * _moveSpeed, 1));
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.gray;
            Gizmos.DrawSphere(transform.position + _point1, 0.2f);
            Gizmos.DrawSphere(transform.position + _point2, 0.2f);
        }
    }
}
