using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : MonoBehaviour
{
  public LineRenderer myLineRenderer;
    public int points;
    public float y_range = 1;
    public float k = 0.01f;
    public Vector3 restLength;
    public float velocity = 0f;
    public bool init = true;
    public float springPos = 1f;

    [SerializeField]
    public Box box;
    void Start()
    {
        myLineRenderer = GetComponent<LineRenderer>();
        //restLength = new Vector3(0, myLineRenderer.transform.localPosition.x + myLineRenderer.positionCount, 0);
        restLength = new Vector3(0, 0, 0);

    }

    void Draw(float time = 1f, float x = 1f)
    {
        myLineRenderer.positionCount = points;
        for(int currentPoint = 0; currentPoint<points;currentPoint++)
        {
          if(currentPoint%2 == 0){
            y_range = -1;
          }
          else{
            y_range = 1;
          }

          if(currentPoint == 0 || currentPoint == 1){
            myLineRenderer.SetPosition(currentPoint, new Vector3((this.transform.localPosition.x + currentPoint) / x, 0, 0));
          }
          else if(currentPoint > points - 3){
            if(currentPoint == points - 1 && init){
              box.transform.localPosition = new Vector3((this.transform.localPosition.x + currentPoint + (box.transform.localScale.x / 2f)) / x, this.transform.localPosition.y, 0);
              restLength = new Vector3(box.transform.localPosition.x, 0, 0);
            }
            myLineRenderer.SetPosition(currentPoint, new Vector3((this.transform.localPosition.x + currentPoint) / x, 0, 0));
          }
          else{
            myLineRenderer.SetPosition(currentPoint, new Vector3((this.transform.localPosition.x + currentPoint) / x, y_range, 0));
          }
        }
    }

    void FixedUpdate()
    {
        Draw(1f, 1f);
        init = false;

        float delta = box.transform.localPosition.x - restLength.x;

        float force =  -k * delta;
        velocity += force;

        float x_pos = box.transform.localPosition.x;

        x_pos = velocity * Time.deltaTime;

        box.transform.localPosition -= new Vector3(x_pos, 0, 0);

        //springPos =
        velocity *= 0.99f;

    }
}
