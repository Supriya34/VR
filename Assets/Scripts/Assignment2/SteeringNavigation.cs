using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SteeringNavigation : MonoBehaviour
{
    public InputActionProperty flyAction;

    public Transform directionIndicator;

    public float speedFactor = 1.0f;

    // bExercise 2.4
    // What are advantages and disadvantages of each technique? Which one do you personally prefer?
    // Advantages and disadvantages of gaze directed steering
    // + precise control for distant targets because of implicit aiming at the target
    // - relatively slow
    // - Can be confusing often times and has a feeling of discomfort.
    // - Needing to focus on the direction you would want to go can be annoying.
    // Advantages and disadvantages of controller directed steering
    // + you can go backwards more easily
    // + agile controls
    // + people already know it from video games
    // - can cause motion sickness with abprubt changes
    // Our opinion:
    // We like the right hand controller directed steering better as we are able to gaze around the world and enjoy the environment.




    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if(flyAction.action.IsPressed())
            transform.position += directionIndicator.forward * flyAction.action.ReadValue<float>() * Time.deltaTime * speedFactor;
    }
}
