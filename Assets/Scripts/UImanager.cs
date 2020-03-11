using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UImanager : MonoBehaviour
{
    private GameObject startCanvas;
    private GameObject mainCanvas;
    private GameObject endCanvas;
    private GameObject ldCanvas;

    private MainManager game;

    private Text numberInText;
    private Text resultInText;

    private Transform entryContainer;
    private Transform entryTemplate;

    private float startedTime;

    private static string answer = string.Empty;

    void Start()
    {
        ldCanvas = GameObject.Find("HighscoresCanvas");
        entryContainer = ldCanvas.transform.Find("LdContainer");
        entryTemplate = entryContainer.Find("LdTemplate");
        entryTemplate.gameObject.SetActive(false);
        startCanvas = GameObject.Find("StartCanvas"); 
        mainCanvas = GameObject.Find("MainCanvas");
        mainCanvas.SetActive(false);
        endCanvas = GameObject.Find("EndCanvas");
        endCanvas.SetActive(false);
        ldCanvas.SetActive(false);

        game = GameObject.Find("Game").GetComponent<MainManager>();
        numberInText = mainCanvas.transform.Find("NumberInText").GetComponent<Text>();
        resultInText = mainCanvas.transform.Find("ResultInText").GetComponent<Text>();
    }

    public void SetNumber(string number)
    {
        numberInText.text = number;
    }

    public void SetResult(string number)
    {
        resultInText.text = number;
    }

    public float GetStartedTime()
    {
        return startedTime;
    }

    public void SetToMain()
    {
        numberInText.text = string.Empty;
        resultInText.text = string.Empty;
        if (endCanvas.activeSelf)
            endCanvas.SetActive(false);
        startCanvas.SetActive(false);
        mainCanvas.SetActive(true);
        startedTime = Time.timeSinceLevelLoad;
        game.RunGame();
    }

    public void SetToEnd()
    {
        mainCanvas.SetActive(false);
        endCanvas.SetActive(true);
    }

    public void SetToStart()
    {
        endCanvas.SetActive(false);
        startCanvas.SetActive(true);
    }

    public void SetAnswerToDefault()
    {
        answer = numberInText.text.ToString();
        game.answerIsSet = true;
    }

    public void SetAnswerToCheeki()
    {
        answer = "Cheeki";
        game.answerIsSet = true;
    }

    public void SetAnswerToBreeki()
    {
        answer = "Breeki";
        game.answerIsSet = true;
    }

    public void SetAnswerToCheekiBreeki()
    {
        answer = "CheekiBreeki";
        game.answerIsSet = true;
    }

    public string GetAnswer()
    {
        string returningAnswer = answer;
        answer = string.Empty;
        return returningAnswer;
    }

    public void ShowHistory()
    {

    }

    public void ShowLeaderboard()
    {
        startCanvas.SetActive(false);
        ldCanvas.SetActive(true);
        StartCoroutine(RestClient.Instance.Get(MainManager.WEB_URL, PrintLeaderboard));
    }

    void PrintLeaderboard(Leaderboard leaderboard)
    {
        float templateHeight = 30f;
        int i = 0;
        
        foreach (LeaderboardNote note in leaderboard.leaderboard)
        {
            Transform entryTransform = Instantiate(entryTemplate, entryContainer);
            RectTransform entryRectTransform = entryTemplate.GetComponent<RectTransform>();
            entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * (i + 1));
            entryTransform.gameObject.SetActive(true);
            entryTransform.Find("NumberLd").GetComponent<Text>().text = (i + 1).ToString();
            entryTransform.Find("ScoreLd").GetComponent<Text>().text = note.score.ToString();
            entryTransform.Find("TimeLd").GetComponent<Text>().text = Mathf.Round(note.timeInGame).ToString();
            i++;
        }
    }

    public void GoBack()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void ExitFromGame()
    {
        Application.Quit();
    }

    void OnDestroy()
    {
        game = null;
    }
}
