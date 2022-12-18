using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElasticBall : Ball
{
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

    public override void onEnvCollision(PObject _hit, string side)
    {
        base.onEnvCollision(_hit, side);

        velocity = this.velocity * -1;
    }

    public override void onBallCollision(Ball b)
    {
        velocity = this.velocity * -1;
    }

    //¯\_(ツ)_/¯
    /*//Ekin(0) + Epot(0) = Ekin(1) + Epot(1) rewritten as V(after) =
    float calculateVelocity(float y_before, float y_after, float currentVelocity, float deltaTime)
    {
        float val = (float)System.Math.Sqrt(System.Math.Abs((5 * System.Math.Pow(currentVelocity, 2.00) + 98 * y_before - 98 * y_after) / 5));
        return val;
    }*/

}
