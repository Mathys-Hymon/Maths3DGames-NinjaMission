using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] private float detectionZone;
    [SerializeField] private float stabAngle;


    private bool hasSeenPlayer, touchWall;
    private PlayerMovement playerRef;

    private void Start()
    {
        playerRef = PlayerMovement.instance;
    }

    private void Update()
    {

        Vector3 PlayerPos = playerRef.transform.position - transform.position ;

        if (PlayerPos.magnitude <= detectionZone)
        {

            RaycastHit hit;
            
            if(Physics.Raycast(transform.position, PlayerPos, out hit))
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

            if (Vector3.Dot(transform.forward, PlayerPos.normalized) < -1 + (stabAngle / 90) && !touchWall)
            {
                gameObject.GetComponent<MeshRenderer>().material.color = Color.black;
                playerRef.setCanStab(false);
                hasSeenPlayer = true;
            }

            else if (PlayerPos.magnitude <= detectionZone / 5 && Vector3.Dot(transform.forward, PlayerPos.normalized) > 1 - (stabAngle / 90) && Mathf.Abs(Vector3.Dot(PlayerPos, playerRef.transform.forward)) > 1.4f && !touchWall)
            {
                gameObject.GetComponent<MeshRenderer>().material.color = Color.blue;
                if (Input.GetMouseButtonDown(0) && playerRef.getCanStab())
                {
                    gameObject.GetComponent<Rigidbody>().freezeRotation = false;
                }
            }
            else
            {
                gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
                if (hasSeenPlayer)
                {
                    hasSeenPlayer = false;
                    playerRef.setCanStab(true);
                }
            }
        }

    }
}