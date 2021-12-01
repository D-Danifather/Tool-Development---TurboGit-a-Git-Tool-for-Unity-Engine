using UnityEngine;
using UnityEditor;

namespace Daniyal_Dehghany_GitHubTool_TurboGit
{
    public class CallPopupWindow : EditorWindow
    {
        private static string error;
        private static string button;

        private static Color guiColor;

        private static Vector2 scrollPos;

        private static bool popupState = false;

        /// <summary>
        /// I wanted to use a popup window for each of my actions --> But i found Unity-Dialogs, which is better performater to use!
        /// </summary>
        /// <param name="_error"></param>
        /// <param name="_button"></param>
        /// <param name="_guiColor"></param>
        public static void CallPopup(string _error, string _button, Color _guiColor)
        {
            CallPopupWindow window = ScriptableObject.CreateInstance<CallPopupWindow>();
            window.position = new Rect(Screen.width - (Screen.width / 2), Screen.height - (Screen.height / 2), 500, 350);
            error = _error;
            button = _button;
            guiColor = _guiColor;

            // Here we check if one popup is steal active --> if yes we wont create a new one until its closed!
            // If other popups are going to be created it will be destroyed!
            if (!popupState)
            {
                // Checks if the error string is empty --> then returns new string
                if (_error == null || _error == string.Empty)
                    error = " Something went wrong... \n Please try again!";

                window.ShowPopup();
                popupState = true;
            }
        }

        void OnGUI()
        {
            EditorGUILayout.Space(15f);

            EditorGUILayout.BeginHorizontal();
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(500), GUILayout.Height(300));
            GUI.color = guiColor;
            GUILayout.Label(error);
            GUI.color = Color.white;
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(5f);

            if (GUILayout.Button(button))
            {
                Close();
                popupState = false;
            }
        }
    }

}
