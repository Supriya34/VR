using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MoveAroundTarget : MonoBehaviour
{
    public Transform target;

    //Vector3 lastPosition = new Vector3();
    //bool init = false;

    public float degreesPerSecond = 20;

    public float radius = 10;

    private Vector3 targetPositionXZ
    {
        get
        {
            return new Vector3(target.position.x, 0, target.position.z);
        }
    }

    private Vector3 positionXZ
    {
        get
        {
            return new Vector3(transform.position.x, 0, transform.position.z);
        }
    }

    private Vector3 directionToTarget
    {
        get
        {
            return (targetPositionXZ - positionXZ);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //transform.Rotate(0, degreesPerSecond * Time.deltaTime, 0);
        //transform.Translate(Vector3.forward * radius);
        //return;
        var newPosition = CalculatePositionUpdate();
        var newRotation = CalculateRotationUpdate(newPosition);
        transform.position = newPosition;
        transform.rotation = newRotation;
    }

    Vector3 CalculatePositionUpdate()
    {
        //Exercise 1.5
        //first try
        //might be correct
        //small wtf: If i change the degreesPerSecond value in the inspector the values accumulate instead of an increase in speed
        //EDIT: does not happen anymore lol
        var angle_value = degreesPerSecond * Time.time;
        var radian_value = (angle_value/180.0f) * Math.PI;
        radian_value = radian_value % (2*Math.PI); //VR Exercise guy helped me with this
        var x = Mathf.Sin((float)radian_value);
        var z = Mathf.Cos((float)radian_value);
        //constructing vector relative to the target and applying the radius
        return new Vector3(target.position.x + x*radius, 0, target.position.z + z*radius);
        
        //second try
        //transform.position = target.position;
        //transform.Rotate( 0, degreesPerSecond * Time.deltaTime, 0);
        //transform.Translate( 0, 0, radius);
        //return transform.position;
    }

    Quaternion CalculateRotationUpdate(Vector3 newPosition)
    {
        //Exercise 1.5 TODO
        //if(init == false){
        //    lastPosition = newPosition;
        //   init = true;
        //    return transform.rotation;
        //}

        //first try
        //var movementDir = newPosition - lastPosition;
        //transform.forward = movementDir;

        //second try
        //transform.Rotate(0,90,0);

        //lastPosition = newPosition;
   
        //return transform.rotation;

        //third try
        var movementDir = newPosition - transform.position;
        return Quaternion.LookRotation(movementDir);
    }
}
