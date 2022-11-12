using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentSwitcher : MonoBehaviour
{
    public List<Transform> parents = new List<Transform>();

    public KeyCode nextParentKey = KeyCode.RightArrow;

    private int currentParent = 0;

    // Start is called before the first frame update
    void Start()
    {
        print("Start");
        //SetParent(0);
    }

    // Update is called once per frame
    void Update()
    {
        var input_boolean = Input.GetKeyDown(nextParentKey);
        if(input_boolean){
            print("Switching camera");
            SetParent((currentParent + 1) % parents.Count);
        }     
    }

    void SetParent(int idx)
    {   
        /*
        TODO: Exercise 1.4 -> 1.)
        Question:
        what is the effect of worldPositionStays?
        Answer:
        As the documentation states, the boolean argument worldPositionStays
        can lead to a modification of the childs transform.
        If worldPositionStays is true a recalculation is performed so that
        the child keeps its current position, scale and rotation (transform)
        regardless of its new parent.
        If worldPositionStays is false no recalculation is performed and
        the childs transform is oriented relative to its new parent.

        Link to documentation: https://docs.unity3d.com/ScriptReference/Transform.SetParent.html
        */
        currentParent = idx;
        transform.SetParent(parents[currentParent], false);    
    }
}