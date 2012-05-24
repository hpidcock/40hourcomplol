using UnityEngine;
using UnityEditor;

public class LoadingCamera : MonoBehaviour
{
	public Color m_Glow;

	private float m_ChangeTime;
	private float m_FlipTime;
	private Rect m_CurrentRune;
	private float m_Fade;
	private int m_State;
	private AsyncOperation m_Loading;

	private Rect[] m_Runes = new Rect[] {new Rect(0.0f, 0.75f, 0.25f, 0.25f),
		new Rect(0.25f, 0.75f, 0.25f, 0.25f),
		new Rect(0.0f, 0.5f, 0.25f, 0.25f),
		new Rect(0.25f, 0.5f, 0.25f, 0.25f),
		new Rect(0.0f, 0.25f, 0.25f, 0.25f),
		new Rect(0.25f, 0.25f, 0.25f, 0.25f)
	};

	void Start()
	{
		m_ChangeTime = Time.time + 2.0f;
		m_FlipTime = Time.time + 0.35f;
		m_CurrentRune = m_Runes[Random.Range(0, m_Runes.Length - 1)];

		m_Fade = 0.0f;
		m_State = 0;

		if (PlayerSettings.advancedLicense)
		{
			m_Loading = Application.LoadLevelAdditiveAsync(LevelData.NextLevel);
		}
	}

	void Update()
	{
		if (m_State == 0)
		{
			m_Fade += Time.deltaTime;
			if (m_Fade >= 1.0f)
			{
				m_State = 1;
				m_Fade = 1.0f;
			}
		}
		else if (m_State == 2 && 
			(m_Loading == null || m_Loading.isDone))
		{
			m_Fade -= Time.deltaTime * 3.0f;
			if (m_Fade <= 0.0f)
			{
				if (m_Loading == null)
				{
					Application.LoadLevel(LevelData.NextLevel);
				}
				else
				{
					DestroyImmediate(this);
				}
				return;
			}
		}

		if (m_FlipTime < Time.time)
		{
			m_FlipTime = Time.time + Random.Range(0.35f, 0.8f);

			m_CurrentRune = m_Runes[Random.Range(0, m_Runes.Length - 1)];
		}

		if (m_ChangeTime < Time.time)
		{
			m_State = 2;
		}
	}

	void OnGUI()
	{
		int w = Screen.width;
		int h = Screen.height;

		m_Glow.a = (Mathf.Sin(Time.time * 2.0f) * 0.125f + 0.75f) * m_Fade;

		GUI.color = m_Glow;
		GUI.DrawTextureWithTexCoords(new Rect(w / 2 - 128 / 2, h / 2 - 128 / 2, 128, 128), guiTexture.texture, new Rect(0.5f, 0.5f, 0.5f, 0.5f));
		GUI.DrawTextureWithTexCoords(new Rect(w / 2 - 32 / 2, h / 2 - 32 / 2, 32, 32), guiTexture.texture, m_CurrentRune);
	}
}