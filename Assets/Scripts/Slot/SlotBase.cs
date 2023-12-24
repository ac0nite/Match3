using System;
using ID;
using Match3.General;
using Newtonsoft.Json;
using UnityEngine;
using Animator = Match3.Animation.Animator;


namespace Match3.Slots
{
    public abstract class SlotBase : MonoBehaviour
    {
        [SerializeField] protected Animator _animator;
        public abstract UniqueID ID { get; set; }
        public abstract GridPosition Position { get; set; }
        public abstract bool IsMatch { get; set; }

        public SlotSerializable ToSerializable()
        {
            return new SlotSerializable()
            {
                ID = ID,
                Position = Position
            };
        }
    }

    [Serializable]
    public struct SlotSerializable
    {
        [JsonProperty("grid")] public GridPosition Position;
        [JsonProperty("id")] public UniqueID ID;
    }
}