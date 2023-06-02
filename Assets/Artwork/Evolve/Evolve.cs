using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// �̈�W�J�n���o�Ǘ�
/// NOTE: �e���C���Ɗe��I�u�W�F�N�g�ɑΉ�
/// </summary>
public class Evolve : MonoBehaviour
{
    [SerializeField, Tooltip("�ő�W�J�T�C�Y")] float _maxAreaSize = 40.0f;
    [SerializeField, Tooltip("�ő�W�J�܂ł̎���")] float _expandTime = 2.0f;
    [SerializeField, Tooltip("���ł܂ł̎���")] float _eraseTime = 5.0f;
    [SerializeField, Tooltip("�̈���̒n�ʂɃA�^�b�`����}�e���A��")] Material _terrainMat;
    [SerializeField, Tooltip("�̈���̓G�ɃA�^�b�`����}�e���A��")] Material _attachMat;

    //���[�J��
    bool _isEvolved = false; //�W�J�����ǂ���
    float _areaSize = 0.0f; //�T�C�Y
    float _evolveTimer = 0.0f; //�W�J����
    Vector3 _startPos; //�W�J�ʒu
    List<Renderer> _objectRenders = new List<Renderer>(); //�}�e���A����ǉ����������_��

    //�m�F�p��SerializeField(�����Ă���)
    [SerializeField] Material _instancedMat;

    /// <summary>
    /// ������
    /// NOTE: �}�e���A���͍ŏ��ɃR�s�[���Ă���
    /// </summary>
    private void Start()
    {
        _instancedMat = GameObject.Instantiate(_attachMat);
    }

    /// <summary>
    /// �X�V
    /// </summary>
    void Update()
    {
        //test code
        if(Input.GetKeyDown(KeyCode.Space))
        {
            //StartEvolve���Ăׂ΂��Ƃ͗ǂ������ɂȂ�
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
    /// �̈�W�J
    /// </summary>
    public void StartEvolve()
    {
        //���łɓ����Ă���Ȃ牽�����Ȃ�
        if (_evolveTimer > 0.0f) return;

        //�W�J�t���O
        _isEvolved = true;
        _startPos = this.transform.position;

        //�}�e���A������������ł���
        _objectRenders.Clear();
        
        //�d������y�ʉ�����Ȃ炵���ق�������
        //���o�X�g�ȏ����D��
        var list = GameObject.FindObjectsOfType<GameObject>();
        foreach ( var obj in list )
        {
            //�����_�������߂��̃I�u�W�F�N�g�ɂ���Material���d����ł���
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
    /// �̈�W�J
    /// </summary>
    void EndEvolve()
    {
        _isEvolved = false;
        Debug.Log("End");

        //NOTE: �k��������Ƃ��͊O��
        _evolveTimer = _eraseTime;
    }

    /// <summary>
    /// �p�����[�^�X�V
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
