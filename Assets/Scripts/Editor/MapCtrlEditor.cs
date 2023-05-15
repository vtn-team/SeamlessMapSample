using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;
using static MapCtrl;

[CustomEditor(typeof(MapCtrl))]
public class MapCtrlEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("マップをPrefab化する"))
        {
            int Id = 1;
            MapCtrl ctrl = target as MapCtrl;
            var lists = ctrl.GetComponentsInChildren<Terrain>();
            List<MapTile> _tiles = new List<MapTile>();
            List<MapData> _datas = new List<MapData>();
            foreach (var t in lists)
            {
                var tile = t.gameObject.GetComponent<MapTile>();
                if (tile == null)
                {
                    tile = t.gameObject.AddComponent<MapTile>();
                }
                tile.SetId(Id);
                _tiles.Add(tile);

                // Prefabを作成or上書き
                Debug.Log(string.Format("{0}/Map/Map{1}.prefab", Application.dataPath, Id));

                MapData data = new MapData();
                data.Id = Id;
                data.Pos = t.transform.position;
                data.Prefab = PrefabUtility.SaveAsPrefabAssetAndConnect(t.gameObject, string.Format("{0}/Map/Map{1}.prefab", Application.dataPath, Id), InteractionMode.AutomatedAction);
                _datas.Add(data);
                Id++;
            }
            ctrl.SetTiles(_datas);
            foreach (var t in lists)
            {
                GameObject.DestroyImmediate(t.gameObject);
            }
        }
    }
}
