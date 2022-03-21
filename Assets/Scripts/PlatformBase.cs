using Dinojump.Schemas;
using UnityEngine;

public class PlatformBase : MonoBehaviour
{
    [SerializeField]
    private float SpeedLerp = .02f;
    public PlatformSchema PlatformSchema;
    public string Key;

    Vector2 previousPos = new Vector2(0,0);
    // Start is called before the first frame update
    void Start()
    {
          
    }

    // Update is called once per frame
    void Update()
    {
        if (PlatformSchema?.position != null)
        {
            if (PlatformSchema.position.y != previousPos.x || PlatformSchema.position.y != previousPos.x)
            {
                var t = Time.deltaTime / SpeedLerp;
                Vector2 desiredPostion = new Vector3(PlatformSchema.position.x, PlatformSchema.position.y);
                transform.position = Vector2.Lerp(transform.position, desiredPostion, t);
                previousPos = new Vector2(PlatformSchema.position.x, PlatformSchema.position.y);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Vector3 size = new Vector3(PlatformSchema.size.width, PlatformSchema.size.height);
        Gizmos.DrawWireCube(new Vector3(PlatformSchema.position.x, PlatformSchema.position.y), size);
    }
}
