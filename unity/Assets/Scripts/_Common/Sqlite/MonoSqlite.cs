﻿//using UnityEngine;
//using System;
//using Mono.Data.Sqlite;
//
//public class MonoSqlite
//{
//	private SqliteConnection dbConnection;
//	private SqliteCommand dbCommand;
//	private SqliteDataReader reader;
//
//	/// <summary>
//	///  string to connect. The simpliest one looks like "URI=file:filename.db"
//	/// </summary>
//	public void DbAccess (string connectionString)
//	{
//		OpenDB (connectionString);
//	}
//	
//	public void OpenDB (string connectionString)
//	{
//		dbConnection = new SqliteConnection (connectionString);
//		dbConnection.Open ();
//		Debug.Log ("Connected to db");
//	}
//	
//	public void CloseSqlConnection ()
//	{
//		if (dbCommand != null) {
//			dbCommand.Dispose ();
//		}
//		dbCommand = null;
//		if (reader != null) {
//			reader.Dispose ();
//		}
//		reader = null;
//		if (dbConnection != null) {
//			dbConnection.Close ();
//		}
//		dbConnection = null;
//		Debug.Log ("Disconnected from db.");
//	}
//	
//	public SqliteDataReader CreateTable (string name, string[] col, string[] colType)
//	{
//		if (col.Length != colType.Length) {
//			throw new SqliteException ("columns.Length != colType.Length");
//		}
//		string query = "CREATE TABLE " + name + " (" + col[0] + " " + colType[0];
//		for (int i = 1; i < col.Length; ++i) {
//			query += ", " + col[i] + " " + colType[i];
//		}
//		query += ")";
//		return ExecuteQuery (query);
//	}
//	
//	public SqliteDataReader ExecuteQuery (string sqlQuery)
//	{
//		dbCommand = dbConnection.CreateCommand ();
//		dbCommand.CommandText = sqlQuery;
//		
//		reader = dbCommand.ExecuteReader ();
//
//		return reader;
//	}
//	
//	public SqliteDataReader ReadFullTable (string tableName)
//	{
//		string query = "SELECT * FROM " + tableName;
//		return ExecuteQuery (query);
//	}
//	
//	public SqliteDataReader InsertInto (string tableName, string[] values)
//	{
//		string query = "INSERT INTO " + tableName + " VALUES (" + values[0];
//		for (int i = 1; i < values.Length; ++i) {
//			query += ", " + values[i];
//		}
//		query += ");";
//		return ExecuteQuery (query);
//	}
//	
//	public SqliteDataReader InsertIntoSpecific (string tableName, string[] cols, string[] values)
//	{
//		if (cols.Length != values.Length) {
//			throw new SqliteException ("columns.Length != values.Length");
//		}
//		string query = "INSERT INTO " + tableName + "(" + cols[0];
//		for (int i = 1; i < cols.Length; ++i) {
//			query += ", " + cols[i];
//		}
//		query += ") VALUES (" + values[0];
//		for (int i = 1; i < values.Length; ++i) {
//			query += ", " + values[i];
//		}
//		query += ")";
//		return ExecuteQuery (query);
//	}
//	
//	public SqliteDataReader DeleteContents (string tableName)
//	{
//		string query = "DELETE FROM " + tableName;
//		return ExecuteQuery (query);
//	}
//	
//	public SqliteDataReader SelectWhere (string tableName, string[] items, string[] col, string[] operation, string[] values)
//	{
//		if (col.Length != operation.Length || operation.Length != values.Length) {
//			throw new SqliteException ("col.Length != operation.Length != values.Length");
//		}
//		string query = "SELECT " + items[0];
//		for (int i = 1; i < items.Length; ++i) {
//			query += ", " + items[i];
//		}
//		query += " FROM " + tableName + " WHERE " + col[0] + operation[0] + "'" + values[0] + "' ";
//		for (int i = 1; i < col.Length; ++i) {
//			query += " AND " + col[i] + operation[i] + "'" + values[0] + "' ";
//		}
//		
//		return ExecuteQuery (query);	
//	}
//}
