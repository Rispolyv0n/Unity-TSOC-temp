using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class sceneSwitch : MonoBehaviour
{
    
    public string SceneName; // if closing current scene, SceneName should be the name of the current scene
    public bool addScene;
    public bool switchScene;
    public bool closeScene;

    void Awake()
    {
        var btn = GetComponent<Button>();
        if (btn != null)
        {
            btn.onClick.AddListener(OnMouseDown);
        }
    }

    void OnMouseDown()
    {
        //SceneManager.LoadScene(SceneName,LoadSceneMode.Single);


        if (addScene == true) {
            SceneManager.LoadScene(SceneName,LoadSceneMode.Additive);
            //SceneManager.SetActiveScene(SceneManager.GetSceneByName(SceneName));
        }
        else if (switchScene == true) {
            SceneManager.LoadScene(SceneName,LoadSceneMode.Single);
        }
        else if (closeScene == true) {
            //string theSceneName = SceneManager.GetActiveScene().name;
            SceneManager.UnloadScene(SceneName);
        }
    }
}