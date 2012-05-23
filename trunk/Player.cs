using UnityEngine;
using System.Collections;
using System.Collections.Generic;

internal static class PlayerSettings
{
	private static KeyCode[,] m_Keyboard = new KeyCode[(int)KeySet.Count, (int)KeyBind.Count] { 
		{KeyCode.None, KeyCode.None, KeyCode.None, KeyCode.None, KeyCode.None, KeyCode.None, KeyCode.None},
		{KeyCode.None, KeyCode.A, KeyCode.D, KeyCode.W, KeyCode.S, KeyCode.Tab, KeyCode.Q},
		{KeyCode.None, KeyCode.LeftArrow, KeyCode.RightArrow, KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.Return, KeyCode.Backslash}
	};

	public const float MoveForceAir = 320.0f;
	public const float MoveForceGrounded = 640.0f;
	public const float JumpForce = 8.0f;

	public static KeyCode KeyBinding(KeySet set, KeyBind bind)
	{
		return m_Keyboard[(int)set, (int)bind];
	}
}


public class Player : MonoBehaviour 
{
	GameLogic m_Game;

	public SpawnPoint m_SpawnPoint;
	public PlayerEnum m_Player;

	KeySet m_KeySet = KeySet.Invalid;

	float m_LastJump = 0.0f;

	void Start()
	{
		m_Game = (GameLogic)FindSceneObjectsOfType(typeof(GameLogic))[0];

		switch (m_Player)
		{
			case PlayerEnum.PlayerA:
				m_KeySet = KeySet.Left;
				break;
			case PlayerEnum.PlayerB:
				m_KeySet = KeySet.Right;
				break;
			default:
				throw new UnityException("Player cannot be undefined.");
		}

		if (m_SpawnPoint == null)
		{
			throw new UnityException("SpawnPoint must be set to the start point.");
		}

		rigidbody.constraints = RigidbodyConstraints.FreezePositionZ | 
			RigidbodyConstraints.FreezeRotationX | 
			RigidbodyConstraints.FreezeRotationY | 
			RigidbodyConstraints.FreezeRotationZ;

		transform.position = m_SpawnPoint.transform.position;
	}
	
	void Update()
	// Update is called once per frame
	{
		Ray feetRay = new Ray(collider.bounds.center, new Vector3(0.0f, -1.0f, 0.0f));

		RaycastHit[] hits = Physics.SphereCastAll(feetRay, collider.bounds.extents.x, collider.bounds.extents.y);

		bool onGround = false;
		// See if we are standing on somethng other than us.
		foreach (RaycastHit hit in hits)
		{
			if (hit.rigidbody != this.rigidbody &&
				(collider.bounds.center.y - hit.point.y) < (collider.bounds.extents.y + 0.001f) &&
				hit.point.y < collider.bounds.center.y + collider.bounds.extents.y) // Check if the hit point is under the feet.
			{
				onGround = true;
				break;
			}
		}
		

		if (Input.GetKey(PlayerSettings.KeyBinding(m_KeySet, KeyBind.Left)))
		{
			rigidbody.AddForce((onGround ? (-PlayerSettings.MoveForceGrounded) : (-PlayerSettings.MoveForceAir)) * Time.deltaTime, 0.0f, 0.0f, ForceMode.Acceleration);
		}

		if (Input.GetKey(PlayerSettings.KeyBinding(m_KeySet, KeyBind.Right)))
		{
			rigidbody.AddForce((onGround ? PlayerSettings.MoveForceGrounded : PlayerSettings.MoveForceAir) * Time.deltaTime, 0.0f, 0.0f, ForceMode.Acceleration);
		}

		if (onGround &&
			Input.GetKey(PlayerSettings.KeyBinding(m_KeySet, KeyBind.Up)) &&
			Time.time - m_LastJump > 0.1f)
		{
			m_LastJump = Time.time;
			rigidbody.AddForce(0.0f, PlayerSettings.JumpForce, 0.0f, ForceMode.Impulse);
		}

		if (!onGround &&
			Input.GetKey(PlayerSettings.KeyBinding(m_KeySet, KeyBind.Down)))
		{
			rigidbody.AddForce(0.0f, -PlayerSettings.JumpForce, 0.0f, ForceMode.Acceleration);
		}
	}
	
	void OnCollisionStay(Collision a_collision)
	{
	}
	
	void OnCollisionEnter(Collision a_collision)
	{
		if (a_collision.collider.tag == "Button")
		{
			ButtonLogic logic = a_collision.gameObject.GetComponentInChildren<ButtonLogic>();
			logic.m_activators.Add(GetInstanceID());
			Debug.Log("Active");
		}
	}
	
	void OnCollisionExit(Collision a_collision)
	{
		if (a_collision.collider.tag == "Button")
		{
			ButtonLogic logic = a_collision.gameObject.GetComponentInChildren<ButtonLogic>();
			logic.m_activators.Remove(GetInstanceID());
			Debug.Log("Inactive");
		}
	}
}
