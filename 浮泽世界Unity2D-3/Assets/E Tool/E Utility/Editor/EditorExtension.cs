using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SelectAllOfTag : ScriptableWizard
{
    public string searchTag = "Player";

    [MenuItem("Tools/E Utility/选择指定标签的所有对象")]
    public static void SelectAllOfTagWizard()
    {
        DisplayWizard<SelectAllOfTag>("选择指定标签的所有对象", "选择");
    }

    private void OnWizardCreate()
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag(searchTag);
        Selection.objects = gameObjects;
    }
}