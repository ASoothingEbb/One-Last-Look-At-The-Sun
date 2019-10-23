using UnityEngine;
using UnityEngine.Experimental.VFX;
public class ScaleEmitWithVelocity : MonoBehaviour
{
    Rigidbody body;
    VisualEffect vfx;    

    public float rateScale = 10;

    public void Start()
    {
        body = GameObject.FindGameObjectWithTag("player").GetComponent<Rigidbody>();
        vfx = GetComponent<VisualEffect>();
    }
    // Update is called once per frame
    void Update()
    {
        vfx.SetFloat("rate", rateScale * (-body.velocity.y));
        vfx.SetVector3("velocity", new Vector3(0, 2 * -body.velocity.y, 0));
    }
}
