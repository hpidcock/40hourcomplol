using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour
{
	public enum KeySet
	{
		KS_INVALID,
		KS_WASD,
		KS_ARROWS,
	}
	
	public KeySet m_keySet;
	
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		switch(m_keySet)
		{
		case KeySet.KS_ARROWS:
			ArrowMovement();
			break;
		case KeySet.KS_WASD:
			WASDMovement();
			break;
		case KeySet.KS_INVALID:
			Debug.Log("KeySet not set. PlayerInput.cs");
			break;
		}
	}
	
	void ArrowMovement()
	{
	}
	
	void WASDMovement()
	{
	}
}
