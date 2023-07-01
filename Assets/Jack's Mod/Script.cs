using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Script : MonoBehaviour
{
    // Overlay variables

    private GameObject textObj, background;
    private Text textComponent;
    public Font arialFont;
    private RectTransform rectTransform;
    private Vector2 originalDelta;

    // Start is called before the first frame update
    void Start()
    {
        // Assigning overlay variables

        blockScript = GetComponent<BlockScript>();
        textObj = GameObject.Find("Text");
        textComponent = textObj.GetComponent<Text>();
        background = GameObject.Find("Background");
        rectTransform = background.GetComponent<RectTransform>();
        originalDelta = rectTransform.sizeDelta;

        UpdateOverlay();

    }

    // Update is called once per frame
    void Update()
    {
       if (Input.GetKeyDown(KeyCode.R)) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
       }
    }

    public void UpdateOverlay()
    {
        textComponent.font = arialFont;

        rectTransform.sizeDelta = originalDelta;

    }
}
