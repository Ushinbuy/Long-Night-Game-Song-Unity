using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartScreenButton : MonoBehaviour
{
    [SerializeField] private Button startGameButton, passIntroButton;
    [SerializeField] private Animator animator;

    void Start()
    {
        CanvasMeneger.isStartPassed = false;
        startGameButton.onClick.AddListener(StartMovie);
        passIntroButton.onClick.AddListener(LoadLevelAndPass);
        passIntroButton.gameObject.SetActive(false);
    }

    void StartMovie()
    {
        startGameButton.gameObject.SetActive(false);
        animator.SetTrigger("start_movie");
        passIntroButton.gameObject.SetActive(true);

        BackGroundManager.StartLevel();
        ScoreManager.ResetScores();
    }

    void LoadLevel()
    {
        SceneManager.LoadScene("Level_1");
    }

    void LoadLevelAndPass()
    {
        CanvasMeneger.isStartPassed = true;
        SceneManager.LoadScene("Level_1");
    }
}
