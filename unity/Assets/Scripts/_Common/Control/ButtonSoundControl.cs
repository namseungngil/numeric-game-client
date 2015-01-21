using UnityEngine;

[AddComponentMenu("NGUI/Interaction/ButtonSoundControl")]
public class ButtonSoundControl : MonoBehaviour
{
	public AudioClip audioClip;
	
	[Range(0f, 1f)] public float volume = 1f;
	[Range(0f, 2f)] public float pitch = 1f;
	
	void OnClick ()
	{
		Register register = Register.Instance ();

		if (register.GetButtonSound ()) {
			NGUITools.PlaySound(audioClip, volume, pitch);
		}
	}
}
