using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public enum Commodity
{
    Food,
    Timber,
    Iron,
    Gold,
}

public class Game : MonoBehaviour {

    public bool viewAll = false;
    public List<Noble> nobles = new List<Noble>();
    public Dictionary<string, Province> provinces = new Dictionary<string, Province>();
    public ProvinceScript highlightedProvince, selectedProvince;
    private ProvinceScript mouseDowned;
    public Color highlightColor = Color.yellow;
    public Color selectedColor = Color.red;

	// Use this for initialization
	void Start ()
    {
        CreateNobles();
	}
	
	// Update is called once per frame
	void Update ()
    {
        MouseProvinceActions();
	}

    public void MouseProvinceActions()
    {
        HighlightProvinces();
        SelectProvinces();
    }
    public void HighlightProvinces()
    {
        if (!Camera.main.GetComponent<CameraControl>().moved)
        {
            if (highlightedProvince != null) highlightedProvince.Highlight(false);
            highlightedProvince = null;

            Ray mouseRay = Camera.main.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
            Debug.DrawRay(mouseRay.origin, mouseRay.direction);
            RaycastHit hitInfo;
            if (Physics.Raycast(mouseRay, out hitInfo))
            {
                highlightedProvince = hitInfo.transform.GetComponent<ProvinceScript>();
                if (highlightedProvince == selectedProvince) highlightedProvince = null;
            }

            if (highlightedProvince != null) highlightedProvince.Highlight(true);
            //return currentlyOver;
        }
    }
    public void SelectProvinces()
    {
        if (!Camera.main.GetComponent<CameraControl>().moved)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (highlightedProvince != null) mouseDowned = highlightedProvince;
            }
            if (Input.GetMouseButtonUp(0))
            {
                if (highlightedProvince != null && mouseDowned != null && mouseDowned == highlightedProvince)
                {
                    if (selectedProvince != null) selectedProvince.Select(false);
                    highlightedProvince = null;
                    selectedProvince = mouseDowned;
                    mouseDowned = null;
                    selectedProvince.Select(true);
                }
            }
            if (Input.GetMouseButtonUp(1))
            {
                if (selectedProvince != null)
                {
                    selectedProvince.Select(false);
                    selectedProvince = null;
                }
            }
        }
        else mouseDowned = null;
    }

    public void CreateNobles()
    {
        List<Province> randomSelection = new List<Province>(provinces.Values.ToArray<Province>());
        Province selected;

        selected = randomSelection[Random.Range(0, randomSelection.Count)];
        randomSelection.Remove(selected);
        NobleScript.CreateComponent(Camera.main.gameObject, Color.blue, "Anjou").noble.ClaimLand(selected);
        selected = randomSelection[Random.Range(0, randomSelection.Count)];
        randomSelection.Remove(selected);
        NobleScript.CreateComponent(Camera.main.gameObject, new Color(153f / 255f, 0f, 153f / 255f), "Albion").noble.ClaimLand(selected);
        selected = randomSelection[Random.Range(0, randomSelection.Count)];
        randomSelection.Remove(selected);
        NobleScript.CreateComponent(Camera.main.gameObject, new Color(204f / 255f, 153f / 255f, 51f / 255f), "Burgandy").noble.ClaimLand(selected);
        selected = randomSelection[Random.Range(0, randomSelection.Count)];
        randomSelection.Remove(selected);
        NobleScript.CreateComponent(Camera.main.gameObject, Color.cyan, "Aragon").noble.ClaimLand(selected);
        selected = randomSelection[Random.Range(0, randomSelection.Count)];
        randomSelection.Remove(selected);
        NobleScript.CreateComponent(Camera.main.gameObject, Color.red, "Valois").noble.ClaimLand(selected);

        //nobles.Add(new Noble(Color.yellow, "Pope"));
    }
}
