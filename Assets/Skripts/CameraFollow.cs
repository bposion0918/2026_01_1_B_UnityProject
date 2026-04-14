using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public Transform target;
    public Vector3 offset = new Vector3(0, 5, -10);
    public float smoothSpeed = 0.125f;

    private void LateUpdate()
    {
        //LateUpdate()를 사용하는 이유는 카메라가 플레이어의 이동을 모두 처리한 이후에 따라가게 하기 위해 

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        transform.LookAt(target.position);
    }
}
