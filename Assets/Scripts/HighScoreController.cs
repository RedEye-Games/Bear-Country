using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreController : MonoBehaviour
{
    public GameObject scorePrefab;

    public void UpdateScores(HighScoreData highScores)
    {
        highScores.scores.Sort(SortByMostRecent);
        Score mostRecentScore = highScores.scores[0];
        Debug.Log("Most recent score is " + mostRecentScore.score + " at " + mostRecentScore.timeStamp);

        highScores.scores.Sort(SortByScore);
        int listSize;

        if (highScores.scores.Count > 10) { listSize = 10; }
        else { listSize = highScores.scores.Count; }

        List<Score> returningScores = highScores.scores.GetRange(0, listSize);
        returningScores.Reverse();

        highScores.scores.Reverse();
        
        int index = 0;
        int recentScoreRank = highScores.scores.FindIndex(x => x == mostRecentScore);

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

    static int SortByMostRecent(Score score1, Score score2)
    {
        return DateTime.Compare(score1.timeStamp, score2.timeStamp);
    }

    static int SortByScore(Score score1, Score score2)
    {
        return score1.score.CompareTo(score2.score);
    }
}
