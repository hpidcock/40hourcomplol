using UnityEngine;
using System.Collections;

public class Camera : MonoBehaviour
{
	GameLogic m_Game;

	public float m_Fade = 1.0f;

	void Start()
	{
		m_Game = (GameLogic)FindSceneObjectsOfType(typeof(GameLogic))[0];

		camera.far = 1000.0f;
	}

	void OnGUI()
	{
		GUI.color = new Color(1.0f, 1.0f, 1.0f, m_Fade);
		GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), guiTexture.texture);
	}

	void Update()
	{
		Vector3 lastPos = transform.position;

		Vector3 minPos = new Vector3(1e30f, 1e30f, 1e30f);
		Vector3 maxPos = new Vector3(-1e30f, -1e30f, -1e30f);

		foreach (Player player in m_Game.m_Players)
		{
			Vector3 pos = player.transform.position;
			minPos.x = Mathf.Min(minPos.x, pos.x);
			minPos.y = Mathf.Min(minPos.y, pos.y);
			minPos.z = Mathf.Min(minPos.z, pos.z);

			maxPos.x = Mathf.Max(maxPos.x, pos.x);
			maxPos.y = Mathf.Max(maxPos.y, pos.y);
			maxPos.z = Mathf.Max(maxPos.z, pos.z);

			if (player.m_controlState == PlayerControlState.PCS_OBJECT &&
				player.m_currentTelePlatform != null)
			{
				pos = player.m_currentTelePlatform.transform.position;
				minPos.x = Mathf.Min(minPos.x, pos.x);
				minPos.y = Mathf.Min(minPos.y, pos.y);
				minPos.z = Mathf.Min(minPos.z, pos.z);

				maxPos.x = Mathf.Max(maxPos.x, pos.x);
				maxPos.y = Mathf.Max(maxPos.y, pos.y);
				maxPos.z = Mathf.Max(maxPos.z, pos.z);
			}
		}

		const float approach = 64.0f;

		transform.position = (transform.position * approach + (minPos + maxPos) / 2.0f) / (approach + 1.0f);
		transform.position = new Vector3(transform.position.x, transform.position.y, -100.0f);

		float targetSize = 0.0f;

		if (maxPos.x - minPos.x > maxPos.y - minPos.y)
		{
			targetSize = (maxPos.x - minPos.x) / 2.0f;
		}
		else
		{
			targetSize = (maxPos.y - minPos.y) / 2.0f;
		}
		camera.orthographicSize = Mathf.Clamp((camera.orthographicSize * approach + targetSize) / (approach + 1.0f), 16.0f, 1e30f);

		const string paralaxName = "paralax_";

		Vector3 moveDelta = transform.position - lastPos;

		GameObject[] objects = (GameObject[])FindSceneObjectsOfType(typeof(GameObject));
		foreach(GameObject obj in objects)
		{
			string t = obj.tag.ToLower();
			if (t.Length > paralaxName.Length &&
				t.Substring(0, paralaxName.Length) == paralaxName)
			{
				int i = 0;
				int.TryParse(t.Substring(paralaxName.Length), out i);

				if (i < 0)
				{
					float k = i * -1 + 1;
					k *= 0.5f;

					obj.transform.position += new Vector3(moveDelta.x * k, moveDelta.y * k, 0.0f);
				}
				else if (i > 0)
				{
					float k = i + 1;
					k *= 2.0f;

					obj.transform.position += new Vector3(moveDelta.x / k, moveDelta.y / k, 0.0f);
				}
			}
		}
	}
}
