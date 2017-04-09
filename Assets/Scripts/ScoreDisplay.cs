using System.Collections;
using System.Collections.Generic;
using SurviveTheNight;
using UnityEngine;

public class ScoreDisplay : MonoBehaviour
{
    public GameObject player;

    void OnGUI()
    {
        int w = Screen.width, h = Screen.height;

        GUIStyle style = new GUIStyle();

        Rect rect = new Rect(0, 0, w, h * 5 / 100);
        style.alignment = TextAnchor.UpperRight;
        style.fontSize = h * 5 / 100;
        style.normal.textColor = new Color(173.0f, 0.0f, 0.0f, 1.0f);
        style.padding.right = 10;
        Player playerScript = player.GetComponent<Player>();
        string score = playerScript.score.ToString();
        GUI.Label(rect, score, style);
    }
}