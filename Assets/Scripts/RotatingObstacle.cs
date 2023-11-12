using UnityEngine;

public class RotatingObstacle : MonoBehaviour
{

    void Update()
    {
        transform.Rotate(new Vector3(0, 0, 1));
    }
}
