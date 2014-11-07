using UnityEngine;
using System.Collections;

public class WebviewComponent : MonoBehaviour
{
	public string url = "http://unity3d.com/";
	public int backgroundX = 50;
	public int backgroundY = 100;
	public int webViewSizeX = 40;
	public int webViewSizeY = 120;
	// component
	private WebViewObject webViewObject;
	private UISprite webViewUISprite;
	
	void Start ()
	{
		webViewUISprite = GameObject.Find ("Background").GetComponent<UISprite> ();
		
		webViewObject = (new GameObject ("WebViewObject")).AddComponent<WebViewObject> ();
		webViewObject.Init ();
		webViewObject.LoadURL (url);
		Open ();
	}
	
	private void Open ()
	{
		int width = Screen.width - backgroundX;
		int height = Screen.height - backgroundY;
		
		UIRoot mRoot = NGUITools.FindInParents<UIRoot> (gameObject);
		float ratio = (float)mRoot.activeHeight / Screen.height;
		
		int NGUIwidth = (int)(Mathf.Ceil (width * ratio));
		int NGUIheight = (int)(Mathf.Ceil (height * ratio));
		
		int x = NGUIwidth / 2;
		int y = NGUIheight / 2;
		
		webViewUISprite.SetRect (-x, -y, NGUIwidth, NGUIheight);
		
		width = width - webViewSizeX;
		height = height - webViewSizeY;
		
		x = ((Screen.width / 2) - (width / 2));
		y = ((Screen.height / 2) - (height / 2));
		
		webViewObject.SetMargins (x, y, x, y);
		webViewObject.SetVisibility (true);
	}
	
	private void Close ()
	{
		webViewObject.SetVisibility (false);
	}
	
	public void On ()
	{
		Open ();
	}
	
	public void Off ()
	{
		Close ();
	}
}
