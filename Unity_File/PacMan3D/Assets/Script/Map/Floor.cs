using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 地板都是相同的正方体prefab，只是材质不同
/// </summary>
public class Floor : GenerateCoinMapObject<Floor>
{
    public static new int poolSize => 100;

    protected override void OnCreate()
    {
        base.OnCreate();
        transform.position = transform.position + Vector3Int.down;
    }
}
