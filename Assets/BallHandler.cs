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
            CheckEnvCollision(b);
            checkBallCollision(b);
        }
    }

    float collisionReboundSingleAxis (Ball b, Ball _b) {
      float collisionForce = ((1 - 1) * b.direction.x + (2 * 1 * _b.direction.x)) / (1 + 1);
      return collisionForce;
    }

    float[,] invertMatrix2x2 (float[,] matrix) {
      return new float [,] {{matrix[0, 0], matrix[1, 0]}, {matrix[0, 1], matrix[1, 1]}};
    }

    float[,] createRotationMatrixFromAngle (float theta) {
      float cos = (float)System.Math.Cos(theta);
      float sin = (float)System.Math.Sin(theta);

      return new float [,] {{cos, sin}, {-sin, cos}};
    }

    Vector3 matrix2x2MultiplyVector2 (float[,] matrix, Vector3 direction) {
      Vector3 MatrixRow1Vec = new Vector3(matrix[0, 0], matrix[0, 1], 0);
      Vector3 MatrixRow2Vec = new Vector3(matrix[1, 0], matrix[1, 1], 0);

      return new Vector3(Vector3.Dot(direction, MatrixRow1Vec), Vector3.Dot(direction, MatrixRow2Vec), 0);
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

                distance = new Vector3(distance.y * -1f, distance.x, 0);
                /*
                                Debug.DrawRay(b.transform.position, distance, Color.green, 5f);
                                //Debug.Break();

                                b.setDirection(Vector3.Reflect(_b.direction, distance), _b.velocity);

                                _b.setDirection(Vector3.Reflect(temp_direction, distance), temp_vel);*/


                float col_angle = (float)System.Math.Atan2(System.Math.Abs(distance.y), System.Math.Abs(distance.x));

                Vector3 reboundVelocity1 = new Vector3(0,0,0);
                Vector3 reboundVelocity2 = new Vector3(0,0,0);

                if(col_angle > 0.01f){
                  Debug.Log(col_angle + " angel col");
                  float[,] mat = createRotationMatrixFromAngle(col_angle);
                  float[,] inv_mat = invertMatrix2x2(mat);

                  Vector3 vel1 = matrix2x2MultiplyVector2(mat, b.direction);
                  Vector3 vel2 = matrix2x2MultiplyVector2(mat, _b.direction);

                  float vxTotal = vel1.x - vel2.x;

                  vel1.x = collisionReboundSingleAxis(b, _b);
                  vel2.x = vxTotal + vel1.x;

                  reboundVelocity1 = matrix2x2MultiplyVector2(inv_mat, vel2);
                  reboundVelocity2 = matrix2x2MultiplyVector2(inv_mat, vel1);

                  if(Vector3.Dot(vel1, vel2) > 0.0) {
                      vel1.x *= -1;
                    }
                    // position adjustment
                  Vector3 particle1Adjustment = matrix2x2MultiplyVector2(inv_mat, new Vector3(0, 0, 0));
                  b.transform.position += particle1Adjustment;
                  _b.transform.position = b.transform.position + ogDistance;

                }

                b.setDirection(reboundVelocity1, _b.velocity);

                _b.setDirection(reboundVelocity2, temp_vel);

                // b.setDirection(_b.direction, _b.velocity);
                //
                // _b.setDirection(temp_direction, temp_vel);



            }
        }
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
