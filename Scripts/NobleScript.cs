using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class NobleScript : MonoBehaviour {

    public bool playerControlled;
    public Noble noble { get; private set; }

    public static NobleScript CreateComponent(GameObject where, Color nC, string name)
    {
        NobleScript theScript = where.AddComponent<NobleScript>();
        theScript.noble = new Noble(theScript, nC, name);
        Camera.main.GetComponent<Game>().nobles.Add(theScript.noble);
        return theScript;
    }

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        StartCoroutine(ShowKnownPlayerProvinces());
        AIControl();
	}

    void OnGUI()
    {
        if (playerControlled)
        {
            if (SelectedNeighbour())
            {
                if (GUI.Button(new Rect(0, 0, 100, 100), "Scout"))
                {
                    Province selectedProvince = Camera.main.GetComponent<Game>().selectedProvince.province;
                    StartCoroutine(noble.StartScout(selectedProvince));
                }
                if (GUI.Button(new Rect(100, 0, 100, 100), "Attack"))
                {
                    Province selectedProvince = Camera.main.GetComponent<Game>().selectedProvince.province;
                    StartCoroutine(noble.StartAttack(selectedProvince));
                }
            }
        }
    }

    public void AIControl()
    {
        if (!playerControlled)
        {
            Province[] ownedLands = noble.GetOwnedLands();
            Dictionary<string, Province> knownLands = noble.GetKnownLands();
            bool army = false, relations = false;
            for (int i = 0; i < ownedLands.Length; i++)
            {
                string[] landNeighbours = ownedLands[i].GetNeighbours();
                for (int j = 0; j < landNeighbours.Length; j++)
                {
                    Province neighbourLand = Camera.main.GetComponent<Game>().provinces[landNeighbours[j]];
                    if (!knownLands.ContainsKey(neighbourLand.provinceName) && !noble.GetActiveScouts().Contains(neighbourLand.provinceName))
                    {
                        StartCoroutine(noble.StartScout(neighbourLand));
                        relations = true;
                        break;
                    }
                    else if (neighbourLand.owner == null && !noble.GetActiveAttacks().Contains(neighbourLand.provinceName))
                    {
                        StartCoroutine(noble.StartAttack(neighbourLand));
                        army = true;
                        break;
                    }
                }

                if (army || relations) break;
            }
        }
    }

    public bool SelectedNeighbour()
    {
        Province[] ownedLands = noble.GetOwnedLands();
        ProvinceScript currentlySelected = Camera.main.GetComponent<Game>().selectedProvince;
        string[] neighbours = null;
        if (currentlySelected != null) neighbours = currentlySelected.province.GetNeighbours();
        if (neighbours != null && ownedLands != null && currentlySelected.province.owner != noble)
        {
            for (int i = 0; i < ownedLands.Length; i++)
            {
                if (neighbours.Contains(ownedLands[i].provinceName)) return true;
            }
        }
        return false;
    }
    public IEnumerator ShowKnownPlayerProvinces()
    {
        //Debug.Log(noble.name);
        if (playerControlled)
        {
            Dictionary<string, Province> knownLands = noble.GetKnownLands();
            //Debug.Log("Known Lands: " + knownLands.Length);
            for (int i = 0; i < knownLands.Count; i++)
            {
                knownLands.Values.ElementAt(i).provinceScript.ownerShown = knownLands.Values.ElementAt(i).owner;
                knownLands.Values.ElementAt(i).provinceScript.visible = true;
                yield return null;
            }
        }
    }
}
