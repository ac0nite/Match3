using Cysharp.Threading.Tasks;
using Match3.Models;
using UnityEngine;
using Animator = Match3.Animation.Helper.Animator;

namespace Match3.General
{
    public class Tile : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _renderer;
        [SerializeField] private Animator _animation;
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

        public Tile Initialise(TileModel spriteModel)
        {
            _animation.Initialise(spriteModel.SpriteAnimation);
            _animation.PlayIdle(true);

            ID = spriteModel.Id;
            
            return this;
        }

        public UniTask PlayDestroyAnimationAsync()
        {
            var utcs = new UniTaskCompletionSource();
            _animation.PlayDestroy(() => utcs.TrySetResult());
            return utcs.Task;
        }
    }
}