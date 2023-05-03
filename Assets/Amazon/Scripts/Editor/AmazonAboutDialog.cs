using UnityEditor;
using UnityEngine;
using AmazonAds;
public class AmazonAboutDialog : ScriptableWizard {
    public static void ShowDialog () {
        DisplayWizard<AmazonAboutDialog> (AmazonConstants.titleAboutDialog, AmazonConstants.aboutDialogOk);
    }

    protected override bool DrawWizardGUI () {
        EditorGUILayout.LabelField (AmazonConstants.labelSdkVersion);

        EditorGUILayout.Space ();
        if (GUILayout.Button (AmazonConstants.buttonReleaseNotes))
            Application.OpenURL (AmazonConstants.RELEASE_NOTES_URL); 
        EditorGUILayout.LabelField ("\n");
        EditorGUILayout.LabelField (AmazonConstants.labelReportIssues);
        return false;
    }

    private void OnWizardCreate () { }
}