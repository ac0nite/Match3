using DG.Tweening;
using Match3.Animation.Id;
using UnityEngine;

namespace Match3.Animation.Helper
{
    public delegate void DestroyEndedAnimationDelegate();
    public class AnimationHelper : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        public DestroyEndedAnimationDelegate OnDestroyEndedAnimationDelegate;

        public void Initialise(RuntimeAnimatorController controller)
        {
            _animator.runtimeAnimatorController = controller;
            _animator.enabled = true;
        }

        public void Dispose()
        {
            _animator.enabled = false;
            _animator.runtimeAnimatorController = null;
        }

        public void PlayIdle(float speed, float delay)
        {
            //UnityEngine.Debug.Log($"PlayIdle! {speed} {delay}");
            
            DOVirtual.DelayedCall(delay, () =>
            {
                PlayAnimation(AnimationId.Idle, speed);
            });
        }

        public void PlayDestroy(float speed)
        {
            PlayAnimation(AnimationId.Destroy, speed);
        }

        private void PlayAnimation(int id, float speed)
        {
            _animator.SetTrigger(id);
            _animator.SetFloat(AnimationId.Speed, speed);
        }

        private void DestroyEndAnimation()
        {
            OnDestroyEndedAnimationDelegate?.Invoke();
        }
    }
}