using UnityEngine;
using System.Collections;

public class GameLogic : MonoBehaviour
{
	public Player[] m_Players = null;

	void Start()
	{
		m_Players = (Player[])FindSceneObjectsOfType(typeof(Player));

		if (m_Players.Length != 2)
		{
			Debug.DebugBreak();
		}
	}
	
	void Update()
	{
	
	}
}
