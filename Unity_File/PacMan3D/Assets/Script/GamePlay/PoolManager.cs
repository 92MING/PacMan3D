using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public interface IPoolObject { }
public static class PoolObjectExtension
{
    public static void ReturnToPool(this object obj)
    {
        PoolManager.ReturnInstance(ref obj);
    }
}

/// <summary>
/// 继承此类，可以实现对象池
/// </summary>
/// <typeparam name="ChildCls"></typeparam>
public abstract class PoolObject: IPoolObject 
{
    public static int poolSize => 10; //定义对象池大小, 默认10
    public virtual void OnGot() { }
    public virtual void OnReturn() { }
}
/// <summary>
/// 继承此类，可以实现对象池. 用于MonoBehaviour
/// </summary>
/// <typeparam name="ChildCls"></typeparam>
public abstract class PoolMonoObject: MonoBehaviour, IPoolObject
{
    public static int poolSize => 10; //定义对象池大小, 默认10
    public static GameObject prefab => throw new System.NotImplementedException(); //必须有prefab
    public virtual void OnGot() { }
    public virtual void OnReturn() { }
}

public class PoolManager : Manager<PoolManager>
{
    private static Dictionary<string, Stack<object>> _pool = new Dictionary<string, Stack<object>>();
    private static GameObject _poolParentObj = null; //场景内的‘Pool’游戏物件，负责装载着MonoBehaviour的游戏对象
    private static GameObject poolParentObj
    {
        get
        {
            if (_poolParentObj is null)
            {
                _poolParentObj = new GameObject();
                _poolParentObj.name = "Pool";
                DontDestroyOnLoad(_poolParentObj);
            }
            return _poolParentObj;
        }
    }

    protected override void Awake()
    {
        base.Awake();
        if (_poolParentObj is null)
        {
            _poolParentObj = new GameObject();
            _poolParentObj.name = "Pool";
            DontDestroyOnLoad(_poolParentObj);
        }
        //初始化对象池
        var types = System.Reflection.Assembly.GetExecutingAssembly().GetTypes();
        foreach (var type in types)
        {
            if (type.GetInterfaces().Any(i=>i.Equals(typeof(IPoolObject))) && !type.IsAbstract)
            {
                var poolSize = (type.GetProperty("poolSize", BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy).GetValue(null) as int?) ?? 10; //默认大小
                if (type.IsSubclassOf(typeof(PoolObject)))
                {
                    var stack = new Stack<object>();
                    for (int i = 0; i < poolSize; i++)
                    {
                        var obj = System.Activator.CreateInstance(type);
                        stack.Push(obj);
                    }
                    _pool.Add(type.Name, stack);
                }
                else
                {
                    var stack = new Stack<object>();
                    GameObject prefab = type.GetProperty("prefab", BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy)?.GetValue(null) as GameObject;
                    if (prefab is null)
                    {
                        Debug.LogWarning("Type " + type.Name + " no prefab found!(Prefab name: " + type.Name + ")");
                        continue;
                    }
                    for (int i = 0; i < poolSize; i++)
                    {
                        var obj = Instantiate(prefab);
                        obj.SetActive(false);
                        obj.transform.parent = poolParentObj.transform;
                        if (!obj.TryGetComponent(type, out var comp))
                        {
                            stack.Push(obj.AddComponent(type));
                        }
                        else
                        {
                            stack.Push(comp);
                        }
                    }
                    _pool.Add(type.Name, stack);
                }
            }
        }
    }

    public static object GetInstance(System.Type type)
    {
        if (_pool.TryGetValue(type.Name, out var stack))
        {
            if (stack.Count == 0)
            {
                if (type.IsSubclassOf(typeof(PoolMonoObject)))
                {
                    var prefab = type.GetProperty("prefab",BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy)?.GetValue(null);
                    if (prefab is null)
                    {
                        Debug.LogWarning("Type " + type.Name + " no prefab found!(Prefab name: " + type.Name + ")");
                        return null;
                    }
                    else
                    {
                        var obj = Instantiate(prefab as GameObject);
                        obj.SetActive(true);
                        if (!obj.TryGetComponent(type, out var comp))
                        {
                            comp = obj.AddComponent(type);
                            (comp as PoolMonoObject)?.OnGot();
                            return comp;
                        }
                        else
                        {
                            (comp as PoolMonoObject)?.OnGot();
                            return comp;
                        }
                    }
                }
                else
                {
                    var obj = System.Activator.CreateInstance(type);
                    (obj as PoolObject).OnGot();
                    return obj;
                }
            }
            else
            {
                if (type.IsSubclassOf(typeof(PoolMonoObject)))
                {
                    var comp = stack.Pop() as MonoBehaviour;
                    comp.gameObject.SetActive(true);
                    (comp as PoolMonoObject)?.OnGot();
                    return comp;
                }
                else
                {
                    var obj = stack.Pop();
                    (obj as PoolObject)?.OnGot();
                    return obj;
                }
            }
        }
        else
        {
            Debug.LogWarning("Type " + type.Name + " can't be found in pool.");
            return null;
        }
    }
    public static T GetInstance<T>() where T : class, IPoolObject
    {
        return GetInstance(typeof(T)) as T;
    }
    public static T GetInstance<T>(GameObject newParent) where T: PoolMonoObject
    {
        var comp = GetInstance<T>();
        comp.transform.parent = newParent.transform;
        return comp;
    }

    //自动GetType寻找类型
    public static void ReturnInstance(ref object obj)
    {
        var type = obj.GetType();
        var typePoolSize = (type.GetProperty("poolSize", BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy)?.GetValue(null) as int?) ?? 10;
        if (_pool.TryGetValue(type.Name, out var stack))
        {
            if (stack.Count >= typePoolSize)
            {
                if (type.IsSubclassOf(typeof(PoolMonoObject)))
                {
                    var _obj = (obj as PoolMonoObject);
                    if (_obj != null)
                    {
                        _obj.OnReturn();
                        Destroy(_obj.gameObject);
                    }
                }
                else
                {
                    (obj as PoolObject)?.OnReturn();
                }
            }
            else
            {
                if (type.IsSubclassOf(typeof(PoolMonoObject)))
                {
                    var mono = obj as PoolMonoObject;
                    if (mono != null)
                    {
                        mono.OnReturn();
                        mono.gameObject.SetActive(false);
                        mono.transform.parent = poolParentObj.transform;
                    }
                }
                else
                {
                    (obj as PoolObject)?.OnReturn();
                }
                stack.Push(obj);
            }
            obj = null;
        }
        else
        {
            Debug.LogWarning("Type " + type.Name + " can't be found in pool.");
        }
    }
    //指定类型
    public static void ReturnInstance(System.Type type, ref object obj)
    {
        var typePoolSize = (type.GetProperty("poolSize", BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy)?.GetValue(null) as int?) ?? 10;
        if (_pool.TryGetValue(type.Name, out var stack))
        {
            if (stack.Count >= typePoolSize)
            {
                if (type.IsSubclassOf(typeof(PoolMonoObject)))
                {
                    var _obj = (obj as PoolMonoObject);
                    if (_obj != null)
                    {
                        _obj.OnReturn();
                        Destroy(_obj.gameObject);
                    }
                }
                else
                {
                    (obj as PoolObject)?.OnReturn();
                }
            }
            else
            {
                if (type.IsSubclassOf(typeof(PoolMonoObject)))
                {
                    var mono = obj as PoolMonoObject;
                    if (mono != null)
                    {
                        mono.OnReturn();
                        mono.gameObject.SetActive(false);
                        mono.transform.parent = poolParentObj.transform;
                    }
                }
                else
                {
                    (obj as PoolObject)?.OnReturn();
                }
                stack.Push(obj);
            }
            obj = null;
        }
        else
        {
            Debug.LogWarning("Type " + type.Name + " can't be found in pool.");
        }
    }

}
