using UnityEngine;
using System.Collections;
using System;

public class ProvinceScript : MonoBehaviour {

    //public string provinceName { get; private set; }
    //public Noble owner { get; private set; }
    public Province province { get; private set; }
    public static Color noOwnerShieldColor = Color.white;
    //public Commodity commodity;
    public Transform shield;
    public Color grassColor;
    private Material material, shieldMaterial;
    public float maxColorOffset = 0.5f;

    public Noble ownerShown;
    public bool visible;
    //public bool commodityKnown { get; private set; }

	// Use this for initialization
	void Start ()
    {
        province = new Province(gameObject.name.Replace("_", " "), this);

        //RandomizeCommodity();

        Camera.main.GetComponent<Game>().provinces.Add(province.provinceName, province);
        material = new Material(Shader.Find("Legacy Shaders/Diffuse"));
        RandomizeColor();
        GetComponent<Renderer>().material = material;
	}
	
	// Update is called once per frame
	void Update ()
    {
        SetShield();
	}

    /*public void SetOwnership(Noble n)
    {
        if (owner != null)
        {
            owner.Unclaim(this);
        }
        owner = n;
        if (owner.playerControlled) Scout();
    }*/

    public void SetShield()
    {
        Vector3 cameraDirection = (Camera.main.transform.position - shield.position).normalized;
        shield.up = cameraDirection;

        if (visible || Camera.main.GetComponent<Game>().viewAll) shield.gameObject.SetActive(true);
        else shield.gameObject.SetActive(false);

        if (shieldMaterial == null)
        {
            shieldMaterial = new Material(Shader.Find("Legacy Shaders/Diffuse"));
            shield.GetComponent<Renderer>().material = shieldMaterial;
        }

        if (Camera.main.GetComponent<Game>().viewAll && province.owner != null) shieldMaterial.color = province.owner.nobleColor;
        else if (!Camera.main.GetComponent<Game>().viewAll && ownerShown != null) shieldMaterial.color = ownerShown.nobleColor;
        else shieldMaterial.color = noOwnerShieldColor;

        if (province.commodity == Commodity.Food) shield.FindChild("Food").gameObject.SetActive(true);
        else shield.FindChild("Food").gameObject.SetActive(false);
        if (province.commodity == Commodity.Timber) shield.FindChild("Timber").gameObject.SetActive(true);
        else shield.FindChild("Timber").gameObject.SetActive(false);
        if (province.commodity == Commodity.Iron) shield.FindChild("Iron").gameObject.SetActive(true);
        else shield.FindChild("Iron").gameObject.SetActive(false);
        if (province.commodity == Commodity.Gold) shield.FindChild("Gold").gameObject.SetActive(true);
        else shield.FindChild("Gold").gameObject.SetActive(false);

        visible = false;
    }

    /*public void Scout()
    {
        commodityKnown = true;
        lastOwnerKnown = owner;
    }*/

    /*public void RandomizeCommodity()
    {
        Array values = Enum.GetValues(typeof(Commodity));
        commodity = (Commodity) values.GetValue(UnityEngine.Random.Range(0, values.Length));
    }*/

    public void RandomizeColor()
    {
        grassColor = new Color(Color.green.r + UnityEngine.Random.Range(-maxColorOffset, maxColorOffset), Color.green.g + UnityEngine.Random.Range(-maxColorOffset, maxColorOffset), Color.green.b + UnityEngine.Random.Range(-maxColorOffset, maxColorOffset));
        material.color = grassColor;
    }

    public void Highlight(bool onOff)
    {
        if (onOff) material.color = Camera.main.GetComponent<Game>().highlightColor;
        else material.color = grassColor;
    }
    public void Select(bool onOff)
    {
        if (onOff) material.color = Camera.main.GetComponent<Game>().selectedColor;
        else material.color = grassColor;
    }
}
