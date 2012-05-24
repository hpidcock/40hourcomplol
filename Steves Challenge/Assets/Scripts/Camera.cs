using UnityEngine;
using System.Collections;

public class Camera : MonoBehaviour
{
	GameLogic m_Game;

	public float m_Fade = 1.0f;

	void Start()
	{
		m_Game = (GameLogic)FindSceneObjectsOfType(typeof(GameLogic))[0];
	}

	void OnGUI()
	{
		GUI.color = new Color(1.0f, 1.0f, 1.0f, m_Fade);
		GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), guiTexture.texture);
	}
	
	void Update()
	{
		Vector3 minPos = new Vector3(1e30f, 1e30f, 1e30f);
		Vector3 maxPos = new Vector3(-1e30f, -1e30f, -1e30f);

		foreach(Player player in m_Game.m_Players)
		{
			Vector3 pos = player.transform.position;
			minPos.x = Mathf.Min(minPos.x, pos.x);
			minPos.y = Mathf.Min(minPos.y, pos.y);
			minPos.z = Mathf.Min(minPos.z, pos.z);

			maxPos.x = Mathf.Max(maxPos.x, pos.x);
			maxPos.y = Mathf.Max(maxPos.y, pos.y);
			maxPos.z = Mathf.Max(maxPos.z, pos.z);
		}

		const float approach = 64.0f;

		transform.position = (transform.position * approach + (minPos + maxPos) / 2.0f) / (approach + 1.0f);
		transform.position = new Vector3(transform.position.x, transform.position.y, -1.0f);

		camera.orthographicSize = Mathf.Clamp((camera.orthographicSize * approach + (maxPos.x - minPos.x) / 2.0f) / (approach + 1.0f), 16.0f, 1e30f);
	}
}
