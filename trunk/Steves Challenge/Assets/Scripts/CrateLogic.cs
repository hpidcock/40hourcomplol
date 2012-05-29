using UnityEngine;
using System.Collections;

// Copy-pasta from PlatformLogic because if PlatformLogic is just added as a component, Start() is called afterward - with this,
// it overwrites the physics settings (has gravity, locking axes)

public class CrateLogic : PlatformLogic {
	
	
	//const float m_crateMass = 0.001f;
	// Use this for initialization
	void Start () 
	{
		rigidbody.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
		rigidbody.useGravity = true;
		rigidbody.mass = m_mass;
		rigidbody.drag = m_drag;
	}
	
	// Update is called once per frame
	void Update () {
	
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
	
	public override void Activate()
	{
		base.Activate();
		rigidbody.useGravity = false;
		Debug.Log("Gravity off"); 
	}
	
	public override void Deactivate()
	{
		base.Deactivate();
		rigidbody.useGravity = true;
		Debug.Log("Gravity on");
	}
}
