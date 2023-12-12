using System;
using UnityEngine;

namespace Common.Debug
{
    public class MatchGame : MonoBehaviour
    {
        [SerializeField] private int _row;
        [SerializeField] private int _column;
        [SerializeField] private float _tileSize = 0.6f;

        [SerializeField] private IconSpriteModel[] _spriteModels;
        [SerializeField] private Slot _slotPrefab;
        [SerializeField] private Item _itemPrefab;
        
        private Vector3 _originalPosition;
        public SlotPool SlotPool;
        public ItemPool ItemPool;
        public Checker Checker;

        private Slot[,] _slotGrid;

        private void Start()
        {
            var capacity = _row * _column * 2;
            SlotPool = new SlotPool(_slotPrefab,capacity);
            ItemPool = new ItemPool(_itemPrefab, capacity);
            _slotGrid = new Slot[_row, _column];
            Checker = new Checker(_slotGrid);
            
            CreateLevel();
        }

        private void CreateLevel()
        {
            _originalPosition = GetOriginPosition(_row, _column);

            for (int i = 0; i < _row; i++)
            {
                for (int j = 0; j < _column; j++)
                {
                    var position = GetWorldPosition(i, j);
                    var index = UnityEngine.Random.Range(0, _spriteModels.Length);
                    var item = ItemPool.Get().Initialise(_spriteModels[index]);
                    var slot = SlotPool.Get().Initialise(position, new GridPosition(i, j));
                    if (j != _column-1)
                    {
                        slot = slot.SetItem(item);
                    }
                    slot.SetActive(true);

                    _slotGrid[i, j] = slot;
                }
            }
        }
        
        private Vector3 GetOriginPosition(int rowCount, int columnCount)
        {
            var offsetY = Mathf.Floor(rowCount / 2.0f) * _tileSize;
            var offsetX = Mathf.Floor(columnCount / 2.0f) * _tileSize;

            return new Vector3(-offsetX, offsetY);
        }
        
        public GridPosition GetGridPositionByPointer(Vector3 worldPointerPosition)
        {
            var rowIndex = (worldPointerPosition - _originalPosition).y / _tileSize;
            var columnIndex = (worldPointerPosition - _originalPosition).x / _tileSize;

            return new GridPosition(Convert.ToInt32(-rowIndex), Convert.ToInt32(columnIndex));
        }
        
        private Vector3 GetWorldPosition(int rowIndex, int columnIndex)
        {
            return new Vector3(columnIndex, -rowIndex) * _tileSize + _originalPosition;
        }
    }
}