using UnityEngine;
using System.Collections;
using UnityEngine.UIElements;

public class BasicController : MonoBehaviour
{
    [Header("Param�tres de mouvement")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;
    public float gravity = -9.81f;
    public bool canMove = true;

    [Header("Param�tres de collision")]
    public LayerMask groundMask;

    private CharacterController controller;
    private Animator animator;
    private Vector3 velocity;
    private bool isGrounded;

    private Vector3 forwardAxis;
    private Vector3 rightAxis;

    private float originalHeight;
    private Vector3 originalCenter;

    public float DeadHeight;
    public Vector3 DeadCenter;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        originalHeight = controller.height;
        originalCenter = controller.center;

        animator.SetBool("Fight", false);

        SetupAxesRelativeToPlayer();
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(transform.position + Vector3.down * (controller.height / 2), 0.1f, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if (!canMove)
        {
            animator.SetInteger("Speed", 0);
        }
        else
        {
            MoveFromInputs();
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

        // Test des animations
        //TestAnimations();
    }

    private void AdjustCharacterController(float height, Vector3 center)
    {
        controller.height = height;
        controller.center = center;
    }

    private void MoveFromInputs()
    {
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");

        Vector3 direction = (forwardAxis * vertical + rightAxis * horizontal).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float angle = Mathf.LerpAngle(transform.eulerAngles.y, targetAngle, rotationSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            controller.Move(direction * moveSpeed * Time.deltaTime);

            animator.SetInteger("Speed", 2);
        }
        else
        {
            animator.SetInteger("Speed", 0);
        }
    }

    private void SetupAxesRelativeToPlayer()
    {
        forwardAxis = transform.forward.normalized;
        rightAxis = transform.right.normalized;
    }

    private void TestAnimations()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            StartCoroutine(PlayAnimation("Attack1"));
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            StartCoroutine(PlayAnimation("Attack2"));
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(PlayAnimation("Attack3"));
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            StartCoroutine(PlayAnimation("Boost1"));
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            StartCoroutine(PlayAnimation("Damage"));
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            StartCoroutine(PlayAnimationDead("Dead"));
            AdjustCharacterController(DeadHeight, DeadCenter);
        }
    }

    private IEnumerator PlayAnimation(string animationName)
    {
        animator.SetBool(animationName, true);
        Debug.Log(animationName);
        yield return new WaitForSeconds(3.0f);
        animator.SetBool(animationName, false);
        AdjustCharacterController(originalHeight, originalCenter);
    }

    private IEnumerator PlayAnimationDead(string animationName)
    {
        animator.SetBool(animationName, true);
        Debug.Log(animationName);
        yield return new WaitUntil(() => !animator.GetBool("Fight"));
        animator.SetBool(animationName, false);
        AdjustCharacterController(originalHeight, originalCenter);
    }
}