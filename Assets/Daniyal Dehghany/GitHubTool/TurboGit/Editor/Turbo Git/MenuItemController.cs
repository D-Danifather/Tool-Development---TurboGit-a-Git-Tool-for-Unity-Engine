using UnityEditor;

namespace Daniyal_Dehghany_GitHubTool_TurboGit
{
    public class MenuItemController : EditorWindow
    {
        // Add menu item named "Turbo Git" to the Window menu
        [MenuItem("Tools/Daniyal Dehghany/GitHub Tool/ Turbo Git %#g")]
        private static void ShowLoginWindow()
            => TurboGitLogin.CallLoginWindow();
    }
}
