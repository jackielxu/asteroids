using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LeaderBoard : MonoBehaviour
{
	private Text leaderBoardText;
	private LeaderBoard leaderBoard;
	// Use this for initialization
	void Start ()
	{

	}
	
	// Update is called once per frame
	void Update ()
	{
		leaderBoardText = gameObject.GetComponent<Text> ();
		string text = "<size=24><b>Leader Board</b></size>" + "\n";
		if (!PlayerPrefs.HasKey ("HighScore-1")) {
			for (int i = 1; i<11; i+=2) {
				text += "<b>" + i.ToString () + ". </b>" + "<i><b>0</b></i>" + "\t\t\t\t<b>" + (i + 1).ToString () + ". </b>" + "<i><b>0</b></i>" + "\n";
			}
			leaderBoardText.text = text;
		} else {
			for (int i = 1; i<11; i+=2) {
				text += "<b>" + i.ToString () + ". </b>" + "<i><b>" + PlayerPrefs.GetInt ("HighScore-" + i.ToString ()).ToString () + "</b></i>\t\t\t\t<b>" + (i + 1).ToString () + ". </b><i><b>" + PlayerPrefs.GetInt ("HighScore-" + (i + 1).ToString ()).ToString () + "</b></i>\n";
			}
			leaderBoardText.text = text;
		}
	}
}
