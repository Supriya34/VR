using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AvatarBodySelection : MonoBehaviour
{
    public List<GameObject> bodyPrefabs = new List<GameObject>();

    public InputActionProperty switchBodyAction;

    public LayerMask mirrorLayer;

    public Transform headTransform;

    private GameObject bodyInstance;

    public int currentBodyIndex = 0;

    public bool disableInputHandling = false;

    public bool rayHit = false;

    // Start is called before the first frame update
    void Start()
    {
        AttachBodyPrefab(0);
    }

    // Update is called once per frame
    void Update()
    {
        int nextBodyIndex = CalcNextBodyIndex();
        rayHit = IsLookingAtMirror(); //calling this here to see rayHit in the UI
        if (nextBodyIndex != -1 && rayHit)
            AttachBodyPrefab(nextBodyIndex);
    }

    int CalcNextBodyIndex() // -1 means invalid aka "do nothing"
    {
        //retrieve horizontal input value from the joystick (of the right controller)
        Vector2 stickPosition = switchBodyAction.action.ReadValue<Vector2>();
        float horizontalStickPosition = stickPosition.x; // -1.0 <-> 0.0 <-> 1.0

        //reset and wait for input
        if(horizontalStickPosition == 0)
        {
            disableInputHandling = false;
            return -1;
        }

        //prevent continuous dress changes while the joystick keeps being held to either side
        if (disableInputHandling == true) {
            return -1;
        }

        //user is holding stick to the right
        if (horizontalStickPosition > 0 && currentBodyIndex < bodyPrefabs.Count - 1)
        {
            currentBodyIndex = currentBodyIndex + 1;
        }
        
        //user is holding stick to the left
        if (horizontalStickPosition < 0 && currentBodyIndex >= 1)
        {
            currentBodyIndex = currentBodyIndex - 1;
        }

        //prevent continuous dress changes while the joystick keeps being held to either side
        disableInputHandling = true;
        return currentBodyIndex;
    }

    bool IsLookingAtMirror()
    {
        rayHit = Physics.Raycast(headTransform.position, headTransform.forward, 20, mirrorLayer.value);
        return rayHit;
    }

    private void AttachBodyPrefab(int index)
    {
        if(index < 0 || index >= bodyPrefabs.Count)
        {
            Debug.LogWarning("Body selection offers no prefab at given index '" + index + "'");
            return;
        }

        if(bodyInstance != null)
        {
            Destroy(bodyInstance);
            bodyInstance = null;
        }

        currentBodyIndex = index;
        bodyInstance = Instantiate(bodyPrefabs[currentBodyIndex], transform);
    }
}
