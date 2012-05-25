using UnityEngine;
using System.Collections;

// Hacky sounds woo
// 

public class SoundManager : MonoBehaviour
{
	public AudioClip m_footsteps;
	public AudioClip m_buttonPress;
	public AudioClip m_crateHit;
	public AudioClip m_spikeKill;
	public AudioClip m_squash;
	public AudioClip m_teleLoop;
	public AudioClip m_thud;
	public AudioClip m_bgMusic;

	public GameObject m_emitterPrefab;

	// Use this for initialization
	void Start()
	{
		PlaySound(SoundType.ST_BGMUSIC, true, transform.position);
	}

	// Update is called once per frame
	void Update()
	{

	}

	// Plays a sound from a target location
	public GameObject PlaySound(SoundType a_sound, bool a_loop, Vector3 a_pos)
	{
		AudioClip clip = null;
		float volume = 1.0f;
		switch (a_sound)
		{
			case SoundType.ST_FOOTSTEPS:
				clip = m_footsteps;
				break;
			case SoundType.ST_BUTTONPRESS:
				clip = m_buttonPress;
				break;
			case SoundType.ST_CRATEHIT:
				clip = m_crateHit;
				break;
			case SoundType.ST_SPIKEKILL:
				clip = m_spikeKill;
				volume = 0.2f;
				break;
			case SoundType.ST_SQUASH:
				clip = m_squash;
				break;
			case SoundType.ST_TELELOOP:
				clip = m_teleLoop;
				break;
			case SoundType.ST_THUD:
				clip = m_thud;
				break;
			case SoundType.ST_BGMUSIC:
				clip = m_bgMusic;
				volume = 0.1f;
				break;
		}

		if (clip != null)
		{
			//AudioSource.PlayClipAtPoint(clip, a_pos);
			GameObject tempObj = (GameObject)Instantiate(m_emitterPrefab);
			AudioSource source = tempObj.GetComponent<AudioSource>();
			tempObj.transform.position = a_pos;
			source.clip = clip;
			source.loop = a_loop;
			source.volume = volume;
			source.Play();
			if (!a_loop)
			{
				Destroy(tempObj, clip.length);	// Destroy it when it's done so it doesn't have to be handled elsewhere if we don't want to deal with it
			}
			return tempObj;
		}

		return null;	// No audio played.
	}
}
