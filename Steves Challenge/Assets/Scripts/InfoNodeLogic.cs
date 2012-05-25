using UnityEngine;
using System.Collections;

public class InfoNodeLogic : MonoBehaviour 
{
	Renderer m_planeRenderer;
	public GameObject m_infoPlane;	// Because I'm a bad
	
	public PlayerEnum m_player;	// Player specific tutorials per node. Could have it so nodes can be shared and the position of the infoPlane is moved, but fuck it.
	float m_randomSineVariant;
	const float m_sineDampening = 0.005f;
	const float m_speedMultiplier = 4.0f;	// lol copy-paste from platformLogic.
	
	// Use this for initialization
	void Start () 
	{
		collider.isTrigger = true;
		m_planeRenderer = m_infoPlane.GetComponentInChildren<Renderer>();
		if (m_planeRenderer != null)
		{
			m_planeRenderer.enabled = false;
		}
		m_randomSineVariant = Random.Range(0.0f, 5.0f);
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (m_infoPlane != null)
		{
			Vector3 pos = m_planeRenderer.transform.position;
			pos.y += Mathf.Sin(Time.realtimeSinceStartup * m_speedMultiplier) * m_sineDampening;// + m_randomSineVariant;
			m_planeRenderer.transform.position = pos;
		}
	}
	
	void OnTriggerEnter(Collider a_collider)
	{
		Debug.Log("trigger entered");
		if (IsThisPlayer(a_collider.gameObject))
		{
			m_planeRenderer.enabled = true;
		}
	}
	
	void OnTriggerExit(Collider a_collider)
	{
		if (IsThisPlayer(a_collider.gameObject))
		{
			m_planeRenderer.enabled = false;
		}
	}
	
	bool IsThisPlayer(GameObject a_obj)
	{
		Player player = a_obj.GetComponent<Player>();
		if (player != null)
		{
			if (player.m_Player == m_player)
			{
				return true;
			}
		}
		return false;
	}
}
