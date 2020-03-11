using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    private int number = 0;
    private UImanager uiManager;
    public bool answerIsSet = false;

    public static string WEB_URL = "http://localhost:5000/api/leaderboard/";

    public void RunGame()
    {
        answerIsSet = false;
        StartCoroutine("CheekiBreekiDefault");
    }

    void Start()
    {
        uiManager = GameObject.Find("UImanager").GetComponent<UImanager>();
    }

    private string NumberOrCheekiBreeki(int number)
    {
        if (number == 0) return "0";

        //default size is 16 symbols
        StringBuilder result = new StringBuilder();

        if (number % 3 == 0) result.Append("Cheeki");
        if (number % 5 == 0) result.Append("Breeki");
        if (result.Length == 0) result.Append(number.ToString());
        
        return result.ToString();
    }

    private IEnumerator CheekiBreekiDefault()
    {
        Debug.Log("Start");
        for (number = 0; number <= 100; number++)
        {
            string result = NumberOrCheekiBreeki(number);
            uiManager.SetNumber(number.ToString());
            float timeWait = Time.timeSinceLevelLoad;
            while (!answerIsSet && Time.timeSinceLevelLoad - timeWait < GetTimeForAnswer((float)number))
            { 
                yield return null;
            }
            uiManager.SetResult(result);
            string answerString = uiManager.GetAnswer();
            if (answerString != result)
            {
                uiManager.SetToEnd(); 
                break;
            }
            yield return new WaitForSeconds(1);
            uiManager.SetResult(string.Empty);
            answerIsSet = false;
        }
        float timePlayed = Time.timeSinceLevelLoad - uiManager.GetStartedTime();
        PutNote(timePlayed, number);
    }

    void PutNote(float timePlayed, int score)
    {
        StartCoroutine(RestClient.Instance.Post(WEB_URL, new LeaderboardNote
        {
            timeInGame = timePlayed,
            score = score
        }));
    }

    //asymptote is 2
    private float GetTimeForAnswer(float x)
    {
        float res = 10f / (float)System.Math.Pow(2f, x) + 2f;
        return res;
    }

    void OnDestroy()
    {
        uiManager = null;
    }
} 
