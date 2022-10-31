using Attributes;
using UnityEngine;
using Util;

namespace PointPathing
{
    public class PointPathMover : MonoBehaviour
    {
        private Vector3 Destination => _path[_index].Position;

        private IndexLooper _index;
        [SerializeField] private float _speed = 5f;
        [SerializeField] private float _turnSpeed = 4f;
        [SerializeField] private PointPather _path;
        
#if UNITY_EDITOR
        [ReadOnly]
#endif
        [SerializeField]
        private bool _isMoving;
        
        private void Start()
        {
            StartPathing();
        }

        public void StartPathing()
        {
            if(_path == null) return;
            if(_path.Length <= 1) return;

            _isMoving = true;
            _index = _path.Length;
            transform.position = Destination;
        }

        public void StopPathing()
        {
            _isMoving = false;
        }

        private void FixedUpdate()
        {
            if(!_isMoving) return;

            var pos = transform.position;
            var dest = Destination;
            var distance = Vector3.Distance(pos, dest);

            if(distance < _speed * Time.deltaTime * 1.1f)
            {
                _index.Next();
            }

            MoveToPoint();
        }

        private void MoveToPoint()
        {
            var pos = transform.position;
            var des = Destination;

            var direction = (des - pos).normalized;

            //  rotate towards point
            var rotFrom = transform.rotation;
            var rotTo = direction != Vector3.zero ? 
                Quaternion.LookRotation(direction) :
                rotFrom;

            //  rotate towards destination
            transform.rotation = Quaternion.RotateTowards(rotFrom, rotTo, _turnSpeed);

            //  move towards destination
            transform.Translate(direction * (_speed * Time.deltaTime), Space.World);
        }
    }
}