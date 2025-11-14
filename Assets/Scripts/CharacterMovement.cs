using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Vector2 targetPosition;
    private bool isMoving = false;

    private Rigidbody2D rb;
    private Animator animator;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        targetPosition = rb.position;
    }

    void Update()
    {
        // Detect mouse click
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            MoveTo(clickPosition);
        }

        // Determine direction for animation if moving
        if (isMoving)
        {
            Vector2 direction = (targetPosition - rb.position).normalized;
            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
                animator.SetInteger("direction", direction.x > 0 ? 2 : 1); // Right : Left
            else
                animator.SetInteger("direction", direction.y > 0 ? 3 : 0); // Up : Down
        }

        // Update animation immediately based on current movement state
        animator.SetBool("isMoving", isMoving);
    }

    void FixedUpdate()
    {
        if (isMoving)
        {
            Vector2 newPosition = Vector2.MoveTowards(rb.position, targetPosition, moveSpeed * Time.fixedDeltaTime);
            rb.MovePosition(newPosition);

            // Check if arrived
            if (Vector2.Distance(rb.position, targetPosition) < 0.01f)
            {
                rb.MovePosition(targetPosition); // Snap to exact position
                isMoving = false; // Stop moving

                // Force Idle animation immediately
                animator.Play("idlebase"); // Replace "Idle" with your Idle clip name
            }
        }
    }


    public void MoveTo(Vector2 target)
    {
        targetPosition = target;
        isMoving = true;
    }

    public bool HasArrived()
    {
        return !isMoving;
    }
}
