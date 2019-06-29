using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreController : MonoBehaviour
{
    public GameObject scorePrefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateScores(HighScoreData highScores)
    {
        Score recentScore = highScores.scores[0];
        Debug.Log("Most recent score is " + recentScore.score + " at " + recentScore.timeStamp);
        highScores.scores.Sort((Score x, Score y) => DateTime.Compare(x.timeStamp, y.timeStamp));
        highScores.scores.Sort(SortByScore);
        int listSize;
        if (highScores.scores.Count > 10)
        {
            listSize = 10;
        }
        else
        {
            listSize = highScores.scores.Count;
        }
        highScores.scores.Reverse();
        int recentScoreRank = highScores.scores.FindIndex(x => x == recentScore);
        List<Score> returningScores = highScores.scores.GetRange(0, listSize);
        int index = 0;
        foreach (var score in returningScores)
        {
            GameObject newReturningScore = Instantiate(scorePrefab, transform, false);
            newReturningScore.GetComponent<Text>().text = (index + 1) + ". " + score.score;
            if (index == recentScoreRank)
            {
                newReturningScore.GetComponent<Text>().color = new Color(0.9058824f, 0.7450981f, 0.1215686f, 1f);
            }
            index++;
        }

    }

    static int SortByScore(Score score1, Score score2)
    {
        return score1.score.CompareTo(score2.score);
    }
}
