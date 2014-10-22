using UnityEngine;
using System.Collections;

public enum UIStatus
{
	Default = 1,
	Notification
}

public class UIManager : MonoBehaviour
{
	public const float WINDOW_OPEN_TIME = 0.15f;
	protected const string BACKGROUND = "Background";
	protected const string CANCEL = "Cancel";
	protected const float WINDOW_FORWARD_TIME = 0.3f;
	protected UIStatus uiStatus;
	protected GameObject panel100;
	protected GameObject panel200;
	protected Animation backgroundAnimation;

	private IEnumerator DelayAnimation (GameObject gO, float time) {
		yield return new WaitForSeconds (time);
		gO.SetActive (true);
		Animation anim = gO.GetComponent<Animation> ();
		if (anim != null) {
			anim.Play (Config.ANIMATION_BUTTON);
		}
	}

	protected void Start ()
	{
		uiStatus = UIStatus.Default;
		
		panel100 = GameObject.Find (Config.PANEL100);
		panel200 = GameObject.Find (Config.PANEL200);
		
		panel200.SetActive (false);
	}
	
	protected IEnumerator GameObjectSetActive (GameObject gO, float time, GameObject startGO = null)
	{
		yield return new WaitForSeconds (time);
		gO.SetActive (false);
		
		if (startGO == null) {
			panel100.SetActive (true);
		} else {
			CommonNotification (startGO);
		}
	}

	protected void CommonNotification (GameObject gO)
	{
		if (uiStatus != UIStatus.Default) {
			return;
		}

		uiStatus = UIStatus.Notification;
		gO.SetActive (true);
		PlayAnimation (gO, true);

		panel100.SetActive (false);
	}

	protected void CommonCancel (GameObject gO, GameObject startGO = null)
	{
		if (uiStatus != UIStatus.Notification) {
			return;
		}

		uiStatus = UIStatus.Default;
		PlayAnimation (gO);
		
		StartCoroutine (GameObjectSetActive (gO, WINDOW_FORWARD_TIME, startGO));
	}

	public void PlayAnimation (GameObject gO, bool flag = false) {
		float time = WINDOW_OPEN_TIME;
		foreach (Transform gObj in gO.GetComponentsInChildren<Transform> ()) {
			if (gObj.name == BACKGROUND) {
				gObj.gameObject.GetComponent<Animation> ().Play (Config.ANIMATION_CHECKMARK);
			} else if (gObj.name != CANCEL) {
				if (flag) {
					gObj.gameObject.SetActive (false);
					time += 0.1f;
					StartCoroutine (DelayAnimation (gObj.gameObject, time));
				}
			}
		}
	}

	public void Setting ()
	{
		CommonNotification (panel200);
	}

	public void Cancel ()
	{
		CommonCancel (panel200);
	}

	public void SoundBack ()
	{
	}
	
	public void SoundButton ()
	{
	}
	
	public void Guide ()
	{
	}
	
	public void Board ()
	{
		Application.OpenURL (Config.BOARD_URL);
	}
}
