using UnityEditor;
using UnityEngine;

/// <summary>
/// Editor Klasse, fügt dem Inspector in Unity einen Button hinzu, mit dem man manuell
/// das Starten des Lesevorgangs initialisieren kann.
/// </summary>
[CustomEditor(typeof(ValuePrinter))]
public class ValuePrinterEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        ValuePrinter script = (ValuePrinter) target;
        if (GUILayout.Button("Start Reading"))
        {
            script.StartValueRead();
        }

    }
}