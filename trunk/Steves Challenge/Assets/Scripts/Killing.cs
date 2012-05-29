using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Killing : MonoBehaviour
{
	SoundManager m_soundManager;
	GameLogic m_game;
    void Start()
    {
		m_game = (GameLogic)FindSceneObjectsOfType(typeof(GameLogic))[0];
		m_soundManager = m_game.GetComponent<SoundManager>();
    }

    void Update()
    {
    }

    void OnTriggerEnter(Collider obj)
    {
        Debug.Log("omg omg omg");

        Player player = obj.GetComponent<Player>();

        if (player != null)
        {
			foreach (Player p in m_game.m_Players)
			{
				p.Kill();
			}
			m_soundManager.PlaySound(SoundType.ST_SPIKEKILL, false, transform.position);
			
        }
    }
}
