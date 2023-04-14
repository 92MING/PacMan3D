using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MongoDB.Driver;
using MongoDB.Bson;

public class NetworkManager : Manager<NetworkManager>
{
    public readonly string SERVERIP = "";
    public static string connectionString = "mongodb+srv://sad9jai:04875398MON@csci3100.9m8rnfx.mongodb.net/pacman?retryWrites=true&w=majority";

    // MongoDB database name
    public static string databaseName = "pacman";

    public static MongoClient client = new MongoClient(NetworkManager.connectionString);

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

    public static void UploadDataToDB(string collectionName, string jsonDate)
    {
        MongoClient client = new MongoClient(NetworkManager.connectionString);
        IMongoDatabase database;
        IMongoCollection<BsonDocument> collection;
        database = client.GetDatabase(NetworkManager.databaseName);
        collection = database.GetCollection<BsonDocument>(collectionName);

        BsonDocument doc = BsonDocument.Parse(jsonDate);
        collection.InsertOne(doc);

        Debug.Log("Inserted document: " + doc.ToString());
    }

    public static List<string> DownloadDataToDB(string collectionName)
    {
        IMongoDatabase database = client.GetDatabase(NetworkManager.databaseName);
        IMongoCollection<BsonDocument> collection = database.GetCollection<BsonDocument>(collectionName);

        var doc = collection.Find(new BsonDocument()).ToList();

        List<string> outputJsons = new List<string> { };

        foreach (BsonDocument document in doc)
        {
            outputJsons.Add(document.ToJson());
        }

        return outputJsons;
    }

   



    


}
