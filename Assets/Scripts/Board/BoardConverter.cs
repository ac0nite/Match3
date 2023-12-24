using System;
using System.Collections.Generic;
using Match3.General;
using Match3.Slots;
using Newtonsoft.Json;
using UnityEngine;

namespace Match3.Board
{
    public class BoardConverter
    {
        public static string ToJson(Slot[] slots)
        {
            BoardDetails details = new BoardDetails();
            details.SlotSerializables = new SlotSerializable[slots.Length];

            for (int i = 0; i < slots.Length; i++)
            {
                details.SlotSerializables[i] = slots[i].ToSerializable();
            }

            string json = String.Empty;
            // try
            // {
            //     json = JsonConvert.SerializeObject(details);
            // }
            // catch (Exception e)
            // {
            //     Console.WriteLine(e);
            //     throw;
            // }
            
            ToDetails(json);

            return json;
        }

        public static BoardDetails ToDetails(string json)
        {
            try
            {
                return JsonConvert.DeserializeObject<BoardDetails>(json);
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogWarning($"FAILED TO DESERIALIZE! [{e.Message}]");
                return null;
            }
        }
    }


    [Serializable]
    public class BoardDetails
    {
        [JsonProperty("board")] public SlotSerializable[] SlotSerializables;
    }
}