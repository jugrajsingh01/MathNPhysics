using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealisticBall : Ball
{
    [SerializeField]
    public float bounciness = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        radius = transform.localScale.x / 2f;
        velocity = -5f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        velocity = velocity + a * Time.deltaTime;
        //calculateVelocity(transform.position.y - dy, transform.position.y);
        float y_pos = transform.position.y;
        y_pos = velocity * Time.deltaTime;

        transform.position += new Vector3(0, y_pos, 0);
    }

    public override void onEnvCollision(BoundingBox hit, string side)
    {
        base.onEnvCollision(hit, side);

        velocity = this.velocity * -1 * bounciness;
    }

    public override void onBallCollision(Ball b)
    {
        velocity = this.velocity * -1 * bounciness;
    }
}
