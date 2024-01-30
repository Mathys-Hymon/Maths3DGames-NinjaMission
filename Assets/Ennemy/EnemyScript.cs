using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] private float detectionZone;
    [SerializeField] private float stabAngle;
    [SerializeField] private AudioClip[] audioSound;


    private bool hasSeenPlayer, touchWall, dead, playSound, isJumping;
    private Quaternion initialRot;

    private void Start()
    {
        initialRot = transform.rotation;
        transform.GetChild(0).gameObject.SetActive(false);
    }

    private void Update()
    {

        if(!dead)
        {
            Vector3 PlayerPos = PlayerMovement.instance.transform.position - transform.position;
           
            if (PlayerPos.magnitude <= detectionZone)
            {
                RaycastHit hit;

                if (Physics.Raycast(transform.position, PlayerPos, out hit))
                {
                    if (hit.collider.gameObject.GetComponent<PlayerMovement>() != null)
                    {
                        touchWall = false;
                    }
                    else
                    {
                        touchWall = true;
                    }
                }
                if (Vector3.Dot(transform.forward, PlayerPos.normalized) < -1 + (stabAngle / 50) && !touchWall)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(-PlayerPos);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10 * Time.deltaTime);
                    transform.GetChild(3).GetChild(0).gameObject.SetActive(true);
                    transform.GetChild(3).GetChild(1).gameObject.SetActive(false);
                    if(!isJumping)
                    {
                        isJumping = true;
                        Invoke(nameof(ResetJump), 3);
                        GetComponent<Rigidbody>().AddForce(transform.up * 150);
                        var audioSource = GetComponent<AudioSource>();
                        audioSource.pitch = 1 - PlayerMovement.instance.getSus();
                        audioSource.clip = audioSound[0];
                        audioSource.Play();
                    }
                    PlayerMovement.instance.setCanStab(false);
                    hasSeenPlayer = true;
                }

                else if (PlayerPos.magnitude <= detectionZone / 5 && Vector3.Dot(transform.forward, PlayerPos.normalized) > 1 - (stabAngle / 90) && Mathf.Abs(Vector3.Dot(PlayerPos, PlayerMovement.instance.transform.forward)) > 0.9f && !touchWall)
                {
                    transform.GetChild(3).GetChild(0).gameObject.SetActive(false);
                    if(PlayerMovement.instance.getCanStab())
                    {
                        transform.GetChild(3).GetChild(1).gameObject.SetActive(true);
                    }
                    if (Input.GetMouseButtonDown(0) && PlayerMovement.instance.getCanStab() && !dead)
                    {
                        gameObject.GetComponent<Rigidbody>().freezeRotation = false;
                        gameObject.GetComponent<Rigidbody>().AddForce(transform.forward);
                        gameObject.GetComponent<MeshRenderer>().enabled = false;
                        transform.GetChild(0).gameObject.SetActive(true);
                        transform.GetChild(1).GetComponent<MeshRenderer>().enabled = false;
                        transform.GetChild(2).GetComponent<MeshRenderer>().enabled = false;
                        var audioSource = GetComponent<AudioSource>();
                        audioSource.clip = audioSound[1];
                        audioSource.Play();
                        dead = true;
                        Invoke(nameof(destroySelf), 0.2f);
                    }
                }
                else
                {
                    transform.GetChild(3).GetChild(0).gameObject.SetActive(false);
                    transform.GetChild(3).GetChild(1).gameObject.SetActive(false);
                    transform.rotation = Quaternion.Slerp(transform.rotation, initialRot, 2 * Time.deltaTime);
                    if (hasSeenPlayer)
                    {
                        hasSeenPlayer = false;
                        PlayerMovement.instance.setCanStab(true);
                    }
                }
            }
            else
            {
                transform.GetChild(3).GetChild(0).gameObject.SetActive(false);
                transform.GetChild(3).GetChild(1).gameObject.SetActive(false);
                transform.rotation = Quaternion.Slerp(transform.rotation, initialRot, 2 * Time.deltaTime);
                if (hasSeenPlayer)
                {
                    hasSeenPlayer = false;
                    PlayerMovement.instance.setCanStab(true);
                }
            }

        }
        else
        {
            transform.GetChild(3).GetChild(0).gameObject.SetActive(false);
            transform.GetChild(3).GetChild(1).gameObject.SetActive(false);
            if (hasSeenPlayer)
            {
                hasSeenPlayer = false;
                PlayerMovement.instance.setCanStab(true);
            }
        }
    }

    private void destroySelf()
    {
        Destroy(gameObject.GetComponent<EnemyScript>());
    }

    private void ResetJump()
    {
        isJumping = false;
    }
}