using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlatformLogic : MonoBehaviour
{
	GameLogic m_Game;

	public bool m_active = false;
	const float m_sineDampening = 0.005f;
	const float m_speedMultiplier = 4.0f;
	const float m_mass = 10.0f;
	const float m_drag = 1.0f;

	public List<Player> m_RidingPlayers = new List<Player>();

	// Use this for initialization
	void Start()
	{
		m_Game = (GameLogic)FindSceneObjectsOfType(typeof(GameLogic))[0];

		rigidbody.constraints = RigidbodyConstraints.FreezePositionZ |
			RigidbodyConstraints.FreezeRotationX |
			RigidbodyConstraints.FreezeRotationY |
			RigidbodyConstraints.FreezeRotationZ;
		rigidbody.mass = m_mass;
		rigidbody.drag = m_drag;
		rigidbody.useGravity = false;
	}

	// Update is called once per frame
	void Update()
	{
		m_RidingPlayers.Clear();

		if (m_active)
		{
			MoFugginSineWave();

			Bounds b = collider.bounds;
			b.SetMinMax(b.min, b.max + new Vector3(0, 2, 0));

			foreach (Player p in m_Game.m_Players)
			{
				if (b.Contains(p.collider.bounds.center))
				{
					m_RidingPlayers.Add(p);
				}
			}
		}
	}

	void MoFugginSineWave()
	{
		Vector3 pos = transform.position;
		pos.y += Mathf.Sin(Time.realtimeSinceStartup * m_speedMultiplier) * m_sineDampening;
		transform.position = pos;
	}

	void OnCollisionEnter(Collision a_collision)
	{
		switch (a_collision.collider.tag)
		{
			case "Button":
				ButtonLogic logic = a_collision.gameObject.GetComponentInChildren<ButtonLogic>();
				logic.m_activators.Add(GetInstanceID());
				break;
		}
	}

	void OnCollisionExit(Collision a_collision)
	{
		switch (a_collision.collider.tag)
		{
			case "Button":
				ButtonLogic logic = a_collision.gameObject.GetComponentInChildren<ButtonLogic>();
				logic.m_activators.Remove(GetInstanceID());
				break;
		}
	}
}
