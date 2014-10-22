using UnityEngine;
using System.Collections;

public class NumericPlayerPrefs : MonoBehaviour
{
	// const
	private const string LOVE = "Love";
	private const string LOVE_FLAG = "LoveFlag";
	private const string LOVE_TIME = "LoveTime";
	private const string LOVE_TIME_ERROR = "99999999999999";
	private const int LOVE_FLAG_TRUE = 1;
	// static
	private static NumericPlayerPrefs instance;

	public static NumericPlayerPrefs Instance ()
	{
		if (instance == null) {
			instance = new NumericPlayerPrefs ();
			if (PlayerPrefs.GetInt (LOVE_FLAG) != LOVE_FLAG_TRUE) {

				instance.SetLove (Config.LOVE_MAX);
				PlayerPrefs.SetInt (LOVE_FLAG, LOVE_FLAG_TRUE);
			}
		}

		return instance;
	}

	public void SetLove (int love)
	{
		PlayerPrefs.SetInt (LOVE, love);
	}

	public void SetLove (int love, string loveTime)
	{
		SetLove (love);
		PlayerPrefs.SetString (LOVE_TIME, loveTime);
	}

	public int GetLove ()
	{
		return PlayerPrefs.GetInt (LOVE);
	}

	public string GetLoveTime ()
	{
		return PlayerPrefs.GetString (LOVE_TIME, LOVE_TIME_ERROR);
	}
}
