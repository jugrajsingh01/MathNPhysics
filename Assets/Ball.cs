using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Random;

public class Ball : MonoBehaviour
{
    [SerializeField]
    public Vector3 direction;

    float x;
    float y;

    public float a = 9.8f;

    [SerializeField]
    public float radius;

    [SerializeField]
    public float velocity;

    //float r = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        radius = transform.localScale.x / 2f;

        x = NextFloat(-180f, 180f);
        y = NextFloat(-180f, 180f);
        velocity = NextFloat(0.1f, 1f);

        direction = new Vector3(x, y, 0);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position += direction.normalized * velocity * Time.deltaTime;
    }

    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 0.10f);

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, direction.normalized);


    }

    float NextFloat(float min, float max)
    {
        System.Random random = new System.Random();
        double val = (random.NextDouble() * (max - min) + min);
        return (float)val
        ;
    }

    public void setDirection(Vector3 _direction, float vel)
    {
        direction = _direction;
        velocity = vel;
    }

    public virtual void onEnvCollision(BoundingBox hit, string side)
    {
        if (string.Equals(side, "LEFT"))
        {
            Vector3 temp = transform.position;
            transform.position = new Vector3((hit.center.x + hit.half_x) + 1 * radius, temp.y, 0);

            direction = Vector3.Reflect(direction, Vector3.left);
        }
        if (string.Equals(side, "RIGHT"))
        {
            Vector3 temp = transform.position;
            transform.position = new Vector3((hit.center.x - hit.half_x) - 1 * radius, temp.y, 0);

            direction = Vector3.Reflect(direction, Vector3.right);
        }
        if (string.Equals(side, "UP"))
        {
            Vector3 temp = transform.position;
            transform.position = new Vector3(temp.x, (hit.center.y + hit.half_y) + 1 * radius, 0);

            direction = Vector3.Reflect(direction, Vector3.up);
        }
        if (string.Equals(side, "DOWN"))
        {
            Vector3 temp = transform.position;
            transform.position = new Vector3(temp.x, (hit.center.y - hit.half_y) - 1 * radius, 0);

            direction = Vector3.Reflect(direction, Vector3.down);
        }
    }

    public virtual void onBallCollision(Ball b)
    {

    }


    /*
        public void checkBallCollision(Ball b, List<Ball> Balls, List<Ball> temp)
        {
            if (temp.Count == 0 || temp.Count == 1)
            {
                temp = new List<Ball>(Balls);
            }
            temp.Remove(b);

            foreach (Ball _b in temp.ToArray())
            {
                Vector3 distance = b.transform.position - _b.transform.position;
                float dist = System.Math.Abs(distance.magnitude);

                if (dist < radius * 2f)
                {
                    i++;
                    Debug.Log(i);
                    temp.Remove(_b);

                    Vector3 temp_direction = b.direction;
                    float temp_vel = b.velocity;

                    b.transform.position = (Vector3.Normalize(distance) * 1f) + _b.transform.position;

                    *//*distance = new Vector3(distance.y * -1f, distance.x, 0);

                    Debug.DrawRay(b.transform.position, distance, Color.green, 5f);
                    //Debug.Break();


                    float temp_vel = b.velocity;
                    b.setDirection(Vector3.Reflect(_b.direction, distance), _b.velocity);

                    _b.setDirection(Vector3.Reflect(temp_direction, distance), temp_vel);*//*

                    b.setDirection(_b.direction, _b.velocity);

                    _b.setDirection(temp_direction, temp_vel);
                }
            }
        }

        public void CheckEnvCollision(Ball b, List<BoundingBox> collidables)
        {
            float y = b.transform.position.y;
            float x = b.transform.position.x;

            foreach (BoundingBox child in collidables)
            {
                if (child.CompareTag("Wall"))
                {
                    if (((child.center.x + child.half_x) + 2 * radius > b.transform.position.x) && (child.center.x - child.half_x < b.transform.position.x - radius && b.transform.position.x - radius < child.center.x + child.half_x) && (b.transform.position.y < child.center.y + child.half_y && b.transform.position.y > child.center.y - child.half_y))
                    {


                        //PUSH BACK
                        Vector3 temp = b.transform.position;
                        b.transform.position = new Vector3((child.center.x + child.half_x) + 1 * radius, temp.y, 0);

                        b.direction = Vector3.Reflect(b.direction, Vector3.left);
                        break;
                    }

                    if (((child.center.x - child.half_x) - 2 * radius < b.transform.position.x) && (child.center.x - child.half_x < b.transform.position.x + radius && b.transform.position.x + radius < child.center.x + child.half_x) && (b.transform.position.y < child.center.y + child.half_y && b.transform.position.y > child.center.y - child.half_y))
                    {


                        Vector3 temp = b.transform.position;
                        b.transform.position = new Vector3((child.center.x - child.half_x) - 1 * radius, temp.y, 0);

                        b.direction = Vector3.Reflect(b.direction, Vector3.right);
                        break;
                    }

                    if (((child.center.y - child.half_y) - 2 * radius < b.transform.position.y) && (child.center.y - child.half_y < b.transform.position.y + radius && b.transform.position.y + radius < child.center.y + child.half_y) && (b.transform.position.x < child.center.x + child.half_x && b.transform.position.x > child.center.x - child.half_x))
                    {


                        Vector3 temp = b.transform.position;
                        b.transform.position = new Vector3(temp.x, (child.center.y - child.half_y) - 1 * radius, 0);

                        b.direction = Vector3.Reflect(b.direction, Vector3.down);
                        break;
                    }

                    if (((child.center.y + child.half_y) + 2 * radius > b.transform.position.y) && (child.center.y - child.half_y < b.transform.position.y - radius && b.transform.position.y - radius < child.center.y + child.half_y) && (b.transform.position.x < child.center.x + child.half_x && b.transform.position.x > child.center.x - child.half_x))
                    {

                        Vector3 temp = b.transform.position;
                        b.transform.position = new Vector3(temp.x, (child.center.y + child.half_y) + 1 * radius, 0);

                        b.direction = Vector3.Reflect(b.direction, Vector3.up);
                        break;
                    }
                }
            }
        }*/
}