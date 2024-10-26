using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomCollectable : MonoBehaviour
{
    public Room room;

    [HideInInspector, Range(1, 3)] public int tier = 1;
    [HideInInspector] public CollectiblesManager.Prefabs1 collectableTierOne;
    [HideInInspector] public CollectiblesManager.Prefabs2 collectableTierTwo;
    [HideInInspector] public CollectiblesManager.Prefabs3 collectableTierThree;

    private void Start()
    {
        if (PlayerPrefs.HasKey(SceneManager.GetActiveScene().name)) Destroy(gameObject);

        StartCoroutine(SpawnCollectable());
    }

    public class PrefabPath
    {
        public int tier = 1;
        public CollectiblesManager.Prefabs1 collectableTierOne;
        public CollectiblesManager.Prefabs2 collectableTierTwo;
        public CollectiblesManager.Prefabs3 collectableTierThree;

        public PrefabPath(int tier, CollectiblesManager.Prefabs1 collectableTierOne,
            CollectiblesManager.Prefabs2 collectableTierTwo,
            CollectiblesManager.Prefabs3 collectableTierThree)
        {
            this.tier = tier;
            this.collectableTierOne = collectableTierOne;
            this.collectableTierTwo = collectableTierTwo;
            this.collectableTierThree = collectableTierThree;
        }

        public string GetPath()
        {
            string prefabPath = "Collectables/";

            switch (tier)
            {
                case 1:
                    prefabPath += "1/" + collectableTierOne.ToString();
                    break;
                case 2:
                    prefabPath += "2/" + collectableTierTwo.ToString();
                    break;
                case 3:
                    prefabPath += "3/" + collectableTierThree.ToString();
                    break;
            }
            
            return prefabPath;
        }

        public string GetTierAndName()
        {
            string prefabPath = "Tier: ";

            switch (tier)
            {
                case 1:
                    prefabPath += "1, " + collectableTierOne.ToString();
                    break;
                case 2:
                    prefabPath += "2, " + collectableTierTwo.ToString();
                    break;
                case 3:
                    prefabPath += "3, " + collectableTierThree.ToString();
                    break;
            }

            return prefabPath;
        }
    }

    IEnumerator SpawnCollectable()
    {
        string prefabPath = new PrefabPath(tier, collectableTierOne, collectableTierTwo, collectableTierThree).GetPath();

        yield return new WaitUntil(() => CollectiblesManager.instance.collectiblesLoaded);
        yield return null;

        CollectableStructure collectableStructure = new CollectableStructure(room.roomIndex, room.roomCollectibles.index, false, transform.position, prefabPath);

        if (!CollectiblesManager.instance.IsCollectableSaved(collectableStructure)) 
            CollectiblesManager.instance.SpawnCollectable(prefabPath, transform.position, room);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        GUIStyle style = new GUIStyle(EditorStyles.selectionRect);

        style.alignment = TextAnchor.MiddleCenter;
        style.fontStyle = FontStyle.Bold;
        style.fontSize = 10;

        Handles.Label(transform.position, new PrefabPath(tier, 
            collectableTierOne, collectableTierTwo, collectableTierThree).GetTierAndName(), style);
    }
#endif
}
