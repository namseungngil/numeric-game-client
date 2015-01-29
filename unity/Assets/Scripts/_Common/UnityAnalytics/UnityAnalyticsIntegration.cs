using UnityEngine;
using System.Collections;
using UnityEngine.Cloud.Analytics;

public class UnityAnalyticsIntegration : MonoBehaviour
{
	void Start ()
	{
		const string projectId = "435d6ab2-8324-40a1-b2ae-23584d642056";
		UnityAnalytics.StartSDK (projectId);
	}
}
