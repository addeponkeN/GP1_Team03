using UnityEngine;

namespace Util
{
    public static class GizmosExtended
    {
        public static void DrawLines(Transform[] points)
            => DrawLines(points, Color.white);

        public static void DrawLines(Transform[] points, Color color)
        {
            if(points is not { Length: > 1 }) return;

            Gizmos.color = color;

            for(int i = 0; i < points.Length - 1; i++)
            {
                var p1 = points[i];
                var p2 = points[i + 1];

                if(p1 == null || p2 == null)
                    continue;
                Gizmos.DrawLine(p1.position, p2.position);
            }

            Gizmos.color = Color.white;
        }

        public static void DrawLines(Vector3[] points)
            => DrawLines(points, Color.white);

        public static void DrawLines(Vector3[] points, Color color)
        {
            if(points is not { Length: > 1 }) return;

            Gizmos.color = color;

            for(int i = 0; i < points.Length - 1; i++)
            {
                var p1 = points[i];
                var p2 = points[i + 1];
                Gizmos.DrawLine(p1, p2);
            }

            Gizmos.color = Color.white;
        }
    }
}