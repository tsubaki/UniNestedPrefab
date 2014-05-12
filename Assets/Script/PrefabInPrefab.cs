using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PrefabInPrefabController))]
public class PrefabInPrefab : MonoBehaviour
{
	public GameObject prefab;

	[ContextMenu("add prefab")]
	public void AddPrefabList ()
	{
		var prefabCheck = GetComponent<PrefabInPrefabController> ();
		if (prefabCheck == null)
			prefabCheck = gameObject.AddComponent<PrefabInPrefabController> ();

		prefabCheck.AddPrefabinprefab (this);
	}

	[ContextMenu("Reset prefab")]
	void ResetAtPrefabList ()
	{
		var prefabCheck = GetComponent<PrefabInPrefabController> ();
		if (prefabCheck == null)
			return;

		prefabCheck.RemovePrefabInPrefab (this);
		prefabCheck.AddPrefabinprefab (this);
	}

}
