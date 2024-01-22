using System;
using Board.Settings;
using Match3.Utils;
using UnityEngine;

namespace Board
{
    [Serializable]
    public struct BoardSize
    {
        public int Row;
        public int Column;
        public int Capacity => Row * Column;
    }
    public class BoardBounds
    {
        private readonly BoundsBoardSettingsSO _settings;
        private readonly float _spriteOriginalSize;
        private readonly BoardEdge _edge;

        private const float PixelPerUnit = 100;

        public BoardBounds(BoundsBoardSettingsSO settings, Camera camera)
        {
            _settings = settings;

            _spriteOriginalSize = _settings.SpritePixelSize/PixelPerUnit;

            var offsetPercentViewportX = DeviceUtils.IsTablet ? _settings.TabletOffsetHorizontalPercentX : 0f;
            _edge = new BoardEdge(camera, _settings.Top, _settings.Bottom, _settings.Edge, offsetPercentViewportX);
        }
        
        public void Calculate(BoardSize size)
        {
            float totalWidthX = _spriteOriginalSize * _settings.OffsetScale * size.Column;
            float totalWidthY = _spriteOriginalSize * _settings.OffsetScale * size.Row;

            float scaleX = _edge.BottomSize / totalWidthX;
            float scaleY = _edge.TopSize / totalWidthY;

            TileScale = scaleX < scaleY ? scaleX : scaleY; 
            
            TileSize = _spriteOriginalSize * _settings.OffsetScale * TileScale;

            float offsetX = scaleY < scaleX ? (_edge.BottomSize - TileSize * size.Column) * 0.5f : 0f;
            
            float startX = _edge.BottomLeft.x + TileSize * 0.5f + offsetX;
            float startY = _edge.BottomLeft.y + TileSize * 0.5f;
            
            OriginalPosition = new Vector3(startX, startY);
        }

        public Vector3 OriginalPosition { get; private set; }
        public Vector3 WorldPosition(int rowIndex, int columnIndex) => new Vector3(
            OriginalPosition.x + columnIndex * TileSize, 
            OriginalPosition.y + rowIndex * TileSize,
            0);
        public float TileSize { get; private set; }
        public float TileScale { get; private set; }
        public BoardEdge Edge => _edge;
    }
    
    [Serializable]
    public struct BoardEdge
    {
        public BoardEdge(Camera camera, float top, float bottom, float edge, float offsetHorizontalPercent = 0f)
        {
            var tabletOffsetX = (1 - edge) * offsetHorizontalPercent;
            BottomLeft = camera.ViewportToWorldPoint(new Vector3(edge + tabletOffsetX , bottom));
            BottomRight = camera.ViewportToWorldPoint(new Vector3(1 - edge - tabletOffsetX, bottom));
            TopLeft = camera.ViewportToWorldPoint(new Vector3(edge + tabletOffsetX, top));
            TopRight = camera.ViewportToWorldPoint(new Vector3(1 - edge - tabletOffsetX, top));
        }
        
        public Vector3 BottomLeft;
        public Vector3 BottomRight;
        public Vector3 TopLeft;
        public Vector3 TopRight;

        public float BottomSize => BottomRight.x - BottomLeft.x;
        public float TopSize => TopLeft.y - BottomRight.y;
    }
}