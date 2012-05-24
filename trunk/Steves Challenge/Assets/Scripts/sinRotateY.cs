using UnityEngine;
using System.Collections;

public class sinRotateY : MonoBehaviour {
float bobValue;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	bobValue = 100/* speed at which amplitude is traveled, more = larger max amplitude*/ * Mathf.Sin(Time.realtimeSinceStartup*39f/*Time Taken to resolve WAVE, lower means longer time taken*/);
	
	
	
		transform.Rotate (new Vector3(0, bobValue, 0)*Time.deltaTime);
	}
}
