using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : Manager<NetworkManager>
{
    public static void SaveMapToServer(MapJson mapJson)
    {
        var jsonStr = mapJson.jsonStr();
        // TODO
    }
}
