using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour {

    [SerializeField]
    private GameObject[] objectPrefabs;
    //List of objects in pool

    private List<GameObject> pooledObjects = new List<GameObject>();
    //GameObject of specific type
    public GameObject GetObject(string type)
    {
        //If there's an object of that type in the field(disabled)
        foreach(GameObject go in pooledObjects)
        {
            if (go.name == type && !go.activeInHierarchy)
            {
                go.SetActive(true);
                return go;
            }
        }

        //If the pool didn't contain the object, we need to create it
        for (int i = 0; i < objectPrefabs.Length; i++)
        {
            //If we have a prefab for creating the object
            if (objectPrefabs[i].name == type)
            {
                //Instantiate the prefab of the correct type
                GameObject newObject = Instantiate(objectPrefabs[i]);

                //Add new monster of this type to pool
                pooledObjects.Add(newObject);

                newObject.name = type;
                return newObject;
            }
          
        }
        return null;
    }


    public void ReleaseObject(GameObject gameObject)
    {
        gameObject.SetActive(false);
    }
}
