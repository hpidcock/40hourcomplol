using UnityEngine;

public class SpikeShooter : ButtonTrigger
{
    bool m_Enabled = false;
    float m_LastFire = 0.0f;

    public float m_Frequency = 1.0f;

    public GameObject m_Prefab;
    public Quaternion m_Rotation = Quaternion.identity;

    void Start()
    {
    }

    void Update()
    {
        if (Time.time - m_LastFire < m_Frequency)
        {
            m_LastFire = Time.time;

            GameObject newObj = (GameObject)Instantiate(m_Prefab);
            newObj.transform.rotation = m_Rotation;
        }
    }

    public override void UpdateActive()
    {
        m_Enabled = true;
    }

    public override void UpdateInactive()
    {
        m_Enabled = false;
    }
}

