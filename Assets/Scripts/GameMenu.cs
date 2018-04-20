using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour {

    public TextMeshProUGUI timeText;
    public TextMeshProUGUI btnText;
    public Button gameBtn;

    [Header("End")]
    public GameObject endGame;
    public TextMeshProUGUI endText;

    public void UpdateTime(float time) {
        timeText.text = "Time: " + time;
    }

    public void ChangeGameBtn(string text) {
        btnText.text = text;
        if (text == "Pause") {
            var click = new Button.ButtonClickedEvent();
            click.AddListener(new UnityEngine.Events.UnityAction(Game.Instance.PauseGame));
            gameBtn.onClick = click;
        }
        if (text == "Play") {
            var click = new Button.ButtonClickedEvent();
            click.AddListener(new UnityEngine.Events.UnityAction(Game.Instance.PlayGame));
            gameBtn.onClick = click;
        }
    }

    public void EndGame(string text) {
        endGame.SetActive(true);

        endText.text = text;
    }
}
