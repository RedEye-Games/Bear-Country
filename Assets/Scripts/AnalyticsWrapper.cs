using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;


public static class AnalyticsWrapper
{
    private static bool ShouldSendEvent() {
        //return Debug.isDebugBuild == false;
        return true;
    }

    private static void PreProcessParams(Dictionary<string, object> eventParams)
    {
        //eventParams.Add("appVersion", Application.version);
        //eventParams.Add("isDebugBuild", Debug.isDebugBuild);
    }

    private static bool PreCall(string debugMessage, Dictionary<string, object> eventParams)
    {
        if (Debug.isDebugBuild) Debug.Log("ANALYTICS: " + debugMessage);
        PreProcessParams(eventParams);
        return ShouldSendEvent();
    }


    public static class Report
    {
        public static void CustomEvent(string eventName, Dictionary<string, object> eventParams)
        {
            if (Debug.isDebugBuild) eventName = "DEBUG: " + eventName;
            if (!PreCall("custom event: " + eventName, eventParams)) return;

            AnalyticsEvent.Custom(eventName, eventParams);
        }
        public static void CustomEvent(string eventName) { CustomEvent(eventName, null); }


        public static class Tutorial
        {
            public static void Start(string tutorialName, string cameFrom)
            {
                string debugMessage = "tutorial start: " + tutorialName + ". cameFrom: " + cameFrom;

                Dictionary<string, object> eventParams = new Dictionary<string, object>
                {
                    { "cameFrom", cameFrom}
                };

                if (!PreCall(debugMessage, eventParams)) return;
                AnalyticsEvent.TutorialStart(tutorialName, eventParams);
            }

            public static void Complete(string tutorialName, string cameFrom, float timeElapsed)
            {
                string debugMessage = "tutorial complete: " + tutorialName + ". cameFrom: " + cameFrom + ". timeElapsed: " + timeElapsed;

                Dictionary<string, object> eventParams = new Dictionary<string, object>
                {
                    { "cameFrom", cameFrom },
                    { "durationViewed", timeElapsed }
                };

                if (PreCall(debugMessage, eventParams)) AnalyticsEvent.TutorialComplete(tutorialName, eventParams);
            }
        }

        public static void ScreenVisit(string screenName, string cameFrom)
        {
            string debugMessage = "screenVisit: " + screenName + ". cameFrom: " + cameFrom;
            Dictionary<string, object> eventParams = new Dictionary<string, object>
            {
                { "cameFrom", cameFrom}
            };

            bool shouldSend = PreCall(debugMessage, eventParams);
            if (shouldSend) AnalyticsEvent.ScreenVisit(screenName, eventParams);
        }

        public static void GameStart(GameData gameData)
        {
            string debugMessage = "gameStart";
            Dictionary<string, object> eventParams = new Dictionary<string, object>();

            eventParams.Add("numberOfRounds", gameData.Settings.numberOfRounds);
            eventParams.Add("boardWidth", gameData.Settings.boardWidth);
            eventParams.Add("boardHeight", gameData.Settings.boardHeight);
            eventParams.Add("usedSharedString", gameData.UsedSharedString);
            eventParams.Add("launchedFrom", gameData.LaunchedFrom);
            if (PreCall(debugMessage, eventParams)) AnalyticsEvent.GameStart(eventParams);
        }

        public static void GameFinish(GameData gameData)
        {
            string debugMessage = "gameFinish";
            Dictionary<string, object> eventParams = new Dictionary<string, object>
            {
                { "gameData", gameData }
            };

            if (PreCall(debugMessage, eventParams)) AnalyticsEvent.GameOver(null, eventParams);
        }
    }
}