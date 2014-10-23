using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace ASSETBUNDLE
{
	public class AssetBundleClient
	{
		// class
		private class AssetBundleRef
		{
			public AssetBundle assetBundle = null;
			public string url;
			public int version;
			public AssetBundleRef (string u, int v) {
				url = u;
				version = v;
			}
		}
		// delegate
		public delegate void FinishedDelegate (WWW www);
		private FinishedDelegate mOnDone;
		private FinishedDelegate mOnFail;

		public FinishedDelegate OnDone {
			set {
				mOnDone = value;
			}
		}
		
		public FinishedDelegate OnFail {
			set {
				mOnFail = value;
			}
		}

		// component
		MonoBehaviour monoBehaviour;
		// variable
		public float progress;
		// array
		private static Dictionary<string, AssetBundleRef> dictionaryAssetBundleRef;
		// constructor
		public AssetBundleClient ()
		{
			progress = 0;
			dictionaryAssetBundleRef = new Dictionary<string, AssetBundleRef> ();
		}

		// get an assetbundle
		public AssetBundle GetAssetBundle (string url, int version)
		{
			string keyName = url + version.ToString ();
			AssetBundleRef assetBundleRef;
			if (dictionaryAssetBundleRef.TryGetValue (keyName, out assetBundleRef)) {
				return assetBundleRef.assetBundle;
			} else {
				return null;
			}
		}

		//download and assetbundle
		public IEnumerator DownloadAssetBundle (string url, int version)
		{
			string keyName = url + version.ToString ();
			if (dictionaryAssetBundleRef.ContainsKey (keyName)) {
				yield return null;
			} else {
				using (WWW www = WWW.LoadFromCacheOrDownload (url, version)) {
					while (!www.isDone) {
						progress = www.progress;
					}
					yield return www;
					if (www.error != null) {
						Debug.LogError ("WWW download error : " + www.error);
						if (mOnFail != null) {
							mOnFail (www);
						}

					} else {
						AssetBundleRef assetBundleRef = new AssetBundleRef (url, version);
						assetBundleRef.assetBundle = www.assetBundle;
						dictionaryAssetBundleRef.Add (keyName, assetBundleRef);
						if (mOnDone != null) {
							mOnDone (www);
						}
					}
				}
			}
		}

		// unload an assetbundle
		public void unload (string url, int version, bool allFlag)
		{
			string keyName = url + version.ToString ();
			AssetBundleRef assetBundleRef;
			if (dictionaryAssetBundleRef.TryGetValue (keyName, out assetBundleRef)) {
				assetBundleRef.assetBundle.Unload (allFlag);
				assetBundleRef.assetBundle = null;
				dictionaryAssetBundleRef.Remove (keyName);
			}
		}
	}
}
