using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Random;

public class BallMovement : MonoBehaviour
{
  [SerializeField]
  public Vector3 direction;
  float x = 0;
  float y = 0;

  float r = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
      x = NextFloat(-180f, 180f);
      y = NextFloat(-180f, 180f);

      direction = new Vector3(x, y, 0);
    }

    // Update is called once per frame
    void Update()
    {
      transform.position += direction.normalized * 10 * Time.deltaTime;
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

    public void setDirection(Vector3 _direction){
      Debug.Log("direcction from : " + direction + " setting to : " + _direction);

      direction = _direction;
    }


  }
