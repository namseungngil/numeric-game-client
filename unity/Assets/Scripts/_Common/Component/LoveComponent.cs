using UnityEngine;
using System;
using System.Collections;

public class LoveComponent : MonoBehaviour
{
	// const
	private const string NOTIFICATION_TEXT = "Max has a heart.";
	private const string LOVE = "Love";
	private const string LOVE_TIME = "LoveTime";
	private const string LOVE_COUNT = "LoveCount";
	private const int LOVE_RECOVERY = 60;
	private const int TIMER = 1;
	// component
	private UILabel loveTimeUILabel;
	private UILabel loveCountUILabel;
	private Register numericPlayerPrefs;
	private QueryModel dataQuery;
	// variable
	private int love;
	private int loveTime;
	private float timer;

	void Start ()
	{
		numericPlayerPrefs = Register.Instance ();
		dataQuery = QueryModel.Instance ();
		GameObject loveGameObject = GameObject.Find (LOVE);
		if (loveGameObject != null) {
			foreach (UILabel uiLabel in loveGameObject.GetComponentsInChildren<UILabel> ()) {
				if (uiLabel.gameObject.name == LOVE_TIME) {
					loveTimeUILabel = uiLabel;
				} else if (uiLabel.gameObject.name == LOVE_COUNT) {
					loveCountUILabel = uiLabel;
				}
			}
		}

		InitLove ();
	}

	void Update ()
	{
		if (love < Config.LOVE_MAX) {
			string timeLabel;

			if (loveTime < 0) {
				timeLabel = "00:00";
				InitLove ();
			} else {
				string temp = ConvertSecondToTime (loveTime);
				timeLabel = temp.Substring (0, 2) + ":" + temp.Substring (2);
			}

			timer -= Time.deltaTime;
			if (timer <= 0) {
				timer = TIMER;
				loveTime--;
			}

			if (loveTimeUILabel != null) {
				loveTimeUILabel.text = timeLabel;
			}
		}
		
		if (loveCountUILabel != null) {
			loveCountUILabel.text = "" + love + "/" + Config.LOVE_MAX;
		}

	}

	private int ConvertTimeToSecond (string time)
	{
		int tempScond = 0;
		
		// hour
		tempScond += (int.Parse (time.Substring (0, 1)) * 3600);
		// minute
		tempScond += (int.Parse (time.Substring (1, 3)) * 60);
		// second
		tempScond += int.Parse (time.Substring (3));

		return tempScond;
	}
	
	private string ConvertSecondToTime (int second)
	{
		string temp = "";

		// minute
		string tempMinute = "" + ((second % 3600) / 60);
		if (tempMinute.Length < 1) {
			tempMinute = "00";
		} else if (tempMinute.Length == 1) {
			tempMinute = "0" + tempMinute;
		}
		temp += tempMinute;
		// second
		string tempSecond = "" + ((second % 3600) % 60);
		if (tempSecond.Length < 1) {
			tempSecond = "00";
		} else if (tempSecond.Length == 1) {
			tempSecond = "0" + tempSecond;
		}
		temp += tempSecond;
		
		return temp;
	}
	
	private void InitLove ()
	{
		timer = TIMER;

		love = numericPlayerPrefs.GetLove ();
		Debug.Log ("love : " + love);

		if (love < Config.LOVE_MAX) {
			string tempLoveTime = numericPlayerPrefs.GetLoveTime ();
			if (tempLoveTime == "") {
				SetLove (Config.LOVE_MAX);
			}
			Debug.Log ("tempLoveTime : " + tempLoveTime + " Length : " + tempLoveTime.Length);

			int[] dateArray = Date.Slice (tempLoveTime);

			Debug.Log ("date : " + dateArray[0] + " _ " + dateArray[1] + " _ " + dateArray[2] + " _ " + dateArray[3] + " _ " + dateArray[4] + " _ " + dateArray[5]);

			DateTime start = new DateTime (dateArray[0], dateArray[1], dateArray[2], dateArray[3], dateArray[4], dateArray[5]);
			DateTime end = DateTime.Now;
			var seconds = (end - start).TotalSeconds;

			if (seconds >= LOVE_RECOVERY) {
				loveTime = (int)(LOVE_RECOVERY - (seconds % LOVE_RECOVERY));
				int temp = (int)(seconds / LOVE_RECOVERY);
				int upLove = 0;
				while (upLove < temp) {
					upLove++;
				}

				love += upLove;
				if (love >= Config.LOVE_MAX) {
					love = Config.LOVE_MAX;
					SetLove (love);
				} else {
					int tempSeconds = (int)(seconds * temp);
					SetLove (love, start.AddSeconds (tempSeconds).ToString (Config.DATA_TIME));
				}
			} else {
				loveTime = (int)(LOVE_RECOVERY - seconds);
			}

			Debug.Log ("loveTime : " + loveTime);
		}
	}

	private void SetLove (int love, string time = "")
	{
		if (time == "") {
			numericPlayerPrefs.SetLove (love);
		} else {
			numericPlayerPrefs.SetLove (love, time);
		}

		if (love < Config.LOVE_MAX) {
			int tempInt = (Config.LOVE_MAX - love) * LOVE_RECOVERY;
			string tempTime = DateTime.Now.AddSeconds (tempInt).ToString (Config.DATA_TIME);

			string[] tempString = new string[] {
				tempTime, Config.GAME_TITME, NOTIFICATION_TEXT 
			};
			Notification.Register (tempString);
		} else {
			Notification.Unregister ();
		}
	}
	
	private IEnumerator TimerLove ()
	{
		yield return new WaitForSeconds (TIMER);

		string timeLabel = ConvertSecondToTime (LOVE_RECOVERY);
		if (love < Config.LOVE_MAX) {
			if (loveTime < 0) {
				InitLove ();
				timeLabel = "00:00";
			} else {
				string temp = ConvertSecondToTime (loveTime + 1);
				timeLabel = temp.Substring (0, 2) + ":" + temp.Substring (2);
				StartCoroutine (TimerLove ());
			}
			loveTime--;
			if (loveTimeUILabel != null) {
				loveTimeUILabel.text = timeLabel;
			}
		}

		if (loveCountUILabel != null) {
			loveCountUILabel.text = "" + love + "/" + Config.LOVE_MAX;
		}
	}

	public bool UseLove ()
	{
		if (love > 0) {
			love -= 1;
			if (love == Config.LOVE_MAX - 1) {
				SetLove (love, dataQuery.Date ());
				InitLove ();
			} else {
				SetLove (love);
			}
			return true;
		}
		return false;
	}
}
