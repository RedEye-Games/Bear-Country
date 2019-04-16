using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class GameData
{
    public GameSettings Settings { get; private set; }
    public string SharedString { get; private set; }

    private float _timeStarted;
    private float _timeFinished;
    public bool HasStarted { get; private set; }
    public bool IsComplete { get; private set; }
    public int CurrentRound { get; private set; }
    public bool UsedSharedString { get; private set; }
    public float GameDuration => _timeFinished - _timeStarted;
    public LaunchedFromValue LaunchedFrom;

    public GameData(GameSettings gameSettings, string sharedString) {
        Settings = gameSettings;
        SharedString = sharedString;
        UsedSharedString = sharedString != null;
    }

    public int roundsRemaining => Settings.numberOfRounds - CurrentRound;

    public enum LaunchedFromValue { main_menu, play_again_button }
    public void Begin(LaunchedFromValue launchedFrom)
    {
        _timeStarted = Time.time;
        HasStarted = true;
        CurrentRound = 0;
        this.LaunchedFrom = launchedFrom;
        AnalyticsWrapper.Report.GameStart(this);
    }

    public void Complete()
    {
        _timeFinished = Time.time;
        IsComplete = true;
    }

    public void GoToNextRound()
    {
        if (CurrentRound < Settings.numberOfRounds) CurrentRound++;
        else { Debug.LogError("tried to advance beyond number of rounds"); }
        Debug.Log("hey, now it is round " + CurrentRound);
    }
}
