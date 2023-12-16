using System;
using UnityEngine;

namespace Match3.Models
{
    [Serializable]
    public class TileModel
    {
        [SerializeField] private string _id;
        [SerializeField] private RuntimeAnimatorController _animatorController;

        public string Id => _id;
        public RuntimeAnimatorController AnimatorController => _animatorController;
    }
}