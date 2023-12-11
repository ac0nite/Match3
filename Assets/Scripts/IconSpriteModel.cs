using System;
using UnityEngine;

namespace Common
{
    [Serializable]
    public class IconSpriteModel
    {
        [SerializeField] private string _name;
        [SerializeField] private Sprite _icon;

        public string Name => _name;
        public Sprite Icon => _icon;
    }
}