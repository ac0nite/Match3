using UnityEngine;
using UnityEngine.Serialization;

namespace Board.Settings
{
    [CreateAssetMenu(menuName = "Gameplay/BoardSettings", fileName = "BoardSettingsSO_")]
    public class BoundsBoardSettingsSO : ScriptableObject
    {
        public float SpritePixelSize;
        // public BoundsViewport BoundsViewport;
        [Range(0,1)] public float Top;
        [Range(0,1)] public float Bottom;
        [Range(0,1)] public float Edge;
        [Range(0,1)] public float OffsetScale;
        [Range(0,0.5f)] public float TabletOffsetHorizontalPercentX;
    }
}