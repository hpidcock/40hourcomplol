using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Has a vector of ButtonTrigger scripts - all of which are linked to the button.
// pressing the button toggles m_isActive changing the update style

public class ButtonLogic : MonoBehaviour 
{	
	public List<GameObject> m_triggerObjects = new List<GameObject>();
	List<ButtonTrigger> m_triggerScripts = new  List<ButtonTrigger>();
	
	public HashSet<int> m_activators = new HashSet<int>();
	
	// Use this for initialization
	void Start()
	{
		foreach(GameObject t in m_triggerObjects)
		{
			m_triggerScripts.Add(t.GetComponent<ButtonTrigger>());
		}
	}
	
	// Update is called once per frame
	void Update()
	{
		if (m_activators.Count != 0)
		{
			UpdateActive();
		}
		else
		{
			UpdateInactive();
		}
	}
	
	void UpdateActive()
	{
		foreach(ButtonTrigger t in m_triggerScripts)
		{
			t.UpdateActive();
		}
	}
	
	void UpdateInactive()
	{
		foreach(ButtonTrigger t in m_triggerScripts)
		{
			t.UpdateInactive();
		}
	}
}
