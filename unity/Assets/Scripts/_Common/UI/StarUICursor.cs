using UnityEngine;

[RequireComponent(typeof(UISprite))]
[AddComponentMenu("NGUI/Star/UICursor")]
public class StarUICursor : MonoBehaviour
{
	public static StarUICursor instance;
	
	// Camera used to draw this cursor
	public Camera uiCamera;
	
	Transform mTrans;
	UISprite mSprite;
	
	UIAtlas mAtlas;
	string mSpriteName;
	
	void Awake () { instance = this; }
	void OnDestroy () { instance = null; }
	
	void Start ()
	{
		mTrans = transform;
		mSprite = GetComponent<UISprite> ();
		
		if (uiCamera == null)
			uiCamera = NGUITools.FindCameraForLayer(gameObject.layer);
		
		if (mSprite != null)
		{
			mAtlas = mSprite.atlas;
			mSpriteName = mSprite.spriteName;
//			if (mSprite.depth < 100) mSprite.depth = 100;
		}
	}
	
	void Update ()
	{
		if (uiCamera == null)
			uiCamera = NGUITools.FindCameraForLayer(gameObject.layer);

		Vector3 pos = Input.mousePosition;
		
		if (uiCamera != null)
		{
			pos.x = Mathf.Clamp01(pos.x / Screen.width);
			pos.y = Mathf.Clamp01(pos.y / Screen.height);
			mTrans.position = uiCamera.ViewportToWorldPoint(pos);

			if (uiCamera.isOrthoGraphic)
			{
				Vector3 lp = mTrans.localPosition;
				lp.x = Mathf.Round(lp.x);
				lp.y = Mathf.Round(lp.y);
				mTrans.localPosition = lp;
			}
		}
		else
		{
			pos.x -= Screen.width * 0.5f;
			pos.y -= Screen.height * 0.5f;
			pos.x = Mathf.Round(pos.x);
			pos.y = Mathf.Round(pos.y);
			mTrans.localPosition = pos;
		}
	}

	public static void Clear ()
	{
		if (instance != null && instance.mSprite != null)
			Set(instance.mAtlas, instance.mSpriteName);
	}
	
	public static void Set (UIAtlas atlas, string sprite)
	{
		Debug.Log ("Set");
		if (instance != null && instance.mSprite)
		{
			instance.mSprite.atlas = atlas;
			instance.mSprite.spriteName = sprite;
//			instance.mSprite.MakePixelPerfect();
			instance.Update();
		}
	}
}
