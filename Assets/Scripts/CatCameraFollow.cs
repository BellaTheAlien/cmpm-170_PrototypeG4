using UnityEngine;

public class CatFollowCamera : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 2, -4);
    public float followSpeed = 6f;

    void LateUpdate()
    {
        if (target == null) return;

        // Smooth follow
        Vector3 desiredPos = target.position + target.TransformDirection(offset);
        transform.position = Vector3.Lerp(transform.position, desiredPos, followSpeed * Time.deltaTime);

        // Look at the cat
        transform.LookAt(target.position + Vector3.up * 0.5f);
    }
}
