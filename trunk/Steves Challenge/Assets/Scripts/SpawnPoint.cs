using UnityEngine;
using System.Collections;

public class SpawnPoint : MonoBehaviour
{
	GameLogic m_Game;

	public SpawnPoint m_Next;

	void Start()
	{
		m_Game = (GameLogic)FindSceneObjectsOfType(typeof(GameLogic))[0];
	}
	
	void Update()
	{
		if (m_Next != null)
			Debug.DrawLine(this.transform.position, m_Next.transform.position, Color.red);
	}
}
