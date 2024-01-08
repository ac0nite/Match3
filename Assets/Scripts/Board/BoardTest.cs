using System;
using Match3.Utils;
using UnityEngine;

namespace Board
{
    [Serializable]
    struct BoardBounds
    {
        public BoardBounds(Camera camera, BoundsViewport viewport, float offsetPercentViewportX = 0f)
        {
            var tabletOffsetX = (1 - viewport.BottomLeft.x) * offsetPercentViewportX;
            BottomLeft = camera.ViewportToWorldPoint(new Vector3(viewport.BottomLeft.x + tabletOffsetX , viewport.BottomLeft.y));
            BottomRight = camera.ViewportToWorldPoint(new Vector3(1 - viewport.BottomLeft.x - tabletOffsetX, viewport.BottomLeft.y));
            TopLeft = camera.ViewportToWorldPoint(new Vector3(viewport.BottomLeft.x + tabletOffsetX, viewport.Top));
            TopRight = camera.ViewportToWorldPoint(new Vector3(1 - viewport.BottomLeft.x - tabletOffsetX, viewport.Top));
        }
        
        public Vector3 BottomLeft;
        public Vector3 BottomRight;
        public Vector3 TopLeft;
        public Vector3 TopRight;

        public float BottomSize => BottomRight.x - BottomLeft.x;
        public float TopSize => TopLeft.y - BottomRight.y;
    }

    [Serializable]
    struct BoundsViewport
    {
        public float Top;
        public Vector2 BottomLeft;
    }
    
    public class BoardTest : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private BoundsViewport _boundsViewport;
        [SerializeField] private Vector2Int Size;
        [SerializeField, Range(0,2)] private float OffsetPositionScale;
        [SerializeField, Range(0, 0.5f)] private float _tabletOffsetPercentViewportX; 
        [SerializeField] private GameObject _prefab;
        
        private BoardBounds _bounds;
        private BoardBounds _boundsScreen;

        private void Start()
        {
            PlaceSprites();
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
            Gizmos.DrawSphere(_bounds.BottomLeft, 0.2f);
            Gizmos.DrawSphere(_bounds.BottomRight, 0.2f);
            Gizmos.DrawSphere(_bounds.TopLeft, 0.2f);
            Gizmos.DrawSphere(_bounds.TopRight, 0.2f);
        }  
#endif
    }
}