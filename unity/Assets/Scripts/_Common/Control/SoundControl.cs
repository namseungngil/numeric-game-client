using UnityEngine;
using System.Collections;

public class SoundControl : MonoBehaviour
{
	private AudioSource audioSource;

	void Awake ()
	{
		Register register = Register.Instance ();
		Debug.Log (register.GetBackSound ());

		audioSource = gameObject.GetComponent<AudioSource> ();
		audioSource.playOnAwake = register.GetBackSound ();
	}

	public void Set (bool flag)
	{
		if (flag) {
			audioSource.Play ();
		} else {
			audioSource.Stop ();
		}
	}
}
