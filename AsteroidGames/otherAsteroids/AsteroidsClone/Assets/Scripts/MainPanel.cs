using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainPanel : MonoBehaviour
{
	private RectTransform rectTransform;
	// Use this for initialization
	void Start ()
	{
		rectTransform = gameObject.GetComponent<RectTransform> ();
		rectTransform.sizeDelta = new Vector2 (Screen.width * 2 / 3, Screen.height);
	}
	
	// Update is called once per frame
	void Update ()
	{
		rectTransform.sizeDelta = new Vector2 (Screen.width * 2 / 3, Screen.height);
	}
}
