using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollViewScript : MonoBehaviour {
    public List<GameObject> itemScrollList;
    public GameObject itemScroll;
    public GameObject content;
	// Use this for initialization
	void Start () {
        itemScrollList = new List<GameObject>();
        content = this.transform.Find("Content").gameObject;
            
         }
	
	public void DownloadList(List<string> names)
    {
        foreach(Transform child in content.transform)
        {
            Destroy(child.gameObject);
        }
        foreach(string name in names)
        {
            var instance = GameObject.Instantiate(itemScroll);
            instance.transform.SetParent(content.transform);
            instance.GetComponentInChildren<Text>().text = name;
            itemScrollList.Add(instance);
        }
    }
}
