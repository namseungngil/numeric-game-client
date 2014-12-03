using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneManager : SSSceneManager
{
	// const
	private string BACKGROUND = "Background";
	[SerializeField]
	public AudioClip[]
		m_Clips;
	// array
	private Dictionary<string, AudioClip> m_Audios;
	
	protected override void Awake ()
	{
		base.Awake ();
		
		m_SolidCamera.camera.tag = "MainCamera";
		m_SolidCamera.AddComponent<AudioListener> ();
		m_SolidCamera.AddComponent<AudioSource> ();
		m_SolidCamera.audio.loop = true;
		
		m_Audios = new Dictionary<string, AudioClip> ();
		m_Audios.Add (BACKGROUND, m_Clips [0]);

		PlayBGM (BACKGROUND);
	}
	
	protected override void PlayBGM (string bgmName)
	{
		AudioSource source = Camera.main.audio;
		AudioClip clip = source.clip;
		
		if (clip != null && clip.name == bgmName && source.isPlaying) {
			return;
		}

		source.clip = m_Audios [bgmName];
		source.clip.name = bgmName;

		Register register = Register.Instance ();
		if (!register.GetBackSound ()) {
			return;
		}

		source.Play ();
	}
	
	protected override void StopBGM ()
	{
		Camera.main.audio.Stop ();
	}
	
	protected override void OnAnimationFinish (string sceneName)
	{
	}
}
