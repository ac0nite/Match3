using System;
using UnityEngine;

namespace Common
{
    [Serializable]
    public class IconSpriteModel
    {
        [SerializeField] private string _id;
        [SerializeField] private Sprite _icon;

        public string Id => _id;
        public Sprite Icon => _icon;
    }
}