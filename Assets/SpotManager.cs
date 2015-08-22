using UnityEngine;
using System.Collections;
using InventoryQuest.Game;
using UnityEngine.UI;
using InventoryQuest.Components;
using System.Linq;
using System.Text;

public class SpotManager : MonoBehaviour {

	//All spots 
	//GenerationStorage.Instance.Spots
	//Current spot
	//CurrentGame.Instance.Spot

	Text _text;

	// Use this for initialization
	void Start () {
		_text = GetComponent<Text>();
		StringBuilder sb = new StringBuilder ();
		foreach (var item in GenerationStorage.Instance.Spots) {
			sb.AppendLine(item.Name);
		}
		_text.text = sb.ToString ();

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
