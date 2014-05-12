using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class PrefabInPrefabController : MonoBehaviour
{
	void Start ()
	{
		CreatePrefab ();
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

	[ContextMenu("プレハブの更新")]
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
#endif

	[ContextMenu("プレハブ生成")]
	void CreatePrefab ()
	{
		foreach (var prefabInPrefab in GetComponents<PrefabInPrefab>())
			prefabInPrefab.AddPrefabList ();
		
		ClearPrefab ();
		enabled = false;

	}

#if UNITY_EDITOR
	[ContextMenu("不完全な参照を破棄")]
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

	[ContextMenu("子プレハブを破棄")]
	void RemoveAllRegistedPrefab ()
	{
		var removePrefabList = new List<PrefabItem> (registerPrefabList);

		foreach (var removePrefab in removePrefabList) {
			DestroyImmediate (removePrefab.instancedObject);
			registerPrefabList.Remove (removePrefab);
		}
	}

	[ContextMenu("子のプレハブを登録")]
	void RegisterAllChildPrefab ()
	{
		var childCount = transform.childCount;
		var pip = new List<PrefabInPrefab> (GetComponents<PrefabInPrefab> ());
		
		for (int i=0; i<childCount; i++) {
			var child = transform.GetChild (i);
			var type = PrefabUtility.GetPrefabType (child);
			
			if (type == PrefabType.PrefabInstance) {
				var root = (Transform)PrefabUtility.GetPrefabParent (child);
				
				if (pip.Find ((item) => item.prefab == root.gameObject) == null) {
					
					var prefabInPrefab = gameObject.AddComponent<PrefabInPrefab> ();
					prefabInPrefab.prefab = root.gameObject;
					
					var prefabItem = new PrefabItem ()
					{
						prefabInPrefab = prefabInPrefab,
						instancedObject = child.gameObject,
					};
					
					registerPrefabList.Add (prefabItem);
				}
			}
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
