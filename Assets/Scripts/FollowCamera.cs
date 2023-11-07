using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public bool IsActive;
    public Transform Target;
    public Vector3 Offset;

    void Update()
    {
        if (IsActive) transform.position = Target.position + Offset;
    }
}
