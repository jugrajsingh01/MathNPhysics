using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : PObject
{
    public GameObject anchorpoint;
    public LineRenderer myLineRenderer;
    public int points;
    public float y_range = 1;
    public float k = 1f;
    public Vector3 restLength;
    public float _velocity = 0f;
    public bool init = true;
    public float springPos = 1f;

    [SerializeField]
    public Wall box;

    void Start()
    {
        if(anchorpoint != null)
        {
            transform.position = anchorpoint.transform.position;
        }
        _velocity = velocity;
        //myLineRenderer = GetComponent<LineRenderer>();
        //restLength = new Vector3(0, myLineRenderer.transform.localPosition.x + myLineRenderer.positionCount, 0);
        restLength = box.transform.localPosition;
        points = (int)restLength.x;
        box.direction = new Vector3(1 * _velocity,0,0);
    }

    void Draw(float time = 1f, float x = 1f)
    {
        if (anchorpoint != null)
        {
            transform.position = anchorpoint.transform.position;
        }
        myLineRenderer.positionCount = points;
        for (int currentPoint = 0; currentPoint < points; currentPoint++)
        {
            if (currentPoint % 2 == 0)
            {
                y_range = -1;
            }
            else
            {
                y_range = 1;
            }

            if (currentPoint == 0 || currentPoint == 1)
            {
                myLineRenderer.SetPosition(currentPoint, new Vector3((myLineRenderer.transform.localPosition.x + currentPoint) / x, 0, 0));
            }
            else if (currentPoint > points - 3)
            {
                if (currentPoint == points - 1 && init)
                {
                    box.transform.localPosition = new Vector3((myLineRenderer.transform.localPosition.x + currentPoint + (box.transform.localScale.x / 2f)) / x, myLineRenderer.transform.localPosition.y, 0);
                    //restLength = new Vector3(box.transform.localPosition.x, 0, 0); it can now be custom set
                    //waarom comment ik in het engels de docent kan gewoon nederlands 

                }
                myLineRenderer.SetPosition(currentPoint, new Vector3((myLineRenderer.transform.localPosition.x + currentPoint) / x, 0, 0));
            }
            else
            {
                myLineRenderer.SetPosition(currentPoint, new Vector3((myLineRenderer.transform.localPosition.x + currentPoint) / x, y_range, 0));
            }
        }
    }

    void FixedUpdate()
    {
        if (anchorpoint != null)
        {
            Wall p = anchorpoint.GetComponent<Wall>();
            velocity += p.pot_energy / p.a * Time.deltaTime;
            Debug.Log(p.pot_energy);
        }
        Draw(1f, springPos);
        init = false;

        float delta = box.transform.localPosition.x - restLength.x;

        float force = -k * delta;
        velocity += force;

        float x_pos = velocity * Time.deltaTime;

        box.transform.localPosition += new Vector3(x_pos, 0, 0);

        if (!(box.transform.localPosition.x - box._BoundingBox.half_x > myLineRenderer.transform.localPosition.x))
        {
            velocity = velocity * -1f * 0.5f;
        }

        //springPos = 10*x^2−35*x+35
        //springPos = 10 * x ^ 2−35 * x + 35; guess i didnt need this :|
        //this took way too long to figure out 
        springPos = restLength.x / box.transform.localPosition.x;
        box.velocity = velocity;
        box.direction = new Vector3(1, 0, 0);

    }
}
