using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement instance;

    [SerializeField] private float MouseSensitivity;
    [SerializeField] private float MoveSpeed;
    [SerializeField] private float JumpForce;
    [SerializeField] private GameObject susJauge;

    private Rigidbody Rigid;
    private float susOmeter;
    private GameObject cam;
    private Vector2 input, CameraInput;
    private bool canStab = true, dead;
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

        EnemyScript[] enemyAlive = GameObject.FindObjectsOfType<EnemyScript>();

        if(enemyAlive.Length > 0 )
        {
            if (Input.GetKeyDown("space"))
            {
                Rigid.AddForce(transform.up * JumpForce * 100);
            }
            CameraInput.x += Input.GetAxisRaw("Mouse Y") * MouseSensitivity;
            CameraInput.y += Input.GetAxisRaw("Mouse X") * MouseSensitivity;
            CameraInput.x = Mathf.Clamp(CameraInput.x, -60, 60);

            if (!canStab && susOmeter <= 15)
            {
                susOmeter += Time.deltaTime;
                susJauge.transform.localPosition = susJauge.transform.localPosition + new Vector3(Time.deltaTime * 50, 0, 0);
            }
            else if (susOmeter > 0 && susOmeter < 15)
            {
                susOmeter -= Time.deltaTime * 2;
                susJauge.transform.localPosition = susJauge.transform.localPosition - new Vector3(Time.deltaTime * 100, 0, 0);
            }
            else if (!dead && susOmeter >= 15)
            {
                dead = true;
                SceneManager.LoadScene("DeadScene");
            }
        }
        else
        {
            SceneManager.LoadScene("WinScene");
        }
        
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

    public float getSus()
    {
        return susOmeter/100;
    }
}
