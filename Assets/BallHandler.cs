using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallHandler : MonoBehaviour
{
  static int i = 0;
  [SerializeField]
  List<BoundingBox> collidables = new List<BoundingBox>();

  [SerializeField]
  List<BallMovement> Balls = new List<BallMovement>();

  [SerializeField]
  List<PerfectBounce> perfectBouncingBalls = new List<PerfectBounce>();

  [SerializeField]
  List<BallMovement> temp;

  float radius = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
      Balls.AddRange(GameObject.FindObjectsOfType<BallMovement>());
      temp = new List<BallMovement>(Balls);
      collidables.AddRange(GameObject.FindObjectsOfType<BoundingBox>());
    }

    void FixedUpdate()
    {
      foreach(PerfectBounce pb in perfectBouncingBalls){
        Bounce(pb);
      }
      // foreach(BallMovement b in Balls){
      //   CheckEnvCollision(b);
      //   checkBallCollision(b);
      // }
    }

    void Bounce(PerfectBounce b){
        Vector3 movement = new Vector3(0, 0, 0);
    }

    void checkBallCollision(BallMovement b){
      if(temp.Count == 0 || temp.Count == 1){
        temp = new List<BallMovement>(Balls);
      }
      temp.Remove(b);

      foreach(BallMovement _b in temp.ToArray()){
        Vector3 distance = b.transform.position - _b.transform.position;
        float dist = System.Math.Abs(distance.magnitude);

        if(dist < radius * 2f){
          i++;
          Debug.Log(i);
          temp.Remove(_b);

          Vector3 temp_direction = b.direction;

          b.transform.position = (Vector3.Normalize(distance) * 1f) + _b.transform.position;

          distance = new Vector3(distance.y * -1f, distance.x, 0);

          Debug.DrawRay(b.transform.position, distance, Color.green, 5f);
          //Debug.Break();


          float temp_vel = b.velocity;
          b.setDirection(Vector3.Reflect(_b.direction, distance), _b.velocity);

          _b.setDirection(Vector3.Reflect(temp_direction, distance), temp_vel);
        }
      }
    }

    void CheckEnvCollision(BallMovement b){
      float y = b.transform.position.y;
      float x = b.transform.position.x;

      foreach(BoundingBox child in collidables){
        if(child.CompareTag("Wall")){
          if(((child.center.x + child.half_x) + 2 * radius > b.transform.position.x) && (child.center.x - child.half_x < b.transform.position.x - radius && b.transform.position.x - radius < child.center.x + child.half_x) && (b.transform.position.y < child.center.y + child.half_y && b.transform.position.y > child.center.y - child.half_y)){


            //PUSH BACK
            Vector3 temp = b.transform.position;
            b.transform.position = new Vector3((child.center.x + child.half_x) + 1 * radius, temp.y, 0);

            b.direction = Vector3.Reflect(b.direction, Vector3.left);
            break;
          }

          if(((child.center.x - child.half_x) - 2 * radius < b.transform.position.x) && (child.center.x - child.half_x  < b.transform.position.x + radius && b.transform.position.x + radius < child.center.x + child.half_x) && (b.transform.position.y < child.center.y + child.half_y && b.transform.position.y > child.center.y - child.half_y)){


            Vector3 temp = b.transform.position;
            b.transform.position = new Vector3((child.center.x - child.half_x) - 1 * radius, temp.y, 0);

            b.direction = Vector3.Reflect(b.direction, Vector3.right);
            break;
          }

          if(((child.center.y - child.half_y) - 2 * radius < b.transform.position.y) && (child.center.y - child.half_y  < b.transform.position.y + radius && b.transform.position.y + radius < child.center.y + child.half_y) && (b.transform.position.x < child.center.x + child.half_x && b.transform.position.x > child.center.x - child.half_x)){


            Vector3 temp = b.transform.position;
            b.transform.position = new Vector3(temp.x, (child.center.y - child.half_y) - 1 * radius, 0);

            b.direction = Vector3.Reflect(b.direction, Vector3.down);
            break;
          }

          if(((child.center.y + child.half_y) + 2 * radius > b.transform.position.y) && (child.center.y - child.half_y  < b.transform.position.y - radius && b.transform.position.y - radius < child.center.y + child.half_y) && (b.transform.position.x < child.center.x + child.half_x && b.transform.position.x > child.center.x - child.half_x)){

            Vector3 temp = b.transform.position;
            b.transform.position = new Vector3(temp.x, (child.center.y + child.half_y) + 1 * radius, 0);

            b.direction = Vector3.Reflect(b.direction, Vector3.up);
            break;
          }
        }
      }
    }

}
