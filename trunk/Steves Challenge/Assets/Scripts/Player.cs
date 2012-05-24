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
	public const float MoveForceGrounded = 840.0f;
	public const float JumpForce = 10.0f;

	public const float MaxXVelocity = 3.0f;
	public const float MaxYVelocity = 4.25f;

	public const float PlatformForce = 10.0f;

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
	bool m_Alive;
	float m_DeathTime;

	KeySet m_KeySet = KeySet.Invalid;
	public PlayerControlState m_controlState = PlayerControlState.PCS_PLAYER;

	float m_LastJump = 0.0f;

	public TeleTrigger m_currentTeleLogic = null;	// Current teleTrigger the player is in, NULL if it's not in any
	public PlatformLogic m_currentTelePlatform = null;	// Same as m_currentTeleLogic

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

		rigidbody.mass = 1.0f;
	}

	void Update()
	{
		rigidbody.WakeUp();

		if (m_Alive)
		{
			Ray feetRay = new Ray(collider.bounds.center, new Vector3(0.0f, -1.0f, 0.0f));

			RaycastHit[] hits = Physics.SphereCastAll(feetRay, collider.bounds.extents.x, collider.bounds.extents.y);

			bool onGround = false;
			// See if we are standing on somethng other than us.
			foreach (RaycastHit hit in hits)
			{
				if (hit.collider.GetComponent<Player>() == null &&
					(collider.bounds.center.y - hit.point.y) < (collider.bounds.extents.y + 0.001f) &&
					hit.point.y < collider.bounds.center.y + collider.bounds.extents.y) // Check if the hit point is under the feet.
				{
					onGround = true;
					break;
				}
			}

			switch (m_controlState)
			{
				case PlayerControlState.PCS_OBJECT:
					if (m_currentTelePlatform != null)
					{
						ObjectMovement();
					}
					break;
				case PlayerControlState.PCS_PLAYER:
					PlayerMovement(onGround);
					break;
			}


			if (Input.GetKeyDown(PlayerSettings.KeyBinding(m_KeySet, KeyBind.Cycle)))
			{
				if (m_currentTeleLogic)
				{
					m_currentTelePlatform = m_currentTeleLogic.GetNextPlatform();
				}
			}
			if (Input.GetKeyDown(PlayerSettings.KeyBinding(m_KeySet, KeyBind.Hold)))
			{
				ToggleMovementStyle();
			}
		}
		else
		{
			if (Time.time - m_DeathTime > 0.5f)
			{
				Respawn();
			}
		}
	}

	void OnCollisionStay(Collision a_collision)
	{
	}

	void OnCollisionEnter(Collision a_collision)
	{
		switch (a_collision.collider.tag)
		{
			case "Button":
				ButtonLogic logic = a_collision.gameObject.GetComponentInChildren<ButtonLogic>();
				logic.m_activators.Add(GetInstanceID());
				break;
		}
	}

	void OnCollisionExit(Collision a_collision)
	{
		switch (a_collision.collider.tag)
		{
			case "Button":
				ButtonLogic logic = a_collision.gameObject.GetComponentInChildren<ButtonLogic>();
				logic.m_activators.Remove(GetInstanceID());
				break;
		}
	}

	void OnTriggerEnter(Collider a_collision)
	{
		switch (a_collision.collider.tag)
		{
			case "TeleTrigger":
				Debug.Log("Entering trigger volume");

				TeleTrigger logic = a_collision.GetComponent<TeleTrigger>();
				m_currentTelePlatform = logic.GetNextPlatform();
				m_currentTeleLogic = logic;
				break;
		}
	}

	void OnTriggerExit(Collider a_collision)
	{
		switch (a_collision.collider.tag)
		{
			case "TeleTrigger":
				m_currentTeleLogic.ResetLastPlatform();
				Debug.Log("Leaving trigger volume");
				m_currentTeleLogic = null;
				m_currentTelePlatform = null;
				break;
		}
	}

	public void Kill()
	{
		m_Alive = false;
		m_DeathTime = Time.time;

		m_controlState = PlayerControlState.PCS_PLAYER;
		m_currentTeleLogic = null;
		m_currentTelePlatform = null;

		renderer.enabled = false;
		collider.enabled = false;
	}

	public void Respawn()
	{
		m_Alive = true;

		rigidbody.velocity = Vector3.zero;
		renderer.enabled = true;
		collider.enabled = true;

		transform.position = m_SpawnPoint.transform.position;
	}

	void ToggleMovementStyle()
	{
		if (m_controlState == PlayerControlState.PCS_OBJECT)
		{
			m_controlState = PlayerControlState.PCS_PLAYER;
			rigidbody.velocity = Vector3.zero;
			rigidbody.mass = 1.0f;
		}
		else
		{
			m_controlState = PlayerControlState.PCS_OBJECT;
			rigidbody.velocity = Vector3.zero;
			rigidbody.mass = 1e30f;
		}
	}

	void PlayerMovement(bool a_onGround)
	{
		if (Input.GetKey(PlayerSettings.KeyBinding(m_KeySet, KeyBind.Left)))
		{
			rigidbody.AddForce((a_onGround ? (-PlayerSettings.MoveForceGrounded) : (-PlayerSettings.MoveForceAir)) * Time.deltaTime, 0.0f, 0.0f, ForceMode.Acceleration);
		}

		if (Input.GetKey(PlayerSettings.KeyBinding(m_KeySet, KeyBind.Right)))
		{
			rigidbody.AddForce((a_onGround ? PlayerSettings.MoveForceGrounded : PlayerSettings.MoveForceAir) * Time.deltaTime, 0.0f, 0.0f, ForceMode.Acceleration);
		}

		if (a_onGround &&
			Input.GetKey(PlayerSettings.KeyBinding(m_KeySet, KeyBind.Up)) &&
			Time.time - m_LastJump > 0.1f)
		{
			m_LastJump = Time.time;
			rigidbody.AddForce(0.0f, PlayerSettings.JumpForce, 0.0f, ForceMode.Impulse);
		}

		if (!a_onGround &&
			Input.GetKey(PlayerSettings.KeyBinding(m_KeySet, KeyBind.Down)))
		{
			rigidbody.AddForce(0.0f, -PlayerSettings.JumpForce, 0.0f, ForceMode.Acceleration);
		}

		if (Mathf.Abs(rigidbody.velocity.x) > PlayerSettings.MaxXVelocity)
		{
			if(rigidbody.velocity.x > 0.0f)
			{
				rigidbody.AddForce(new Vector3(PlayerSettings.MaxXVelocity - rigidbody.velocity.x, 0.0f, 0.0f), ForceMode.Acceleration);
			}
			else
			{
				rigidbody.AddForce(new Vector3((rigidbody.velocity.x + PlayerSettings.MaxXVelocity) * -1.0f, 0.0f, 0.0f), ForceMode.Acceleration);
			}
		}

		if (rigidbody.velocity.y > PlayerSettings.MaxYVelocity)
		{
			rigidbody.AddForce(new Vector3(0.0f, PlayerSettings.MaxYVelocity - rigidbody.velocity.y, 0.0f), ForceMode.Acceleration);
		}
	}

	void ObjectMovement()
	{
		List<Rigidbody> bodies = new List<Rigidbody>();

		bodies.Add(m_currentTelePlatform.rigidbody);
		foreach (Player p in m_currentTelePlatform.m_RidingPlayers)
		{
			if (p != this)
			{
				bodies.Add(p.rigidbody);
			}
		}

		if (Input.GetKey(PlayerSettings.KeyBinding(m_KeySet, KeyBind.Left)))
		{
			foreach(Rigidbody rb in bodies)
			{
				rb.AddForce(-PlayerSettings.PlatformForce, 0.0f, 0.0f, ForceMode.Acceleration);
			}
		}

		if (Input.GetKey(PlayerSettings.KeyBinding(m_KeySet, KeyBind.Right)))
		{
			foreach(Rigidbody rb in bodies)
			{
				rb.AddForce(PlayerSettings.PlatformForce, 0.0f, 0.0f, ForceMode.Acceleration);
			}
		}

		if (Input.GetKey(PlayerSettings.KeyBinding(m_KeySet, KeyBind.Up)))
		{
			foreach(Rigidbody rb in bodies)
			{
				rb.AddForce(0.0f, PlayerSettings.PlatformForce, 0.0f, ForceMode.Acceleration);
			}
		}

		if (Input.GetKey(PlayerSettings.KeyBinding(m_KeySet, KeyBind.Down)))
		{
			foreach (Rigidbody rb in bodies)
			{
				rb.AddForce(0.0f, -PlayerSettings.PlatformForce, 0.0f, ForceMode.Acceleration);
			}
		}
	}
}
