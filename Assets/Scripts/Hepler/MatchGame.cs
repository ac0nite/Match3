using Debug;
using UnityEngine;

namespace Common.Debug
{
    public class MatchGame : MonoBehaviour
    {
        [SerializeField] private ApplicationContext _applicationContext;
        private IBoardRenderer _boardRenderer;
        private IGameplay _gamePlay;
        private IStateMachine<BaseState> _stateMachine;

        private void Awake()
        {
            _applicationContext.Construct();
        }

        private void Start()
        {
            _stateMachine = _applicationContext.Resolve<IStateMachine<BaseState>>();
            _stateMachine.Next<InitialiseState>();

            // _boardRenderer = _applicationContext.Resolve<IBoardRenderer>();
            // _boardRenderer.Create();
            // _boardRenderer.Random();
            //
            // _gamePlay = _applicationContext.Resolve<IGameplay>();
            // _gamePlay.SetActive(true);
        }

        // private void CreateLevel()
        // {
        //     _originalPosition = GetOriginPosition(_row, _column);
        //     _board.Initialise(_row, _column);
        //
        //     for (int i = 0; i < _row; i++)
        //     {
        //         for (int j = 0; j < _column; j++)
        //         {
        //             // var position = GetWorldPosition(i, j);
        //             //var slot = SlotPool.Get().Initialise(position, new GridPosition(i, j));
        //             
        //             var slot = SlotPool.Get();
        //             slot.Initialise(_board, i, j);
        //             slot.SetGridPosition(new GridPosition(i, j));
        //             slot.SetForceWorldPosition(GetWorldPosition(i, j));
        //             slot.SetTile(GetPoolTile());
        //             
        //             // if (j != _column-1)
        //             // {
        //             //     var item = tilePool.Get().Initialise(_spriteModels[index]);
        //             //     slot = slot.SetItem(item);
        //             // }
        //             
        //             // var index = UnityEngine.Random.Range(0, _spriteModels.Length);
        //             // var item = tilePool.Get().Initialise(_spriteModels[index]);
        //
        //             // var item = GetPoolTile();
        //             //
        //             // slot = slot.SetTile(item);
        //             
        //             slot.SetActive(true);
        //
        //             BoardSlot[i, j] = slot;
        //         }
        //     }
        // }
        //
        // private Tile GetPoolTile()
        // {
        //     var index = UnityEngine.Random.Range(0, _spriteModels.Length);
        //     return tilePool.Get().Initialise(_spriteModels[index]);
        // }

        // private void Update()
        // {
        //     if (Input.GetKeyDown(KeyCode.C))
        //     {
        //         for (int i = 0; i < _row; i++)
        //         {
        //             for (int j = 0; j < _column; j++)
        //             {
        //                 var slot = BoardSlot[i, j];
        //                 if(!slot.IsEmpty) 
        //                     tilePool.Put(slot.Tile);
        //                 SlotPool.Put(slot);
        //             }
        //         }
        //     }
        //     else if (Input.GetKeyDown(KeyCode.U))
        //     {
        //         CreateLevel();
        //     }
        //     else if (Input.GetKeyDown(KeyCode.G))
        //     {
        //         Shifting.AllShift();
        //     }
        // }

        // private Vector3 GetOriginPosition(int rowCount, int columnCount)
        // {
        //     var offsetY = Mathf.Floor(rowCount / 2.0f) * _tileSize;
        //     var offsetX = Mathf.Floor(columnCount / 2.0f) * _tileSize;
        //
        //     return new Vector3(-offsetX, offsetY);
        // }
        //
        // public GridPosition GetGridPositionByPointer(Vector3 worldPointerPosition)
        // {
        //     var rowIndex = (worldPointerPosition - _originalPosition).y / _tileSize;
        //     var columnIndex = (worldPointerPosition - _originalPosition).x / _tileSize;
        //
        //     return new GridPosition(Convert.ToInt32(-rowIndex), Convert.ToInt32(columnIndex));
        // }
        //
        // public Vector3 GetWorldPosition(int rowIndex, int columnIndex)
        // {
        //     return new Vector3(columnIndex, -rowIndex) * _tileSize + _originalPosition;
        // }
        //
        // public Vector3 GetWorldPosition(GridPosition position)
        // {
        //     return new Vector3(position.ColumnIndex, -position.RowIndex) * _tileSize + _originalPosition;
        // }
    }
}