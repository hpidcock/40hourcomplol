using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnPoint : MonoBehaviour
{
	public SpawnPoint m_Next;
	public SpawnPointLocation m_location;	// Not needed for checkpoints

	void Start()
	{
	}
	
	void Update()
	{
		if (m_Next != null)
			Debug.DrawLine(this.transform.position, m_Next.transform.position, Color.red);
	}
}
