using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;

public enum CharacterAnimation
{
    IDLE,
    WALK_FRONT,
    WALK_BACK,
    WALK_LEFT,
    WALK_RIGHT,
    ATK,
    SKILL,
    SENSE,
    GET_HIT,
    STUN,
    DIE,
    VICTORY,
}

/// <summary>
/// 角色基類
/// </summary>
[RequireComponent(typeof(Rigidbody))]
[System.Serializable]
public abstract class Character: MonoBehaviour
{
    public static LinkedList<Type> AllCharacterType = new LinkedList<Type>();
    public static Dictionary<string, GameObject> AllCharacterPrefab = new Dictionary<string, GameObject>();

    public abstract int maxHP { get; }
    public abstract int maxAttack { get; }
    public abstract int maxDefence { get; }
    public abstract int maxSpeed { get; }

    protected int _hp;
    public int hp => _hp;
    protected int _attack;
    public int attack => _attack;
    protected int _defence;
    public int defence => _defence;
    protected int _speed;
    public int speed => _speed;
    protected Rigidbody _rigidbody;
    protected Animator _animator;

    private float forceAmount = 5f;
    private bool invisibleMode = false;

    void Awake()
    {
        if (!TryGetComponent(out _rigidbody))
        {
            _rigidbody = gameObject.AddComponent<Rigidbody>();
        }
        _animator = GetComponentInChildren<Animator>();
        _hp = maxHP;
        _attack = maxAttack;
        _defence = maxDefence;
        _speed = maxSpeed;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            Forward();
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            Left();
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            Backward();
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            Right();
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            if (!invisibleMode)
            {
                InvisibleMode();
            }
        }
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void LoadAllCharacterType()
    {
        foreach (var clsType in Assembly.GetExecutingAssembly().GetTypes())
        {
            if (clsType.IsSubclassOf(typeof(Character)) && !clsType.IsAbstract)
            {
                AllCharacterType.AddLast(clsType);
                AllCharacterPrefab.Add(clsType.Name, GameManager.LoadPrefab(clsType.Name));
            }
        }
    }

    protected void switchAnimation(CharacterAnimation ani)
    {
        _animator.SetInteger("state", (int)ani);
    }

    private void Forward()
    {
        _rigidbody.AddForce(Vector3.forward * forceAmount, ForceMode.Impulse);
    }

    private void Backward()
    {
        _rigidbody.AddForce(Vector3.back * forceAmount, ForceMode.Impulse);
    }

    private void Left()
    {
        _rigidbody.AddForce(Vector3.left * forceAmount, ForceMode.Impulse);
    }

    private void Right()
    {
        _rigidbody.AddForce(Vector3.right * forceAmount, ForceMode.Impulse);
    }

    private void Jump()
    {
        _rigidbody.AddForce(Vector3.up * forceAmount, ForceMode.Impulse);
    }

    private void InvisibleMode()
    {
        float invisibleTime = 5f;
        IEnumerator InvisibleModeCountDown()
        {
            invisibleMode = true;
            yield return new WaitForSeconds(invisibleTime);
            invisibleMode = false;
        }

        StartCoroutine(InvisibleModeCountDown());
    }
}
