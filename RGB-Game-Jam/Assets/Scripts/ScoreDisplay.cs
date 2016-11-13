using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreDisplay : MonoBehaviour
{
    public Canvas hudTarget;
    
    private GameObject[] scoreBars;
    private Vector2 screen;
    private GameController gController;

    void Start()
    {
        screen = new Vector2(Screen.width, Screen.height);

        Rect barRect = new Rect(0.0f, screen.y, screen.x / 3.0f, 10.0f);
        Vector2 offset = new Vector2(barRect.width / 2.0f, barRect.height / -2.0f);
        scoreBars = new GameObject[3];
        scoreBars[0] = CreateUIPanel("rScore", Color.red, barRect, offset);
        scoreBars[1] = CreateUIPanel("gScore", Color.green, barRect, offset + new Vector2(barRect.width, 0.0f));
        scoreBars[2] = CreateUIPanel("bScore", Color.blue, barRect, offset + new Vector2(barRect.width * 2.0f, 0.0f));

        gController = GetComponent<GameController>();
    }

    void Update()
    {
        float totalScore = gController.GetTotalScore();
        Vector3 scorePercentages = new Vector3(gController.rScore / totalScore,
            gController.gScore / totalScore, gController.bScore / totalScore);

        if (totalScore != 0.0f)
        {
            AdjustUIPanel(scoreBars[0], new Vector2(0.0f, screen.y - 10.0f), screen.x * scorePercentages.x);
            AdjustUIPanel(scoreBars[1], scoreBars[0], screen.x * scorePercentages.y);
            AdjustUIPanel(scoreBars[2], scoreBars[1], screen.x * scorePercentages.z);
        }
    }

    private GameObject CreateUIPanel(string name, Color color, Rect position, Vector2 offset, Transform parent = null)
    {
        GameObject newPanel = new GameObject(name);
        newPanel.AddComponent<CanvasRenderer>();
        Image i = newPanel.AddComponent<Image>();
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

    private Vector2 GetOffset(Rect r, bool x = true, bool y = true)
    {
        return new Vector2(x ? r.width / 2.0f : 0.0f, 
            y ? r.height / -2.0f : 0.0f);
    }
}
