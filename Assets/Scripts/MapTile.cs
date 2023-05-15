using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class MapTile : MonoBehaviour
{
    [SerializeField] int _id;

    public int Id => _id;

#if UNITY_EDITOR
    public void SetId(int id)
    {
        _id = id;
    }
#endif
}
