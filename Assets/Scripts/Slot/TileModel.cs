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
            get
            {
                Debug.Log($"{_uniqueID} == {null}");
                var id0 = new UniqueID("111");
                var id2 = new UniqueID("222");
                var t = id0 == id2;
                if (_uniqueID == null)
                    _uniqueID = new UniqueID(_id);

                return _uniqueID;
            }
        }
        public SpriteAnimationObject SpriteAnimation => _spriteAnimation;
    }
}