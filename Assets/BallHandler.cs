using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallHandler : MonoBehaviour
{
    static int i = 0;
    [SerializeField]
    List<Wall> collidables = new List<Wall>();

    [SerializeField]
    List<Ball> Balls = new List<Ball>();

    [SerializeField]
    List<Ball> temp;

    // Start is called before the first frame update
    void Start()
    {
        Balls.AddRange(GameObject.FindObjectsOfType<Ball>());
        temp = new List<Ball>(Balls);
        collidables.AddRange(GameObject.FindObjectsOfType<Wall>());
    }

    void FixedUpdate()
    {
        foreach (Ball b in Balls)
        {
          move(b);
            CheckEnvCollision(b);
            checkBallCollision(b);
        }
    }

    void checkBallCollision(Ball b)
    {
        if (temp.Count == 0 || temp.Count == 1)
        {
            temp = new List<Ball>(Balls);
        }
        temp.Remove(b);

        foreach (Ball _b in temp.ToArray())
        {
            Vector3 distance = b.transform.position - _b.transform.position;
            Vector3 ogDistance = _b.transform.position - b.transform.position;
            float dist = System.Math.Abs(distance.magnitude);

            if (dist < _b.radius + b.radius)
            {
                i++;
                //Debug.Log(i);
                temp.Remove(_b);

                Vector3 temp_direction = b.direction;
                float temp_vel = b.velocity;

                b.transform.position = (Vector3.Normalize(distance) * (_b.radius + b.radius)) + _b.transform.position;

                //distance = new Vector3(distance.y * -1f, distance.x, 0);

                /*
                                Debug.DrawRay(b.transform.position, distance, Color.green, 5f);
                                //Debug.Break();

                                b.setDirection(Vector3.Reflect(_b.direction, distance), _b.velocity);

                                _b.setDirection(Vector3.Reflect(temp_direction, distance), temp_vel);*/


                float col_angle = (float)System.Math.Atan2(distance.y, distance.x);

                float cos = (float)System.Math.Cos(col_angle);
                float sin = (float)System.Math.Sin(col_angle);

                float vx1 = b.direction.x*cos+b.direction.y*sin;
                float vy1 = b.direction.y*cos-b.direction.x*sin;
                float vx2 = _b.direction.x*cos+_b.direction.y*sin;
                float vy2 = _b.direction.y*cos-_b.direction.x*sin;

                float vx1final = (0 *vx1+2*1*vx2)/2;
                float vx2final = (0 *vx2+2*1*vx1)/2;

                // update velocity
                vx1 = vx1final;
                vx2 = vx2final;

                //rotate vel back
                b.direction.x = vx1*cos-vy1*sin;
                b.direction.y = vy1*cos+vx1*sin;
                _b.direction.x = vx2*cos-vy2*sin;
                _b.direction.y = vy2*cos+vx2*sin;

                b.setDirection(b.direction, _b.velocity);

                _b.setDirection(_b.direction, temp_vel);

                // b.setDirection(_b.direction, _b.velocity);
                //
                // _b.setDirection(temp_direction, temp_vel);



            }
        }
    }

    void move(Ball b){
      b.transform.position += b.direction.normalized * b.velocity * Time.deltaTime;
    }


    bool CheckEnvCollision(Ball b)
    {
        float y = b.transform.position.y;
        float x = b.transform.position.x;

        foreach (Wall _child in collidables)
        {
            BoundingBox child = _child._BoundingBox;
            if (child.CompareTag("Wall"))
            {
                if (((child.center.x + child.half_x) + 2 * b.radius > b.transform.position.x) && (child.center.x - child.half_x < b.transform.position.x - b.radius && b.transform.position.x - b.radius < child.center.x + child.half_x) && (b.transform.position.y < child.center.y + child.half_y && b.transform.position.y > child.center.y - child.half_y))
                {
                    b.onEnvCollision(_child, "LEFT");
                    return true;
                }

                if (((child.center.x - child.half_x) - 2 * b.radius < b.transform.position.x) && (child.center.x - child.half_x < b.transform.position.x + b.radius && b.transform.position.x + b.radius < child.center.x + child.half_x) && (b.transform.position.y < child.center.y + child.half_y && b.transform.position.y > child.center.y - child.half_y))
                {
                    b.onEnvCollision(_child, "RIGHT");
                    return true;
                }

                if (((child.center.y - child.half_y) - 2 * b.radius < b.transform.position.y) && (child.center.y - child.half_y < b.transform.position.y + b.radius && b.transform.position.y + b.radius < child.center.y + child.half_y) && (b.transform.position.x < child.center.x + child.half_x && b.transform.position.x > child.center.x - child.half_x))
                {
                    b.onEnvCollision(_child, "DOWN");
                    return true;
                }

                if (((child.center.y + child.half_y) + 2 * b.radius > b.transform.position.y) && (child.center.y - child.half_y < b.transform.position.y - b.radius && b.transform.position.y - b.radius < child.center.y + child.half_y) && (b.transform.position.x < child.center.x + child.half_x && b.transform.position.x > child.center.x - child.half_x))
                {
                    b.onEnvCollision(_child, "UP");
                    return true;
                }
            }
        }
        return false;
    }

}
