using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class VegetationGenerator : MonoBehaviour
{
    public List<GameObject> vegetationPrefabs = new List<GameObject>();

    private List<GameObject> instances = new List<GameObject>();

    public List<Collider> restrictedBounds = new List<Collider>();

    public int numObjects = 30;

    public Vector3 vegetationBoundsMin = new Vector3(-30, 0, -30);

    public Vector3 vegetationBoundsMax = new Vector3(30, 0, 30);

    public bool reset = false;

    private System.Random getRandom = new System.Random();

    // Start is called before the first frame update
    void Start()
    { 
       GenerateVegetation();
        
        //Instantiate(vegetationPrefabs[0], new Vector3(0, 0, 0), (Quaternion.AngleAxis(0.0f, new Vector3(0, 1, 0))));
        //transform.rotation = Quaternion.Euler(0, 40, 0);
    }

    // Update is called once per frame
    void Update()
    {
        // TODO: Exercise 1.2 -> 3.)
        // check & handle "reset" to regenerate vegetation instances
        
        if (reset==true)
        {
            // foreach (var i in instances)
            //{
            // if (i != null)
            //  {
            // ClearVegetation(i);
            //Destroy(Vegetation.transform.GetChild(i).gameObject);

            //  }
            //  }
            for(int i =0; i< GameObject.Find("Vegetation").transform.childCount; i++)
            {
                ClearVegetation(i);
               
            }
            //Debug.Log("reached here");
           // instances.Clear();
            GenerateVegetation();
            reset = false;
        }
    }

    void ClearVegetation(int toDelete)
    {
        //Destroy(toDelete);
        
        Destroy(GameObject.Find("Vegetation").transform.GetChild(toDelete).gameObject);
        
        //Destroy(toDelete.GetComponent<MeshRenderer>());
    }


    public void GenerateVegetation()
    {
        // TODO: Exercise 1.2 -> 1.)
        // Instantiate & transform random "vegetationPrefab"


        int count, index, x, y, z;
      

        for (int i = 0; i < numObjects; i++)
        {
            count = vegetationPrefabs.Count;
            index = getRandom.Next(count);
            

            x = getRandom.Next(-30, 30);
            z = getRandom.Next(-30, 30);
            Vector3 randomTranslationCordinates = new Vector3(x, 0, z);
            
            //vegetationPrefabs[index].transform.parent;//GameObject.Find("Vegetation").transform;

            GameObject tempobject = vegetationPrefabs[index];

            //vegetationPrefabs[index] = Instantiate(vegetationPrefabs[index], GameObject.Find("Vegetation").transform);

            tempobject = Instantiate(tempobject, GameObject.Find("Vegetation").transform);

            //vegetationPrefabs[index].transform.position = randomTranslationCordinates;
            //vegetationPrefabs[index].SetActive(true);
            //instances.Add(vegetationPrefabs[index]);

            tempobject.transform.position = randomTranslationCordinates;
            tempobject.SetActive(true);
            instances.Add(tempobject);
            
            //converting getRandom to float and generating random angle rotation
            float randomAngle = (float)getRandom.Next(1, 360);
        }
      

        // Collisions need to be resolved at a later time,
        // because Unity physics loop (Unity-internal evaluation of collisions)
        // runs separate from Update() loop
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

        // your code here

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
