using System;
using UnityEngine;

namespace Common
{
    [Serializable]
    public class IconSpriteModel
    {
        [SerializeField] private string _id;
        [SerializeField] private RuntimeAnimatorController _animatorController;

        public string Id => _id;
        public RuntimeAnimatorController AnimatorController => _animatorController;
    }
}