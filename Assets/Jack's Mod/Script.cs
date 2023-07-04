﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using Gamekit2D;


public class Script : MonoBehaviour
{
    // Overlay variables

    private PlayerCharacter playerScript;
    private GameObject textObj, ellen;
    private CapsuleCollider2D playerCollider;
    private Text textComponent;
    public Font arialFont;
    private string currentScene, jumpString;
    private int[] currentJumpConfig = new int[7];

    private Dictionary<string, int[]> jumpConfig;
    private Dictionary<string, string> symbolConvert;

    private int currentJump = 0, selector = 0, countDown = 0, jumpsUsed;

    private bool togglePaused = false;

    // Start is called before the first frame update
    void Start()
    {

        currentScene = SceneManager.GetActiveScene().name;

        ellen = GameObject.Find("Ellen");
        playerScript = ellen.GetComponent<PlayerCharacter>();

        // Jump Power dictionaries stored per level

        jumpConfig = new Dictionary<string, int[]>()
        {
            { "Level 1", new int[] { 1, 2, 3, 4, 5, 6, 7 } },
            { "Level 2", new int[] { 1, 2, 1, 2, 1, 2, 1 } },
            { "Level 3", new int[] { 2, 3, 4, 5, 7, 8, 9 } },
            { "Level 4", new int[] { 7, 7, 7, 7, 7, 7, 7 } },
            { "Level 5", new int[] { 3, 4, 5, 6, 7, 8, 9 } },
        };

        currentJumpConfig = jumpConfig[currentScene];
        playerScript.jumpSpeed = currentJumpConfig[currentJump] * 10;

        // Converting the Jump Powers for display.

        symbolConvert = new Dictionary<string, string>()
        {{"1", "➊"}, {"2","➋"}, {"3","➌"}, {"4","➍"}, {"5","➎"}, {"6","➏"}, {"7","➐"}, {"8","➑"}, {"9","➒"}};

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

        if (Input.GetKeyDown(KeyCode.P))
        {
            SceneManager.LoadScene(currentScene);
        }

       // Y and U toggle;

        if (Input.GetKey(KeyCode.Y) && currentJumpConfig.Length > 0 && !togglePaused)
        {
            togglePaused = true;
            Invoke("UndoTogglePause", 0.3f);
            currentJump = currentJump == 0 ? currentJumpConfig.Length-1 : currentJump - 1;
            playerScript.jumpSpeed = currentJumpConfig[currentJump] * 10;
            selector = currentJump * 3;
            UpdateOverlay();
        }

        if (Input.GetKey(KeyCode.U) && currentJumpConfig.Length > 0 && !togglePaused)
        {
            togglePaused = true;
            Invoke("UndoTogglePause", 0.3f);
            currentJump = currentJump == currentJumpConfig.Length-1 ? 0 : currentJump + 1;
            playerScript.jumpSpeed = currentJumpConfig[currentJump] * 10;
            selector = currentJump * 3;
            UpdateOverlay();
        }

        // Jump

        if (Input.GetKeyDown(KeyCode.Space))
        {

            playerCollider = ellen.GetComponent<CapsuleCollider2D>();

            // Time Buffer to stop extra jumps.

            if (togglePaused)
            {
                playerScript.jumpSpeed = 0;
                Invoke("SetJumpSpeed", 0.3f);
            }
.
            if (currentJumpConfig.Length > 0 && playerCollider.IsTouchingLayers(LayerMask.GetMask("Platform")) & !togglePaused)
            {
                // Statistic

                jumpsUsed++;

                // Time Buffer to stop jump powers being used up too quickly in succession.

                togglePaused = true;
                Invoke("UndoTogglePause", 0.6f);

                // Removing the Jump Power from the array

                var tempJumps = new List<int>();
                tempJumps = currentJumpConfig.ToList();
                tempJumps.RemoveAt(currentJump);
                currentJumpConfig = tempJumps.ToArray();
                currentJump = currentJump == currentJumpConfig.Length ? currentJump - 1 : currentJump;
                Invoke("SetJumpSpeed", 0.3f);
                selector = selector == currentJumpConfig.Length * 3 ? selector - 2 : selector;
                jumpString = currentJumpConfig.Length > 0 ? jumpString.Remove(selector, 3) : "";
                selector = selector == ((currentJumpConfig.Length * 3) - 2) ? selector - 1 : selector;
                UpdateOverlay();

                // Starting the countdown.

                if (currentJumpConfig.Length == 0)
                {
                    countDown = 6;
                    InvokeRepeating("restartCount", 0f, 1f);
                }
            }
        }
    }

    public void UpdateOverlay()
    {
        // fallback font

        textComponent.font = arialFont;

        // Highlighted Jump Power

        var jumpPowers = currentJumpConfig.Length > 0 ? jumpString.Substring(0, Math.Max(selector,0)) + "<color=green>" + jumpString[selector] + "</color>" + jumpString.Substring(selector+1): "<color=blue>Restarting in " + countDown + " seconds</color>";

        textComponent.text = "Select your jump power:\n"
                    + "\n"
                    + jumpPowers + "\n"
                    + "\n"
                    + "Type U and Y to toggle";
    }

    public void UndoTogglePause()
    {
        togglePaused = false;
    }

    public void SetJumpSpeed()
    {
        playerScript.jumpSpeed = currentJumpConfig.Length > 0 ? currentJumpConfig[currentJump] * 10 : 0;
    }

    public void restartCount()
    {
        countDown--;
        UpdateOverlay();
        if (countDown == 0)
        {
            SceneManager.LoadScene(currentScene);
        }
    }
}
