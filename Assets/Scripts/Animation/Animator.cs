using System;
using Cysharp.Threading.Tasks;
using GabrielBigardi.SpriteAnimator;
using Match3.Animation.Id;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Match3.Animation
{
    public interface IAnimator
    {
        void Initialise(SpriteAnimationObject animationObject);
        int SortingOrder { get; set; }
        void PlayIdle(bool isBeginFrameRandom);
        void PlayDestroy(Action callback);
        UniTask PlayDestroyAsync();
        void Stop();
        bool IsEmpty { get; }
    }
    public class Animator : SpriteAnimator, IAnimator
    {
        public void Initialise(SpriteAnimationObject animationObject)
        {
            Debug.Log($"Initialise", transform);
            ChangeAnimationObject(animationObject);
        }

        public int SortingOrder
        {
            get => _spriteRenderer.sortingOrder;
            set => _spriteRenderer.sortingOrder = value;
        }

        public void PlayIdle(bool isBeginFrameRandom)
        {
            var frame = isBeginFrameRandom
                ? Random.Range(0, GetAnimationByName(AnimationId.Idle).Frames.Count)
                : 0;

            Play(AnimationId.Idle, frame);
        }

        public void PlayDestroy(Action callback)
        {
            Play(AnimationId.Destroy);
            OnComplete(callback);
        }

        public UniTask PlayDestroyAsync()
        {
            var utcs = new UniTaskCompletionSource();
            PlayDestroy(() => utcs.TrySetResult());
            return utcs.Task; 
        }

        public void Stop()
        {
            Pause();
            //_currentAnimation = null;
            //ChangeAnimationObject(null);
            Debug.Log($"Stop", transform);
        }

        public bool IsEmpty { get => _currentAnimation == null; }
    }
}