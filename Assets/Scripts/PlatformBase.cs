using Dinojump.Schemas;
using UnityEngine;

public class PlatformBase : MonoBehaviour
{
    [SerializeField]
    private float SpeedLerp = .02f;
    public PlatformSchema PlatformSchema;
    public string Key;

    float platformYOffset = 1.2f;

    void FixedUpdate()
    {
        if (PlatformSchema?.position != null && (PlatformSchema?.position.x != transform.position.x || PlatformSchema?.position.y != transform.position.y))
        {
                var t = Time.deltaTime * SpeedLerp;
                Vector2 desiredPostion = new Vector3(PlatformSchema.position.x, PlatformSchema.position.y + platformYOffset);
                transform.position = Vector2.Lerp(transform.position, desiredPostion, t);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Vector3 size = new Vector3(PlatformSchema.size.width, PlatformSchema.size.height);
        Gizmos.DrawWireCube(new Vector3(PlatformSchema.position.x, PlatformSchema.position.y), size);
    }
}
