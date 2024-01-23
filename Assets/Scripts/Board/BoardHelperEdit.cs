using System;
using System.IO;
using Common;
using Match3.Board;
using Match3.Context;
using Match3.Models;
using UnityEditor;
using UnityEngine;

namespace Match3.Utils
{
#if UNITY_EDITOR
    [CustomEditor(typeof(ApplicationContext))]
    public class BoardHelperEdit : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            ApplicationContext myScript = (ApplicationContext)target;

            if (GUILayout.Button("Save current board (use runtime)"))
            {
                var board = myScript.Resolve<IBoardModel>();
                if(board.Slots == null) return;
                var json = BoardConverter.ToJson(board);
                string filepath = Path.Combine("Assets//settings", $"Board_{DateTime.Now.ToString("hmmsstt")}.json");
                File.WriteAllText(filepath, json);
                AssetDatabase.SaveAssets();
                UnityEngine.Debug.Log($"Save to: {filepath}");
            }
        }
    }
#endif
}