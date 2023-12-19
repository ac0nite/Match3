using System;
using Cysharp.Threading.Tasks;
using GabrielBigardi.SpriteAnimator;
using Match3.Animation.Id;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Match3.Animation.Helper
{
    public class Animator : MonoBehaviour
    {
        [SerializeField] private SpriteAnimator _spriteAnimator;

        public void Initialise(SpriteAnimationObject spriteAnimation)
        {
            _spriteAnimator.ChangeAnimationObject(spriteAnimation);
        }

        public void PlayIdle(bool isBeginFrameRandom)
        {
            var frame = isBeginFrameRandom
                ? Random.Range(0, _spriteAnimator.GetAnimationByName(AnimationId.Idle).Frames.Count)
                : 0;

            _spriteAnimator.Play(AnimationId.Idle, frame);
        }

        public void PlayDestroy(Action callback)
        {
            _spriteAnimator.Play(AnimationId.Destroy);
            _spriteAnimator.OnComplete(callback);
        }
    }
}