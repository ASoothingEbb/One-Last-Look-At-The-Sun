using UnityEngine;
using UnityEngine.Experimental.VFX;
public class ScaleEmitWithVelocity : MonoBehaviour
{
    Player player;
    Rigidbody body;
    VisualEffect vfx;    

    public float rateScale = 10;
    public float accelScale = 2;

    public void Start()
    {
        GameObject p = GameObject.FindGameObjectWithTag("player");
        body = p.GetComponent<Rigidbody>();
        player = p.GetComponent<Player>();
        vfx = GetComponent<VisualEffect>();
    }
    // Update is called once per frame
    void Update()
    {
        vfx.SetFloat("rate", rateScale * (-body.velocity.y) * (player.accelerated ? accelScale : 1));
        vfx.SetVector3("velocity", new Vector3(0, 2 * -body.velocity.y, 0));
    }
}
