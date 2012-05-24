using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Killing : MonoBehaviour
{
	SoundManager m_soundManager;
    void Start()
    {
		GameLogic game = (GameLogic)FindSceneObjectsOfType(typeof(GameLogic))[0];
		m_soundManager = game.GetComponent<SoundManager>();
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
            player.Kill();
			m_soundManager.PlaySound(SoundType.ST_SPIKEKILL, false, transform.position);
			
        }
    }
}
