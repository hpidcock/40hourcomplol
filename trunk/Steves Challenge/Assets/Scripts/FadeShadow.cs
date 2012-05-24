using UnityEngine;
using System.Collections;

public class FadeShadow : MonoBehaviour
{
	void Start()
	{
	}

	void Update()
	{
		float a = (Mathf.Sin(Time.time) + 1.0f) * 0.1f;
		renderer.material.SetColor("_Add", new Color(a, a, a, 0.0f));
	}
}
