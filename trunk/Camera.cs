using UnityEngine;
using System.Collections;

public class Camera : MonoBehaviour
{
	GameLogic m_Game;

	void Start()
	{
		GameLogic[] logic = (GameLogic[])FindSceneObjectsOfType(typeof(GameLogic));

		if (logic.Length == 0)
		{
			throw new UnityException("Scene does not have a GameLogic object.");
		}

		m_Game = logic[0];
	}
	
	void Update()
	{
	
	}
}
