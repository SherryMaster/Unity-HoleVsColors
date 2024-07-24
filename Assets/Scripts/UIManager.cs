using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    #region Singleton

    public static UIManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    #endregion

    [Header("LevelProgressUI")]
    [SerializeField] int scene_offset = 1;
    [SerializeField] TMP_Text nextLevelText;
    [SerializeField] TMP_Text currentLevelText;
    [SerializeField] Image progressFill;

    // Start is called before the first frame update
    void Start()
    {
        progressFill.fillAmount = 0f;
        setLevelProgressText();
    }

    void setLevelProgressText()
    {
        int level = SceneManager.GetActiveScene().buildIndex + scene_offset;
        currentLevelText.text = level.ToString();
        nextLevelText.text = (level+1).ToString();
    }

    public void UpdateLevelProgress()
    {
        float val = 1f - ((float)Level.Instance.objectsInScene / Level.Instance.totalObjects);
        progressFill.DOFillAmount(val, 0.5f);
    }
}
