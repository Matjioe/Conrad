using UnityEngine;
using System.Collections;

public class ConradController : MonoBehaviour
{
	public float speed = 10.0F;
	public bool isMoving = false;

	public bool isFlipping = false;
	public bool m_FacingRight = true;

	public bool isGoingUp = false;
	public bool isGoingDown = false;

	private bool m_InputFacingRight = true;
	private Animator animator;

	void Start()
	{
		animator = GetComponent<Animator>();
	}

	void Update()
	{
		UpdateInput();
		MoveCharacter();
	}

	public void Flip()
	{
		// Switch the way the player is labelled as facing.
		m_FacingRight = !m_FacingRight;
		
		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;

		isFlipping = false;
	}

	void UpdateInput()
	{
		float vert = Input.GetAxis("Vertical");
		float hor = Input.GetAxis("Horizontal");

		if (hor == 0)
		{
			animator.SetBool("walk", false);
			if (!isGoingUp && vert > 0.0f)
			{
				animator.SetTrigger("up");
				isGoingUp = true;
			}
			if (!isGoingDown && vert < 0.0f)
			{
				animator.SetTrigger("down");
				isGoingDown = true;
			}
		}
		else
		{
			// Turnaround
			bool shouldTurnAround = false;
			if (!isFlipping)
			{
				if ((m_InputFacingRight && hor < 0.0f) || (m_InputFacingRight == false && hor > 0.0f))
				{
					animator.SetTrigger("turnAround");
					shouldTurnAround = true;
					m_InputFacingRight = !m_InputFacingRight;
					isFlipping = true;
				}
			}

			if (vert > 0.0f && !isGoingUp)
			{
				animator.SetTrigger("jumpForward");
				isGoingUp = true;
			}
			
			// If we are turning around while moving, we can continue moving.
			// If we are not turning around we can move.
			if(isFlipping == false && isGoingUp == false && isGoingDown == false)
			{
				animator.SetBool("walk", true);
				
				if (Input.GetButtonDown("Fire1"))
				{
					animator.SetBool("run", true);
				}

				if (Input.GetButtonUp("Fire1"))
				{
					animator.SetBool("run", false);
				}
			}
		}
	}

	public void MoveCharacter()
	{
		if (isMoving)
		{
			if (m_FacingRight)
			{
				transform.Translate(speed * Time.deltaTime, 0, 0);
			}
			else
			{
				transform.Translate(-speed * Time.deltaTime, 0, 0);
			}
		}
	}

	public void EnableMoving()
	{
		isMoving = true;
	}

	public void DisableMoving()
	{
		isMoving = false;
	}

	public void OffsetCharacter()
	{
	}
}
