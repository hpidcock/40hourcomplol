using UnityEngine;
using System.Collections;

public class AudioPlayer : MonoBehaviour {
	
	public AudioSource m_source;
	SoundManager m_soundManager;

	// Use this for initialization
	void Start () {
		m_source = GetComponent<AudioSource>();
		if (m_source == null)
		{
			throw new UnityException("No AudioSource. Attach it to " + gameObject.name);
		}
		
		m_source.rolloffMode = AudioRolloffMode.Custom;
		m_source.maxDistance = 1500.0f;
		
		m_soundManager = (SoundManager)FindSceneObjectsOfType(typeof(SoundManager))[0];
		if (m_soundManager == null)
		{
			throw new UnityException("No SoundManager. Attach it to CoreScripts");
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
//	public void PlaySound(SoundType a_sound, bool a_loop)
//	{	
//		audio.loop = a_loop;
//		audio.clip = m_soundManager.GetSound(a_sound);
//		audio.Play();
//		Debug.Log("Play");
//	}
}
