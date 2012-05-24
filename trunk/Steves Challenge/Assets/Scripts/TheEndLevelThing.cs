using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TheEndLevelThing : MonoBehaviour
{
	public string m_NextScene = "Menu";

	GameLogic m_Game;
	Camera m_Camera;

	bool m_LevelChanging = false;
	HashSet<Player> m_Players = new HashSet<Player>();

	void Start()
	{
		m_Game = (GameLogic)FindSceneObjectsOfType(typeof(GameLogic))[0];
		m_Camera = (Camera)FindSceneObjectsOfType(typeof(Camera))[0];
	}

	void Update()
	{
		if (m_LevelChanging)
		{
			m_Camera.m_Fade += Time.deltaTime * 0.5f;

			if (m_Camera.m_Fade >= 1.0f)
			{
				EndGame();
			}
		}
		else
		{
			bool changeLevel = true;

			foreach (Player player in m_Game.m_Players)
			{
				changeLevel = changeLevel && m_Players.Contains(player);
			}

			if (changeLevel && m_Game.m_Players.Length > 0)
			{
				m_LevelChanging = true;
				m_Camera.m_Fade = 0.0f;
			}
		}
	}

	public void EndGame()
	{
		LevelData.NextLevel = m_NextScene;
		Application.LoadLevel("Loading");
	}

	void OnTriggerEnter(Collider collider)
	{
		Player player = collider.GetComponent<Player>();

		if (player != null)
		{
			m_Players.Add(player);
		}
	}

	void OnTriggerExit(Collider collider)
	{
		Player player = collider.GetComponent<Player>();

		if (player != null)
		{
			m_Players.Remove(player);
		}
	}
}