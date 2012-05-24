using UnityEngine;
using System.Collections;

public class sinRotateX : MonoBehaviour {
float bobValue;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	bobValue = 1f/* speed at which amplitude is traveled, more = larger max amplitude*/ * Mathf.Sin(Time.realtimeSinceStartup*5f/*Time Taken to resolve WAVE, lower means longer time taken*/);
	
	transform.Rotate (new Vector3(bobValue, 0, 0)*Time.deltaTime);
	}
}
