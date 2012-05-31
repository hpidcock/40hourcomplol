using UnityEngine;
using System.Collections;

public class TopEndLevelLogic : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter(Collider a_collision)
	{
		Player player = a_collision.gameObject.GetComponent<Player>();
		if (player != null)
		{
			LevelData.TopPlayer = player.m_Player;
		}		
	}
}
