using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class MapCtrl : MonoBehaviour
{
    [Serializable]
    public class MapData
    {
        public int Id;
        public Vector3 Pos;
        public GameObject Prefab;
    }

    const float SIZE = 200.0f;
    [SerializeField] List<MapData> _tiles;

    List<MapTile> _maps = new List<MapTile>();
    static MapCtrl _instance;

    private void Awake()
    {
        _instance = this;
    }

    static bool isInCircle(Vector3 f, Vector3 t, float r)
    {
        return (f.x - t.x) * (f.x - t.x) + (f.z - t.z) * (f.z - t.z) < r * r;
    }

    static public void Load(Vector3 position, float distance)
    {
        var list = SearchNearBy(position, distance);
        _instance._maps.ForEach(m =>
        {
            var del = list.Where(t => t.Id == m.Id);
            if (del.Count() > 0)
            {
                list.Remove(del.First());
            }
        });
        list.ForEach(t => {
            var map = GameObject.Instantiate(t.Prefab, _instance.transform);
            _instance._maps.Add(map.GetComponent<MapTile>());
        });
    }

    static public void Remove(Vector3 position, float distance)
    {
        List<MapTile> delQueue = new List<MapTile>();
        _instance._maps.ForEach(m =>
        {
            if (isInCircle(m.transform.position, position, distance)) return;
            if (isInCircle(m.transform.position + new Vector3(SIZE, 0, 0), position, distance)) return;
            if (isInCircle(m.transform.position + new Vector3(0, 0, SIZE), position, distance)) return;
            if (isInCircle(m.transform.position + new Vector3(SIZE, 0, SIZE), position, distance)) return;
            delQueue.Add(m);
        });
        delQueue.ForEach(m =>
        {
            GameObject.Destroy(m.gameObject);
            _instance._maps.Remove(m);
        });
    }

    static public List<MapData> SearchNearBy(Vector3 position, float distance)
    {
        return _instance._tiles.Where(t =>
        {
            if (isInCircle(t.Pos, position, distance)) return true;
            if (isInCircle(t.Pos + new Vector3(SIZE,0,0), position, distance)) return true;
            if (isInCircle(t.Pos + new Vector3(0, 0, SIZE), position, distance)) return true;
            if (isInCircle(t.Pos + new Vector3(SIZE, 0, SIZE), position, distance)) return true;
            return false;
        }).ToList();
    }


#if UNITY_EDITOR
    public void SetTiles(List<MapData> data)
    {
        _tiles = data;
    }
#endif
}
