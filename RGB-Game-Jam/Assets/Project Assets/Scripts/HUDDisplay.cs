using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(GameController))]
public class HUDDisplay : MonoBehaviour
{
    public Text uiTimerText;
    private GameController gController;

    void Start()
    {
        gController = GetComponent<GameController>();
    }

    void Update()
    {
        if (uiTimerText)
        {
            int minutes = (int)(gController.remainingTime / 60.0f);
            int seconds = (int)(gController.remainingTime - (minutes * 60.0f));

            if (gController.remainingTime > 0.0f)
                uiTimerText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
            else uiTimerText.text = "Time's up!";
        }
    }
}
