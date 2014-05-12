using UnityEngine;
using System.Collections;

public class LoadPrefab : MonoBehaviour
{
	public GameObject prefab;

	void Start ()
	{
		var item = (GameObject)GameObject.Instantiate (prefab);
		item.transform.position = transform.position;
	}

}
