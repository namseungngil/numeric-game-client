using UnityEngine;
using System.Collections;

public class FPSDisplayComponent : MonoBehaviour
{
	public  float updateInterval = 0.5f;
	private float accum = 0; // FPS accumulated over the interval
	private int frames = 0; // Frames drawn over the interval
	private float timeleft; // Left time for current interval
	private string text;
	
	void Start ()
	{
		timeleft = updateInterval;
	}
	
	void Update ()
	{
		timeleft -= Time.deltaTime;
		accum += Time.timeScale / Time.deltaTime;
		++frames;
		
		// Interval ended - update GUI text and start new interval
		if (timeleft <= 0.0) {
			// display two fractional digits (f2 format)
			float fps = accum / frames;
			string format = System.String.Format ("{0:F2}", fps);
			text = "" + format;

			//	DebugConsole.Log(format,level);
			timeleft = updateInterval;
			accum = 0.0F;
			frames = 0;
		}
	}

	void OnGUI ()
	{
		GUI.Label (new Rect (10, 10, 100, 50), "FPS : " + text);
	}
}