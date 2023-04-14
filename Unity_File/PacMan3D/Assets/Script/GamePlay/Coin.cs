using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum CoinType
{
    Gold, Silver, Brass
}

public class Coin : PoolMonoObject
{
    public static new GameObject prefab => ResourcesManager.GetPrefab("Coin");
    public static new int poolSize => 60;

    private Tween _rotationAni=null;
    private MeshRenderer _meshRenderer;
    private CoinType _coinType;
    public CoinType coinType => _coinType;
    public IGenerateCoinMapObject thisMapObj;
    
    void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
    }
    
    public override void OnGot()
    {
        base.OnGot();
        GameManager.currentCoinNum++;
        startAnimation();
        BindListenerToGameManager();
    }
    public override void OnReturn()
    {
        base.OnReturn();
        GameManager.currentCoinNum--;
        stopAnimation();
        UnbindListenerToGameManager();
    }

    void startAnimation()
    {
        if (_rotationAni != null) return;
        _rotationAni = transform.DORotate(new Vector3(0, 360, 0), 3f, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Incremental);
    }
    void pauseAnimation()
    {
        _rotationAni?.Pause();
    }
    void continueAnimation()
    {
        _rotationAni?.Play();
    }
    void stopAnimation()
    {
        if (_rotationAni is null) return;
        _rotationAni.Kill();
        _rotationAni = null;
    }

    void BindListenerToGameManager()
    {
        GameManager.OnGamePause.AddListener(pauseAnimation);
        GameManager.OnGameContinue.AddListener(continueAnimation);
    }
    void UnbindListenerToGameManager()
    {
        GameManager.OnGamePause.RemoveListener(pauseAnimation);
        GameManager.OnGameContinue.RemoveListener(continueAnimation);
    }

    public void setCoinType(CoinType type)
    {
        _coinType = type;
        _meshRenderer.material = ResourcesManager.GetMaterial(type.ToString());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<CharacterBase>(out var character))
        {
            character.gainCoin(_coinType);
            thisMapObj?.OnCoinEaten();
        }
        this.ReturnToPool();
    }
}
