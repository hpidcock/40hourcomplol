using UnityEngine;
using System.Collections;
using System.Collections.Generic;

internal static class PlayerValues
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
	
	public const float MaxCrateVelocity = 5.0f;

	public const float PlatformForce = 10.0f;
	
	public const float FootstepDelay = 0.3f;
	public const float FootstepThreshold = 0.05f;	// Force required to cause the sound to play
	public const float RunAnimSpeedModifier = 0.2f;	// Speed at which the player runs in relation to movement

	public static KeyCode KeyBinding(KeySet set, KeyBind bind)
	{
		return m_Keyboard[(int)set, (int)bind];
	}
}

public class Player : MonoBehaviour
{
	GameLogic m_Game;
	SoundManager m_soundManager;

	public SpawnPoint m_SpawnPoint;
	public PlayerEnum m_Player;
	bool m_Alive;
	float m_DeathTime;
	
	Animation m_playerAnim;
	AnimationState m_runAnim;

	KeySet m_KeySet = KeySet.Invalid;
	public PlayerControlState m_controlState = PlayerControlState.PCS_PLAYER;

	float m_LastJump = 0.0f;

	public TeleTrigger m_currentTeleLogic = null;	// Current teleTrigger the player is in, NULL if it's not in any
	public PlatformLogic m_currentTelePlatform = null;	// Same as m_currentTeleLogic
	
	GameObject m_teleSound = null;	// Keep track of the tele sound so it can be destroyed when channeling stops
	
	float m_footstepTime = 0.0f;

	void Start()
	{
		m_Game = (GameLogic)FindSceneObjectsOfType(typeof(GameLogic))[0];
		m_soundManager = m_Game.GetComponent<SoundManager>();

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
		
		SetSpawnPoint();

		rigidbody.constraints = RigidbodyConstraints.FreezePositionZ |
			RigidbodyConstraints.FreezeRotationX |
			RigidbodyConstraints.FreezeRotationY |
			RigidbodyConstraints.FreezeRotationZ;

		transform.position = m_SpawnPoint.transform.position;

		rigidbody.mass = 1.0f;
		
		m_playerAnim = GetComponentInChildren<Animation>();
		m_playerAnim.Play("idle");
		
		foreach (AnimationState a in m_playerAnim)
		{
			if (a.name == "run")
				m_runAnim = a;
		}
	}
	
	void SetSpawnPoint()
	{
		SpawnPoint[] spawnPoints = (SpawnPoint[])FindSceneObjectsOfType(typeof(SpawnPoint));
		SpawnPointLocation loc = SpawnPointLocation.SPL_BOTTOM;
		if (m_Player == LevelData.TopPlayer)
		{
			loc = SpawnPointLocation.SPL_TOP;
		}
		foreach (SpawnPoint s in spawnPoints)
		{
			if (s.m_location == loc)
			{
				m_SpawnPoint = s;
			}
		}
	}

	void Update()
	{
		rigidbody.WakeUp();

		if (m_Alive)
		{
			Ray feetRay = new Ray(collider.bounds.center, new Vector3(0.0f, -1.0f, 0.0f));

			RaycastHit[] hits = Physics.SphereCastAll(feetRay, collider.bounds.extents.x/2.0f, collider.bounds.extents.y);

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
			
			UpdateFootstepSound(onGround);


			if (Input.GetKeyDown(PlayerValues.KeyBinding(m_KeySet, KeyBind.Cycle)))
			{
				if (m_currentTeleLogic)
				{
					m_currentTelePlatform = m_currentTeleLogic.GetNextPlatform();
				}
			}
			if (Input.GetKeyDown(PlayerValues.KeyBinding(m_KeySet, KeyBind.Hold)))
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
	
	void UpdateFootstepSound(bool a_onGround)
	{
		if (a_onGround && Mathf.Abs(rigidbody.velocity.x) > PlayerValues.FootstepThreshold)
		{
			m_footstepTime += Time.deltaTime * Mathf.Abs(rigidbody.velocity.x) * PlayerValues.RunAnimSpeedModifier;
			if (m_footstepTime > PlayerValues.FootstepDelay)
			{
				m_footstepTime = 0.0f;
				m_soundManager.PlaySound(SoundType.ST_FOOTSTEPS, false, transform.position);
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
				m_soundManager.PlaySound(SoundType.ST_BUTTONPRESS, false, a_collision.gameObject.transform.position);
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
				m_soundManager.PlaySound(SoundType.ST_BUTTONPRESS, false, a_collision.gameObject.transform.position);
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
	
	void OnTriggerStay(Collider a_collision)
	{
		switch (a_collision.collider.tag)
		{
			case "Reset":
				if (Input.GetKeyDown(PlayerValues.KeyBinding(m_KeySet, KeyBind.Hold)))
				{
					ResetTower tower = a_collision.collider.gameObject.GetComponent<ResetTower>();
				if (tower)
				{
					tower.ResetPositions();
				}
				}
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
		if (m_controlState == PlayerControlState.PCS_OBJECT)
		{
			ToggleMovementStyle();
		}
		
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
			if (m_currentTelePlatform != null)
			{
				m_currentTelePlatform.Deactivate();
			}
			if (m_teleSound)
			{
				Destroy(m_teleSound);
				m_teleSound = null;
			}
		}
		else
		{
			if (m_currentTelePlatform != null)
			{
				m_teleSound = m_soundManager.PlaySound(SoundType.ST_TELELOOP, true, transform.position);
				m_controlState = PlayerControlState.PCS_OBJECT;
				rigidbody.velocity = Vector3.zero;
				m_currentTelePlatform.Activate();
			}
		}
	}

	void PlayerMovement(bool a_onGround)
	{
		UpdateAnimations(a_onGround);
		m_runAnim.speed = rigidbody.velocity.x * PlayerValues.RunAnimSpeedModifier;
		
		if (Input.GetKey(PlayerValues.KeyBinding(m_KeySet, KeyBind.Left)))
		{
			rigidbody.AddForce((a_onGround ? (-PlayerValues.MoveForceGrounded) : (-PlayerValues.MoveForceAir)) * Time.deltaTime, 0.0f, 0.0f, ForceMode.Acceleration);
		}

		if (Input.GetKey(PlayerValues.KeyBinding(m_KeySet, KeyBind.Right)))
		{
			rigidbody.AddForce((a_onGround ? PlayerValues.MoveForceGrounded : PlayerValues.MoveForceAir) * Time.deltaTime, 0.0f, 0.0f, ForceMode.Acceleration);
		}

		if (a_onGround &&
			Input.GetKey(PlayerValues.KeyBinding(m_KeySet, KeyBind.Up)) &&
			Time.time - m_LastJump > 0.1f)
		{
			m_LastJump = Time.time;
			rigidbody.AddForce(0.0f, PlayerValues.JumpForce, 0.0f, ForceMode.Impulse);
			m_playerAnim.Stop();
			m_playerAnim.Play("jump");
		}

		if (!a_onGround &&
			Input.GetKey(PlayerValues.KeyBinding(m_KeySet, KeyBind.Down)))
		{
			rigidbody.AddForce(0.0f, -PlayerValues.JumpForce, 0.0f, ForceMode.Acceleration);
		}

		if (Mathf.Abs(rigidbody.velocity.x) > PlayerValues.MaxXVelocity)
		{
			if(rigidbody.velocity.x > 0.0f)
			{
				rigidbody.AddForce(new Vector3(PlayerValues.MaxXVelocity - rigidbody.velocity.x, 0.0f, 0.0f), ForceMode.Acceleration);
			}
			else
			{
				rigidbody.AddForce(new Vector3((rigidbody.velocity.x + PlayerValues.MaxXVelocity) * -1.0f, 0.0f, 0.0f), ForceMode.Acceleration);
			}
		}

		if (rigidbody.velocity.y > PlayerValues.MaxYVelocity)
		{
			rigidbody.AddForce(new Vector3(0.0f, PlayerValues.MaxYVelocity - rigidbody.velocity.y, 0.0f), ForceMode.Acceleration);
		}	
	}
	
	void UpdateAnimations(bool a_onGround)
	{
		// Deal with animations
		if (Input.GetKey(PlayerValues.KeyBinding(m_KeySet, KeyBind.Left)))
		{
			PlayRunAnim(new Vector3(0, 270, 0), a_onGround);
		}
		else if (Input.GetKey(PlayerValues.KeyBinding(m_KeySet, KeyBind.Right)))
		{
			PlayRunAnim(new Vector3(0, 90, 0), a_onGround);
		}
		
		//if (a_onGround && Mathf.Abs(rigidbody.velocity.x) < 0.1f && Mathf.Abs(rigidbody.velocity.y) < 0.1f)
		if (a_onGround && Mathf.Abs(rigidbody.velocity.x) < 0.1f)
		{
			m_playerAnim.Play("idle");
			transform.rotation = Quaternion.Euler(0,0,0);
		}
	}
	
	void PlayRunAnim(Vector3 a_rotation, bool a_onGround)
	{
		if (a_onGround)
		{
			if (!m_playerAnim.IsPlaying("run"))
			{
				m_playerAnim.Play("run");
			}
		}
		transform.rotation = Quaternion.Euler(a_rotation);
	}

	void ObjectMovement()
	{
		m_playerAnim.Play("idle");
		
		List<Rigidbody> bodies = new List<Rigidbody>();
		
		bodies.Add(m_currentTelePlatform.rigidbody);
		foreach (Player p in m_currentTelePlatform.m_RidingPlayers)
		{
			bodies.Add(p.rigidbody);
		}

		if (Input.GetKey(PlayerValues.KeyBinding(m_KeySet, KeyBind.Left)))
		{
			foreach(Rigidbody rb in bodies)
			{
				rb.AddForce(-PlayerValues.PlatformForce, 0.0f, 0.0f, ForceMode.Acceleration);
			}
		}

		if (Input.GetKey(PlayerValues.KeyBinding(m_KeySet, KeyBind.Right)))
		{
			foreach(Rigidbody rb in bodies)
			{
				rb.AddForce(PlayerValues.PlatformForce, 0.0f, 0.0f, ForceMode.Acceleration);
			}
		}

		if (Input.GetKey(PlayerValues.KeyBinding(m_KeySet, KeyBind.Up)))
		{
			foreach(Rigidbody rb in bodies)
			{
				rb.AddForce(0.0f, PlayerValues.PlatformForce, 0.0f, ForceMode.Acceleration);
			}
		}

		if (Input.GetKey(PlayerValues.KeyBinding(m_KeySet, KeyBind.Down)))
		{
			foreach (Rigidbody rb in bodies)
			{
				rb.AddForce(0.0f, -PlayerValues.PlatformForce, 0.0f, ForceMode.Acceleration);
			}
		}
	}
}
