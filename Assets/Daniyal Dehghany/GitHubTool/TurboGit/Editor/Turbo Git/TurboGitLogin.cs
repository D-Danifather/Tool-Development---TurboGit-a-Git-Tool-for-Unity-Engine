using UnityEngine;
using UnityEditor;
using Daniyal_Dehghany_GitHubTool_TurboGit.Git;
using Daniyal_Dehghany_GitHubTool_TurboGit.Utillities;

namespace Daniyal_Dehghany_GitHubTool_TurboGit
{
    public class TurboGitLogin : EditorWindow
    {

        #region Public

        public static string username = string.Empty;
        public static string eMail = string.Empty;
        public static string password = string.Empty;
        public static string remote = string.Empty;

        public static bool globalCheck = false;

        #endregion

        #region Private

        #region Instance & Definition

        private static TurboGitLogin Instance;

        // Textures for button icons
        private static Texture2D texture2D_TurboIcon;
        private static Texture2D texture2D_SignInButton;
        private static Texture2D texture2D_ResetButton;
        private static Texture2D texture2D_SignUpButton;
        private static Texture2D texture2D_ExitButton;
        private static Texture2D texture2D_Download_Button;

        #endregion

        #region Read only members

        private static readonly string GITHUB_SIGNUP_LINK = "https://github.com/signup?ref_cta=Sign+up&ref_loc=header+logged+out&ref_page=%2F&source=header-home";
        private static readonly string GIT_DOWNLOAD_LINK = "https://git-scm.com/downloads";

        // Editorprefs keys --> For saving the user data
        private static readonly string TAB_TEXT = "Turbo Git Login";
        private static readonly string TAB_TOOL_TIPP = "Settings for Turbo Git Login";
        private static readonly string USER_USERNAME = "USER_USERNAME";
        private static readonly string USER_EMAIL = "USER_EMAIL";
        private static readonly string USER_PASSWORD = "USER_PASSWORD";
        private static readonly string GIT_REMOTE = "GIT_REMOTE";
        private static readonly string GIT_LOCAL_GLOBAL = "GIT_LOCAL_GLOBAL";

        private static readonly Vector2 MIN_MAX_SIZE = new Vector2(500f, 495f);

        #endregion

        #endregion

        #region Create login window

        public static void CallLoginWindow()
        {
            // Show existing window instance --> If one doesn't exist, make one
            Instance = (TurboGitLogin)GetWindow(typeof(TurboGitLogin));

            // Add Texture --> Icon (Logo) 
            texture2D_TurboIcon = (Texture2D)Resources.Load("Turbo_Icon");

            // Set Tab Informations such as Text | Texture2D --> Logo | Tooltip 
            Instance.titleContent = new GUIContent(TAB_TEXT, texture2D_TurboIcon, TAB_TOOL_TIPP);

            // Set the minimum and maximum size of the window to a fix size
            Instance.minSize = MIN_MAX_SIZE;
            //Instance.maxSize = Instance.minSize;

            Instance.Show();
        }

        #endregion

        private void OnEnable()
            => GetUserData();

        private void OnGUI()
            => DrawLoginMenu();

        #region Draw Menu

        private static void DrawLoginMenu()
        {
            GUI.color = Color.white;

            GUILayout.Label("Login", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            #region User Inputs and Informations

            // Login Informations
            EditorGUILayout.HelpBox("To start using Turbo Git, you must enter your credentials to identify yourself as the author of your work. If your not sure what to do, go with the Mouse Pointer on following Lables for more Informations. (Every Buttons and Labels have Informations and Tipps!)", MessageType.Info);
            EditorGUILayout.Space();

            // Username | E-Mail | Password | Remote
            username = EditorGUILayout.TextField(new GUIContent("Username", "Enter your GitHub Username. \nEverything has to match exactly!"), username);
            EditorGUILayout.Space();

            eMail = EditorGUILayout.TextField(new GUIContent("E-Mail", "Enter the same E-Mail, which is registered on your GitHub Account."), eMail);
            EditorGUILayout.Space();

            password = EditorGUILayout.PasswordField(new GUIContent("Password:", "Enter the same Password, which is registered on your GitHub Account"), password);
            EditorGUILayout.Space();

            remote = EditorGUILayout.TextField(new GUIContent("Remote (HTTPS)", "Enter your Remote (Repository). \n" +
                "If you dont know how to create one follow these rules: --> \n" +
                "Go to your GitHub. \n" +
                "Log in to your Account. \n" +
                "Click the Repositories New button in the top-right. You’ll have an option there to initialize the repository with a README file, but you dont have to do it (Its just a normal text/noting file for yourself). \n" +
                "Click the “Create repository” button. \n" +
                "Now, you will see your HTTPS and SSH. \n" +
                "Turbo Git works only with 'HTTPS'! Select your 'HTTPS' and go on!"), remote);
            EditorGUILayout.Space();

            // Check for global or local repository! + Global toggle informations
            GUILayout.Label(new GUIContent("Use global Git", "Activate this check box if you want to use your git globaly or deactivate it for use git localy. By default it will always be local! \n" +
                "The global option tells Git to always use this information for anything you do on your system. If you use local, the configuration applies only to the current repository."), EditorStyles.label);
            //globalCheck = GUI.Toggle(new Rect(155, 163, 12, 12), globalCheck, string.Empty);
            globalCheck = GUI.Toggle(new Rect(155, 192, 12, 12), globalCheck, string.Empty);
            EditorGUILayout.Space();

            // Login Informations
            EditorGUILayout.HelpBox("If you dont have a GitHub Account yet, Click below hier on Sign up button to create one! \n" +
                "Make sure that the latest Git is installed on your PC before you sign in. \n" +
                "If you dont have Git yet, Click below hier on Download Git button to download the latest Git!", MessageType.Warning);
            EditorGUILayout.Space();

            #endregion

            #region Sign in Button

            // Login Button
            texture2D_SignInButton = (Texture2D)Resources.Load("Turbo_SignIn");
            bool signIn = GUILayout.Button(new GUIContent("  Sign in", texture2D_SignInButton, "Creates a connection between Turbo Git Tool and your Git Hub using your datas!"));
            if (signIn)
            {
                if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(eMail) && !string.IsNullOrEmpty(password) && !string.IsNullOrEmpty(remote))
                {
                    SetUserData();

                    if (globalCheck)
                    {
                        ProcessClass.GitCommand(Enum_Git_Commands.GLOBAL_USERNAME);
                        ProcessClass.GitCommand(Enum_Git_Commands.GLOBAL_EMAIL);
                    }
                    else
                    {
                        ProcessClass.GitCommand(Enum_Git_Commands.LOCAL_USERNAME);
                        ProcessClass.GitCommand(Enum_Git_Commands.LOCAL_EMAIL);
                    }

                    // Close the actual window (Login)
                    Instance.Close();

                    // Open the Account window
                    TurboGitAccount.ShowAccountWindow();
                }
                else
                    EditorUtility.DisplayDialog("Error", "Username, E-Mail, Password and Remote must be available! \nCorrect it and try again...", "Exit");

            }
            EditorGUILayout.Space();

            #endregion

            #region Reset Button

            // Reset Button
            texture2D_ResetButton = (Texture2D)Resources.Load("Turbo_Reset");
            bool reset = GUILayout.Button(new GUIContent("  Reset", texture2D_ResetButton, "Resets everything back to default!"));
            if (reset)
            {
                username = string.Empty;
                eMail = string.Empty;
                password = string.Empty;
                remote = string.Empty;

                globalCheck = false;

                // Deletes all existing keys for new data!
                DeleteUserData();
            }
            EditorGUILayout.Space();

            #endregion

            #region Sign up Button

            // Sign up Button
            texture2D_SignUpButton = (Texture2D)Resources.Load("Turbo_SignUp");
            bool signUp = GUILayout.Button(new GUIContent("  Sign up", texture2D_SignUpButton, "Redirect you to GitHub website to create a new account!"));
            if (signUp)
                Application.OpenURL(GITHUB_SIGNUP_LINK);
            EditorGUILayout.Space();

            #endregion

            #region Download Git Button

            // Download Git 
            texture2D_Download_Button = (Texture2D)Resources.Load("Turbo_Download");
            bool download = GUILayout.Button(new GUIContent("  Download Git", texture2D_Download_Button, "Redirect you to Git website to download the latest Git version!"));
            if (download)
                Application.OpenURL(GIT_DOWNLOAD_LINK);
            EditorGUILayout.Space();

            #endregion

            #region Exit Button

            // Exit / Close Turbo Git
            texture2D_ExitButton = (Texture2D)Resources.Load("Turbo_Close");
            bool exit = GUILayout.Button(new GUIContent("  Close", texture2D_ExitButton, "Close the Turbo Git Tool"));
            if (exit)
                Instance.Close();
            EditorGUILayout.Space();

            #endregion
        }

        #endregion

        // Set user data
        private static void SetUserData()
        {
            EditorPrefs.SetString(USER_USERNAME, username);

            EditorPrefs.SetString(USER_EMAIL, eMail);

            EditorPrefs.SetString(GIT_REMOTE, remote);

            EditorPrefs.SetString(USER_PASSWORD, CodeUtills.Base64Encode(password));

            EditorPrefs.SetBool(GIT_LOCAL_GLOBAL, globalCheck);
        }

        // Get user data
        private static void GetUserData()
        {
            if (EditorPrefs.HasKey(USER_USERNAME))
                username = EditorPrefs.GetString(USER_USERNAME);

            if (EditorPrefs.HasKey(USER_EMAIL))
                eMail = EditorPrefs.GetString(USER_EMAIL);

            if (EditorPrefs.HasKey(USER_PASSWORD))
                password = CodeUtills.Base64Decode(EditorPrefs.GetString(USER_PASSWORD));

            if (EditorPrefs.HasKey(GIT_REMOTE))
                remote = EditorPrefs.GetString(GIT_REMOTE);

            if (EditorPrefs.GetBool(GIT_LOCAL_GLOBAL))
                globalCheck = EditorPrefs.GetBool(GIT_LOCAL_GLOBAL);
        }

        // Delete user data
        private static void DeleteUserData()
        {
            if (EditorPrefs.HasKey(USER_USERNAME))
                EditorPrefs.DeleteKey(USER_USERNAME);

            if (EditorPrefs.HasKey(USER_EMAIL))
                EditorPrefs.DeleteKey(USER_EMAIL);

            if (EditorPrefs.HasKey(USER_PASSWORD))
                EditorPrefs.DeleteKey(USER_PASSWORD);

            if (EditorPrefs.HasKey(GIT_REMOTE))
                EditorPrefs.DeleteKey(GIT_REMOTE);

            if (EditorPrefs.GetBool(GIT_LOCAL_GLOBAL))
                EditorPrefs.DeleteKey(GIT_LOCAL_GLOBAL);
        }

    }
}