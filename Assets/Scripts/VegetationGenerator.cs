using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class VegetationGenerator : MonoBehaviour
{
    public List<GameObject> vegetationPrefabs = new List<GameObject>();

    private List<GameObject> instances = new List<GameObject>();

    public List<Collider> restrictedBounds = new List<Collider>();

    public int numObjects = 1;

    public Vector3 vegetationBoundsMin = new Vector3(-30, 0, -30);

    public Vector3 vegetationBoundsMax = new Vector3(30, 0, 30);

    public bool reset = false;

    private System.Random getRandom = new System.Random();
     
    public List<prefabsStruct> customPrefabSelection = new List<prefabsStruct>(); //list of prefab structs

    [Serializable]
    public struct prefabsStruct
    {
        //defining the properties we have
        public GameObject prefab;
        public int count;
    }

    //[SerializeField]
    //public prefabsStruct prefabstest; //for the field to appear in the console


    // Start is called before the first frame update
    void Start()
    { 
       GenerateVegetation();
    }

    // Update is called once per frame
    void Update()
    {
        // TODO: Exercise 1.2 -> 3.)
        // check & handle "reset" to regenerate vegetation instances
        
        if (reset==true)
        {
            reset = false;
            ClearVegetation();
            GenerateVegetation();    
        }

    } 

    void ClearVegetation()
    {
        //first try   does not remove all objects but half of it
        /**
        print(instances.Count);
        for(int i = 0; i < instances.Count; i++)
        {
            print(i);
            GameObject destroytest = instances[0];
            //instances.RemoveAt(i);
            instances.Remove(destroytest);
            Destroy(destroytest);
        }
        **/

        //second try
        foreach (GameObject element in instances)
        {
            Destroy(element);
        }
        /**
        little hack by manu to resolve access to deleted gameobjects
        MissingReferenceException: The object of type 'GameObject' has been destroyed but you are still trying to access it.
        Your script should either check if it is null or you should not destroy the object.
        UnityEngine.GameObject.GetComponent[T]()(at / Users / bokken / buildslave / unity / build / Runtime / Export / Scripting / GameObject.bindings.cs:28)
        VegetationGenerator +< ResolveCollisions > d__14.MoveNext()(at Assets / Scripts / VegetationGenerator.cs:174)
        UnityEngine.SetupCoroutine.InvokeMoveNext(System.Collections.IEnumerator enumerator, System.IntPtr returnValueAddress)(at / Users / bokken / buildslave / unity / build / Runtime / Export / Scripting / Coroutines.cs:17)
        **/
        instances = new List<GameObject>();
    }


    public void GenerateVegetation()
    {
        // TODO: Exercise 1.2 -> 1.)
        // Instantiate & transform random "vegetationPrefab"


        int count, index, x, z;

        if (customPrefabSelection.Count > 0)
        {
            for (int i = 0; i < customPrefabSelection.Count; i++)//no of element count loop
            {
                prefabsStruct currentStuct = customPrefabSelection[i];//in each iterate assigning it to currentStruct
                for (int j = 0; j < currentStuct.count; j++)
                { //our property

                    //getting random values from within the set boundries given above in the min and max boundary
                    x = getRandom.Next(-30, 30);
                    z = getRandom.Next(-30, 30);
                    Vector3 randomTranslationCordinates = new Vector3(x, 0, z);

                    //placeholder value tempobject so when we destroy we dont destroy the vegetation prefab object but rather the placeholder object
                    GameObject tempobject = currentStuct.prefab; //gameobject in our struct

                    tempobject = Instantiate(tempobject, GameObject.Find("Vegetation").transform);
                    //idea by lucky to add collider if it is not available
                    //if  not tempobject try get GetComponent <collider>
                    //temp.Add
                    tempobject.transform.position = randomTranslationCordinates;

                    //defining range for rotate using unity engine 
                    tempobject.transform.Rotate(0, UnityEngine.Random.Range(0, 90), 0);
                    tempobject.SetActive(true);
                    instances.Add(tempobject);

                }

            }
        }
        else
        {
            for (int i = 0; i < numObjects; i++)
            {

                //count of the number of prefabs in the list
                count = vegetationPrefabs.Count;

                //limiting the randomly generated index to the number of prefab that is in the list
                index = getRandom.Next(count);

                //getting random values from within the set boundries given above in the min and max boundary
                x = getRandom.Next(-30, 30);
                z = getRandom.Next(-30, 30);
                Vector3 randomTranslationCordinates = new Vector3(x, 0, z);

                //vegetationPrefabs[index].transform.parent;//GameObject.Find("Vegetation").transform;

                //placeholder value tempobject so when we destroy we dont destroy the vegetation prefab object but rather the placeholder object
                GameObject tempobject = vegetationPrefabs[index];

                //Instantiate temp object with "Vegetation".transform as its parent object
                tempobject = Instantiate(tempobject, GameObject.Find("Vegetation").transform);

                tempobject.transform.position = randomTranslationCordinates;

                //defining range for rotate using unity engine 
                tempobject.transform.Rotate(0, UnityEngine.Random.Range(0, 90), 0);
                tempobject.SetActive(true);
                instances.Add(tempobject);
            }
        }

        //  Collisions need to be resolved at a later time,
        //because Unity physics loop(Unity-internal evaluation of collisions)
        //runs separate from Update() loop
        StartCoroutine(ResolveCollisions());
    }

    IEnumerator ResolveCollisions()
    {
        yield return new WaitForSeconds(2);
        bool resolveAgain = false;

        // TODO: Exercise 1.2 -> 2.)
        // check & handle bounds intersection of each instance with "restrictedBounds"
        //declare a new object of type Collider

        Collider objectCollider;
        //assign collider type of one of the generated instance inside the collider object

        foreach (var i in instances)
        {
            objectCollider = i.GetComponent<Collider>();
            if (IsInRestrictedBounds(objectCollider))
            {
                i.transform.position = new Vector3(getRandom.Next(-30, 30), 0, getRandom.Next(-30, 30));
            }
        }

        foreach (var i in instances)
        {
            objectCollider = i.GetComponent<Collider>();
            if (IsInRestrictedBounds(objectCollider))
            {
                resolveAgain = true;
            }
        }

        //pass the collider object into IsInRestrictedBound

        // resolve again (delayed), after new random transform applied to colliding instances
        if (resolveAgain)
        {
            StartCoroutine(ResolveCollisions());
        }

    }

    bool IsInRestrictedBounds(Collider co)
    {
        // TODO: part of Exercise 1.2-> 2.)
        //loop through the list of all restricted bounds
        for (int i = 0; i < restrictedBounds.Count; i++)
        {
            //check if the collider object of the instance lies within restricted bounds
            if (co.bounds.Intersects(restrictedBounds[i].bounds))
            {
                return true;
            }
        }

        return false;
    }
}