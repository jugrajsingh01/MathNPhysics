using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundingBox : MonoBehaviour
{
  public float half_y { get; set; }
  public float half_x { get; set; }
  public Vector3 x { get; set; }
  public Vector3 center { get; set; }
    // Start is called before the first frame update
    void Start()
    {
      center = transform.position;

      half_y = transform.localScale.y / 2f;
      half_x = transform.localScale.x / 2f;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
