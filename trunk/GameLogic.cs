using UnityEngine;
using System.Collections;

public class GameLogic : MonoBehaviour
{
	Player[] m_Players = null;

	void Start()
	{
		m_Players = (Player[])FindObjectsOfType(typeof(Player));

		if (m_Players.Length != 0)
		{
			throw new UnityException("Player count is not 2.");
		}
	}
	
	void Update()
	{
	
	}
}
