using UnityEngine;

namespace Match3.Utils
{
    public class Bezier
    {
        private readonly Transform[] P;
        
        /// Натяжение сплайна
        float _tension;
        
        private int _len;
        private int _lenMinusOne;
        
        private int _segmentIndex;
        private float _localT;
        private int i0, i1, i2, i3;
        private Vector3 p0, p1, p2, p3;
        public Bezier(float tension = 0.5f, params Transform[] points)
        {
            P = points;
            _tension = tension;
            _len = points.Length;
            _lenMinusOne = _len - 1;
        }

        public Vector3 GetPointAtT(float t, out Vector3 tangent)
        {
            _segmentIndex = Mathf.FloorToInt(t * _lenMinusOne);
            _localT = (t * _lenMinusOne) - _segmentIndex;
            
            i0 = Mathf.Clamp(_segmentIndex - 1, 0, _lenMinusOne);
            i1 = Mathf.Clamp(_segmentIndex, 0, _lenMinusOne);
            i2 = Mathf.Clamp(_segmentIndex + 1, 0, _lenMinusOne);
            i3 = Mathf.Clamp(_segmentIndex + 2, 0, _lenMinusOne);

            p0 = P[i0].position;
            p1 = P[i1].position;
            p2 = P[i2].position;
            p3 = P[i3].position;

            tangent = GetCatmullRomTangent(_localT, p0, p1, p2, p3);
            return GetCatmullRomPosition(_localT, p0, p1, p2, p3);
        }
        
        private Vector3 GetCatmullRomPosition(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
        {
            return _tension * 
                   ((2 * p1) + 
                    (-p0 + p2) * t + 
                    (2 * p0 - 5 * p1 + 4 * p2 - p3) * (t * t) + 
                    (-p0 + 3 * p1 - 3 * p2 + p3) * (t * t * t));
        }
        
        private Vector3 GetCatmullRomTangent(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
        {
            return (
                _tension *
                ((-p0 + p2) +
                 (2 * p0 - 5 * p1 + 4 * p2 - p3) * (2 * t) +
                 (-p0 + 3 * p1 - 3 * p2 + p3) * (3 * (t * t)))
                ).normalized;
        }
    }
}