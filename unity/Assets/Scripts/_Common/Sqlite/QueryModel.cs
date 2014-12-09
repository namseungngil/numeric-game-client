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
		questUserColumnName = new string[] {
			STAGE,
			VERSION,
			SCORE,
			TIME,
			HIT,
			CLEAR,
			MISS,
			DATE
		};
	}

	public string Date ()
	{
		return DateTime.Now.ToString (Config.DATA_TIME);
	}

	public void SyncGet (Dictionary<string, string> d)
	{
		List<string> allData = new List<string> ();
		string[] stageData = new string[d.Count];
		int i = 0;
		foreach (KeyValuePair<string, string> kVP in d) {
			stageData [i] = kVP.Key;
			allData.Add (kVP.Key);
			i++;
		}

		DataTable dataTable = SELECT_SYNC (QUEST_USER, STAGE, stageData);
		if (dataTable.Rows.Count > 0) {
			Dictionary<string, string> tempDic = new Dictionary<string, string> ();
			for (int j = 0; j < dataTable.Rows.Count; j++) {
				if ((int)dataTable [j] [QueryModel.SCORE] < int.Parse (d [dataTable [j] [QueryModel.STAGE].ToString ()])) {
					tempDic.Add (dataTable [j] [QueryModel.STAGE].ToString (), dataTable [j] [QueryModel.SCORE].ToString ());
				}

				d.Remove (dataTable [j] [QueryModel.STAGE].ToString ());
			}

			if (tempDic.Count > 0) {
				string[] whereData = new string[dataTable.Rows.Count];
				string[] dataData = new string[dataTable.Rows.Count];

				int index = 0;
				foreach (KeyValuePair<string, string> kVP in tempDic) {
					whereData [index] = kVP.Key;
					dataData [index] = kVP.Value;
					index++;
				}

				UPDATE_BATCH (QUEST_USER, STAGE, SCORE, whereData, dataData);
			}
		}

		allData.Sort ();
		int stage = Config.CARD_COUNT;
		Dictionary<string, string> dic = new Dictionary<string, string> ();
		foreach (string value in stageData) {
			if (value == stage.ToString ()) {

			} else {
				for (int n = stage; n < int.Parse (value); n++) {
					List<int> scoreList = Game.Score (n);
					dic.Add (n.ToString (), scoreList [0].ToString ());
					stage++;
				}
			}
			stage++;
		}

		string[][] data = new string[d.Count + dic.Count][];
		int k = 0;
		string date = Date ();

		foreach (KeyValuePair<string, string> kvP in d) {
			data [k] = new string[] {
				kvP.Key, "1", kvP.Value, "0", "0", "0", "0", date
			};
			k++;
		}

		if (dic.Count > 0) {
			foreach (KeyValuePair<string, string> kvP in dic) {
				data [k] = new string[] {
					kvP.Key, "1", kvP.Value, "0", "0", "0", "0", date
				};
				k++;
			}
		}

		if (data.Length > 0) {
			INSERT_BATCH (QUEST_USER, questUserColumnName, data);
		}
	}

	public DataTable QuestList ()
	{
		DataTable dataTable = SELECT (QUEST_USER);
		return dataTable;
	}

	public DataTable MypageQuestList (int index)
	{
		List<int> list = Game.Quest (index); 

//		DataTable dataTable = SELECT (QUEST_USER, "WHERE " + STAGE + ">=" + list [0] + " AND " + STAGE + "<=" + list [1] + " ORDER BY ASC");
		DataTable dataTable = SELECT (QUEST_USER, "WHERE " + STAGE + ">=" + list [0] + " AND " + STAGE + "<=" + list [1]);

		return dataTable;
	}

	public DataTable MypageStage (string stage)
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
		string[] data = new string[] {
			stage,
			"1",
			score,
			time,
			hit,
			clear,
			miss,
			date
		};
		if (dataTable.Rows.Count > 0) {
			bool flag = false;

			if (int.Parse (score) > (int)dataTable [0] [SCORE]) {
				if (int.Parse (score) > Config.MAX_VALUE) {
					score = Config.MAX_VALUE.ToString ();
				}
				flag = true;
			} else {
				data [2] = dataTable [0] [SCORE].ToString ();
			}

			if (int.Parse (time) < (int)dataTable [0] [TIME]) {
				flag = true;
			} else {
				data [3] = dataTable [0] [TIME].ToString ();
			}

			if (int.Parse (hit) > (int)dataTable [0] [HIT]) {
				flag = true;
			} else {
				data [4] = dataTable [0] [HIT].ToString ();
			}

			if (int.Parse (clear) > (int)dataTable [0] [CLEAR]) {
				flag = true;
			} else {
				data [5] = dataTable [0] [CLEAR].ToString ();
			}

			if (int.Parse (miss) < (int)dataTable [0] [MISS]) {
				flag = true;
			} else {
				data [6] = dataTable [0] [MISS].ToString ();
			}

			if (flag) {
				int tempVersion = (int)dataTable [0] [VERSION] + 1;
				if (tempVersion > Config.MAX_VALUE) {
					tempVersion = 1;
				}
			
				data [1] = "" + tempVersion;
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

		string [][] data = new string [stage][];

		for (int i = 0; i < stage; i++) {
			data [i] = new string [] {
				(i + 9).ToString (),
				(99999).ToString (),
				(99999).ToString (),
				(99999).ToString (),
				(99999).ToString (),
				(0).ToString ()
			};
		}

		INSERT_BATCH (QUEST_USER, questUserColumnName, data);
	}
}
