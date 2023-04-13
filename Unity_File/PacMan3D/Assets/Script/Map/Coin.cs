using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum CoinType
{
    Gold, Silver, Brass
}

public class Coin : MonoBehaviour
{
    private Tween _rotationAni=null;
    private MeshRenderer _meshRenderer;
    private CoinType _coinType;
    public CoinType coinType => _coinType;

    void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    void startAnimation()
    {
        if (_rotationAni != null) return;
        _rotationAni = transform.DORotate(new Vector3(0, 360, 0), 3f, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Incremental);
    }
    void stopAnimation()
    {
        if (_rotationAni is null) return;
        _rotationAni.Kill();
        _rotationAni = null;
    }

    void setCoinType(CoinType type)
    {
        _coinType = type;
        _meshRenderer.material = ResourcesManager.GetMaterial(type.ToString());
    }
    
}
