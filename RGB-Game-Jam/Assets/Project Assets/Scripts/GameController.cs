﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

[RequireComponent(typeof(ScoreDisplay))]
public class GameController : MonoBehaviour
{
    public Camera cameraTarget;
    public int granularity = 100;
    public float matchTime = 3.0f;
    public float remainingTime { get; private set; }
    public enum GameState
    {
        MainMenu,
        Playing,
        GameOver
    }
    public GameState gameState = GameState.Playing;
    public float rScore { get; private set; }
    public float gScore { get; private set; }
    public float bScore { get; private set; }

    private GameObject gameContainer;
    private Vector3 greyCompensation;
    #region RAW VARIABLES (DANGER ZONE)
    private int texW = 0, texH = 0;
    private int checkSpacingX = 0, checkSpacingY = 0;
    private Vector3 colorRaw;
    private int arraySize = 0;
    #endregion

    void Start()
    {
        texW = cameraTarget.targetTexture.width;
        texH = cameraTarget.targetTexture.height;

        checkSpacingX = texW / granularity;
        checkSpacingY = texH / granularity;

        arraySize = granularity * granularity;
        colorRaw = Vector3.zero;
        greyCompensation = Vector3.zero;

        remainingTime = matchTime;

        gameContainer = GameObject.FindGameObjectWithTag("GameContainer");
    }

    void Update()
    {
        if (gameState == GameState.Playing)
        {
            remainingTime -= Time.deltaTime;

            if (remainingTime <= 0.0f)
            {
                remainingTime = 0.0f;

                CalculateScores();
                gameContainer.SetActive(false);
                GetComponent<ScoreDisplay>().ShowScore();
                gameState = GameState.GameOver;
            }
        }
    }

    private void CalculateScores()
    {
        Texture2D mapT2D = RTtoT2D();

        for (int x = 0; x < granularity; x++)
        {
            for (int y = 0; y < granularity; y++)
            {
                Color temp = mapT2D.GetPixel(checkSpacingX * x, checkSpacingY * y);

                if (temp.r > temp.g && temp.r > temp.b)
                    colorRaw.x += 1.0f;
                else if (temp.g > temp.r && temp.g > temp.b)
                    colorRaw.y += 1.0f;
                else if (temp.b > temp.r && temp.b > temp.g)
                    colorRaw.z += 1.0f;
            }
        }

        //if (greyCompensation == Vector3.zero)
        //{
        //    if (colorRaw.x > 0.0f || colorRaw.y > 0.0f || colorRaw.z > 0.0f)
        //        greyCompensation = colorRaw;
        //}
        SetScores();
    }
    
    private Texture2D RTtoT2D()
    {
        RenderTexture.active = cameraTarget.targetTexture;
        Texture2D t2D = new Texture2D(texW, texH);
        t2D.ReadPixels(new Rect(0, 0, texW, texH), 0, 0);
        t2D.Apply();

        return t2D;
    }

    private void SetScores()
    {
        colorRaw = colorRaw / 100.0f;// - greyCompensation) / 100.0f;

        rScore = colorRaw.x;
        gScore = colorRaw.y;
        bScore = colorRaw.z;

        colorRaw = Vector3.zero;
    }

    public float GetTotalScore()
    {
        return rScore + gScore + bScore;
    }

    public void BackToMainMenu()
    {
        if (gameState == GameState.GameOver)
            SceneManager.LoadScene(0);
    }
}
