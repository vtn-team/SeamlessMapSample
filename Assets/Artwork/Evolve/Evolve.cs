using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 領域展開系演出管理
/// NOTE: テレインと各種オブジェクトに対応
/// </summary>
public class Evolve : MonoBehaviour
{
    [SerializeField, Tooltip("最大展開サイズ")] float _maxAreaSize = 40.0f;
    [SerializeField, Tooltip("最大展開までの時間")] float _expandTime = 2.0f;
    [SerializeField, Tooltip("消滅までの時間")] float _eraseTime = 5.0f;
    [SerializeField, Tooltip("領域内の地面にアタッチするマテリアル")] Material _terrainMat;
    [SerializeField, Tooltip("領域内の敵にアタッチするマテリアル")] Material _attachMat;

    //ローカル
    bool _isEvolved = false; //展開中かどうか
    float _areaSize = 0.0f; //サイズ
    float _evolveTimer = 0.0f; //展開時間
    Vector3 _startPos; //展開位置
    List<Renderer> _objectRenders = new List<Renderer>(); //マテリアルを追加したレンダラ

    //確認用のSerializeField(消していい)
    [SerializeField] Material _instancedMat;

    /// <summary>
    /// 初期化
    /// NOTE: マテリアルは最初にコピーしておく
    /// </summary>
    private void Start()
    {
        _instancedMat = GameObject.Instantiate(_attachMat);
    }

    /// <summary>
    /// 更新
    /// </summary>
    void Update()
    {
        //test code
        if(Input.GetKeyDown(KeyCode.Space))
        {
            //StartEvolveを呼べばあとは良い感じになる
            StartEvolve();
        }
        /*
        if (Input.GetKeyUp(KeyCode.Space))
        {
            EndEvolve();
        }
        */
        //

        if (_isEvolved && _evolveTimer < _expandTime)
        {
            _evolveTimer += Time.deltaTime;
            if (_evolveTimer > _expandTime)
            {
                EndEvolve();
            }
            _areaSize = Mathf.Lerp(0.0f, _maxAreaSize, _evolveTimer);
        }
        if (!_isEvolved && _evolveTimer > 0.0f)
        {
            _evolveTimer -= Time.deltaTime;
            if (_evolveTimer < 0.0f)
            {
                _evolveTimer = 0.0f;
                _areaSize = 0.0f;
                RemoveMaterials();
            }
        }

        UpdateEvolve();
    }

    void RemoveMaterials()
    {
        foreach (var render in _objectRenders)
        {
            if (render == null) continue;

            var mats = render.sharedMaterials.ToList();
            var where = mats.Where(m => m == _instancedMat);
            if (where.Count() > 0)
            {
                mats.Remove(where.First());
            }
            render.sharedMaterials = mats.ToArray();
        }
    }

    /// <summary>
    /// 領域展開
    /// </summary>
    public void StartEvolve()
    {
        //すでに動いているなら何もしない
        if (_evolveTimer > 0.0f) return;

        //展開フラグ
        _isEvolved = true;
        _startPos = this.transform.position;

        //マテリアルを差しこんでいく
        _objectRenders.Clear();
        
        //重いから軽量化するならしたほうがいい
        //ロバストな処理優先
        var list = GameObject.FindObjectsOfType<GameObject>();
        foreach ( var obj in list )
        {
            //レンダラを持つ近くのオブジェクトにだけMaterialを仕込んでいく
            var renderer = obj.GetComponentInChildren<Renderer>();
            if(renderer != null)
            {
                Vector3 len = obj.transform.position - _startPos;
                if (len.magnitude < _maxAreaSize)
                {
                    var mats = renderer.sharedMaterials.ToList();
                    mats.Add(_instancedMat);
                    renderer.sharedMaterials = mats.ToArray();
                    _objectRenders.Add(renderer);
                }
            }
        }

        Debug.Log("Start");
    }

    /// <summary>
    /// 領域展開
    /// </summary>
    void EndEvolve()
    {
        _isEvolved = false;
        Debug.Log("End");

        //NOTE: 縮小させるときは外す
        _evolveTimer = _eraseTime;
    }

    /// <summary>
    /// パラメータ更新
    /// </summary>
    void UpdateEvolve()
    {
        //_areaSize = Mathf.Lerp(0.0f, _maxAreaSize, _evolveTimer);

        _terrainMat.SetVector("_PlayerPos", _startPos);
        _terrainMat.SetFloat("_AreaSize", _areaSize);

        _instancedMat.SetVector("_PlayerPos", _startPos);
        _instancedMat.SetFloat("_AreaSize", _areaSize);
    }
}
