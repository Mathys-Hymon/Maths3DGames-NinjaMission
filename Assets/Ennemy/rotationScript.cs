using UnityEngine;

public class rotationScript : MonoBehaviour
{
    void Update()
    {
        transform.LookAt(PlayerMovement.instance.transform.position);
    }
}
