using Board.Settings;
using UnityEngine;

namespace Board
{
    public class BoardTest : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private Vector2Int Size;
        [SerializeField] private GameObject _prefab;
        
        private BoardBounds _bounds;

        [Space] [SerializeField] private BoundsBoardSettingsSO boundsBoardSettings;
        private BoundsParam _param;

        private void Start()
        {
            _param = new BoundsParam(boundsBoardSettings, _camera);
            _param.Calculate(Size);
            
            for (int i = 0; i < Size.y; i++)
            {
                for (int j = 0; j < Size.x; j++)
                {
                    Vector3 spritePosition = new Vector3(_param.OriginalPosition.x + j * _param.TileSize, _param.OriginalPosition.y + i * _param.TileSize, 0);
                    var item = Instantiate(_prefab, spritePosition, Quaternion.identity, transform);
                    item.transform.localScale = Vector3.one * _param.TileScale;
                    item.GetComponent<SpriteRenderer>().sortingOrder = i + j;
                }
            }
            
            //PlaceSprites();
        }

        // private void PlaceSprites()
        // {
        //     float spriteSize = _prefab.GetComponent<SpriteRenderer>().bounds.size.x;
        //
        //     float offsetPercentViewportX = DeviceUtils.IsTablet ? boundsBoardSettings.TabletOffsetPercentViewportX : 0f;
        //     _bounds = new BoardBounds(_camera, boundsBoardSettings.Top, boundsBoardSettings.Bottom, boundsBoardSettings.Edge, offsetPercentViewportX);
        //
        //     float totalWidthX = spriteSize * Size.x;
        //     float totalWidthY = spriteSize * Size.y;
        //
        //     float scaleX = _bounds.BottomSize / totalWidthX;
        //     float scaleY = _bounds.TopSize / totalWidthY;
        //
        //     float scale = scaleX < scaleY ? scaleX : scaleY; 
        //     
        //     spriteSize *= scale;
        //
        //     float offsetX = scaleY < scaleX ? (_bounds.BottomSize - spriteSize * Size.x) * 0.5f : 0f;
        //     
        //     float startX = _bounds.BottomLeft.x + spriteSize * 0.5f + offsetX;
        //     float startY = _bounds.BottomLeft.y + spriteSize * 0.5f;
        //
        //     for (int i = 0; i < Size.y; i++)
        //     {
        //         for (int j = 0; j < Size.x; j++)
        //         {
        //             Vector3 spritePosition = new Vector3(startX + j * spriteSize, startY + i * spriteSize, 0) * OffsetPositionScale;
        //             var item = Instantiate(_prefab, spritePosition, Quaternion.identity, transform);
        //             item.transform.localScale = Vector3.one * scale;
        //         }
        //     }
        // }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if(_param == null) return;
            
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(_param.Bounds.BottomLeft, 0.2f);
            Gizmos.DrawSphere(_param.Bounds.BottomRight, 0.2f);
            Gizmos.DrawSphere(_param.Bounds.TopLeft, 0.2f);
            Gizmos.DrawSphere(_param.Bounds.TopRight, 0.2f);
        }  
#endif
    }
}