using UnityEngine;

[SelectionBase]
public class MovementController : MonoBehaviour
{
    [Header("Start initialization")]
    [SerializeField] public float CameraTargetHeight = 2f;
    [SerializeField] public float CameraDistance = 3f;
    [Space]

    [Header("Realtime initialization")]
    [SerializeField] public Transform CameraTransform;
    [SerializeField] public float CameraMinY = 0.1f;
    [SerializeField] public float CameraMaxY = 3.5f;
    [SerializeField] public float CameraHorizontalSpeed = 5.0F;
    [SerializeField] public float CameraVerticalSpeed = 3.0F;
    [SerializeField] public float MoveSpeed = 4.0f;
    [Space]

    private Vector3 offsetX;
    private Vector3 offsetY;

    void Start()
    {
        offsetX = new Vector3(0, CameraTargetHeight, CameraDistance);
        offsetY = new Vector3(0, 0, CameraDistance);
    }

    // Update is called once per frame
    void Update()
    {
        // Orient Camera to Player transform
        offsetX = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * CameraHorizontalSpeed, Vector3.up) * offsetX;
        offsetY = Quaternion.AngleAxis(Input.GetAxis("Mouse Y") * CameraVerticalSpeed, Vector3.right) * offsetY;
        var limitedY = Mathf.Clamp(offsetY.y, CameraMinY, CameraMaxY);
        CameraTransform.position = transform.position + offsetX + new Vector3(0.0f, limitedY, 0.0f);
        CameraTransform.LookAt(transform.position);

        // Rotate Player in the same direction as Camera
        var rot = Quaternion.LookRotation(new Vector3(CameraTransform.forward.x, 0.0f, CameraTransform.forward.z), Vector3.up);
        transform.rotation = rot;

        // Get user input
        var wPressed = Input.GetKey(KeyCode.W);
        var aPressed = Input.GetKey(KeyCode.A);
        var sPressed = Input.GetKey(KeyCode.S);
        var dPressed = Input.GetKey(KeyCode.D);

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
}
