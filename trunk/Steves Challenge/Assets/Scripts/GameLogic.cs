using UnityEngine;
using System.Collections;

public class GameLogic : MonoBehaviour
{
	Camera m_Camera;
	bool m_FadeIn;

	public Player[] m_Players = null;

	void Start()
	{
		m_Players = (Player[])FindSceneObjectsOfType(typeof(Player));
		
		if (m_Players.Length != 2)
		{
			throw new UnityException("Player count is not 2.");
		}

		m_Camera = (Camera)FindSceneObjectsOfType(typeof(Camera))[0];
		m_Camera.m_Fade = 1.5f;

		m_FadeIn = true;

		Physics.gravity = new Vector3(0.0f, -18.0f, 0.0f);
	}
	
	void Update()
	{
		if (m_FadeIn)
		{
			m_Camera.m_Fade -= Time.deltaTime;

			if (m_Camera.m_Fade <= 0.0f)
			{
				m_Camera.m_Fade = 0.0f;
				m_FadeIn = false;
			}
		}
	}
}
