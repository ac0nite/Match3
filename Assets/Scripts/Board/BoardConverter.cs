using System;
using System.Collections.Generic;
using Match3.General;
using Newtonsoft.Json;
using UnityEngine;

namespace Match3.Board
{
    public class BoardConverter
    {
        public static string ToJson(Slot[,] slots)
        {
            BoardDetails board = new BoardDetails()
            {
                size = new Vector2(slots.GetLength(0), slots.GetLength(1)),
                slots = new List<SlotDetails>(),
            };

            foreach (var slot in slots)
            {
                board.slots.Add(new SlotDetails()
                {
                    grid = new Vector2(slot.Position.RowIndex, slot.Position.ColumnIndex),
                    id = slot.IsEmpty ? String.Empty : slot.Tile.ID
                });
            }

            string json = JsonConvert.SerializeObject(board);
            ToDetails(json);

            return json;
        }

        public static BoardDetails ToDetails(string json)
        {
            // BoardDetails details;
            try
            {
                return JsonConvert.DeserializeObject<BoardDetails>(json);
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogWarning($"FAILED TO DESERIALIZE! [{e.Message}]");
                return null;
            }

            // return details;
        }
    }
    
    [Serializable]
    public struct SlotDetails
    {
        public Vector2 grid;
        public string id;
    }

    [Serializable]
    public struct SlotDetailsX
    {
        public Vector2 grid;
        public string id;
    }
    
    [Serializable]
    public struct BoardDetailsX
    {
        public Vector2 size;
        public List<SlotDetailsX> fff;

        // public BoardDetailsX()
        // {
        //     fff = new List<Vector3>();
        //     size = Vector2.negativeInfinity;
        // }
        // public List<SlotDetails> param;
        //
        // public BoardDetails()
        // {
        //     param = new List<SlotDetails>();
        // }
    }
    

    [Serializable]
    public class BoardDetails
    {
        public Vector2 size;
        public List<SlotDetails> slots;
    }
}