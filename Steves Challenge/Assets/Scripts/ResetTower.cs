using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ResetTower : MonoBehaviour 
{
	
	GameLogic m_Game;
	public List<GameObject> m_resetableObjects;
	List<Pair<GameObject, Vector3>> m_objectPositions = new List<Pair<GameObject, Vector3>>();	// Objects to have their position reset when players die

	// Use this for initialization
	void Start () 
	{
		gameObject.tag = "Reset";
		Collider col = GetComponent<Collider>();
		if (col)
		{
			col.isTrigger = true;
		}
		else
		{
			throw new UnityException("Reset Tower is missing a collider!");
		}
		
		m_Game = (GameLogic)FindSceneObjectsOfType(typeof(GameLogic))[0];
		
		foreach(GameObject g in m_resetableObjects)
		{
			m_objectPositions.Add(new Pair<GameObject, Vector3>(g, g.transform.position));
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	
	public void ResetPositions()
	{
		Debug.Log("Resetting");
		foreach (Pair<GameObject, Vector3> p in m_objectPositions)
		{
			p.First.transform.position = p.Second;
		}
	}
}
