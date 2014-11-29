using UnityEngine;
using System;
using System.Collections.Generic;
using DATA;

public class QueryModel : Query
{
	// instance
	private static QueryModel instance;
	public static QueryModel Instance ()
	{
		if (instance == null) {
			instance = new QueryModel ();
		}

		return instance;
	}
	// quest_user
	public const string QUEST_USER = "quest_user";
	public const string STAGE = "stage";
	public const string VERSION = "version";
	public const string SCORE = "score";
	public const string TIME = "time";
	public const string CLEAR = "clear";
	public const string MISS = "miss";
	public const string HIT = "hit";
	public const string DATE = "date";
	public string[] questUserColumnName;

	private QueryModel ()
	{
		questUserColumnName = new string[] {STAGE, VERSION, SCORE, TIME, HIT, CLEAR, MISS, DATE};
	}

	public string Date ()
	{
		return DateTime.Now.ToString (Config.DATA_TIME);
	}

	public DataTable MypageQuestList (int index)
	{
		List<int> list = Game.Quest (index); 

//		DataTable dataTable = SELECT (QUEST_USER, "WHERE " + STAGE + ">=" + list [0] + " AND " + STAGE + "<=" + list [1] + " ORDER BY ASC");
		DataTable dataTable = SELECT (QUEST_USER, "WHERE " + STAGE + ">=" + list [0] + " AND " + STAGE + "<=" + list [1]);

		return dataTable;
	}

	public DataTable MypageStage (int stage)
	{
		DataTable dataTable = SELECT (QUEST_USER, "WHERE " + STAGE + "=" + stage);

		return dataTable;
	}

	// new string[] {questStage, version, questScore, questTime, questHit, questClear, questMiss, questDate};
	public string[] BattleClear (string stage, string score, string time, string hit, string clear, string miss, string date = "")
	{
		DataTable dataTable = SELECT (QUEST_USER, "WHERE " + STAGE + "=" + stage);
		if (date == "") {
			date = Date ();
		}

		bool returnflag = true;
		string[] data = new string[] {stage, "1", score, time, hit, clear, miss, date};
		if (dataTable.Rows.Count > 0) {
			bool flag = false;

			if (int.Parse (score) > (int)dataTable[0][SCORE]) {
				if (int.Parse (score) > Config.MAX_VALUE) {
					score = Config.MAX_VALUE.ToString ();
				}
				flag = true;
			} else {
				data[2] = dataTable[0][SCORE].ToString ();
			}

			if (int.Parse (time) < (int)dataTable[0][TIME]) {
				flag = true;
			} else {
				data[3] = dataTable[0][TIME].ToString ();
			}

			if (int.Parse (hit) > (int)dataTable[0][HIT]) {
				flag = true;
			} else {
				data[4] = dataTable[0][HIT].ToString ();
			}

			if (int.Parse (clear) > (int)dataTable[0][CLEAR]) {
				flag = true;
			} else {
				data[5] = dataTable[0][CLEAR].ToString ();
			}

			if (int.Parse (miss) < (int)dataTable[0][MISS]) {
				flag = true;
			} else {
				data[6] = dataTable[0][MISS].ToString ();
			}

			if (flag) {
				int tempVersion = (int)dataTable[0][VERSION] + 1;
				if (tempVersion > Config.MAX_VALUE) {
					tempVersion = 1;
				}
			
				data[1] = "" + tempVersion;
				returnflag = UPDATE (QUEST_USER, questUserColumnName, data, "WHERE " + STAGE + "=" + stage);
			} else {
				returnflag = false;
			}

		} else {
			returnflag = INSERT (QUEST_USER, questUserColumnName, data);
		}

		return data;
	}

	public void DBDelete ()
	{
		ALLDELETE (QUEST_USER);
	}

	// dummy
	public void DummyData ()
	{
		int stage = 100;
		for (int i = 0; i < stage; i++) {
			BattleClear (
				(i + 9).ToString (),
				(99999).ToString (),
				(99999).ToString (),
				(99999).ToString (),
				(99999).ToString (),
				(0).ToString ()
				);
		}
	}
}
