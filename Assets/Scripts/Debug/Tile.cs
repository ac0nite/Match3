using System.ComponentModel;
using UnityEngine;

namespace Common.Debug
{
    public class Tile : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _renderer;
        [SerializeField] private AnimationHelper _animation;
        public string ID { get; private set; }

        public void SetActive(bool active)
        {
            gameObject.SetActive(active);
        }

        public void Bind(Transform parent, int orderLayer)
        {
            transform.SetParent(parent, false);
            _renderer.sortingOrder = orderLayer;
        }

        public void UpdateOrder(int order)
        {
            _renderer.sortingOrder = order;
        }

        public Tile Initialise(IconSpriteModel spriteModel)
        {
            _animation.Initialise(spriteModel.AnimatorController);
            _animation.PlayIdle(0.4f, Random.Range(0f,3f));

            ID = spriteModel.Id;
            
            return this;
        }
    }
}