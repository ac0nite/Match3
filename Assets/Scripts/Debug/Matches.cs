using UnityEngine;

namespace Common.Debug
{
    public class Matches : MonoBehaviour
    {
        [SerializeField] private InputSystem _input;
        [SerializeField] private MatchGame _matchGame;
        private GridPosition _beginPositionGrid;
        private GridPosition _endPositionGrid;

        private void Start()
        {
            _input.PointerDown += TryToCheckSlot;
            _input.PointerDrag += TryTooNextCheckSlot;
        }

        private void OnDestroy()
        {
            _input.PointerDown -= TryToCheckSlot;
            _input.PointerDrag -= TryTooNextCheckSlot;
        }

        private void TryToCheckSlot(Vector3 worldPosition)
        {
            _beginPositionGrid = _matchGame.GetGridPositionByPointer(worldPosition);
            if (_matchGame.Checker.IsEmptySlot(_beginPositionGrid))
                _beginPositionGrid = GridPosition.Empty;
            
            UnityEngine.Debug.Log($"TryToCheckSlot {_beginPositionGrid.RowIndex} {_beginPositionGrid.ColumnIndex}");
            
            //Debug_matchGame.Slots[grid.RowIndex, grid.ColumnIndex]
        }
        
        private void TryTooNextCheckSlot(Vector3 worldPosition)
        {
            if (_beginPositionGrid.IsEmpty)
            {
                return;
            }
            
            _endPositionGrid = _matchGame.GetGridPositionByPointer(worldPosition);
            
            if(_matchGame.Checker.IsSlot(_endPositionGrid) && !_beginPositionGrid.IsUp(_endPositionGrid))
                UnityEngine.Debug.Log($"TryTooNextCheckSlot {_endPositionGrid.RowIndex} {_endPositionGrid.ColumnIndex}");
        }
    }
}