using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Common.Debug
{
    public class MatchGame : MonoBehaviour
    {
        [SerializeField] private int _row;
        [SerializeField] private int _column;
        [SerializeField] private float _tileSize = 0.6f;

        [SerializeField] private IconSpriteModel[] _spriteModels;
        [SerializeField] private Slot _slotPrefab;
        [SerializeField] private Tile tilePrefab;

        [SerializeField] private InputSystem _input;
        
        private Vector3 _originalPosition;
        public SlotPool SlotPool;
        public TilePool tilePool;
        public Checker Checker;
        public ShiftingTile Shifting;

        public Slot[,] BoardSlot;
        private IGameplay _gameplay;
        private IMatchingStrategy _matching;

        private void Start()
        {
            var capacity = _row * _column * 2;
            SlotPool = new SlotPool(_slotPrefab,capacity);
            tilePool = new TilePool(tilePrefab, capacity);
            BoardSlot = new Slot[_row, _column];
            Checker = new Checker(BoardSlot);
            Shifting = new ShiftingTile(this);
            _matching = new GeneralMatching(this);
            _gameplay = new Gameplay(this, _input, Shifting, _matching);

            CreateLevel();
            _gameplay.SetActive(true);
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
                    var slot = SlotPool.Get().Initialise(position, new GridPosition(i, j));
                    if (j != _column-1)
                    {
                        var item = tilePool.Get().Initialise(_spriteModels[index]);
                        slot = slot.SetItem(item);
                    }
                    slot.SetActive(true);

                    BoardSlot[i, j] = slot;
                }
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                for (int i = 0; i < _row; i++)
                {
                    for (int j = 0; j < _column; j++)
                    {
                        var slot = BoardSlot[i, j];
                        if(!slot.IsEmpty) 
                            tilePool.Put(slot.Tile);
                        SlotPool.Put(slot);
                    }
                }
            }
            else if (Input.GetKeyDown(KeyCode.U))
            {
                CreateLevel();
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
        
        public Vector3 GetWorldPosition(int rowIndex, int columnIndex)
        {
            return new Vector3(columnIndex, -rowIndex) * _tileSize + _originalPosition;
        }
        
        public Vector3 GetWorldPosition(GridPosition position)
        {
            return new Vector3(position.ColumnIndex, -position.RowIndex) * _tileSize + _originalPosition;
        }
    }
}