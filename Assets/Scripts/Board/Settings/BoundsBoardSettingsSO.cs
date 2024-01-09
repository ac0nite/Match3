using UnityEngine;

namespace Board.Settings
{
    [CreateAssetMenu(menuName = "Gameplay/BoardSettings", fileName = "BoardSettingsSO_")]
    public class BoundsBoardSettingsSO : ScriptableObject
    {
        public float SpritePixelSize;
        public BoundsViewport BoundsViewport;
        public float OffsetPositionScale;
        public float TabletOffsetPercentViewportX;
    }
}