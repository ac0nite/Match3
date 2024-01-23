using System;
using System.Collections.Generic;
using Board;
using Match3.General;
using Match3.Models;
using Newtonsoft.Json;

namespace Match3.Board
{
    public class BoardConverter
    {
        public static string ToJson(IBoardModel board)
        {
            BoardSerialize serialize = new BoardSerialize();
            serialize.Size = (board.Size.Row << 4) + board.Size.Column;
            serialize.Slots = new Dictionary<string, ulong>();
            
            foreach (Slot slot in board.Slots)
            {
                if(slot.IsEmpty) continue;
                if(serialize.Slots.ContainsKey(slot.ID)) 
                    serialize.Slots[slot.ID] = (serialize.Slots[slot.ID] << 8) + (ulong)((slot.Position.RowIndex << 4) + slot.Position.ColumnIndex);
                else
                    serialize.Slots[slot.ID] = (ulong)((slot.Position.RowIndex << 4) + slot.Position.ColumnIndex);

                // var t = details.Slots[slot.ID];
                // Debug.Log($"*[{slot.ID}] [{(t >> 4) & 0xF}, {t & 0xF}]");
            }
            
            foreach (var pair in board.Counter)
            {
                serialize.Slots[pair.Key] = (serialize.Slots[pair.Key] << 0x8) + (ulong)pair.Value;
                // Debug.Log($"c[{pair.Key}] = [{details.Slots[pair.Key] & 0xFF}]");
            }

            // BoardDetails details = new BoardDetails();
            // details.SlotSerializables = new SlotSerializable[slots.Length];
            //
            // for (int i = 0; i < slots.Length; i++)
            // {
            //     details.SlotSerializables[i] = slots[i].ToSerializable();
            // }

            string json = String.Empty;
            try
            {
                json = JsonConvert.SerializeObject(serialize);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
            //Debug.Log(json);
            
            // BoardSerialize deserialize;
            // try
            // {
            //     deserialize = JsonConvert.DeserializeObject<BoardSerialize>(json);
            // }
            // catch (Exception e)
            // {
            //     Console.WriteLine(e);
            //     throw;
            // }
            //
            // BoardSize size = new BoardSize() {Row = deserialize.Size >> 4 & 0xF, Column = deserialize.Size & 0xF};
            // Slot[] slots = new Slot[size.Capacity];
            // foreach (var pair in deserialize.Slots)
            // {
            //     var count = (int)pair.Value & 0xFF;
            //     var value = pair.Value >> 8;
            //     for (int i = 0; i < count; i++)
            //     {
            //         var columnIndex = value & 0xF;
            //         var rowIndex = (value >> 4) & 0xF;
            //         Debug.Log($"[{pair.Key}] [{rowIndex}, {columnIndex}]");
            //         value >>= 8;
            //     }
            // }

            // ToDetails(json);

            return json;
        }

        public static BoardConfig ToDetails(string json)
        {
            BoardSerialize boardSerialize = null;
            try
            {
                boardSerialize = JsonConvert.DeserializeObject<BoardSerialize>(json);
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogWarning($"FAILED TO DESERIALIZE! [{e.Message}]");
                return null;
            }

            BoardConfig config = new BoardConfig();
            config.Size = new BoardSize()
            {
                Row = boardSerialize.Size >> 4 & 0xF, 
                Column = boardSerialize.Size & 0xF
            };

            config.Slots = new Dictionary<string, GridPosition[]>(boardSerialize.Slots.Count);
            
            foreach (var pair in boardSerialize.Slots)
            {
                var count = (int)pair.Value & 0xFF;
                var positions = config.Slots[pair.Key] = new GridPosition[count];
                var value = pair.Value >> 8;
                for (int i = 0; i < count; i++)
                {
                    // var columnIndex = value & 0xF;
                    // var rowIndex = (value >> 4) & 0xF;
                    // Debug.Log($"[{pair.Key}] [{rowIndex}, {columnIndex}]");
                    positions[i] = new GridPosition((int) (value >> 4) & 0xF, (int) value & 0xF);
                    value >>= 8;
                }
            }
            
            return config;
        }
    }


    [Serializable]
    public class BoardSerialize
    {
        [JsonProperty("size")] public int Size;
        [JsonProperty("slots")] public Dictionary<string, ulong> Slots;
    }

    [Serializable]
    public class BoardConfig
    {
        public BoardSize Size;
        public Dictionary<string, GridPosition[]> Slots;
    }
}