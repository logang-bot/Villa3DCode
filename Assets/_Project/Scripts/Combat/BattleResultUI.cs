using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleResultUI : MonoBehaviour
{
    [SerializeField] GameObject panel;
    [SerializeField] TextMeshProUGUI label;
    [SerializeField] float displaySeconds = 2.5f;
    [SerializeField] string hubSceneName = "Hub_Zone01";

    public void ShowResult(bool won)
    {
        panel.SetActive(true);
        label.text = won ? "Victory!" : "Defeat...";
        StartCoroutine(ReturnAfterDelay());
    }

    IEnumerator ReturnAfterDelay()
    {
        yield return new WaitForSeconds(displaySeconds);
        SceneManager.LoadScene(hubSceneName);
    }
}
