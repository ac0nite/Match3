using System;
using GabrielBigardi.SpriteAnimator;
using UnityEngine;

namespace Match3.Models
{
    [Serializable]
    public class TileModel
    {
        [SerializeField] private string _id;
        [SerializeField] private SpriteAnimationObject _spriteAnimation;

        public string Id => _id;
        public SpriteAnimationObject SpriteAnimation => _spriteAnimation;
    }
}