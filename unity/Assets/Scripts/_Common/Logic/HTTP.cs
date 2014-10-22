using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace HTTP
{
	/// <summary>
	/// A handy class to use WWW class and WWWForm class.
	/// 
	/// Features:
	/// 
	/// * Use callback (delegate) instead of coroutine. Of course this class uses 
	///   coroutine internally not to stop other processes.
	/// * Can use timeout. 
	/// * Handle complex constructor of WWW class.
	/// 
	/// Requirements:
	/// 
	/// * Unity 4.5
	/// 
	/// Example usage:
	/// 
	/// using WWWKit;
	/// public class WWWClientExample : MonoBehaviour
	/// {
	///     void Start()
	///     {
	///         // GET request
	///         WWWClient client = new WWWClient(this, "http://example.com/");
	///         client.OnDone = (WWW www) => {
	///             Debug.Log(www.text);
	///         };
	///         client.Request();
	/// 
	///         // POST request
	///         WWWClient http = new WWWClient(this, "http://example.com/");
	///         client.AddData("foo", "bar");
	///         client.OnDone = (WWW www) => {
	///             Debug.Log(www.text);
	///         };
	///         client.Request();
	/// 
	///         // POST request with binary data (file attachment)
	///         byte[] binary = System.Text.Encoding.Unicode.GetBytes("bar");
	///         WWWClient http = new WWWClient(this, "http://example.com/");
	///         client.AddBinaryData("foo", binary, "test.txt", "application/octet-stream");
	///         client.OnDone = (WWW www) => {
	///             Debug.Log(www.text);
	///         };
	///         client.Request();
	/// 
	///         // Handle error
	///         client.OnFail = (WWW www) => {
	///             Debug.Log(www.error);
	///         };
	/// 
	///         // Handle timed out
	///         client.OnDisposed = () => {
	///             Debug.Log("Timed out");
	///         };
	/// 
	///         // Set timeout time (default is infinity)
	///         client.Timeout = 10f;
	/// 
	///         // Add header
	///         client.AddHeader("Cookie", "cookiename=cookievalue");   
	///     }
	/// }
	/// </summary>
	public class WWWClient
	{
		public delegate void FinishedDelegate (WWW www);
		
		public delegate void DisposedDelegate ();
		
		private MonoBehaviour monoBehaviour;
		private string url;
		private WWW wWW;
		private WWWForm wWWFrom;
		private Dictionary<string, string> headers;
		private float timeOut;
		private FinishedDelegate finishedDelegateOnDone;
		private FinishedDelegate finishedDelegateOnFail;
		private DisposedDelegate disposedDelegateOnDisposed;
		private bool mDisposed;
		
		public Dictionary<string, string> Headers {
			set {
				headers = value;
			}
			get {
				return headers;
			}
		}
		
		public float Timeout {
			set {
				timeOut = value;
			}
			get {
				return timeOut;
			}
		}
		
		public FinishedDelegate OnDone {
			set {
				finishedDelegateOnDone = value;
			}
		}
		
		public FinishedDelegate OnFail {
			set {
				finishedDelegateOnFail = value;
			}
		}
		
		public DisposedDelegate OnDisposed {
			set {
				disposedDelegateOnDisposed = value;
			}
		}
		
		public WWWClient (MonoBehaviour mB, string u)
		{
			monoBehaviour = mB;
			url = u;
			headers = new Dictionary<string, string> ();
			wWWFrom = new WWWForm ();
			timeOut = 5f;
			mDisposed = false;
		}
		
		public void AddHeader (string headerName, string value)
		{
			headers.Add (headerName, value);
		}
		
		public void AddData (string fieldName, string value)
		{
			wWWFrom.AddField (fieldName, value);
		}
		
		public void AddBinaryData (string fieldName, byte[] contents)
		{
			wWWFrom.AddBinaryData (fieldName, contents);
		}
		
		public void AddBinaryData (string fieldName, byte[] contents, string fileName)
		{
			wWWFrom.AddBinaryData (fieldName, contents, fileName);
		}
		
		public void AddBinaryData (string fieldName, byte[] contents, string fileName, string mimeType)
		{
			wWWFrom.AddBinaryData (fieldName, contents, fileName, mimeType);
		}
		
		public void Request ()
		{
			monoBehaviour.StartCoroutine (RequestCoroutine ());
		}
		
		public void Dispose ()
		{
			if (wWW != null && !mDisposed) {
				wWW.Dispose ();
				mDisposed = true;
			}
		}
		
		private IEnumerator RequestCoroutine ()
		{
			if (wWWFrom.data.Length > 0) {
				// Overwrite added headers with WWWForm.headers because WWWForm.headers may have required
				// headers to request. For example, WWWForm.headers has Content-Type like
				// 'multipart/form-data; boundary="xxxx"' if WWWForm.AddBinaryData() is called.
				foreach (DictionaryEntry entry in wWWFrom.headers) {
					headers [System.Convert.ToString (entry.Key)] = System.Convert.ToString (entry.Value);
				}
				
				// POST request
				wWW = new WWW (url, wWWFrom.data, headers);
			} else {
				// GET request
				wWW = new WWW (url, null, headers);
			}
			
			yield return monoBehaviour.StartCoroutine (CheckTimeout ());
			
			if (mDisposed) {
				if (disposedDelegateOnDisposed != null) {
					disposedDelegateOnDisposed ();
				}
			} else if (System.String.IsNullOrEmpty (wWW.error)) {
				if (finishedDelegateOnDone != null) {
					finishedDelegateOnDone (wWW);
				}
			} else {
				if (finishedDelegateOnFail != null) {
					finishedDelegateOnFail (wWW);
				}
			}
		}
		
		private IEnumerator CheckTimeout ()
		{
			float startTime = Time.time;
			
			while (!mDisposed && !wWW.isDone) {
				if (timeOut > 0 && (Time.time - startTime) >= timeOut) {
					Dispose ();
					break;
				} else {
					yield return null;
				}
			}	
			yield return null;
		}	
	}
}