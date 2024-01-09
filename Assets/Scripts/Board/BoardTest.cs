using System;
using Board.Settings;
using Match3.Utils;
using UnityEngine;
using UnityEngine.Serialization;

namespace Board
{
    public class BoardTest : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private BoundsViewport _boundsViewport;
        [SerializeField] private Vector2Int Size;
        [SerializeField, Range(0,2)] private float OffsetPositionScale;
        [SerializeField, Range(0, 0.5f)] private float _tabletOffsetPercentViewportX; 
        [SerializeField] private GameObject _prefab;
        
        private BoardBounds _bounds;

        [FormerlySerializedAs("BoardSettings")] [Space] [SerializeField] private BoundsBoardSettingsSO boundsBoardSettings;
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

        private void PlaceSprites()
        {
            float spriteSize = _prefab.GetComponent<SpriteRenderer>().bounds.size.x;

            float offsetPercentViewportX = DeviceUtils.IsTablet ? _tabletOffsetPercentViewportX : 0f;
            _bounds = new BoardBounds(_camera, _boundsViewport, offsetPercentViewportX);

            float totalWidthX = spriteSize * Size.x;
            float totalWidthY = spriteSize * Size.y;

            float scaleX = _bounds.BottomSize / totalWidthX;
            float scaleY = _bounds.TopSize / totalWidthY;

            float scale = scaleX < scaleY ? scaleX : scaleY; 
            
            spriteSize *= scale;

            float offsetX = scaleY < scaleX ? (_bounds.BottomSize - spriteSize * Size.x) * 0.5f : 0f;
            
            float startX = _bounds.BottomLeft.x + spriteSize * 0.5f + offsetX;
            float startY = _bounds.BottomLeft.y + spriteSize * 0.5f;

            for (int i = 0; i < Size.y; i++)
            {
                for (int j = 0; j < Size.x; j++)
                {
                    Vector3 spritePosition = new Vector3(startX + j * spriteSize, startY + i * spriteSize, 0) * OffsetPositionScale;
                    var item = Instantiate(_prefab, spritePosition, Quaternion.identity, transform);
                    item.transform.localScale = Vector3.one * scale;
                }
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(_param.Bounds.BottomLeft, 0.2f);
            Gizmos.DrawSphere(_param.Bounds.BottomRight, 0.2f);
            Gizmos.DrawSphere(_param.Bounds.TopLeft, 0.2f);
            Gizmos.DrawSphere(_param.Bounds.TopRight, 0.2f);
        }  
#endif
    }
}