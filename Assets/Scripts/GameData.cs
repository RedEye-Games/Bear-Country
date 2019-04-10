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
    public float GameDuration => _timeFinished - _timeStarted;


    public GameData(GameSettings gameSettings, string sharedString) {
        Settings = gameSettings;
        SharedString = sharedString;
    }

    public void Begin()
    {
        _timeStarted = Time.time;
        HasStarted = true;
        CurrentRound = 1;
    }

    public void Complete()
    {
        _timeFinished = Time.time;
        IsComplete = true;
    }

    public void NextRound()
    {
        if (CurrentRound < Settings.numberOfRounds) CurrentRound++;
        else { Debug.LogError("tried to advance beyond number of rounds"); }
    }
}
