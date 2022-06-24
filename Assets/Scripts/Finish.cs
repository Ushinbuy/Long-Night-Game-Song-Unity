using UnityEngine;
using UnityEngine.SceneManagement;

public class Finish : MonoBehaviour
{
    void LoadLevel()
    {
        // it function called by animation
        Scene currentScene;
        int numOfSetupScene = 0;
        currentScene = SceneManager.GetActiveScene();
        if (currentScene.buildIndex < 3)
        {
            numOfSetupScene = currentScene.buildIndex + 1;
        }
        SceneManager.LoadScene(numOfSetupScene);
        //PlayerPrefs.SetInt("current score", ScoreManager.score);
    }

    private void SetScore()
    {
        FinishChouse.finishAnimationState = FinishChouse.FinishAnimationState.SET_SCORE;
    }

    private void FinishAnimation()
    {
        FinishChouse.finishAnimationState = FinishChouse.FinishAnimationState.STOP_PLAY;
    }

    private void DestroyThisObject()
    {
        Destroy(gameObject);
    }
}
