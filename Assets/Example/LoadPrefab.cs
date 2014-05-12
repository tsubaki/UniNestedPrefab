using UnityEngine;
using System.Collections;

public class LoadPrefab : MonoBehaviour
{
	public GameObject obk;

	void Start ()
	{
		GameObject.Instantiate (obk);
	}

}
