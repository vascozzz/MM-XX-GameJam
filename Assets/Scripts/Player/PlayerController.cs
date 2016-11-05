using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

[RequireComponent(typeof(CharacterController2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Basic Movement")]
    [SerializeField, Range(1f, 20f)] private float moveSpeed = 6f;

    [Header("Jumping")]
    [SerializeField, Range(0f, 20f)] private float jumpHeight = 4f;
    [SerializeField] private float jumpApexDelay = 0.4f;
    [SerializeField] private bool enableDoubleJump;
    [SerializeField] private float accelerationGrounded = 0f;
    [SerializeField] private float accelerationJump = 0f;

    [Header("Wall Jumping")]
    [SerializeField] private bool enableWallJump;
    [SerializeField, Range(1f, 20f)] private float wallSlideSpeed = 3f;
    [SerializeField] private float wallStickDuration = 0.25f;
    [SerializeField] private float accelerationWallJump = 0.2f;
    [SerializeField] private Vector2 wallJumpClimb;
    [SerializeField] private Vector2 wallJumpOff;
    [SerializeField] private Vector2 wallJumpLeap;

    [Header("Dashing")]
    [SerializeField] private bool enableDash;
    [SerializeField, Range(1f, 60f)] private float dashSpeed = 30f;
    [SerializeField] private float dashDuration = 0.11f;
    [SerializeField] private float dashCooldown = 1f;

    [Header("UI")]
    [SerializeField] private GameObject redShield;
    [SerializeField] private GameObject yellowShield;
    [SerializeField] private GameObject greeenShield;

    // Utility
    private Vector2 input;
    private CharacterController2D cc;
    private Animator animator;
    private SpriteRenderer sr;

    // Movement
    private Vector3 velocity;
    private float gravity;
    private float horizontalSmoothing;
    private bool friction = true;

    // Jumping
    private float jumpVelocity;
    private bool hasDoubleJump;

    // Wall Jumping
    private bool isWallJumping;
    private bool wallSliding;
    private float wallUnstickTime;

    // Dashing
    private bool isDashing;
    private float nextDashTime;
    private float dashStopTime;
    private float dashDirection;
    private TrailRenderer rainbowTrail;

    void Start()
    {
        cc = GetComponent<CharacterController2D>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        rainbowTrail = GetComponent<TrailRenderer>();

        // set gravity and jump velocity based on desired height and apex time
        gravity = -(2 * jumpHeight) / Mathf.Pow(jumpApexDelay, 2);
        jumpVelocity = Mathf.Abs(gravity * jumpApexDelay);

        // set default dash direction
        dashDirection = cc.CollisionState.horizontalDir;

        // set collision events
        cc.OnTriggerStayEvent += OnTriggerStayEvent;
        cc.OnTriggerExitEvent += OnTriggerExitEvent;
    }

    void Update()
    {
        // get user input
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");

        // target velocity
        float acceleration = GetAcceleration();
        float horizontalVel = input.x * moveSpeed;

        // smoothly accelerate towards the target
        if(friction) velocity.x = Mathf.SmoothDamp(velocity.x, horizontalVel, ref horizontalSmoothing, acceleration);

        // double jumping
        if (enableDoubleJump)
        {
            DoubleJump();
        }

        // wall jumping
        if (enableWallJump)
        {
            WallJump();
        }

        // dashing
        if (enableDash)
        {
            Dash();
        }

        // reset vertical velocity on vertical collisions
        if (cc.CollisionState.below || cc.CollisionState.above)
        {
            velocity.y = 0;
        }

        // regular jumping
        Jump();

        // apply gravity
        velocity.y += gravity * Time.deltaTime;

        // dashing should override gravity and vertical movement
        if (isDashing)
        {
            velocity.y = 0f;
        }

        // attempt to move
        cc.Move(velocity * Time.deltaTime, input.y == -1f);

        // Handle animations
        HandleAnimations();
    }

    private float GetAcceleration()
    {
        if (isWallJumping)
        {
            return accelerationWallJump;
        }
        else if (cc.CollisionState.below)
        {
            return accelerationGrounded;
        }
        else
        {
            return accelerationJump;
        }
    }

    private void Jump()
    {
        // perform regular jump
        if (Input.GetKeyDown(KeyCode.Space) && cc.CollisionState.below)
        {
            velocity.y = jumpVelocity;
            hasDoubleJump = true;
        }
    }

    private void DoubleJump()
    {
        //Consume double jump
        if (Input.GetKeyDown(KeyCode.Space) && hasDoubleJump)
        {
            velocity.y = jumpVelocity;
            hasDoubleJump = false;
        }

        //Reset double jump if opportunity was lost
        if (cc.CollisionState.above || cc.CollisionState.below || wallSliding)
        {
            hasDoubleJump = false;
        }
    }

    private void WallJump()
    {
        int wallDir = (cc.CollisionState.left) ? -1 : 1;

        // reset wallsliding status every update
        wallSliding = false;

        // reset walljumping status on any collision
        if (cc.CollisionState.IsColliding())
        {
            isWallJumping = false;
        }

        // sliding down along an horizontal wall
        if ((cc.CollisionState.left || cc.CollisionState.right) && !cc.CollisionState.below && velocity.y < 0f)
        {
            wallSliding = true;

            // clamp vertical velocity
            if (velocity.y < -wallSlideSpeed)
            {
                velocity.y = -wallSlideSpeed;
            }

            // when sliding, player is stuck for a brief period of time to facilitate walljumping
            if (wallUnstickTime > 0f)
            {
                velocity.x = 0f;
                horizontalSmoothing = 0f;

                if (input.x != wallDir && input.x != 0)
                {
                    wallUnstickTime -= Time.deltaTime;
                }
                else
                {
                    wallUnstickTime = wallStickDuration;
                }
            }
            // reset for the following update
            else
            {
                wallUnstickTime = wallStickDuration;
            }
        }

        // perform walljump, but only when already wallsliding
        if (Input.GetKeyDown(KeyCode.Space) && wallSliding)
        {
            isWallJumping = true;
            wallSliding = false;
            hasDoubleJump = true;

            // when jumping against the wall, go up
            if (wallDir == input.x)
            {
                velocity.x = -wallDir * wallJumpClimb.x;
                velocity.y = wallJumpClimb.y;
            }

            // when performing a static jump, go down
            else if (input.x == 0f)
            {
                velocity.x = -wallDir * wallJumpOff.x;
                velocity.y = wallJumpOff.y;
            }

            // otherwise, jump from the wall
            else
            {
                velocity.x = -wallDir * wallJumpLeap.x;
                velocity.y = wallJumpLeap.y;
            }
        }
    }

    private void Dash()
    {
        // reset dashing on horizontal collisions or after set duration
        if (isDashing && ((cc.CollisionState.right || cc.CollisionState.left) || Time.time > dashStopTime))
        {
            isDashing = false;
            velocity.x = 0f;
            rainbowTrail.enabled = false;
        }

        // if dashing, maintain a constant velocity
        if (isDashing)
        {
            velocity.x = dashSpeed * dashDirection;
        }
        else
        {
            if (input.x != 0f)
            {
                dashDirection = input.x;
            }
        }

        // perform dash
        if (Input.GetKeyDown(KeyCode.K) && Time.time > nextDashTime && !wallSliding)
        {
            nextDashTime = Time.time + dashCooldown;
            dashStopTime = Time.time + dashDuration;

            velocity.x = dashSpeed * dashDirection;
            isDashing = true;
            rainbowTrail.Clear();
            rainbowTrail.enabled = true;
        }
    }

    private void HandleAnimations()
    {
        bool isRunning = Mathf.Abs(velocity.x) >= 0.1f && !(cc.CollisionState.left || cc.CollisionState.right);
        int air = -1;
        if (velocity.y > 0.1f) air = 1;
        if (velocity.y < -0.1f) air = -1;
        if (cc.CollisionState.below) air = 0;

        //Set animation parameters
        animator.SetBool("Running", isRunning);
        animator.SetInteger("Air", air);
        animator.SetBool("WallSliding", wallSliding);
        animator.SetBool("Dashing", isDashing);

        //Flip sprite on input direction
        if(!wallSliding)
        {
            if (input.x > 0) sr.flipX = true;
            if (input.x < 0) sr.flipX = false;
        }
        else
        {
            if (cc.CollisionState.right) sr.flipX = false;
            if (cc.CollisionState.left) sr.flipX = true;
        }
    }

    public void ChangeShield(ColorType color, bool activate)
    {
        switch (color)
        {
            case ColorType.Red:
                redShield.SetActive(activate);
                break;

            case ColorType.Yellow:
                yellowShield.SetActive(activate);
                break;

            case ColorType.Green:
                greeenShield.SetActive(activate);
                break;

            default:
                break;
        }
    }

    private void Die()
    {

    }

    void OnTriggerStayEvent(Collider2D col)
    {
        DeathRayController deathRay = col.GetComponent<DeathRayController>();

        if(deathRay != null)
        {
            if(ColorManager.Instance.GetOwner(deathRay.color) == this)
            {
                ChangeShield(deathRay.color, true);
            }
            else
            {
                ChangeShield(deathRay.color, false);
                Die();
            }
        }
    }

    void OnTriggerExitEvent(Collider2D col)
    {
        DeathRayController deathRay = col.GetComponent<DeathRayController>();

        if (deathRay != null)
        {
            ChangeShield(deathRay.color, false);
        }
    }
}