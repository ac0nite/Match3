using System;
using Board.Settings;
using Match3.Utils;
using UnityEngine;

namespace Board
{
    public class BoundsParam
    {
        private readonly BoundsBoardSettingsSO _settings;
        private readonly float _spriteOriginalSize;
        private readonly BoardBounds _bounds;

        private const float PixelPerUnit = 100;

        public BoundsParam(BoundsBoardSettingsSO settings, Camera camera)
        {
            _settings = settings;

            _spriteOriginalSize = _settings.SpritePixelSize/PixelPerUnit;

            var offsetPercentViewportX = DeviceUtils.IsTablet ? _settings.TabletOffsetPercentViewportX : 0f;
            _bounds = new BoardBounds(camera, _settings.BoundsViewport, offsetPercentViewportX);
        }

        public void Calculate(Vector2Int size)
        {
            float totalWidthX = _spriteOriginalSize * _settings.OffsetPositionScale * size.x;
            float totalWidthY = _spriteOriginalSize * _settings.OffsetPositionScale * size.y;

            float scaleX = _bounds.BottomSize / totalWidthX;
            float scaleY = _bounds.TopSize / totalWidthY;

            TileScale = scaleX < scaleY ? scaleX : scaleY; 
            
            TileSize = _spriteOriginalSize * _settings.OffsetPositionScale * TileScale;

            float offsetX = scaleY < scaleX ? (_bounds.BottomSize - TileSize * size.x) * 0.5f : 0f;
            
            float startX = _bounds.BottomLeft.x + TileSize * 0.5f + offsetX;
            float startY = _bounds.BottomLeft.y + TileSize * 0.5f;
            
            OriginalPosition = new Vector3(startX, startY);
        }

        public Vector3 OriginalPosition { get; private set; }
        public float TileSize { get; private set; }
        public float TileScale { get; private set; }

        public BoardBounds Bounds => _bounds;
    }
    
    [Serializable]
    public struct BoardBounds
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
    public struct BoundsViewport
    {
        public float Top;
        public Vector2 BottomLeft;
    }
}