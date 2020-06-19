using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(LevelsManager))]
public class LevelManagerEditor : Editor
{
    public override VisualElement CreateInspectorGUI()
    {
        return base.CreateInspectorGUI();
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Refresh Folder"))
        {
            ReloadFolder();
        }

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("<<Previous"))
        {
            ShowPrevious();
        }
        GUILayout.Label(" INDEX: "+ ((LevelsManager)target).currentLevelIndex);
        if (GUILayout.Button("Next>>"))
        {
            ShowNext();
        }

        GUILayout.EndHorizontal();
        if(GUILayout.Button("SELECT")){
            Debug.Log("LEVEL EXPORTED***");
            MarkSelected();
        }

       
        if (GUILayout.Button("DELETE"))
        {
            Debug.Log("LEVEL EXPORTED***");
           
        }
    }

    void ShowNext()
    {
        LevelsManager levelsManager = (LevelsManager)target;
        levelsManager.ShowNext();
    }

    void ShowPrevious()
    {
        LevelsManager levelsManager = (LevelsManager)target;
        levelsManager.ShowPrevious();
    }

    void MarkSelected()
    {
        LevelsManager levelsManager = (LevelsManager)target;
        levelsManager.MarkSelected();
    }

    void DeleteLevel()
    {
        LevelsManager levelsManager = (LevelsManager)target;
        levelsManager.DeleteLevel();
    }

    void ReloadFolder()
    {
        LevelsManager levelsManager = (LevelsManager)target;
        levelsManager.ReloadFolder();
    }
}
