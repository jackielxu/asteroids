using UnityEngine;
using System.Collections;

public class GoogleAnalyticsManager : MonoBehaviour {

    public GoogleAnalyticsV3 googleAnalytics;
	// Use this for initialization

	void Awake () {
        DontDestroyOnLoad(gameObject);
        googleAnalytics.StartSession();
        googleAnalytics.LogScreen(new AppViewHitBuilder()
        .SetScreenName("Screen_Main")
        .SetCustomDimension(1, "xxxxxx"));
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void LogEvent(string category, string action, string label, long value)
    {
        googleAnalytics.LogEvent(new EventHitBuilder().SetEventCategory(category).SetEventAction(action).SetEventLabel(label).SetEventValue(value).SetCustomDimension(2, System.DateTime.Now.ToString()));
    }
}
