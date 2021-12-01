namespace Daniyal_Dehghany_GitHubTool_TurboGit.Serialized
{
    public class SerializeFiles
    {
        public class UserInformations
        {
            public static string User_Username { get => TurboGitLogin.username; private set => TurboGitLogin.username = value; }
            public static string User_E_Mail { get => TurboGitLogin.eMail; private set => TurboGitLogin.eMail = value; }
            public static string User_Password { get => TurboGitLogin.password; private set => TurboGitLogin.password = value; }
            public static string GIT_Remote { get => TurboGitLogin.remote; private set => TurboGitLogin.remote = value; }
            public static bool User_Global_Git { get => TurboGitLogin.globalCheck; private set => TurboGitLogin.globalCheck = value; }
        }
    }
}

