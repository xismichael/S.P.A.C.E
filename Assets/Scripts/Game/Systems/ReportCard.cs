using TMPro;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReportCard : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private List<Sprite> gradeSprites = new List<Sprite>(13);
    [SerializeField] private Image uiImage;
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private GameObject replayScreen;
    private static readonly float[] CutoffsDesc = {
        97f, 94f, 90f, 88f, 83f, 80f, 77f, 75f, 72f, 65f, 60f, 50f, float.NegativeInfinity
    };
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ShowScore(float score)
    {
        int idx = ScoreToIndex(score);
        Sprite s = gradeSprites[Mathf.Clamp(idx, 0, gradeSprites.Count - 1)];
        uiImage.sprite = s;

    }

    private int ScoreToIndex(float score)
    {
        for (int i = 0; i < CutoffsDesc.Length; i++)
            if (score >= CutoffsDesc[i]) return i;

        return CutoffsDesc.Length - 1; // should never hit (last is -Infinity)
    }

    public void ShowByIndex(int index)
    {
        if (gradeSprites == null || gradeSprites.Count < 13) return;
        int i = Mathf.Clamp(index, 0, gradeSprites.Count - 1);
        uiImage.sprite = gradeSprites[i];
    }

    public void Show()
    {
        replayScreen.SetActive(true);
    }
    public void MatchMade(string message)
    {
        messageText.text += message;
        messageText.text += "\n";
    }

    public void ClearMessages()
    {
        messageText.text = "";
    }
}
