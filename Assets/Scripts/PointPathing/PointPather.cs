using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using Util;

namespace PointPathing
{
    public struct PathPoint
    {
        public Vector3 Position;
        public Vector3 Facing;

        public PathPoint(Vector3 position, Vector3 facing)
        {
            Position = position;
            Facing = facing;
        }
    }

    public class PointPather : MonoBehaviour
    {
        public PathPoint this[int i] => _finalPoints[i];
        public int Length => _finalPoints.Count;

        private List<PathPoint> _finalPoints = new();

        [Header("Takes all Child Objects\n" +
                "and creates a path\n" +
                "in the order of the children.")]
        [SerializeField, ReadOnly]
        private string _notes;

        [SerializeField] private float _interval = .5f;
        [SerializeField] private float _radius = 0.35f;
        [SerializeField] private bool _alwaysDraw = true;

        private void Start()
        {
            var childPoints = transform.GetComponentsInChildren<Transform>();
            _finalPoints.Clear();

            for(int i = 0; i < childPoints.Length - 1; i++)
            {
                var p1 = childPoints[i].position;
                var p2 = childPoints[i + 1].position;

                var distance = Vector3.Distance(p1, p2);

                int steps = (int)(distance / _interval);

                var direction = (p2 - p1).normalized;

                int mask = 1 << 8;
                
                for(int j = 0; j <= steps; j++)
                {
                    var step = _interval * j;
                    var checkPos = p1 + new Vector3(0, 3f, 0) + direction * step;
                    var ray = new Ray(checkPos, -Vector3.up);

                    if(!Physics.Raycast(ray, out var info, 999f, mask))
                        continue;

                    _finalPoints.Add(new PathPoint(info.point, Vector3.zero));
                }
            }
        }

#if UNITY_EDITOR

        private void DrawPoints()
        {
            //  boo
            //  only when in the editor! its ok.
            var childPoints = transform.GetComponentsInChildren<Transform>();

            GizmosExtended.DrawLines(childPoints, Color.yellow);
            for(int i = 0; i < _finalPoints.Count; i++)
            {
                Gizmos.DrawWireSphere(_finalPoints[i].Position, _radius);
            }
        }

        private void OnDrawGizmos()
        {
            if(!_alwaysDraw) return;
            DrawPoints();
        }

        private void OnDrawGizmosSelected()
        {
            if(_alwaysDraw) return;
            DrawPoints();
        }

#endif
    }
}