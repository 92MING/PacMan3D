using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : Manager<NetworkManager>
{
    public readonly string SERVERIP = "";

    public static void SaveMapToServer(MapJson mapJson)
    {
        var jsonStr = mapJson.jsonStr();
        // TODO
    }

    public static MapJson LoadMapFromServer()
    {
        //TODO
        string jsonStr = "";
        MapJson mapJson = JsonUtility.FromJson<MapJson>(jsonStr);
        return mapJson;
    }

    public static User GetUserInfoFromServer()
    {
        //TODO
        string jsonStr = "";
        User user = JsonUtility.FromJson<User>(jsonStr);
        return user;
    }

    public static void GetSettingFromServer()
    {
        //TODO
    }



    


}
