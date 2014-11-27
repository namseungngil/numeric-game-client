using UnityEngine;
using System.Collections;

public class SceneComponent : MonoBehaviour
{
	public Texture fadeTexture;
	internal Color startColor = new Color (0, 0, 0, 1);
	internal Color endColor = new Color (0, 0, 0, 0);
	internal Color currentColor;
	internal float time = 3.0f;

	void Start ()
	{
		currentColor = startColor;
		Destroy (gameObject, time + 1);
	}

	void OnGUI ()
	{
		GUI.depth = -10;
		GUI.color = currentColor;
		GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), fadeTexture);
	}

	void FixedUpdate ()
	{
		currentColor = Color.Lerp (startColor, endColor, Time.time / time);
	}
}
