using UnityEngine;

[SelectionBase]
public class MovementController : MonoBehaviour
{
    [Header("Start initialization")]
    [SerializeField] public float CameraTargetHeight = 2f;
    [SerializeField] public float CameraDistance = 3f;
    [Space]

    [Header("Realtime initialization")]
    [SerializeField] public Animator PlayerAnimator;
    [SerializeField] public Transform CameraTransform;
    [SerializeField] public float CameraMinY = 0.1f;
    [SerializeField] public float CameraMaxY = 3.5f;
    [SerializeField] public float CameraHorizontalSpeed = 5.0F;
    [SerializeField] public float CameraVerticalSpeed = 3.0F;
    [SerializeField] public float MoveSpeed = 4.0f;
    [Space]

    private float mouseXAxis =0.0f;
    private float mouseYAxis = 0.0f;
    private bool wPressed = false;
    private bool aPressed = false;
    private bool sPressed = false;
    private bool dPressed = false;
    private bool attackMouseDown = false;

    private Vector3 offsetX;
    private Vector3 offsetY;

    void Start()
    {
        offsetX = new Vector3(0, CameraTargetHeight, CameraDistance);
        offsetY = new Vector3(0, 0, CameraDistance);
    }

    void Update()
    {
        HandleUserInput();


        // Orient Camera to Player transform
        var inputMouseX = Input.GetAxis("Mouse X");
        var inputMouseY = Input.GetAxis("Mouse Y");
        offsetX = Quaternion.AngleAxis(inputMouseX * CameraHorizontalSpeed, Vector3.up) * offsetX;
        offsetY = Quaternion.AngleAxis(inputMouseY * CameraVerticalSpeed, Vector3.right) * offsetY;
        var limitedY = Mathf.Clamp(offsetY.y, CameraMinY, CameraMaxY);
        CameraTransform.position = transform.position + offsetX + new Vector3(0.0f, limitedY, 0.0f);
        CameraTransform.LookAt(transform.position + new Vector3(0, 1.8f, 0));

        // Rotate Player in the same direction as Camera
        var rot = Quaternion.LookRotation(new Vector3(CameraTransform.forward.x, 0.0f, CameraTransform.forward.z), Vector3.up);
        transform.rotation = rot;

        // Get user input
        var wPressed = Input.GetKey(KeyCode.W);
        var aPressed = Input.GetKey(KeyCode.A);
        var sPressed = Input.GetKey(KeyCode.S);
        var dPressed = Input.GetKey(KeyCode.D);

        if (!wPressed && !sPressed)
        {
            PlayerAnimator.SetFloat("FrontBack", 0.0f);
        }
        if (wPressed)
        {
            PlayerAnimator.SetFloat("FrontBack", 1.0f);
        }
        else if (sPressed)
        {
            PlayerAnimator.SetFloat("FrontBack", -1.0f);
        }

        if (!aPressed && !dPressed)
        {
            PlayerAnimator.SetFloat("LeftRight", 0.0f);
        }
        if (aPressed)
        {
            PlayerAnimator.SetFloat("LeftRight", -1.0f);
        }
        else if (dPressed)
        {
            PlayerAnimator.SetFloat("LeftRight", 1.0f);
        }

        

        var posChanged = false;
        var moveSpeed = MoveSpeed * Time.deltaTime;
        var forwardDir = transform.forward;
        var backwardDir = new Vector3(forwardDir.x * -1.0f, forwardDir.y * -1.0f, forwardDir.z * -1.0f); ;
        var rightDir = transform.right;
        var leftDir = new Vector3(rightDir.x * -1.0f, rightDir.y * -1.0f, rightDir.z * -1.0f);
        Vector3 diffPos = Vector3.zero;

        // Handle forward and backward directions
        if (wPressed && !sPressed)
        {
            if (Input.GetMouseButtonDown(0))
            {
                PlayerAnimator.CrossFade("Jump", 0.0f);
                MoveSpeed = 8;
            }

            diffPos = forwardDir.normalized * moveSpeed;
            posChanged = true;
        }
        else if (sPressed && !wPressed)
        {
            diffPos = backwardDir.normalized * moveSpeed;
            posChanged = true;
        }

        // Handle right and left directions
        if (dPressed && !aPressed)
        {
            diffPos += rightDir.normalized * moveSpeed;
            posChanged = true;
        }
        else if (aPressed && !dPressed)
        {
            diffPos += leftDir.normalized * moveSpeed;
            posChanged = true;
        }

        // Apply position transformations
        if (posChanged)
        {
            transform.position += diffPos;
        }
    }

    private void HandleUserInput()
    {
        // Get user input
        var mouseXAxis = Input.GetAxis("Mouse X");
        var mouseYAxis = Input.GetAxis("Mouse Y");
        var wPressed = Input.GetKey(KeyCode.W);
        var aPressed = Input.GetKey(KeyCode.A);
        var sPressed = Input.GetKey(KeyCode.S);
        var dPressed = Input.GetKey(KeyCode.D);
        var attackMouseDown = Input.GetMouseButtonDown(0);
    }
}
