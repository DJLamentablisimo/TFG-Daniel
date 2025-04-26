using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    [Header("Movimiento")]
    [Tooltip("Velocidad de movimiento normal.")]
    public float speed = 6.0f;
    [Tooltip("Multiplicador de velocidad al correr (mant�n presionado Left Shift).")]
    public float runMultiplier = 1.5f;

    [Header("Salto y Gravedad")]
    [Tooltip("Altura del salto.")]
    public float jumpHeight = 1.2f;
    [Tooltip("Valor de la gravedad (negativo).")]
    public float gravity = -9.81f;
    private Vector3 velocity; // Velocidad actual, incluyendo gravedad
    private bool isGrounded;

    [Header("Detecci�n de Suelo")]
    [Tooltip("Objeto que se ubicar� en los pies del jugador para detectar el suelo.")]
    public Transform groundCheck;
    [Tooltip("Radio de la esfera para la comprobaci�n del suelo.")]
    public float groundDistance = 0.4f;
    [Tooltip("Capa(s) que se considerar�n como suelo.")]
    public LayerMask groundMask;

    [Header("Control de C�mara")]
    [Tooltip("Transform de la c�mara en primera persona (debe ser hijo del jugador).")]
    public Transform playerCamera;
    [Tooltip("Sensibilidad del rat�n.")]
    public float mouseSensitivity = 100f;
    private float xRotation = 0f;  // �ngulo vertical de la c�mara

    // Referencia al CharacterController
    private CharacterController controller;

    AudioSource m_AudioSource;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        m_AudioSource = GetComponent<AudioSource>();
        // Bloquear el cursor para que no se vea fuera de la ventana del juego
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {

        // 1. Comprobar si el jugador est� tocando el suelo
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && velocity.y < 0)
        {
            // Reiniciamos la velocidad vertical para evitar acumulaci�n de gravedad
            velocity.y = -2f;
        }

        // 2. Movimiento del jugador (horizontal y vertical)
        float x = Input.GetAxis("Horizontal"); // A y D o flechas izquierda/derecha
        float z = Input.GetAxis("Vertical");   // W y S o flechas arriba/abajo
        Vector3 move = transform.right * x + transform.forward * z;

        // Opci�n de correr al mantener presionado Left Shift
        bool running = Input.GetKey(KeyCode.LeftShift);
        float currentSpeed = speed * (running ? runMultiplier : 1f);

        controller.Move(move * currentSpeed * Time.deltaTime);

        // 3. Salto
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            // Calcula la velocidad necesaria para alcanzar la altura del salto deseada
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // 4. Aplicar gravedad
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // 5. Control de la c�mara con el rat�n

        // Obtener movimiento del rat�n
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Rotaci�n vertical de la c�mara (pitch)
        xRotation -= mouseY;
        // Limitar la rotaci�n vertical para evitar que la c�mara gire demasiado
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Rotaci�n horizontal del jugador (yaw)
        transform.Rotate(Vector3.up * mouseX);

        // 6. Reproducir sonido de pasos (Footsteps)
        // Se reproducen si el jugador se mueve y est� en el suelo.
        if (isGrounded && move.magnitude > 0.1f)
        {
            if (!m_AudioSource.isPlaying)
            {
                m_AudioSource.Play();
            }
        }
        else
        {
            if (m_AudioSource.isPlaying)
            {
                m_AudioSource.Stop();
            }
        }

    }
}