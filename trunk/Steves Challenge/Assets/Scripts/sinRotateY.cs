using UnityEngine;
using System.Collections;

public class sinRotateY : MonoBehaviour
{
	float m_Value;
	float m_Random;

	void Start()
	{
		m_Random = Random.Range(-3.14159f, 3.14159f);
	}

	void Update()
	{
		m_Value = 1.0f * Mathf.Sin(Time.time + m_Random);
		transform.Rotate(new Vector3(0, m_Value, 0) * Time.deltaTime);
	}
}