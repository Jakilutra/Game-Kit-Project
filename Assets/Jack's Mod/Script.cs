using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Script : MonoBehaviour
{
    // Overlay variables

    private GameObject textObj;
    private Text textComponent;
    public Font arialFont;
    private string currentScene, jumpString;
    private int[] currentJumpConfig = new int[7];

    private Dictionary<string, int[]> jumpConfig;
    private Dictionary<string, string> symbolConvert;

    private int selector = 0;

    private bool togglePaused = false;

    // Start is called before the first frame update
    void Start()
    {

        currentScene = SceneManager.GetActiveScene().name;

        // Jump Power dictionaries stored per level

        jumpConfig = new Dictionary<string, int[]>()
        {
            { "Level 1", new int[] { 1, 1, 1, 1, 1, 1, 1 } },
            { "Level 2", new int[] { 1, 2, 1, 2, 1, 2, 1 } },
            { "Level 3", new int[] { 2, 3, 4, 5, 7, 8, 9 } },
            { "Level 4", new int[] { 7, 7, 7, 7, 7, 7, 7 } },
            { "Level 5", new int[] { 3, 4, 5, 6, 7, 8, 9 } },
        };

        // Converting the Jump Powers for display.

        symbolConvert = new Dictionary<string, string>()
        {{"1", "➊"}, {"2","➋"}, {"3","➌"}, {"4","➍"}, {"5","➎"}, {"6","➏"}, {"7","➐"}, {"8","➑"}, {"9","➒"}};

        currentJumpConfig = jumpConfig[currentScene];
        jumpString = string.Join("  ", jumpConfig[currentScene]);

        foreach (var symbol in symbolConvert)
        {
            jumpString = jumpString.Replace(symbol.Key, symbol.Value);
        }

        // Assigning overlay variables

        textObj = GameObject.Find("Text");
        textComponent = textObj.GetComponent<Text>();

        UpdateOverlay();

    }

    // Update is called once per frame
    void Update()
    {
       // Restart

        if (Input.GetKeyDown(KeyCode.R)) {
            SceneManager.LoadScene(currentScene);
        }

       // J and K toggle;

        if (Input.GetKey(KeyCode.T) & !togglePaused)
        {
            togglePaused = true;
            Invoke("UndoTogglePause", 0.3f);
            selector = selector == 0 ? 18 : selector - 3;
            UpdateOverlay();
        }

        if (Input.GetKey(KeyCode.Y) & !togglePaused)
        {
            togglePaused = true;
            Invoke("UndoTogglePause", 0.3f);
            selector = selector == 18 ? 0 : selector + 3;
            UpdateOverlay();
        }
    }

    public void UpdateOverlay()
    {
        // fallback font

        textComponent.font = arialFont;

        // Highlighted Jump Power

        var jumpPowers = jumpString.Substring(0, Math.Max(selector,0)) + "<color=green>" + jumpString[selector] + "</color>" + jumpString.Substring(selector+1);

        textComponent.text = "Select your jump power:\n"
                    + "\n"
                    + jumpPowers + "\n"
                    + "\n"
                    + "Type T and Y to toggle";
    }

    public void UndoTogglePause()
    {
        togglePaused = false;
    }
}
