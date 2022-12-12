using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallHandler : MonoBehaviour
{
  [SerializeField]
  List<BoundingBox> collidables = new List<BoundingBox>();

  [SerializeField]
  List<BallMovement> Balls = new List<BallMovement>();

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

    // Update is called once per frame
    void Update()
    {
      foreach(BallMovement b in Balls){
        CheckEnvCollision(b);
        checkBallCollision(b);
      }
    }

    void checkBallCollision(BallMovement b){
      if(temp.Count == 0 || temp.Count == 1){
        temp = new List<BallMovement>(Balls);
      }

      temp.Remove(b);

      foreach(BallMovement _b in temp.ToArray()){
        Vector3 distance = b.transform.position - _b.transform.position;
        Debug.Log("Distance vector is : " + distance);
        float dist = System.Math.Abs(distance.magnitude);

        if(dist < radius * 2f){
          temp.Remove(_b);
          Debug.Log("Collision :o");
          Debug.Log("DISTANCE IS: " + dist + "  COMPARE:  " + radius*2f);
          Vector3 temp_direction = b.direction;
          b.setDirection(_b.direction);
          _b.setDirection(temp_direction);

          // if(b.transform.position.x > _b.transform.position.x){
          //   b.transform.position = new Vector3(b.transform.position.x + System.Math.Abs(distance.x), b.transform.position.y, 0);
          //   _b.transform.position = new Vector3(_b.transform.position.x - System.Math.Abs(distance.x), _b.transform.position.y, 0);
          // }
          // else{
          //   b.transform.position = new Vector3(b.transform.position.x - System.Math.Abs(distance.x), b.transform.position.y, 0);
          //   _b.transform.position = new Vector3(_b.transform.position.x + System.Math.Abs(distance.x), _b.transform.position.y, 0);
          // }
          //
          // if(b.transform.position.x > _b.transform.position.x){
          //   b.transform.position = new Vector3(b.transform.position.x, b.transform.position.y + System.Math.Abs(distance.y), 0);
          //   _b.transform.position = new Vector3(_b.transform.position.x, _b.transform.position.y - System.Math.Abs(distance.y), 0);
          // }
          // else{
          //   b.transform.position = new Vector3(b.transform.position.x, b.transform.position.y - System.Math.Abs(distance.y), 0);
          //   _b.transform.position = new Vector3(_b.transform.position.x, _b.transform.position.y + System.Math.Abs(distance.y), 0);
          // }

          GameObject collision_point = GameObject.CreatePrimitive(PrimitiveType.Sphere);
          collision_point.transform.localScale = new Vector3(0.1f,0.1f,0.1f);
          collision_point.transform.position = distance;
          //b.transform.position += new Vector3(1,0,0);
        }
      }
    }

    void CheckEnvCollision(BallMovement b){
      float y = b.transform.position.y;
      float x = b.transform.position.x;

      foreach(BoundingBox child in collidables){
        if(child.CompareTag("Wall")){
          if(((child.center.x + child.half_x) + 2 * radius > b.transform.position.x) && (child.center.x - child.half_x < b.transform.position.x - radius && b.transform.position.x - radius < child.center.x + child.half_x) && (b.transform.position.y < child.center.y + child.half_y && b.transform.position.y > child.center.y - child.half_y)){
            Debug.Log("LEFT WALL");

            //PUSH BACK
            Vector3 temp = b.transform.position;
            b.transform.position = new Vector3((child.center.x + child.half_x) + 1 * radius, temp.y, 0);

            b.direction = Vector3.Reflect(b.direction, Vector3.left);
            break;
          }

          if(((child.center.x - child.half_x) - 2 * radius < b.transform.position.x) && (child.center.x - child.half_x  < b.transform.position.x + radius && b.transform.position.x + radius < child.center.x + child.half_x) && (b.transform.position.y < child.center.y + child.half_y && b.transform.position.y > child.center.y - child.half_y)){
            Debug.Log("RIGHT WALL");

            Vector3 temp = b.transform.position;
            b.transform.position = new Vector3((child.center.x - child.half_x) - 1 * radius, temp.y, 0);

            b.direction = Vector3.Reflect(b.direction, Vector3.right);
            break;
          }

          if(((child.center.y - child.half_y) - 2 * radius < b.transform.position.y) && (child.center.y - child.half_y  < b.transform.position.y + radius && b.transform.position.y + radius < child.center.y + child.half_y) && (b.transform.position.x < child.center.x + child.half_x && b.transform.position.x > child.center.x - child.half_x)){
            Debug.Log("UP WALL");

            Vector3 temp = b.transform.position;
            b.transform.position = new Vector3(temp.x, (child.center.y - child.half_y) - 1 * radius, 0);

            b.direction = Vector3.Reflect(b.direction, Vector3.down);
            break;
          }

          if(((child.center.y + child.half_y) + 2 * radius > b.transform.position.y) && (child.center.y - child.half_y  < b.transform.position.y - radius && b.transform.position.y - radius < child.center.y + child.half_y) && (b.transform.position.x < child.center.x + child.half_x && b.transform.position.x > child.center.x - child.half_x)){
            Debug.Log("DOWN WALL");

            Vector3 temp = b.transform.position;
            b.transform.position = new Vector3(temp.x, (child.center.y + child.half_y) + 1 * radius, 0);

            b.direction = Vector3.Reflect(b.direction, Vector3.up);
            break;
          }
        }
        else if(child.CompareTag("Ground")){

        }
      }
    }

}
