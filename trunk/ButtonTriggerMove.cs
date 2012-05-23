using UnityEngine;
using System.Collections;

// Moves platforms between its starting position and offset, based off the speed as specified in the buttonTrigger parent

public class ButtonTriggerMove : ButtonTrigger 
{
	public Vector3 m_offsetPosition = new Vector3(0,0,0);
	Vector3 m_startPosition;
	Vector3 m_targetPosition;
	//Vector3 m_pos;
	
	Vector3 m_targetDirection;
	Vector3 m_retractDirection;	// No point recalculating these
	
	const float m_positionThreshold = 0.2f;
	
	public override void Start()
	{
		m_startPosition = transform.position;
		m_targetPosition = m_startPosition + m_offsetPosition;
		
		m_targetDirection = (m_targetPosition - m_startPosition).normalized;
		m_retractDirection = (m_startPosition - m_targetPosition).normalized;
	}
	
	public override void UpdateActive()
	{
		Vector3 currPos = transform.position;
		if (!DistanceXYCheck(currPos, m_targetPosition, m_positionThreshold))
		{
			currPos += m_activeUpdateSpeed * m_targetDirection * Time.deltaTime;
			transform.position = currPos;
		}
	}
	
	public override void UpdateInactive()
	{
		Vector3 currPos = transform.position;
		if (!DistanceXYCheck(currPos, m_startPosition, m_positionThreshold))
		{
			currPos += m_inactiveUpdateSpeed * m_retractDirection * Time.deltaTime;
			transform.position = currPos;
		}
	}
	
	// hacky function, checks the x/y components of the vectors individually to see if they're close enough
	bool DistanceXYCheck(Vector3 a_pos1, Vector3 a_pos2, float a_threshold)
	{
		if (Mathf.Abs(a_pos1.x - a_pos2.x) < a_threshold && Mathf.Abs(a_pos1.y - a_pos2.y) < a_threshold)
		{
			return true;
		}
		return false;
	}
}
