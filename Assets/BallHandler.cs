using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallHandler : MonoBehaviour
{
  [SerializeField]
  List<BoundingBox> collidables = new List<BoundingBox>();

  [SerializeField]
  List<BallMovement> Balls = new List<BallMovement>();
    // Start is called before the first frame update
    void Start()
    {
      Balls.AddRange(GameObject.FindObjectsOfType<BallMovement>());
      collidables.AddRange(GameObject.FindObjectsOfType<BoundingBox>());
    }

    // Update is called once per frame
    void Update()
    {
      foreach(BallMovement b in Balls){
        CheckEnvCollision(b);
      }
    }

    void CheckEnvCollision(BallMovement b){
      float y = b.transform.position.y;
      float x = b.transform.position.x;

      float radius = 0.5f;

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
