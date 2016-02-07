using UnityEngine;
using System.Collections;

public class ActionBar : OxGUI3D {

    public float percentComplete = 1f;
    public bool centeredBar = false;
    public bool flip = false;
    private bool flipping = false;
    public float degrees = 180f, seconds = 0.5f, fps = 30f;

    public Color mainColor = Color.blue, backgroundColor = Color.gray;
    private Material borderMaterial, fillMaterial;

    private Vector3 originalLoadingBarPosition, originalLoadingBarScale;
    private Transform loadingBar;

	// Use this for initialization
	protected override void Start ()
    {
        base.Start();

        #region Initialise Materials
        borderMaterial = new Material(Shader.Find("Legacy Shaders/Diffuse"));
        fillMaterial = new Material(Shader.Find("Legacy Shaders/Diffuse"));
        UpdateColors();
        #endregion

        #region Initial Loading Bar Setup
        loadingBar = transform.FindChild("FillingTopHalf").FindChild("LoadingBar");
        originalLoadingBarPosition = loadingBar.localPosition;
        originalLoadingBarScale = loadingBar.localScale;
        #endregion

        #region Set Materials unto ActionBar
        loadingBar.GetComponent<Renderer>().material = borderMaterial;
        transform.FindChild("FillingBottomHalf").GetComponent<Renderer>().material = fillMaterial;
        transform.FindChild("BorderTop").GetComponent<Renderer>().material = borderMaterial;
        transform.FindChild("BorderBottom").GetComponent<Renderer>().material = borderMaterial;
        transform.FindChild("BorderLeft").GetComponent<Renderer>().material = borderMaterial;
        transform.FindChild("BorderRight").GetComponent<Renderer>().material = borderMaterial;
        transform.FindChild("FillingTopHalf").FindChild("Top").GetComponent<Renderer>().material = fillMaterial;
        transform.FindChild("FillingTopHalf").FindChild("Bottom").GetComponent<Renderer>().material = fillMaterial;
        transform.FindChild("FillingTopHalf").FindChild("Left").GetComponent<Renderer>().material = fillMaterial;
        transform.FindChild("FillingTopHalf").FindChild("Right").GetComponent<Renderer>().material = fillMaterial;
        #endregion
    }
	
	// Update is called once per frame
	protected override void Update ()
    {
        base.Update();
        UpdateColors();
        UpdateLoadingBar();
        if (flip)
        {
            flip = false;
            FlipBar(degrees, seconds, fps);
        }
	}

    private void UpdateColors()
    {
        borderMaterial.color = mainColor;
        fillMaterial.color = backgroundColor;
    }
    private void UpdateLoadingBar()
    {
        if (percentComplete > 1) percentComplete = 1f;
        else if (percentComplete < 0) percentComplete = 0f;

        loadingBar.localScale = new Vector3(originalLoadingBarScale.x * percentComplete, originalLoadingBarScale.y, originalLoadingBarScale.z);

        if (centeredBar)
        {
            loadingBar.localPosition = originalLoadingBarPosition;
        }
        else
        {
            loadingBar.localPosition = originalLoadingBarPosition + new Vector3(loadingBar.localScale.x / 2f, 0, 0) - new Vector3(originalLoadingBarScale.x / 2f, 0, 0);
        }
    }

    public void FlipBar(float degrees, float seconds, float fps)
    {
        if (!flipping)
        {
            StartCoroutine(Flip(degrees, seconds, fps));
        }
    }
    private IEnumerator Flip(float degrees, float seconds, float fps)
    {
        flipping = true;

        float countDownDegrees = degrees, splitDegrees = degrees / (fps * seconds), splitSeconds = 1 / fps, currentTime = Time.time;
        Quaternion originalRotation = transform.localRotation;

        while (true)
        {
            //yield return new WaitForSeconds(splitSeconds);
            if (Time.time - currentTime >= splitSeconds)
            {
                transform.Rotate(new Vector3(splitDegrees, 0, 0));
                countDownDegrees -= splitDegrees;
                if (countDownDegrees <= 0) { transform.localRotation = originalRotation * Quaternion.AngleAxis(degrees, Vector3.right); break; }
                currentTime = Time.time;
            }

            yield return null;
        }

        flipping = false;
    }
}
