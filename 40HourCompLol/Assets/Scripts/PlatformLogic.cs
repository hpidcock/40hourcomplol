using UnityEngine;
using System.Collections;

public class PlatformLogic : MonoBehaviour {
	
	public bool m_active = false;
	const float m_sineDampening = 0.005f;
	const float m_speedMultiplier = 4.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
		if (m_active)
		{
			MoFugginSineWave();
		}
	}
	
	void MoFugginSineWave()
	{
		Vector3 pos = transform.position;
		pos.y += Mathf.Sin(Time.realtimeSinceStartup * m_speedMultiplier) * m_sineDampening;
		transform.position = pos;
	}
}
