using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreDisplay : MonoBehaviour
{
    public Canvas hudTarget;
    
    private GameObject[] scoreSprites;
    private GameObject[] scoreText;
    private Vector2 screen;
    private GameController gController;
    private float maxScale;

    void Start()
    {
        screen = new Vector2(Screen.width, Screen.height);

        /* Rect barRect = new Rect(0.0f, screen.y, screen.x / 3.0f, 10.0f);
            Vector2 offset = new Vector2(barRect.width / 2.0f, barRect.height / -2.0f);

            scoreBars[0] = CreateUIPanel("rScore", Color.red, barRect, offset);
            scoreBars[1] = CreateUIPanel("gScore", Color.green, barRect, offset + new Vector2(barRect.width, 0.0f));
            scoreBars[2] = CreateUIPanel("bScore", Color.blue, barRect, offset + new Vector2(barRect.width * 2.0f, 0.0f)); */

        maxScale = screen.x * 0.25f;

        Rect spriteDims = new Rect(screen.x / 2.0f, screen.y / 2.0f, maxScale, maxScale);
        Sprite splatSprite = Resources.Load<Sprite>("splat_white");

        scoreSprites = new GameObject[3];
        scoreSprites[0] = CreateUIPanel("red_score", Color.red, spriteDims, new Vector2((spriteDims.width * -1.1f), 0.0f), splatSprite);
        scoreSprites[1] = CreateUIPanel("green_score", Color.green, spriteDims, Vector2.zero, splatSprite);
        scoreSprites[2] = CreateUIPanel("blue_score", Color.blue, spriteDims, new Vector2((spriteDims.width * 1.1f), 0.0f), splatSprite);

        scoreText = new GameObject[3];
        scoreText[0] = CreateUIText("0%", Color.white, spriteDims.width, scoreSprites[0].transform);
        scoreText[1] = CreateUIText("0%", Color.white, spriteDims.width, scoreSprites[1].transform);
        scoreText[2] = CreateUIText("0%", Color.white, spriteDims.width, scoreSprites[2].transform);

        gController = GetComponent<GameController>();

        for (int i = 0; i < scoreSprites.Length; i++)
        {
            scoreSprites[i].SetActive(false);
            scoreText[i].SetActive(false);
        }
    }

    public void ShowScore()
    {
        AdjustScoreDisplay(0, gController.rScore);
        AdjustScoreDisplay(1, gController.gScore);
        AdjustScoreDisplay(2, gController.bScore);

        for (int i = 0; i < scoreSprites.Length; i++)
        {
            scoreSprites[i].SetActive(true);
            scoreText[i].SetActive(true);
        }
    }
    
    void Update()
    {
        /* float totalScore = gController.GetTotalScore();
            Vector3 scorePercentages = new Vector3(gController.rScore / totalScore,
                gController.gScore / totalScore, gController.bScore / totalScore);

            if (totalScore != 0.0f)
            {
                AdjustUIPanel(scoreBars[0], new Vector2(0.0f, screen.y - 10.0f), screen.x * scorePercentages.x);
                AdjustUIPanel(scoreBars[1], scoreBars[0], screen.x * scorePercentages.y);
                AdjustUIPanel(scoreBars[2], scoreBars[1], screen.x * scorePercentages.z);
            } */
        
    }

    private GameObject CreateUIPanel(string name, Color color, Rect position, Vector2 offset, Sprite sprite = null, Transform parent = null)
    {
        GameObject newPanel = new GameObject(name);
        newPanel.AddComponent<CanvasRenderer>();
        Image i = newPanel.AddComponent<Image>();
        if (sprite)
            i.sprite = sprite;
        i.color = color;

        RectTransform t = newPanel.GetComponent<RectTransform>();
        t.anchoredPosition = Vector2.zero;
        t.position = new Vector2(position.x, position.y) + offset;
        t.sizeDelta = new Vector2(position.width, position.height);

        if (parent != null)
            newPanel.transform.parent = parent;
        else newPanel.transform.parent = hudTarget.transform;

        return newPanel;
    }

    private void AdjustUIPanel(GameObject targetPanel, Vector2 newPosition, float newWidth)
    {
        RectTransform rTransform = targetPanel.GetComponent<RectTransform>();

        rTransform.position = newPosition + GetOffset(rTransform.rect);
        rTransform.sizeDelta = new Vector2(newWidth, rTransform.rect.height);
    }

    private void AdjustUIPanel(GameObject targetPanel, GameObject anchor, float newWidth)
    {
        RectTransform rTransform = targetPanel.GetComponent<RectTransform>();
        RectTransform aTransform = anchor.GetComponent<RectTransform>();

        rTransform.position = (Vector2)aTransform.position + GetOffset(rTransform.rect, true, false) + GetOffset(aTransform.rect, true, false);
        rTransform.sizeDelta = new Vector2(newWidth, rTransform.rect.height);
    }

    private GameObject CreateUIText(string text, Color color, float dimensions, Transform parent = null)
    {
        GameObject newText = new GameObject(text);
        newText.AddComponent<CanvasRenderer>();

        Text t = newText.AddComponent<Text>();
        t.text = text;
        t.font = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
        t.color = color;
        t.alignment = TextAnchor.MiddleCenter;

        RectTransform rT = newText.GetComponent<RectTransform>();
        rT.sizeDelta = new Vector2(dimensions, dimensions);
        t.resizeTextMaxSize = (int)maxScale / 2;
        t.resizeTextForBestFit = true;

        if (parent)
        {
            newText.transform.parent = parent;
            newText.transform.localPosition = Vector3.zero;
        }

        return newText;
    }

    private void AdjustScoreDisplay(int index, float value)
    {
        float scorePerc = value / gController.GetTotalScore();
        Vector2 newDimensions = new Vector2(maxScale * scorePerc, maxScale * scorePerc);
        
        scoreSprites[index].GetComponent<RectTransform>().sizeDelta = newDimensions;
        scoreText[index].GetComponent<RectTransform>().sizeDelta = newDimensions;
        scoreText[index].GetComponent<Text>().text = (scorePerc * 100.0f).ToString("0") + "%";
    }

    private Vector2 GetOffset(Rect r, bool x = true, bool y = true)
    {
        return new Vector2(x ? r.width / 2.0f : 0.0f, 
            y ? r.height / -2.0f : 0.0f);
    }
}
