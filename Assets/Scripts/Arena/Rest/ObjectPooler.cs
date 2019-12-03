/*
	Michał Dominik 21965 CDV
*/

using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectPoolItem
{
	public int amountToPool;
	public GameObject objectToPool;
	public bool expandable;
}

public class ObjectPooler : MonoBehaviour
{
	public static ObjectPooler instance;
	public List<GameObject> pooledObjects;
	public List<ObjectPoolItem> itemsToPool;

	void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		else if (instance != this)
		{
			Destroy(gameObject);
		}
	}

	void Start()
	{
		pooledObjects = new List<GameObject>();
		foreach (ObjectPoolItem item in itemsToPool)
		{
			for (int i = 0; i < item.amountToPool; i++)
			{
				GameObject temp = Instantiate(item.objectToPool);
                temp.transform.SetParent(this.gameObject.transform);
                temp.SetActive(false);
				pooledObjects.Add(temp);
			}
		}
	}

	public GameObject GetPooledObject(string tag)
	{
		for (int i = 0; i < pooledObjects.Count; i++)
		{
			if (!pooledObjects[i].activeInHierarchy && pooledObjects[i].tag == tag)
            { 
				return pooledObjects[i];
			}
		}
		foreach (ObjectPoolItem item in itemsToPool)
		{
			if (item.objectToPool.tag == tag)
			{
				if (item.expandable)
				{
					GameObject temp = Instantiate(item.objectToPool);
					temp.SetActive(false);
					pooledObjects.Add(temp);
                    temp.transform.SetParent(this.gameObject.transform);
                    return temp;
				}
			}
		}
		return null;
	}
}