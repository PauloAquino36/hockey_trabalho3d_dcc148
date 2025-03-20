using UnityEngine;

public class CameraDisco : MonoBehaviour
{
    public Transform disco;

    void Update()
    {
        if (disco != null)
        {
            transform.LookAt(disco);
        }
    }
}
