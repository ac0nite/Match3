using UnityEngine;

namespace Match3.Utils
{
    [ExecuteInEditMode]
    public class Spline : MonoBehaviour
    {
        [SerializeField] private Transform[] points;

        private Bezier _bezier;

        private void Awake()
        {
            _bezier = new Bezier(tension: 0.5f, points);
        }

        public Vector3 GetPosition(float t, out Vector3 tangent)
        {
#if !UNITY_LUNA
            if (points == null || points.Length < 3)
            {
                UnityEngine.Debug.LogError($"The points are not defined or not enough");
                tangent = default;
                return default;
            }

            if (_bezier == null) _bezier = new Bezier(tension: 0.5f, points);
#endif
            return _bezier.GetPointAtT(t, out tangent);
        }

#if UNITY_EDITOR
        [SerializeField] private int _numberOfPointsEditor;
        private Bezier _bezierEditor;
        private Vector3 _tangentEditor;
        private void OnDrawGizmos()
        {
            if (points == null) return;
            if (points.Length < 3) return;

            Vector3 prevPoint = points[0].position;

            if (_bezierEditor == null)
                _bezierEditor = new Bezier(tension: 0.5f, points);

            for (int i = 0; i < _numberOfPointsEditor; i++)
            {
                float t = (float) i / _numberOfPointsEditor;
                Vector3 point = _bezierEditor.GetPointAtT(t, out _tangentEditor);
                Gizmos.DrawLine(prevPoint, point);
                prevPoint = point;
            }
        }
#endif
    }
}