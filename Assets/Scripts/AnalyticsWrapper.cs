using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;


public static class AnalyticsWrapper
{
    private static bool ShouldSendEvent() { 
        return false; 
    }

    private static bool PreCall(string debugMessage)
    {
        if (Debug.isDebugBuild) Debug.Log("ANALYTICS: " + debugMessage);
        return ShouldSendEvent();
    }

    public static class Report
    {
        public static void CustomEvent(string eventName, Dictionary<string, object> eventParams)
        {
            if (Debug.isDebugBuild) eventName = "DEBUG: " + eventName;
            if (!PreCall("custom event: " + eventName)) return;
            AnalyticsEvent.Custom(eventName, eventParams);
        }
        public static void CustomEvent(string eventName) { CustomEvent(eventName, null); }


        public static class Tutorial
        {
            public static void Start(string tutorialName, string cameFrom)
            {
                string debugMessage = "tutorial start: " + tutorialName + ". cameFrom: " + cameFrom;
                if (!PreCall(debugMessage)) return;

                AnalyticsEvent.TutorialStart(tutorialName, new Dictionary<string, object>
                {
                    { "cameFrom", cameFrom }
                });
            }

            public static void Complete(string tutorialName, string cameFrom, float timeElapsed)
            {
                string debugMessage = "tutorial complete: " + tutorialName + ". cameFrom: " + cameFrom + ". timeElapsed: " + timeElapsed;
                if (!PreCall(debugMessage)) return;

                AnalyticsEvent.TutorialComplete(tutorialName, new Dictionary<string, object>
                {
                    { "cameFrom", cameFrom },
                    { "durationViewed", timeElapsed }
                });
            }
        }

        public static void ScreenVisit(string screenName, string cameFrom)
        {
            string debugMessage = "screenVisit: " + screenName + ". cameFrom: " + cameFrom;
            if (!PreCall(debugMessage)) return;

            AnalyticsEvent.ScreenVisit(screenName, new Dictionary<string, object>
            {
                { "cameFrom", cameFrom}
            });
        }

        public static void GameStart()
        {
            string debugMessage = "gameStart";
            if (!PreCall(debugMessage)) return;

            AnalyticsEvent.GameStart();
        }

        public static void GameFinish(GameData gameData)
        {
            string debugMessage = "gameFinish";
            if (!PreCall(debugMessage)) return;

            AnalyticsEvent.GameOver(null, new Dictionary<string, object>
            {
                { "gameData", gameData }
            });
        }
    }
}