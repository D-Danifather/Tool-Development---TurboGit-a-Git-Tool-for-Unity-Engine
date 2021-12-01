using System;
using UnityEngine;
using UnityEditor;
using Daniyal_Dehghany_GitHubTool_TurboGit.Serialized;
using Daniyal_Dehghany_GitHubTool_TurboGit.Utillities;
using Daniyal_Dehghany_GitHubTool_TurboGit.Git;

namespace Daniyal_Dehghany_GitHubTool_TurboGit
{
    public class TurboGitAccount : EditorWindow
    {
        public static string comment;

        #region Private

        #region Tool Bar Enum Instance & Definition

        private static ToolBar_Enum ToolBar_Enum;

        private static TurboGitAccount Instance;

        private static Texture2D texture2D_TurboIcon;
        private static Texture2D texture2D_VersionButton;
        private static Texture2D texture2D_UpdateButton;
        private static Texture2D texture2D_SignOutButton;
        private static Texture2D texture2D_ResetButton;
        private static Texture2D texture2D_InitButton;
        private static Texture2D texture2D_AddButton;
        private static Texture2D texture2D_CommitButton;
        private static Texture2D texture2D_PushButton;
        private static Texture2D texture2D_PullButton;
        private static Texture2D texture2D_CloneButton;

        private static GUIStyle style;

        private static Color backgroundColor = new Color(0.1f, 0.1f, 0.1f);
        private static Color previousBackgroundColor;
        private static Color textColor = Color.cyan;
        private static Color previousTextColor;

        private static DateTime lastRegistrationTime;
        private static DateTime previousLastRegistrationTime;

        private static Vector2 scrollPosStatus;
        private static Vector2 scrollPosHistory;

        private static readonly Vector2 MIN_MAX_SIZE = new Vector2(550f, 470f);

        #endregion

        #region Read only members

        private readonly static string[] toolbarStrings = new string[] { "Account", "Status", "Git", "History" };

        private static readonly string BACKGROUND_COLOR_KEY_R = "BACKGROUND_COLOR_KEY_R";
        private static readonly string BACKGROUND_COLOR_KEY_G = "BACKGROUND_COLOR_KEY_G";
        private static readonly string BACKGROUND_COLOR_KEY_B = "BACKGROUND_COLOR_KEY_B";
        private static readonly string BACKGROUND_COLOR_KEY_A = "BACKGROUND_COLOR_KEY_A";
        private static readonly string TEXT_COLOR_KEY_R = "TEXT_COLOR_KEY_R";
        private static readonly string TEXT_COLOR_KEY_G = "TEXT_COLOR_KEY_G";
        private static readonly string TEXT_COLOR_KEY_B = "TEXT_COLOR_KEY_B";
        private static readonly string TEXT_COLOR_KEY_A = "TEXT_COLOR_KEY_A";
        private static readonly string BOLD_FONT_KEY = "BOLD_FONT_KEY";
        private static readonly string LAST_REGISTRATION_KEY = "LAST_REGISTRATION_KEY";

        private static readonly string TAB_TEXT = "Turbo Git Account";
        private static readonly string TAB_TOOL_TIPP = "Settings for Turbo Git Account";

        #endregion

        private static int toolbarInt;

        // Automaticly update Status & History each x seconds! --> To gain more performance --> without it the tab will be slowly!
        private static float update_Tabs_Eversy_X_Seconds;
        private static float _X_ = 5f;

        private static string scrollStringStatus;
        private static string scrollStringHistory;

        // Check if Bold Font is used for Status and History Text
        private static bool isBoldFont = false;
        private static bool previosIsBoldFont;

        #endregion

        #region Create account window

        public static void ShowAccountWindow()
        {
            // Set the tab to Account alwayas by start
            toolbarInt = 0;

            ToolBar_Enum = (ToolBar_Enum)toolbarInt;

            // Set minimum size of the window
            Instance = (TurboGitAccount)GetWindow(typeof(TurboGitAccount));

            // Add Texture --> Icon (Logo) 
            texture2D_TurboIcon = (Texture2D)Resources.Load("Turbo_Icon");
            // Set Tab Informations such as Text | Texture2D --> Logo | Tooltip 
            Instance.titleContent = new GUIContent(TAB_TEXT, texture2D_TurboIcon, TAB_TOOL_TIPP);

            // Set the minimum and maximum size of the window to a fix size
            Instance.minSize = MIN_MAX_SIZE;
            //Instance.maxSize = Instance.minSize;

            //Show existing window instance --> If one doesn't exist, make one
            Instance.Show();
        }

        #endregion

        private void OnEnable()
            => GetAccountData();

        private void OnGUI()
            => DrawAccountMenu();

        private void Update()        
            // Update Status & History tabs every x seconds! 
            => update_Tabs_Eversy_X_Seconds -= Time.deltaTime;

        #region Draw Account

        private void DrawAccountMenu()
        {
            GUI.color = Color.white;

            // Gui.Toolbar doesnt correct the position with editing window height and width!
            //toolbarInt = GUI.Toolbar(new Rect(10, 10, 475, 30), toolbarInt, toolbarStrings);
            toolbarInt = GUILayout.Toolbar(toolbarInt, toolbarStrings);

            // Checks if isBoldFont is activated or not then sets it
            style = isBoldFont ? EditorStyles.boldLabel : EditorStyles.label;

            EditorGUILayout.Space();
            switch ((ToolBar_Enum)toolbarInt)
            {
                case ToolBar_Enum.Account:
                    AccountBar();
                    break;
                case ToolBar_Enum.Status:
                    StatusBar();
                    break;
                case ToolBar_Enum.Git:
                    GitBar();
                    break;
                case ToolBar_Enum.History:
                    HistoryBar();
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region Account Bar

        private void AccountBar()
        {
            GUILayout.Label("Account", EditorStyles.boldLabel);

            #region User Informations

            // Username
            EditorGUILayout.Space();
            GUILayout.Label("Username:", EditorStyles.label);
            GUI.Label(new Rect(155, 56, int.MaxValue, 15), SerializeFiles.UserInformations.User_Username, EditorStyles.label);

            // E-Mail
            EditorGUILayout.Space();
            GUILayout.Label("E-Mail:", EditorStyles.label);
            GUI.Label(new Rect(155, 81, int.MaxValue, 15), SerializeFiles.UserInformations.User_E_Mail, EditorStyles.label);

            // Password
            EditorGUILayout.Space();
            GUILayout.Label("Password:", EditorStyles.label);
            GUI.Label(new Rect(155, 108, int.MaxValue, 15), CodeUtills.PasswordAsStarsString(SerializeFiles.UserInformations.User_Password), EditorStyles.label);

            // Remote
            EditorGUILayout.Space();
            GUILayout.Label("Remote:", EditorStyles.label);
            GUI.Label(new Rect(155, 131, int.MaxValue, 15), SerializeFiles.UserInformations.GIT_Remote, EditorStyles.label);

            // Globaly Git
            EditorGUILayout.Space();
            GUILayout.Label("Use global Git:", EditorStyles.label);
            // Check if the toggle buttom is on or off and show it to user on account
            string toggleString = SerializeFiles.UserInformations.User_Global_Git ? "Yes" : "No";
            GUI.Label(new Rect(155, 156, int.MaxValue, 15), toggleString, EditorStyles.label);

            // Last Time used Turbo Git
            EditorGUILayout.Space();
            GUILayout.Label("Last registration:", EditorStyles.label);
            GUI.Label(new Rect(155, 181, int.MaxValue, 15), lastRegistrationTime.ToString(), EditorStyles.label);

            #endregion

            #region Data change

            // Set Background Color
            EditorGUILayout.Space();
            backgroundColor = EditorGUILayout.ColorField(new GUIContent("Background Color", "Set your Background Color to any color you did like to use for Status & History!"), backgroundColor);

            // Set Text Color
            EditorGUILayout.Space();
            textColor = EditorGUILayout.ColorField(new GUIContent("Text Color", "Set your Text Color to any color you did like to use for Status & History!"), textColor);

            // Change between normal and bold font
            EditorGUILayout.Space();
            GUILayout.Label(new GUIContent("Activate Bold Font", "If you activate using Bold Font, it only means for the Output Text in Status and History tabs"), EditorStyles.label);
            isBoldFont = GUI.Toggle(new Rect(155, 263, 12, 12), isBoldFont, string.Empty);
            EditorGUILayout.Space();

            #endregion

            #region Version Button

            texture2D_VersionButton = (Texture2D)Resources.Load("Turbo_Git_Logo");
            bool version = GUILayout.Button(new GUIContent("  Git Version", texture2D_VersionButton, "Gets the installed Git Version on your Computer."));
            if (version)
            {
                ProcessClass.GitCommand(Enum_Git_Commands.Version);

                if (ProcessClass.codeSuccessfull)
                    EditorUtility.DisplayDialog("Git Version", ProcessClass.OUTPUT, "Close");
                else
                    EditorUtility.DisplayDialog("Error", ProcessClass.ERROR, "Close");
            }
            EditorGUILayout.Space();

            #endregion

            #region Update Button

            texture2D_UpdateButton = (Texture2D)Resources.Load("Turbo_Update");
            bool update = GUILayout.Button(new GUIContent("  Check for Update", texture2D_UpdateButton, "Checks, if your Git is up to date or needs any update. \n" +
                "If updates are available you can easily choose to download and install it or let it be directly in Unity!"));
            if (update)
            {
                ProcessClass.GitCommand(Enum_Git_Commands.UPDATE_GIT);

                ///Whate if update is available! --> For now it only reminds you, if a update is available or not! If it is, you need 
                ///to update your git manualy!
                if (ProcessClass.codeSuccessfull)
                    EditorUtility.DisplayDialog("Git Update", ProcessClass.OUTPUT, "Close");
                else
                    EditorUtility.DisplayDialog("Error", ProcessClass.ERROR, "Close");
            }
            EditorGUILayout.Space();

            #endregion

            #region Reset Button

            // Reset Button
            texture2D_ResetButton = (Texture2D)Resources.Load("Turbo_Reset");
            bool reset = GUILayout.Button(new GUIContent("  Reset", texture2D_ResetButton, "Resets everything (Colors & Text Font) back to default!"));
            if (reset)
            {
                backgroundColor = new Color(0.1f, 0.1f, 0.1f);
                textColor = Color.cyan;
                isBoldFont = false;
            }
            EditorGUILayout.Space();

            #endregion

            #region Sign out Button

            texture2D_SignOutButton = (Texture2D)Resources.Load("Turbo_SignOut");
            bool signOut = GUILayout.Button(new GUIContent("  Sign out", texture2D_SignOutButton, "It will Sign you out from Git and GitHub and close your connection for now. \n" +
                "Its recommended to always Sign out if your finish with Unity an Git \n" +
                "If you Sign in again it will automaticly create a connection to your Git and GitHub. \n" +
                "All your datas will be safe and you can start using Turbo Git without any problems, where you left off! Nothing will be lost! \n" +
                "If you don't Sign out all of your customasation will get lost! It only saves it if you press Sign out button."));
            if (signOut && EditorUtility.DisplayDialog("Git Sign out", "Are you sure to Sign out from Turbo Git? \n" +
                "Everything will be saved in Turbo Git as it is!", "Yes", "No"))
            {
                EditorUtility.DisplayDialog("Git sign out", "Turbo Git successfully signed out! \n" + CodeUtills.GetCurrentDateTime(), "Close");

                SignOut();
            }

            #endregion
        }

        #endregion

        #region Status Bar

        private void StatusBar()
        {
            GUILayout.Label("Status", EditorStyles.boldLabel);

            EditorGUILayout.Space();
            EditorGUILayout.BeginVertical();
            scrollPosStatus = EditorGUILayout.BeginScrollView(scrollPosStatus, GUILayout.Width(position.width), GUILayout.Height(position.height - 55));

            CodeUtills.OnDrawTextur(new Rect(0, 0, maxSize.x, maxSize.y), InitializeTexture2D(backgroundColor), ScaleMode.ScaleToFit);

            GUI.color = textColor;

            if (update_Tabs_Eversy_X_Seconds <= 0)
            {
                ProcessClass.GitCommand(Enum_Git_Commands.STATUS);

                if (ProcessClass.codeSuccessfull)
                    scrollStringStatus = ProcessClass.OUTPUT;
                else
                    scrollStringStatus = ProcessClass.ERROR;

                update_Tabs_Eversy_X_Seconds = _X_;
            }

            GUILayout.Label(scrollStringStatus, style);
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
        }

        #endregion

        #region Git Bar

        private void GitBar()
        {
            GUILayout.Label("Git", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            #region Init Button

            texture2D_InitButton = (Texture2D)Resources.Load("Turbo_Init");
            bool init = GUILayout.Button(new GUIContent("  Init", texture2D_InitButton, "Create an empty Git repository or reinitialize an existing one. \n\n" +
                "Running git init in an existing repository is safe. It will not overwrite things that are already there. The primary reason for rerunning git init is to pick up newly added templates (or to move the repository to another place)."));
            if (init)
            {
                ProcessClass.GitCommand(Enum_Git_Commands.INIT);

                if (ProcessClass.codeSuccessfull)
                    EditorUtility.DisplayDialog("Git Init", ProcessClass.OUTPUT + "\nAnd also a Git_Ignore example is added to your project path!", "Close");
                else
                    EditorUtility.DisplayDialog("Error", ProcessClass.ERROR, "Close");
            }
            EditorGUILayout.Space();

            #endregion

            #region Add Button

            texture2D_AddButton = (Texture2D)Resources.Load("Turbo_Add");
            bool add = GUILayout.Button(new GUIContent("  Add", texture2D_AddButton, "Add file contents to the index.\n It may take a few seconds! \n\n" +
                "This command updates the index using the current content found in the working tree, to prepare the content staged for the next commit. " +
                "It typically adds the current content of existing paths as a whole, but with some options it can also be used to add content with only part " +
                "of the changes made to the working tree files applied, or remove paths that do not exist in the working tree anymore. \n\n" +
                "The 'index' holds a snapshot of the content of the working tree, and it is this snapshot that is taken as the contents of the next commit. " +
                "Thus after making any changes to the working tree, and before running the commit command, you must use the add command to add any new or modified files to the index. \n\n" +
                "This command can be performed multiple times before a commit. It only adds the content of the specified file(s) at the time the add command is run; " +
                "if you want subsequent changes included in the next commit, then you must run git add again to add the new content to the index. \n\n" +
                "The git status command can be used to obtain a summary of which files have changes that are staged for the next commit. \n\n" +
                "he git add command will not add ignored files by default. If any ignored files were explicitly specified on the command line, git add will fail with a list " +
                "of ignored files. Ignored files reached by directory recursion or filename globbing performed by Git (quote your globs before the shell) will be silently ignored."));
            if (add)
            {
                ProcessClass.GitCommand(Enum_Git_Commands.ADD_ALL);

                // Checks the Error output and changes it --> cause the Unity Display Dialog is text limited and will show empty dialog if there are to much texts!
                string newm = ProcessClass.OUTPUT;
                if (ProcessClass.ERROR == "\nfatal: not a git repository (or any of the parent directories): .git\n")
                    EditorUtility.DisplayDialog("Error", ProcessClass.ERROR, "Close");
                else if (ProcessClass.ERROR.StartsWith("\nwarning: LF will be replaced"))
                    EditorUtility.DisplayDialog("Git Add", "All Changes successfully added to Git. \nReady to be Commited!", "Close");
                else
                    EditorUtility.DisplayDialog("Git Add", "There are no changes to be Added. \nAlready up to date!", "Close");
            }
            EditorGUILayout.Space();

            EditorGUILayout.LabelField(new GUIContent("Add a Comment", "Select your comment for all added files and changes. (These comments are basicly something for your " +
                "self!) \nAll the comments and informations will be available is the History tab, if your commit is successfull!"));

            #endregion

            comment = EditorGUILayout.TextArea(comment, GUILayout.Height(5f * EditorGUIUtility.singleLineHeight));
            EditorGUILayout.Space();

            #region Commit Button

            texture2D_CommitButton = (Texture2D)Resources.Load("Turbo_Commit");
            bool commit = GUILayout.Button(new GUIContent("  Commit", texture2D_CommitButton, "Record changes to the repository. \n\n" +
                "Create a new commit containing the current contents of the index and the given log message describing the changes. \n\n" +
                "You dont have to Push after each Commit. You can create multiple Commits (They will be shown in the 'History' tab) and then if your finish Push it to your GitHub!"));
            if (commit)
            {
                comment = '"' + comment + '"';

                ProcessClass.GitCommand(Enum_Git_Commands.COMMIT);

                if (ProcessClass.codeSuccessfull)
                {
                    if (ProcessClass.OUTPUT == "\nOn branch master\nnothing to commit, working tree clean\n")
                        EditorUtility.DisplayDialog("Git Commit", ProcessClass.OUTPUT, "Close");
                    else if (ProcessClass.OUTPUT.StartsWith("\nOn branch main\n") || ProcessClass.OUTPUT.StartsWith("\nOn branch master\n"))
                        if (ProcessClass.OUTPUT.StartsWith("\nOn branch main\nYour branch is ahead of \'origin/main\' by"))
                            EditorUtility.DisplayDialog("Git Commit", "There is nothing to be Commited! \nBut there are some Commited files.. \nUse Git Push to publish your Commits! \nMore informations in 'Status' tab.", "Close");
                        else
                            EditorUtility.DisplayDialog("Git Commit", "There is nothing to be Commited! \nBut there are untracked files. \nUse Git Add first! \nMore informations in 'Status' tab.", "Close");
                    else if (ProcessClass.OUTPUT.StartsWith("\n[master") || ProcessClass.OUTPUT.StartsWith("\n[main"))
                        EditorUtility.DisplayDialog("Git Commit", "All changes successfully Commited! \nMore informations in the 'History' tab.", "Close");
                    else
                        EditorUtility.DisplayDialog("Git Commit", "Something went wrong! \nPlease try again!", "Close");
                }
                else
                    EditorUtility.DisplayDialog("Error", ProcessClass.ERROR, "Close");

                comment = string.Empty;
            }
            EditorGUILayout.Space();

            #endregion

            #region Push Button

            texture2D_PushButton = (Texture2D)Resources.Load("Turbo_Push");
            bool push = GUILayout.Button(new GUIContent("  Push", texture2D_PushButton, "Update remote refs along with associated objects. \n\n" +
                "Updates remote refs using local refs, while sending objects necessary to complete the given refs."));
            if (push)
            {
                ProcessClass.GitCommand(Enum_Git_Commands.BRANCH);
                ProcessClass.GitCommand(Enum_Git_Commands.REMOTE);
                ProcessClass.GitCommand(Enum_Git_Commands.PUSH);

                if (ProcessClass.codeSuccessfull)
                    //EditorUtility.DisplayDialog("Git Push", ProcessClass.OUTPUT, "Close");
                    EditorUtility.DisplayDialog("Git Push", "If there are any changes with Commit, it is successfully Pushed to your Remote. \nElse there is nothing to Push and your Git is already up to date! \nMore informations in 'Status' tab.", "Close");
                else
                    EditorUtility.DisplayDialog("Error", ProcessClass.ERROR, "Close");
            }
            EditorGUILayout.Space();

            #endregion

            #region Pull Button

            texture2D_PullButton = (Texture2D)Resources.Load("Turbo_Pull");
            bool pull = GUILayout.Button(new GUIContent("  Pull", texture2D_PullButton, "Fetch from and integrate with another repository or a local branch. \n\n" +
                "Incorporates changes from a remote repository into the current branch."));
            if (pull)
            {
                ProcessClass.GitCommand(Enum_Git_Commands.PULL);

                if (ProcessClass.codeSuccessfull)
                    EditorUtility.DisplayDialog("Git Pull", ProcessClass.OUTPUT, "Close");
                else
                    EditorUtility.DisplayDialog("Error", ProcessClass.ERROR, "Close");
            }
            EditorGUILayout.Space();

            #endregion

            #region Clone Button

            // Clone Button
            texture2D_CloneButton = (Texture2D)Resources.Load("Turbo_Clone");
            bool clone = GUILayout.Button(new GUIContent("  Clone", texture2D_CloneButton, "Clone a repository into a new directory! \n\n" +
                "If you want to get a copy of an existing Git repository — for example, a project you’d like to contribute to — the command you need is git clone. " +
                "If you’re familiar with other VCSs such as Subversion, you’ll notice that the command is 'clone' and not 'checkout'. This is an important distinction — " +
                "instead of getting just a working copy, Git receives a full copy of nearly all data that the server has. Every version of every file for the history of the " +
                "project is pulled down by default when you run git clone. In fact, if your server disk gets corrupted, you can often use nearly any of the clones on any client " +
                "to set the server back to the state it was in when it was cloned "));
            if (clone)
            {
                ProcessClass.GitCommand(Enum_Git_Commands.Clone);

                if (ProcessClass.codeSuccessfull)
                    EditorUtility.DisplayDialog("Git Pull", ProcessClass.OUTPUT, "Close");
                else
                    EditorUtility.DisplayDialog("Error", ProcessClass.ERROR, "Close");
            }

            #endregion

        }

        #endregion

        #region History Bar

        private void HistoryBar()
        {
            GUILayout.Label("History", EditorStyles.boldLabel);

            EditorGUILayout.Space();
            EditorGUILayout.BeginVertical();
            scrollPosHistory = EditorGUILayout.BeginScrollView(scrollPosHistory, GUILayout.Width(position.width), GUILayout.Height(position.height - 55));
            CodeUtills.OnDrawTextur(new Rect(0, 0, maxSize.x, maxSize.y), InitializeTexture2D(backgroundColor), ScaleMode.StretchToFill);

            GUI.color = textColor;

            if (update_Tabs_Eversy_X_Seconds <= 0)
            {
                ProcessClass.GitCommand(Enum_Git_Commands.LOG);

                if (ProcessClass.codeSuccessfull)
                    scrollStringHistory = ProcessClass.OUTPUT;
                else
                    scrollStringHistory = ProcessClass.ERROR;

                update_Tabs_Eversy_X_Seconds = _X_;
            }

            GUILayout.Label(scrollStringHistory, style);

            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
        }

        #endregion

        // Sign out and save all customisation datas
        private void SignOut()
        {
            Debug.Log("Signout was a success...!");
            lastRegistrationTime = CodeUtills.GetCurrentDateTime();
            CheckForDataChange();
            Instance.Close();
        }

        private Texture2D InitializeTexture2D(Color color)
        {
            texture2D_TurboIcon = new Texture2D(1, 1, TextureFormat.RGBA32, false);
            texture2D_TurboIcon.SetPixel(0, 0, color);
            texture2D_TurboIcon.Apply();

            return texture2D_TurboIcon;
        }

        // Set User Account Data
        private static void SetAccountData()
        {
            EditorPrefs.SetFloat(BACKGROUND_COLOR_KEY_R, backgroundColor.r);
            EditorPrefs.SetFloat(BACKGROUND_COLOR_KEY_G, backgroundColor.g);
            EditorPrefs.SetFloat(BACKGROUND_COLOR_KEY_B, backgroundColor.b);
            EditorPrefs.SetFloat(BACKGROUND_COLOR_KEY_A, backgroundColor.a);

            EditorPrefs.SetFloat(TEXT_COLOR_KEY_R, textColor.r);
            EditorPrefs.SetFloat(TEXT_COLOR_KEY_G, textColor.g);
            EditorPrefs.SetFloat(TEXT_COLOR_KEY_B, textColor.b);
            EditorPrefs.SetFloat(TEXT_COLOR_KEY_A, textColor.a);

            EditorPrefs.SetBool(BOLD_FONT_KEY, isBoldFont);

            EditorPrefs.SetString(LAST_REGISTRATION_KEY, lastRegistrationTime.ToString());
        }

        // Get User Account Data
        private static void GetAccountData()
        {
            if (EditorPrefs.HasKey(BACKGROUND_COLOR_KEY_R))
                backgroundColor.r = EditorPrefs.GetFloat(BACKGROUND_COLOR_KEY_R);
            if (EditorPrefs.HasKey(BACKGROUND_COLOR_KEY_G))
                backgroundColor.g = EditorPrefs.GetFloat(BACKGROUND_COLOR_KEY_G);
            if (EditorPrefs.HasKey(BACKGROUND_COLOR_KEY_B))
                backgroundColor.b = EditorPrefs.GetFloat(BACKGROUND_COLOR_KEY_B);
            if (EditorPrefs.HasKey(BACKGROUND_COLOR_KEY_A))
                backgroundColor.a = EditorPrefs.GetFloat(BACKGROUND_COLOR_KEY_A);

            if (EditorPrefs.HasKey(TEXT_COLOR_KEY_R))
                textColor.r = EditorPrefs.GetFloat(TEXT_COLOR_KEY_R);
            if (EditorPrefs.HasKey(TEXT_COLOR_KEY_G))
                textColor.g = EditorPrefs.GetFloat(TEXT_COLOR_KEY_G);
            if (EditorPrefs.HasKey(TEXT_COLOR_KEY_B))
                textColor.b = EditorPrefs.GetFloat(TEXT_COLOR_KEY_B);
            if (EditorPrefs.HasKey(TEXT_COLOR_KEY_A))
                textColor.a = EditorPrefs.GetFloat(TEXT_COLOR_KEY_A);

            if (EditorPrefs.HasKey(BOLD_FONT_KEY))
                isBoldFont = EditorPrefs.GetBool(BOLD_FONT_KEY);

            if (EditorPrefs.HasKey(LAST_REGISTRATION_KEY))
                lastRegistrationTime = DateTime.Parse(EditorPrefs.GetString(LAST_REGISTRATION_KEY));
        }

        // Checks if anything is changed (colors & Font-Style & time) --> then saves the new datas 
        private void CheckForDataChange()
        {
            if (previousBackgroundColor != backgroundColor || previousTextColor != textColor || previosIsBoldFont != isBoldFont || previousLastRegistrationTime != lastRegistrationTime)
            {
                SetAccountData();
                previousBackgroundColor = backgroundColor;
                previousTextColor = textColor;
                previosIsBoldFont = isBoldFont;
                previousLastRegistrationTime = lastRegistrationTime;
            }
        }
    }
}