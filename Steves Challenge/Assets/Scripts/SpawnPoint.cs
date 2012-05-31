using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnPoint : MonoBehaviour
{
	GameLogic m_Game;

	public SpawnPoint m_Next;
	public SpawnPointLocation m_location;	// Not needed for checkpoints
	
	public List<GameObject> m_resetableObjects;
	
	List<Pair<GameObject, Vector3>> m_objectPositions = new List<Pair<GameObject, Vector3>>();	// Objects to have their position reset when players die

	void Start()
	{
		m_Game = (GameLogic)FindSceneObjectsOfType(typeof(GameLogic))[0];
		foreach(GameObject g in m_resetableObjects)
		{
			m_objectPositions.Add(new Pair<GameObject, Vector3>(g, g.transform.position));
		}
	}
	
	void Update()
	{
		if (m_Next != null)
			Debug.DrawLine(this.transform.position, m_Next.transform.position, Color.red);
	}
	
	public void ResetPositions()
	{
		foreach (Pair<GameObject, Vector3> p in m_objectPositions)
		{
			p.First.transform.position = p.Second;
		}
	}
}
