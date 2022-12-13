using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Random;

public class BallMovement : MonoBehaviour
{
  [SerializeField]
  public Vector3 direction;

  float x;
  float y;

  public float radius = 2.5f;

  public float velocity = 0.5f;

  //float r = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
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

    float NextFloat(float min, float max){
      System.Random random = new System.Random();
      double val = (random.NextDouble() * (max - min) + min);
      return (float)val
      ;
    }

    public void setDirection(Vector3 _direction, float vel){
      direction = _direction;
      velocity = vel;
    }
  }
