using DG.Tweening;
using Match3.Utils;
using UnityEngine;

namespace Match3.Environment
{
    public interface IAnimationEnvironment
    {
        public void Play();
        public void Stop();
    }
    
    public class AnimationEnvironment : MonoBehaviour, IAnimationEnvironment
    {
        [SerializeField] private Transform _baloon1;
        [SerializeField] private Transform _baloon2;

        [SerializeField] private float _durationMinTime;
        [SerializeField] private float _durationMaxTime;

        [SerializeField] private Spline _spline1;
        [SerializeField] private Spline _spline2;
        private Tweener _tweener1;
        private Tweener _tweener2;
        
        Vector3 tangent;

        private void Start()
        {
            _baloon1.transform.position = _spline1.GetPosition(0, out tangent);
            _baloon2.transform.position = _spline2.GetPosition(0, out tangent);
        }

        [ContextMenu("Play")]
        public void Play()
        {
            _baloon1.gameObject.SetActive(true);
            _baloon2.gameObject.SetActive(true);
            
            _tweener1 = DOVirtual
                .Float(0, 1f, Duration, t => _baloon1.transform.position = _spline1.GetPosition(t, out tangent))
                .SetLoops(100, LoopType.Yoyo);
            
            _tweener2 = DOVirtual
                .Float(0, 1f, Duration, t => _baloon2.transform.position = _spline2.GetPosition(t, out tangent))
                .SetLoops(100, LoopType.Yoyo);
        }

        [ContextMenu("Stop")]
        public void Stop()
        {
            _tweener1.Kill();
            _tweener2.Kill();
            
            _baloon1.gameObject.SetActive(false);
            _baloon2.gameObject.SetActive(false);
        }

        private float Duration => UnityEngine.Random.Range(_durationMinTime, _durationMaxTime);
    }
}