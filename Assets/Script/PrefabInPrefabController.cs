using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class PrefabInPrefabController : MonoBehaviour
{
	[ContextMenu("Re Register")]
	void Start ()
	{
		foreach (var prefabInPrefab in GetComponents<PrefabInPrefab>())
			prefabInPrefab.AddPrefabList ();

		ClearPrefab ();
		enabled = false;
	}

	public List<PrefabItem> registerPrefabList = new List<PrefabItem> ();

	public void RemovePrefabInPrefab (PrefabInPrefab prefabItem)
	{
		var target = registerPrefabList.Find ((item) => item.prefabInPrefab.prefab == prefabItem.prefab);
		DestroyImmediate (target.instancedObject);
		registerPrefabList.Remove (target);
	}

	public void AddPrefabinprefab (PrefabInPrefab prefabItem)
	{
		if (registerPrefabList.Find ((item) => item.prefabInPrefab.prefab == prefabItem.prefab) != null || prefabItem.prefab == null) {
			return;
		}

#if UNITY_EDITOR
		var prefabInstance = (GameObject)UnityEditor.PrefabUtility.InstantiatePrefab (prefabItem.prefab);
#else
		var prefabInstance = (GameObject)Instantiate(prefabItem.prefab);
#endif

		prefabInstance.transform.parent = transform;
		prefabInstance.transform.localPosition = prefabItem.prefab.transform.position;

#if UNITY_EDITOR
		registerPrefabList.Add (new PrefabItem (){
			instancedObject = prefabInstance,
			prefabInPrefab = prefabItem,
		});
#endif
	}

#if UNITY_EDITOR

	[ContextMenu("update prefab")]
	void UpdatePrefab ()
	{
		GameObject temp = new GameObject ("temp");
		var preList = registerPrefabList.ToArray ();

		foreach (var item in registerPrefabList) {
			item.instancedObject.transform.parent = temp.transform;
		}

		registerPrefabList.Clear ();
		var root = PrefabUtility.GetPrefabParent (gameObject);

		enabled = true;

		PrefabUtility.ReplacePrefab (gameObject, root);
		PrefabUtility.RevertPrefabInstance (gameObject);

		enabled = false;

		registerPrefabList = new List<PrefabItem> (preList);
		foreach (var item in preList) {
			item.instancedObject.transform.parent = gameObject.transform;
		}

		DestroyImmediate (temp);
	}

	[ContextMenu("Clean")]
	void ClearPrefab ()
	{
		var removePrefabList = new List<PrefabItem> ();

		foreach (var prefabInPrefab in registerPrefabList) {

			if (prefabInPrefab.instancedObject == null || prefabInPrefab.prefabInPrefab == null) {
				removePrefabList.Add (prefabInPrefab);
			}
		}

		foreach (var removePrefab in removePrefabList) {
			DestroyImmediate (removePrefab.instancedObject);
			registerPrefabList.Remove (removePrefab);
		}
	}

	[ContextMenu("remove all registed prefab")]
	void RemoveAllRegistedPrefab ()
	{
		var removePrefabList = new List<PrefabItem> (registerPrefabList);

		foreach (var removePrefab in removePrefabList) {
			DestroyImmediate (removePrefab.instancedObject);
			registerPrefabList.Remove (removePrefab);
		}
	}

#endif

	[System.Serializable]
	public class PrefabItem
	{
		public PrefabInPrefab prefabInPrefab;
		public GameObject instancedObject;
	}
}
