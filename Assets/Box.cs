using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : PObject
{
  public float kg;
  public float water_density;
  public float volume;
  float buoyancy;
  float mass;
  float areaUnderWater = 0.0f;
  float b_force = 5f;
  float depthPower = 1f;


  [SerializeField]
  static List<Wall> collidables = new List<Wall>();

    // Start is called_ before the first frame update
    void Start()
    {
      collidables.AddRange(GameObject.FindObjectsOfType<Wall>());
      mass = kg * volume;
      y = 0f;
      x = -1f;
      direction = new Vector3(0, -1, 0);
      this._BoundingBox = new BoundingBox(this.transform);
    }

    // Update is called once per frame
    void Update()
    {
      if(this._BoundingBox != null){
        this._BoundingBox = new BoundingBox(this.transform);
      }

      if(!CheckEnvCollision(this)){
        //velocity = buoyancy - (mass * a * Time.deltaTime);
        //calculateVelocity(transform.position.y - dy, transform.position.y);
        // float y_pos = transform.position.y;
        // y_pos = (velocity * mass * a * Time.deltaTime) / 10;
        //
        // transform.position += new Vector3(0, y_pos, 0);

        //in water
        if(transform.position.y < (-8.5f + 6.5f)){
          float buoyantForceMass = b_force * kg;
          float underWaterBuoyantForce = System.Math.Abs(Mathf.Clamp01(((-8.5f + 6.5f)) - transform.position.y) * depthPower);
          float buoyency = buoyantForceMass + (buoyantForceMass * underWaterBuoyantForce);

          velocity = (buoyency * Time.deltaTime) / 10;

          transform.position -= new Vector3(0,velocity,0);
          //Debug.Log("Force: " + forceComponent.force);
					//Debug.Log(underWaterVolume);
        }
        else{
          velocity *= -1f;
          transform.position -= new Vector3(0,velocity,0);
        }
        // else if(transform.position.y > (-8.5f + 6.5f)){
        //   float delta = transform.position.x - 8.5f + 6.5f;
        //
        //   float force = -depthPower * delta;
        //   //float pot_force = (float)(-k * 0.5f * System.Math.Pow(delta, 2));
        //   velocity += force;
        //   float y_pos = velocity * Time.deltaTime;
        //   transform.position -= new Vector3(0, y_pos, 0);
        // }
      }
    }




    bool CheckEnvCollision(Box b)
    {
        foreach (Wall _child in collidables)
        {
            BoundingBox child = _child._BoundingBox;
            if (child.CompareTag("Wall"))
            {
                if (((child.center.x + child.half_x) + 2 * b._BoundingBox.half_x > b.transform.position.x) && (child.center.x - child.half_x < b.transform.position.x - b._BoundingBox.half_x && b.transform.position.x - b._BoundingBox.half_x < child.center.x + child.half_x) && (b.transform.position.y < child.center.y + child.half_y && b.transform.position.y > child.center.y - child.half_y))
                {
                    b.onEnvCollision(_child, "LEFT");
                    return true;
                }

                if (((child.center.x - child.half_x) - 2 * b._BoundingBox.half_x < b.transform.position.x) && (child.center.x - child.half_x < b.transform.position.x + b._BoundingBox.half_x && b.transform.position.x + b._BoundingBox.half_x < child.center.x + child.half_x) && (b.transform.position.y < child.center.y + child.half_y && b.transform.position.y > child.center.y - child.half_y))
                {
                    b.onEnvCollision(_child, "RIGHT");
                    return true;
                }

                if (((child.center.y - child.half_y) - 2 * b._BoundingBox.half_y < b.transform.position.y) && (child.center.y - child.half_y < b.transform.position.y + b._BoundingBox.half_y && b.transform.position.y + b._BoundingBox.half_y < child.center.y + child.half_y) && (b.transform.position.x < child.center.x + child.half_x && b.transform.position.x > child.center.x - child.half_x))
                {
                  velocity = 0;

                    b.onEnvCollision(_child, "DOWN");
                    return true;
                }

                if (((child.center.y + child.half_y) + 2 * b._BoundingBox.half_y > b.transform.position.y) && (child.center.y - child.half_y < b.transform.position.y - b._BoundingBox.half_y && b.transform.position.y - b._BoundingBox.half_y < child.center.y + child.half_y) && (b.transform.position.x < child.center.x + child.half_x && b.transform.position.x > child.center.x - child.half_x))
                {
                    b.onEnvCollision(_child, "UP");
                    return true;
                }
            }
        }
        return false;
    }

    public override void onEnvCollision(PObject _hit, string side)
    {
        BoundingBox hit = _hit._BoundingBox;
        if (string.Equals(side, "LEFT"))
        {
            Vector3 temp = transform.position;
            transform.position = new Vector3((hit.center.x + hit.half_x) + 1 * this._BoundingBox.half_y, temp.y, 0);
        }
        if (string.Equals(side, "RIGHT"))
        {
            Vector3 temp = transform.position;
            transform.position = new Vector3((hit.center.x - hit.half_x) - 1 * this._BoundingBox.half_y, temp.y, 0);
        }
        if (string.Equals(side, "UP"))
        {
            Vector3 temp = transform.position;
            transform.position = new Vector3(temp.x, (hit.center.y + hit.half_y) + 1 * this._BoundingBox.half_y, 0);
        }
        if (string.Equals(side, "DOWN"))
        {
            Vector3 temp = transform.position;
            transform.position = new Vector3(temp.x, (hit.center.y - hit.half_y) - 1 * this._BoundingBox.half_y, 0);
            velocity = 0;
            enabled = false;
        }
    }

}
