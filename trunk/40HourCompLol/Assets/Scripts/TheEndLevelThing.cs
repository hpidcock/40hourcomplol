using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TheEndLevelThing : MonoBehaviour
{
    GameLogic m_Game;

    public string m_NextScene = "Menu";

    HashSet<Player> m_Players = new HashSet<Player>();

    void Start()
    {
        m_Game = (GameLogic)FindSceneObjectsOfType(typeof(GameLogic))[0];
    }

    void Update()
    {
        bool changeLevel = true;

        foreach (Player player in m_Game.m_Players)
        {
            changeLevel = changeLevel && m_Players.Contains(player);
        }

        if (changeLevel && m_Game.m_Players.Length > 0)
        {
            EndGame();
        }
    }

    public void EndGame()
    {
        LevelData.lololoLTololol = "Hahah it works you fuck";
        Application.LoadLevel(m_NextScene);
    }

    void OnTriggerEnter(Collider collider)
    {
        Player player = collider.GetComponent<Player>();

        if(player != null)
        {
            m_Players.Add(player);
        }
    }

    void OnTriggerExit(Collider collider)
    {
        Player player = collider.GetComponent<Player>();

        if (player != null)
        {
            m_Players.Remove(player);
        }
    }
}

