using UnityEngine;
using System.Collections;

public class ButtonTrigger : MonoBehaviour 
{
	public float m_activeUpdateSpeed = 10.0f;
	public float m_inactiveUpdateSpeed = 10.0f;
	
	// Use this for initialization
	public virtual void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
	
	public virtual void UpdateActive()
	{
	}
	
	public virtual void UpdateInactive()
	{
	}
}
