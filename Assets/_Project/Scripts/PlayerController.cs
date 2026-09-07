using UnityEngine;

namespace LaboratorioMovimiento
{
    [RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
    public sealed class PlayerController : MonoBehaviour
    {
        [Header("Movimiento")]
        [SerializeField] private float moveSpeed = 7f;
        [SerializeField] private float jumpForce = 12.5f;
        [SerializeField] private float groundCheckDistance = 0.12f;
        [SerializeField] private LayerMask groundLayer = 1 << 8;
        [Header("Game Feel")]
        [SerializeField] private float fallGravityMultiplier = 1.35f;
        private Rigidbody2D rb;
        private Collider2D bodyCollider;
        private float horizontal;
        private bool jumpRequested;
        private float baseGravity;
        private Vector2 spawn;
        private bool isGrounded;
        public bool IsGrounded => isGrounded;
        public int JumpCount { get; private set; }

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            bodyCollider = GetComponent<Collider2D>();
            rb.freezeRotation = true;
            baseGravity = rb.gravityScale;
            spawn = rb.position;
        }

        private void Update()
        {
            // Leer en Update evita perder pulsaciones entre pasos de física.
            horizontal = Input.GetAxisRaw("Horizontal");
            if (Input.GetButtonDown("Jump")) jumpRequested = true;
        }

        private void FixedUpdate()
        {
            RefreshGrounded();
            rb.linearVelocity = new Vector2(horizontal * moveSpeed, rb.linearVelocity.y);
            if (jumpRequested) TryJump();
            jumpRequested = false;
            rb.gravityScale = baseGravity * (rb.linearVelocity.y < -0.1f ? fallGravityMultiplier : 1f);
            if (rb.position.y < -8f) { rb.position = spawn; rb.linearVelocity = Vector2.zero; }
        }

        private void RefreshGrounded()
        {
            Bounds b = bodyCollider.bounds;
            // Sensor en los pies: las paredes no cuentan como suelo.
            Vector2 center = new Vector2(b.center.x, b.min.y - groundCheckDistance * 0.25f);
            isGrounded = rb.linearVelocity.y <= 0.1f && Physics2D.OverlapBox(center,
                new Vector2(b.size.x * 0.8f, groundCheckDistance), 0f, groundLayer) != null;
        }

        public bool TryJump()
        {
            RefreshGrounded();
            if (!isGrounded) return false;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            isGrounded = false;
            JumpCount++;
            return true;
        }

        private void OnDrawGizmosSelected()
        {
            Collider2D col = GetComponent<Collider2D>();
            if (!col) return;
            Bounds b = col.bounds;
            Gizmos.color = isGrounded ? Color.green : Color.yellow;
            Gizmos.DrawWireCube(new Vector2(b.center.x, b.min.y - groundCheckDistance * 0.25f),
                new Vector2(b.size.x * 0.8f, groundCheckDistance));
        }
    }
}
