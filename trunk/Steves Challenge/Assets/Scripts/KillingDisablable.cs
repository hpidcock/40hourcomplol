using UnityEngine;

public class KillingDisablable : ButtonTrigger
{
    bool m_Enabled = true;

    void Start()
    {
    }

    void Update()
    {
    }

    public override void UpdateActive()
    {
        m_Enabled = false;
    }

    public override void UpdateInactive()
    {
        m_Enabled = true;
    }

    void OnTriggerEnter(Collider obj)
    {
        if (m_Enabled)
        {
            Player player = obj.GetComponent<Player>();

            if (player != null)
            {
                player.Kill();
            }
        }
    }
}
