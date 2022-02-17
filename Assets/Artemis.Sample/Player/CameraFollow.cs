using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform Target;
    
    private void LateUpdate()
    {
        if (Target == null)
        {
            return;
        }

        var currentPosition = (Vector2) transform.position;
        var desiredPosition = (Vector2) Target.position;

        transform.position = Vector2.Lerp(currentPosition, desiredPosition, Time.deltaTime * 32);
    }
}
