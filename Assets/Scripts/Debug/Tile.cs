using System.ComponentModel;
using UnityEngine;

namespace Common.Debug
{
    public class Tile : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _renderer;
        public string ID { get; private set; }

        public void SetActive(bool active)
        {
            gameObject.SetActive(active);
            _renderer.enabled = active;
        }

        public void SetColor(Color color)
        {
            _renderer.color = color;
        }

        public Tile Initialise(IconSpriteModel spriteModel)
        {
            _renderer.sprite = spriteModel.Icon;
            ID = spriteModel.Id;
            return this;
        }
    }
}