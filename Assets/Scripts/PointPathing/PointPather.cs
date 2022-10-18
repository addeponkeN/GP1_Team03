using System;
using Unity.Collections;
using UnityEngine;
using Util;

namespace PointPathing
{
    public class PointPather : MonoBehaviour
    {
        public Transform this[int i] => _points[i];
        public int Length => _points.Length;

        private Transform[] _points;

        [Header("Takes all Child Objects\n" +
                "and creates a path\n" +
                "in the order of the children.")]
        [SerializeField, ReadOnly]
        private string _notes;

        [SerializeField] private float _radius = 0.35f;
        [SerializeField] private bool _alwaysDraw = true;

        private void Awake()
        {
            _points = transform.GetComponentsInChildren<Transform>();
        }

#if UNITY_EDITOR

        private void DrawPoints()
        {
            //  boo
            //  only when in the editor! its ok.
            _points = transform.GetComponentsInChildren<Transform>();
            
            GizmosExtended.DrawLines(_points, Color.yellow);
            for(int i = 0; i < _points.Length; i++)
            {
                Gizmos.DrawWireSphere(_points[i].position, _radius);
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