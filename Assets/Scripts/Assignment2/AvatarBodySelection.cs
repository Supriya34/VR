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

    private int currentBodyIndex = 0;

    private bool disableInputHandling = false;

    // Start is called before the first frame update
    void Start()
    {
        AttachBodyPrefab(0);
    }

    // Update is called once per frame
    void Update()
    {
        int nextBodyIndex = CalcNextBodyIndex();
        if(nextBodyIndex != -1 && IsLookingAtMirror())
            AttachBodyPrefab(nextBodyIndex);
    }

    int CalcNextBodyIndex() // -1 means invalid aka "do nothing"
    {

        Vector2 switchInput = switchBodyAction.action.ReadValue<Vector2>();

        int bodySelectionIndex = Mathf.RoundToInt(switchInput.x);
        Debug.Log(bodySelectionIndex);
        //currentBodyIndex = currentBodyIndex + bodySelectionIndex;
        Debug.Log(currentBodyIndex + bodySelectionIndex);


        return currentBodyIndex + bodySelectionIndex;
    }

    bool IsLookingAtMirror()
    {

        bool rayHit = Physics.Raycast(headTransform.position, headTransform.forward, 20, mirrorLayer);
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
