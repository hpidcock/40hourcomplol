using UnityEngine;

public class MenuCamera : MonoBehaviour
{
	public Color m_Color;

	private float m_Fade;
	private int m_State;

	void Start()
	{
		m_Fade = 0.0f;
		m_State = 0;
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
		else if (m_State == 2)
		{
			m_Fade -= Time.deltaTime;
			if (m_Fade <= 0.0f)
			{
				Application.LoadLevel("Loading");
			}
		}
		else if (m_State == 3)
		{
			m_Fade -= Time.deltaTime;
			if (m_Fade <= 0.0f)
			{
				Application.Quit();
			}
		}
	}

	void OnGUI()
	{
		m_Color.a = m_Fade;
		GUI.color = m_Color;

		if(GUI.Button(new Rect(Screen.width / 2 - 32, Screen.height / 2 - 32, 64, 32), "Start"))
		{
			LevelData.NextLevel = "Section_Intro";
			m_State = 2;
		}

		if (GUI.Button(new Rect(Screen.width / 2 - 32, Screen.height / 2 + 4, 64, 32), "Quit"))
		{
			m_State = 3;
		}
	}
}