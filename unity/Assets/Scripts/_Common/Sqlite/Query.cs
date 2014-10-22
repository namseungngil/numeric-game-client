using UnityEngine;
using System.Collections;

public class Query
{
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
	// sql
	protected SqliteDatabase sqliteDatabase;

	protected Query ()
	{
		Debug.Log ("Query");
		sqliteDatabase = new SqliteDatabase ("numeric");
		masterVersionColumnName = new string[] {version, questDate};
		questMasterColumnName = new string[] {questStage, questCard, questLevel, questDate};
		questUserColumnName = new string[] {questStage, version, questTime, questHit, questClear, questMiss, questDate};
	}

	private string CommonInsert (string query, string[] sA)
	{
		int index = 0;
		foreach (string s in sA) {
			index++;

			query += s;

			if (sA.Length > index) {
				query += ",";
			}
		}

		return query;
	}

	public bool INSERT_BATCH (string tableName, string[] columnName, string[][] data)
	{
		string query = "INSERT INTO " + tableName + " (";
		query = CommonInsert (query, columnName);
		query += ")VALUES";
		for (int i = 0; i < data.Length; i++) {
			query += "(";
			if (columnName.Length != data[i].Length) {
				return false;
			}
			query = CommonInsert (query, data[i]);
			query += ")";

			if ( i == (data.Length - 1) ) {
				query += ";";
			} else {
				query += ",";
			}
		}
		Debug.Log (query);

		return sqliteDatabase.ExecuteNonQuery (query);
	}

	public bool INSERT (string tableName, string[] columnName, string[] data)
	{
		if (columnName.Length != data.Length) {
			return false;
		}

		string query = "INSERT INTO " + tableName + " (";
		query = CommonInsert (query, columnName);
		query += ")VALUES(";
		query = CommonInsert (query, data);
		query += ")";
		Debug.Log (query);

		return sqliteDatabase.ExecuteNonQuery (query);
	}

	public bool UPDATE (string tableName, string[] columnName, string[] data, string whereQuery)
	{
		if (columnName.Length != data.Length) {
			return false;
		}

		string query = "UPDATE " + tableName + " SET ";
		for (int i = 0; i < columnName.Length; i++) {
			string temp = columnName [i] + "=" + data [i];
			query += temp;
			if (i < (columnName.Length - 1)) {
				query += ",";
			} else {
				query += ";";
			}
		}

		query += " " + whereQuery;

		return sqliteDatabase.ExecuteNonQuery (query);
	}

	public DataTable SELECT (string tableName, string addQuery = null)
	{
		string query = "SELECT * FROM " + tableName;

		if (addQuery != null) {
			query += " " + addQuery;
		}

		query += ";";

		Debug.Log (query);

		DataTable dataTabe = sqliteDatabase.ExecuteQuery (query);

		return dataTabe;
	}

	public bool DELETE ()
	{
		return true;
	}

	public void ALLDELETE (string tableName)
	{
		string query = "DELETE FROM " + tableName;
		sqliteDatabase.ExecuteNonQuery (query);
	}
}
