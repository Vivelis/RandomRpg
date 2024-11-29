using UnityEngine;

public class BasicController : MonoBehaviour
{
    [Header("Paramètres de mouvement")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;
    public float gravity = -9.81f; // Gravité appliquée
    public bool canMove = true; // Permet d'activer/désactiver le mouvement

    [Header("Paramètres de collision")]
    public LayerMask groundMask; // Masque pour identifier le sol

    private CharacterController controller;
    private Animator animator;
    private Vector3 velocity;
    private bool isGrounded;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        animator.SetBool("Fight", false);
    }

    void Update()
    {
        if (!canMove)
        {
            animator.SetInteger("Speed", 0);
            return;
        }

        // Vérifie si le joueur est au sol
        isGrounded = Physics.CheckSphere(transform.position + Vector3.down * (controller.height / 2), 0.1f, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Garde le joueur au sol
        }

        // Mouvement horizontal
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            // Calcul de l'angle de rotation
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float angle = Mathf.LerpAngle(transform.eulerAngles.y, targetAngle, rotationSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            // Mouvement
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * moveSpeed * Time.deltaTime);

            animator.SetInteger("Speed", 2);
        }
        else
        {
            animator.SetInteger("Speed", 0);
        }

        // Gravité
        velocity.y += gravity * Time.deltaTime;

        // Applique le mouvement vertical (gravité)
        controller.Move(velocity * Time.deltaTime);
    }
}