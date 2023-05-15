using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class MapLoader : MonoBehaviour
{
    [SerializeField] float _loadDistance = 200;
    [SerializeField] float _removeDistance = 200;

    void Update()
    {
        MapCtrl.Load(this.transform.position, _loadDistance);
        MapCtrl.Remove(this.transform.position, _removeDistance);
    }
}
