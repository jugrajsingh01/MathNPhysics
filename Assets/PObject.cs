using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PObject : MonoBehaviour
{
    public BoundingBox _BoundingBox;
    public float x;
    public float y;

    [SerializeField]
    public Vector3 direction;

    public float a = 9.8f;

    [SerializeField]
    public float velocity;

    [SerializeField]
    public float pot_energy = 0f;

    // Start is called before the first frame update
    void Start()
    {
      //this._BoundingBox = new BoundingBox(this.transform);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public virtual void onEnvCollision(PObject _hit, string side)
    {

    }
}
