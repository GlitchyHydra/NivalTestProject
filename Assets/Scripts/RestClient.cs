using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class RestClient : MonoBehaviour
{
    private static RestClient _instance;

    public static RestClient Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<RestClient>();
                if (_instance == null)
                {
                    GameObject go = new GameObject();
                    go.name = typeof(RestClient).Name;
                    _instance = go.AddComponent<RestClient>();
                    DontDestroyOnLoad(go);
                }
            }
            return _instance;
        }
    }
        
    public IEnumerator Get(string url, System.Action<Leaderboard> callback)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError)
            {
                Debug.Log(www.error);
            }
            else
            {
                if (www.isDone)
                {
                    string jsonResult = System.Text.Encoding.UTF8.GetString(www.downloadHandler.data);
                    Leaderboard leaderboard = JsonUtility.FromJson<Leaderboard>("{\"leaderboard\":" + jsonResult + "}");
                    callback(leaderboard);
                }
            }
        }
    }

    public IEnumerator Post(string url, LeaderboardNote leaderboardNote)
    {
        string jsonData = JsonUtility.ToJson(leaderboardNote);
        using (UnityWebRequest www = UnityWebRequest.Post(url + "Post", jsonData))
        {
            www.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(jsonData));
            www.SetRequestHeader("Content-Type", "application/json");
            www.uploadHandler.contentType = "application/json";

            yield return www.SendWebRequest();

            if (www.isNetworkError)
            {
                Debug.Log(www.error);
            }
            else
            {
                if (www.isDone)
                {
                }
            }
        }
    }
}
