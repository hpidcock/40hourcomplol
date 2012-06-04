using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TeleTrigger : MonoBehaviour 
{
	
	public PlatformLogic m_platform;	// Vector of movable platforms
	LineRenderer m_lineRenderer; 
	
	// Use this for initialization
	void Start () {
		m_lineRenderer = gameObject.AddComponent<LineRenderer>();
		m_lineRenderer.SetPosition(0, transform.position);
		m_lineRenderer.SetPosition(1, m_platform.transform.position);
		m_lineRenderer.enabled = false;
	
	}
	
	// Update is called once per frame
	void Update () {
		if (m_platform.m_active)
		{
			m_lineRenderer.SetPosition(1, m_platform.transform.position);
		}
	
	}
	
	public PlatformLogic GetNextPlatform()
	{
		if (m_lineRenderer != null)
		{
			m_lineRenderer.enabled = true;
		}
		m_platform.m_active = true;
		return m_platform;
	}
	
	public void ResetLastPlatform()
	{
		if (m_lineRenderer != null)
		{
			m_lineRenderer.enabled = false;
		}
		m_platform.m_active = false;
	}
}






//public class TeleTrigger : MonoBehaviour 
//{
//	
//	public List<PlatformLogic> m_platforms;	// Vector of movable platforms
//	int	m_lastPlatformIndex = 0;	// Keeps track of the last platform returned. lol index. FUCK THE PO-LICE
//
//	// Use this for initialization
//	void Start () {
//	
//	}
//	
//	// Update is called once per frame
//	void Update () {
//	
//	}
//	
//	public PlatformLogic GetNextPlatform()
//	{
//		if (m_platforms.Count > 0)
//		{
//			Debug.Log(" more than one platform; index is " + m_lastPlatformIndex.ToString());
//			if (m_platforms[m_lastPlatformIndex] != null)
//			{
//				//m_platforms[m_lastPlatformIndex].m_active = false;
//				m_platforms[m_lastPlatformIndex].Deactivate();
//			}
//			if (m_lastPlatformIndex + 1 >= m_platforms.Count)
//			{
//				Debug.Log("Over the index size");
//				m_lastPlatformIndex = 0;
//				m_platforms[0].m_active = true;
//				return m_platforms[0];
//			}
//			else
//			{
//				Debug.Log("Platform count is " + m_platforms.Count.ToString());
//				Debug.Log("platform index is less than size; " + m_lastPlatformIndex.ToString());
//				++m_lastPlatformIndex;
//				//m_platforms[m_lastPlatformIndex].m_active = true;
//				m_platforms[m_lastPlatformIndex].Activate();
//				return m_platforms[m_lastPlatformIndex];
//			}
//		}
//		else
//		{
//			return null;
//		}
//	}
//	
//	public void ResetLastPlatform()
//	{
//		if (m_platforms[m_lastPlatformIndex] != null)
//		{
//			m_platforms[m_lastPlatformIndex].m_active = false;
//		}
//	}
//}
