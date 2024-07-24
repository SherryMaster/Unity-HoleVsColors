using UnityEngine;
using UnityEngine.SceneManagement;

public class UnderGroundCollision : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!Game.isGameOver)
        {
            string tag = other.tag;
            if (tag.Equals("Object"))
            {
                Level.Instance.objectsInScene--;
                UIManager.Instance.UpdateLevelProgress();
                Destroy(other.gameObject);

                if (Level.Instance.objectsInScene == 0)
                {
                    Level.Instance.LoadNextLevel();
                }
            }
            if (tag.Equals("Obstacle"))
            {
                Game.isGameOver = true;
                Level.Instance.RestartLevel();
            }
        }
        
    }
}
