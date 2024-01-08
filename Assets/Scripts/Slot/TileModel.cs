using System;
using GabrielBigardi.SpriteAnimator;
using ID;
using UnityEngine;

namespace Match3.Models
{
    [Serializable]
    public class TileModel
    {
        [SerializeField] private string _id;
        [SerializeField] private SpriteAnimationObject _spriteAnimation;

        private UniqueID _uniqueID = null;
        public UniqueID ID
        {
            get { return _uniqueID ??= new UniqueID(_id); }
        }
        public SpriteAnimationObject SpriteAnimation => _spriteAnimation;
    }
}