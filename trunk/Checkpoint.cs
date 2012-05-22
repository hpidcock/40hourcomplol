using UnityEngine;
using System.Collections;

class Checkpoint : SpawnPoint
{
	void OnCollisionEnter(Collision collision)
	{
		Player player = collision.gameObject.GetComponent<Player>();

		if (player != null)
		{
			if (player.m_SpawnPoint.m_Next == this)
			{
				player.m_SpawnPoint = this;
				Debug.Log("Checkpoint changed.");
			}
			else
			{
				Debug.Log("Checkpoint was not changed.");
			}
		}
	}
}
