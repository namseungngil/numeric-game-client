using UnityEngine;
using System;
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
	// quest_master_version
	public string masterVersion = "master_version";
	public string version = "version";
	public string[] masterVersionColumnName;
	// quest_master
	public string questMaster = "quest_master";
	public static string questStage = "stage";
	public string questCard = "card";
	public string questLevel = "level";
	public string questDate = "date";
	public string[] questMasterColumnName;
	// quest_user
	public string questUser = "quest_user";
	public string questTime = "time";
	public static string questClear = "clear";
	public string questMiss = "miss";
	public string questHit = "hit";
	public string[] questUserColumnName;

	private QueryModel ()
	{
		masterVersionColumnName = new string[] {version, questDate};
		questMasterColumnName = new string[] {questStage, questCard, questLevel, questDate};
		questUserColumnName = new string[] {questStage, version, questTime, questHit, questClear, questMiss, questDate};
	}

	public string Date ()
	{
		return DateTime.Now.ToString (Config.DATA_TIME);
	}

	public void MasterData ()
	{
		DataArray dataArray = new DataArray ();

		string[][] masterData = dataArray.questMaster;
		string[][] temp = new string[masterData.Length][];

		string date = Date ();
		for (int i = 0; i < masterData.Length; i++) {

			temp[i] = new string[] {masterData[i][0], masterData[i][1], masterData[i][2], date};
		}

		DataTable dataTable = SELECT (masterVersion);
		bool flag = false;
		if (dataTable.Rows.Count <= 0) {
			flag = true;
		} else {
			if (("" + dataTable[0][version]) != dataArray.questMasterVersion) {
				flag = true;
				ALLDELETE (questMaster);
				ALLDELETE (masterVersion);
			}
		}

		if (flag) {
			Debug.Log ("Master Data update");
			INSERT_BATCH (questMaster, questMasterColumnName, temp);
			INSERT (masterVersion, masterVersionColumnName, new string[] {dataArray.questMasterVersion, date});
		}
	}

	public DataTable QuestUser ()
	{
		DataTable dataTable = SELECT (questUser);
		return dataTable;
	}

	public DataTable MypageQuestList ()
	{
		DataTable dataTable = QuestUser ();
		if (dataTable.Rows.Count > 0) {
			Debug.Log ("Quest user > 0");
			string tempStage = "" + dataTable[dataTable.Rows.Count - 1][questStage];
			tempStage = "" + (int.Parse (tempStage) + 1);
			DataTable tempQuestMaster = SELECT (questMaster, "WHERE " + questStage + "=" + tempStage);
			if (tempQuestMaster.Rows.Count > 0) {
				object[] tempObject = new object[questUserColumnName.Length];
				for (int i = 0; i < questUserColumnName.Length; i++) {
					if (i == 0) {
						tempObject[i] = tempStage;
					} else {
						tempObject[i] = "";
					}
				}

				dataTable.AddRow (tempObject);
			}
		}

		Debug.Log (dataTable.Rows.Count);

		return dataTable;
	}

	public DataTable MypageQuestDetail (string stage)
	{
		DataTable dataTable = SELECT (questMaster, "WHERE " + questStage + "=" + stage);

		return dataTable;
	}

	// new string[] {questStage, version, questTime, questHit, questClear, questMiss, questDate};
	public bool BattleClear (string stage, string time, string hit, string clear, string miss, string date = "")
	{
		DataTable dataTable = SELECT (questUser, "WHERE " + questStage + "=" + stage);
		if (date == "") {
			date = Date ();
		}
		string[] data = new string[] {stage, "1", time, hit, clear, miss, date};
		if (dataTable.Rows.Count > 0) {
			bool flag = false;
			if (int.Parse (time) < (int)dataTable[0][questTime]) {
				flag = true;
			} else {
				data[2] = (string)dataTable[0][questTime];
			}

			if (int.Parse (hit) > (int)dataTable[0][questHit]) {
				flag = true;
			} else {
				data[3] = (string)dataTable[0][questHit];
			}

			if (int.Parse (clear) > (int)dataTable[0][questClear]) {
				flag = true;
			} else {
				data[4] = (string)dataTable[0][questClear];
			}

			if (int.Parse (miss) < (int)dataTable[0][questMiss]) {
				flag = true;
			} else {
				data[5] = (string)dataTable[0][questMiss];
			}

			if (flag) {
				int tempVersion = (int)dataTable[0][version] + 1;
				if (tempVersion > Config.MAX_VERSION) {
					tempVersion = 1;
				}
			
				data[1] = "" + tempVersion;
				return UPDATE (questUser, questUserColumnName, data, "WHERE " + questStage + "=" + stage);
			} else {
				return true;
			}
		} else {
			return INSERT (questUser, questUserColumnName, data);
		}
	}
	
/*
 * Sample
 * */
	public void DBInsert ()
	{
		string[] data = new string[] {"9", "10", "3", "20140925172600"};
		
		INSERT (questUser, questUserColumnName, data);
	}
	
	public void DBSelect (GameObject gO)
	{
		DataTable dataTable = SELECT (questUser);
		
		Debug.Log (dataTable.Rows.Count);
		string temp = "" + (int)dataTable[0]["stage"] + "//" + (int)dataTable[0]["time"];
		Debug.Log (temp);
		gO.GetComponentInChildren<UILabel> ().text = temp;
	}
	
	public void DBDelete ()
	{
		ALLDELETE (questUser);
	}
}
