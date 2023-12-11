using UnityEngine;

namespace Common
{
    public interface IGameplayBoardRenderer
    {
        void Construct(IconSpriteModel[] spriteModels);
        void Initialise();
    }
    public class GameplayBoard : MonoBehaviour, IGameplayBoardRenderer
    {
        [SerializeField] private int _rowCount;
        [SerializeField] private int _columnCount;
        [SerializeField] private float _tileSize;
        [SerializeField] private GridSlot _gridSlotPrefab;
        
        private IconSpriteModel[] _sprites;
        private ISlotGenerator _slotGenerator;

        public void Construct(IconSpriteModel[] spriteModels)
        {
            _sprites = spriteModels;
            _slotGenerator = new SlotGenerator(_gridSlotPrefab, _rowCount * _columnCount);
        }

        public void Initialise()
        {
            _slotGenerator.Allocate(4);
        }
    }
}