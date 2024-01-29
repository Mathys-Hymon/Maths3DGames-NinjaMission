using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement instance;

    [SerializeField] private float MouseSensitivity;
    [SerializeField] private float MoveSpeed;
    [SerializeField] private float JumpForce;

    private Rigidbody Rigid;
    private GameObject cam;
    private Vector2 input, CameraInput;
    private Quaternion cameraRot;
    private bool canStab = true;
    private void Start()
    {
        instance = this;
        Cursor.lockState = CursorLockMode.Locked;
        Rigid = GetComponent<Rigidbody>();
        cam = transform.GetChild(0).gameObject;
    }
    private void Update()
    {
        input = new Vector2(Input.GetAxisRaw("Vertical"), Input.GetAxisRaw("Horizontal")).normalized;
        if (Input.GetKeyDown("space"))
        {
            Rigid.AddForce(transform.up * JumpForce *100);
        }
        CameraInput.x += Input.GetAxisRaw("Mouse Y") * MouseSensitivity;
        CameraInput.y += Input.GetAxisRaw("Mouse X") * MouseSensitivity;
    }
    private void FixedUpdate()
    {   
        Rigid.MovePosition(transform.position + (transform.forward * input.x * MoveSpeed * Time.fixedDeltaTime) + (transform.right * input.y * MoveSpeed * Time.fixedDeltaTime));
        CameraRotation();
    }
    private void CameraRotation()
    {
        transform.rotation = Quaternion.Euler(0, CameraInput.y, 0);
        cam.transform.rotation = Quaternion.Euler(-CameraInput.x, CameraInput.y, 0);
    }

    public void setCanStab(bool newcanStab)
    {
        canStab = newcanStab;
    }
    public bool getCanStab()
    {
        return canStab;
    }
}
