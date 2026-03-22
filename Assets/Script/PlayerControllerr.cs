using System.Collections;
using UnityEngine;

public class PlayerControllerr : MonoBehaviour
{
    [Header("Movement")]
    public float speed;
    public float jumpForce;
    private float inputHorizontal;
    private float inputVertical;
    private Rigidbody2D rb;
    private bool facingRight = true;
    [Header("Jump")]
    public int extraJumpsValue;
    public float jumpTime;
    private int extraJumps;
    private float jumpTimeCounter;
    private bool isJumping;
    [Header("Ground")]
    private bool isGrounded;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatIsGround;
    [Header("Ladder")]
    public float distance;
    public LayerMask whatIsLadder;
    private bool isClimbing;
    RaycastHit2D hitInfo;
    [Header("Effect")]
    public Animator playerAnimator;
    public float squashAmount = 0.7f;
    public float squashSpeed = 10f;
    private Vector3 originalScale;
    bool wasGrounded;
    bool isSquashing = false;

    public static bool isGamePaused = false;
    [Header("Dash")]
    public float dashSpeed = 15f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 0.5f;

    bool isDashing = false;
    bool canDash = true;
    float originalGravity;

    [Header("Wall Slide")]
    public Transform wallCheck;
    public float wallCheckDistance = 0.5f;
    public LayerMask whatIsWall;
    public float wallSlideSpeed = 2f;

    bool isTouchingWall;
    bool isWallSliding;

 

    void Start()
    {
        extraJumps = extraJumpsValue;
        rb = GetComponent<Rigidbody2D>();

        originalScale = transform.localScale;
        originalGravity = rb.gravityScale;
    }
    void FixedUpdate()
    {
        if (isGamePaused || isDashing ) return;
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);
        playerAnimator.SetBool("Jump", !isGrounded);

        inputHorizontal = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(inputHorizontal * speed, rb.linearVelocity.y);
        if (inputHorizontal != 0)
        {
            playerAnimator.SetBool("Run", true);
        }
        else
        {
            playerAnimator.SetBool("Run", false);
        }
        if (!isSquashing && Mathf.Abs(inputHorizontal) > 0.1f)
        {
            if (inputHorizontal > 0 && !facingRight)
                Flip();
            else if (inputHorizontal < 0 && facingRight)
                Flip();
        }
    }
    void Update()
    {
        if (isGamePaused) return;

        isTouchingWall = Physics2D.Raycast(transform.position, facingRight ? Vector2.right : Vector2.left, wallCheckDistance, whatIsWall);
        PlayerInput();

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash && !isDashing)
        {
            StartCoroutine(Dash());
            playerAnimator.SetTrigger("Dash");
        }

        if (isGrounded == true)
        {
            extraJumps = extraJumpsValue;
            if (rb.linearVelocity.y == 0)
            {
                isJumping = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) && extraJumps > 0)
        {

            isJumping = true;
            jumpTimeCounter = jumpTime;
            rb.linearVelocity = Vector2.up * jumpForce;
            extraJumps--;
        }
        else if (Input.GetKeyDown(KeyCode.Space) && extraJumps == 0 && isGrounded == true)
        {

            isJumping = true;
            jumpTimeCounter = jumpTime;
            rb.linearVelocity = Vector2.up * jumpForce;
        }

        if (Input.GetKey(KeyCode.Space) && isJumping)
        {
            if (jumpTimeCounter > 0)
            {
                rb.linearVelocity = Vector2.up * jumpForce;
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            isJumping = false;
        }

        hitInfo = Physics2D.Raycast(transform.position, Vector2.up, distance, whatIsLadder);
        if (hitInfo.collider != null)
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                isClimbing = true;
            }
        }

        else
        {
            isClimbing = false;
        }

        if (isClimbing == true && hitInfo.collider != null)
        {
            inputVertical = Input.GetAxisRaw("Vertical");
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, inputVertical * speed);
            rb.gravityScale = 0;
        }
        else
        {
            rb.gravityScale = originalGravity;
        }
        if (playerAnimator != null)
        {
            //playerAnimator.SetFloat("velocityX", Mathf.Abs(inputHorizontal));
            playerAnimator.SetFloat("velocityY", rb.linearVelocity.y);

        }
        if (!wasGrounded && isGrounded)
        {
            StartCoroutine(Squash());
        }

        wasGrounded = isGrounded;

        if (isTouchingWall && !isGrounded && rb.linearVelocity.y < 0)
        {
            isWallSliding = true;
        }
        else
        {
            isWallSliding = false;
        }
        if (isWallSliding)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, -wallSlideSpeed);
        }
    }

    void Flip()
    {
        facingRight = !facingRight;

        float direction = facingRight ? 1 : -1;

        transform.localScale = new Vector3(
            direction * Mathf.Abs(originalScale.x),
            transform.localScale.y,
            1
        );
    }
    void PlayerInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            playerAnimator.SetTrigger("Victory");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            playerAnimator.SetTrigger("Hurt");
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            playerAnimator.SetTrigger("Dead");
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            playerAnimator.SetTrigger("Spawn");
        }
    }
    IEnumerator Squash()
    {
        isSquashing = true;

        float direction = facingRight ? 1 : -1;

        transform.localScale = new Vector3(
            direction * Mathf.Abs(originalScale.x) * 1.2f,
            originalScale.y * squashAmount,
            1
        );

        yield return new WaitForSeconds(0.1f);

        transform.localScale = new Vector3(
            direction * Mathf.Abs(originalScale.x),
            originalScale.y,
            1
        );

        isSquashing = false;
    }
    IEnumerator Dash()
    {
        isDashing = true;
        canDash = false;

        float dashX = Input.GetAxisRaw("Horizontal");
        float dashY = Input.GetAxisRaw("Vertical");

        Vector2 dashDir = new Vector2(dashX, dashY).normalized;

        // ถ้าไม่ได้กดทิศ ให้ dash ไปข้างหน้าที่หันอยู่
        if (dashDir == Vector2.zero)
        {
            dashDir = facingRight ? Vector2.right : Vector2.left;
        }

        rb.gravityScale = 0;

        float timer = 0f;

        while (timer < dashDuration)
        {
            rb.linearVelocity = dashDir * dashSpeed;
            timer += Time.deltaTime;
            yield return null;
        }

        rb.gravityScale = originalGravity;
        isDashing = false;

        // รอจนกว่าจะแตะพื้น
        yield return new WaitUntil(() => isGrounded);

        // หน่วง cooldown หลังแตะพื้น (optional)
        yield return new WaitForSeconds(dashCooldown);

        canDash = true;
    }
}