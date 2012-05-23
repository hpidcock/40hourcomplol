using UnityEngine;
using System.Collections;

public class PlatformLogic : MonoBehaviour {
	
	public bool m_active = false;
	const float m_sineDampening = 0.005f;
	const float m_speedMultiplier = 4.0f;
	const float m_mass = 10.0f;
	const float m_drag = 1.0f;
	
	// Use this for initialization
	void Start () 
	{
		rigidbody.constraints = RigidbodyConstraints.FreezePositionZ | 
			RigidbodyConstraints.FreezeRotationX | 
			RigidbodyConstraints.FreezeRotationY | 
			RigidbodyConstraints.FreezeRotationZ;
		rigidbody.mass = m_mass;
		rigidbody.drag = m_drag;

	}
	
	// Update is called once per frame
	void Update () {
		
		if (m_active)
		{
			MoFugginSineWave();
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
		switch(a_collision.collider.tag)
		{
		case "Button":
			ButtonLogic logic = a_collision.gameObject.GetComponentInChildren<ButtonLogic>();
			logic.m_activators.Add(GetInstanceID());
			break;
		}
	}
	
	void OnCollisionExit(Collision a_collision)
	{
	switch(a_collision.collider.tag)
		{
		case "Button":
			ButtonLogic logic = a_collision.gameObject.GetComponentInChildren<ButtonLogic>();
			logic.m_activators.Remove(GetInstanceID());
			break;
		}
	}
}
