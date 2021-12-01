using System;
using System.IO;
using System.Diagnostics;
using UnityEngine;
using Daniyal_Dehghany_GitHubTool_TurboGit.Serialized;

namespace Daniyal_Dehghany_GitHubTool_TurboGit.Git
{
    public class ProcessClass : MonoBehaviour
    {

        public static string INPUT;
        public static string OUTPUT;
        public static string ERROR;

        public static bool codeSuccessfull;

        #region Read only members

        // Set the working directory (dynamicly), to get the right path
        private static readonly string WORKING_DIRECTORY = Application.dataPath.Replace("/Assets", "");
        private static readonly string FILE_NAME = "git";

        // Git commands as strings
        private static readonly string INIT = "init";
        private static readonly string STATUS = "status";
        private static readonly string ADD_ALL = "add .";
        private static readonly string COMMIT = "commit -m ";
        private static readonly string PUSH = "push ";
        private static readonly string PULL = "pull ";
        private static readonly string VERSION = "--version";
        private static readonly string GLOBAL_USERNAME = "config --global user.name ";
        private static readonly string GLOBAL_EMAIL = "config --global user.email ";
        private static readonly string LOCAL_USERNAME = "config --local user.name ";
        private static readonly string LOCAL_EMAIL = "config --local user.email ";
        private static readonly string SIGN_OUT = "exit";
        private static readonly string UPDATE = "update-git-for-windows";
        private static readonly string BRANCH = "branch -M main";
        private static readonly string REMOTE = "remote add origin ";
        private static readonly string LOG = "log";
        private static readonly string CLONE = "clone ";

        #endregion

        /// <summary>
        /// This methode works with an Enumeration to call each command using an enum --> the string commands are already defined, just call them!
        /// </summary>
        /// <param name="_Commands"></param>
        public static void GitCommand(Enum_Git_Commands _Commands)
        {
            OUTPUT = "";
            ERROR = "";

            switch (_Commands)
            {
                case Enum_Git_Commands.INIT:
                    Call_Git_Command(INIT);
                    CopyGitIgnoreFileToTheRightPath();
                    break;
                case Enum_Git_Commands.STATUS:
                    Call_Git_Command(STATUS);
                    break;
                case Enum_Git_Commands.ADD_ALL:
                    Call_Git_Command(ADD_ALL);
                    break;
                case Enum_Git_Commands.COMMIT:
                    Call_Git_Command(COMMIT, TurboGitAccount.comment);
                    break;
                case Enum_Git_Commands.PUSH:
                    int counter = 19; // --> (https://github.com/) ==> 19 chars!
                    string repository = SerializeFiles.UserInformations.GIT_Remote.Substring(counter, SerializeFiles.UserInformations.GIT_Remote.Length - counter);
                    Call_Git_Command(PUSH + "https://" + SerializeFiles.UserInformations.User_Username + ":" + SerializeFiles.UserInformations.User_Password + "@github.com/" + repository);
                    break;
                case Enum_Git_Commands.PULL:
                    Call_Git_Command(PULL, SerializeFiles.UserInformations.GIT_Remote + " main");
                    break;
                case Enum_Git_Commands.Version:
                    Call_Git_Command(VERSION);
                    break;
                case Enum_Git_Commands.GLOBAL_USERNAME:
                    Call_Git_Command(GLOBAL_USERNAME, TurboGitLogin.username);
                    break;
                case Enum_Git_Commands.GLOBAL_EMAIL:
                    Call_Git_Command(GLOBAL_EMAIL, TurboGitLogin.eMail);
                    break;
                case Enum_Git_Commands.LOCAL_USERNAME:
                    Call_Git_Command(LOCAL_USERNAME, TurboGitLogin.username);
                    break;
                case Enum_Git_Commands.LOCAL_EMAIL:
                    Call_Git_Command(LOCAL_EMAIL, TurboGitLogin.eMail);
                    break;
                case Enum_Git_Commands.SIGN_OUT:
                    Call_Git_Command(SIGN_OUT);
                    break;
                case Enum_Git_Commands.UPDATE_GIT:
                    Call_Git_Command(UPDATE);
                    break;
                case Enum_Git_Commands.BRANCH:
                    Call_Git_Command(BRANCH);
                    break;
                case Enum_Git_Commands.REMOTE:
                    Call_Git_Command(REMOTE, SerializeFiles.UserInformations.GIT_Remote);
                    break;
                case Enum_Git_Commands.LOG:
                    Call_Git_Command(LOG);
                    break;
                case Enum_Git_Commands.Clone:
                    Call_Git_Command(CLONE, SerializeFiles.UserInformations.GIT_Remote);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// This code is gonna call a command to gitbash + if there should be a userinput
        /// </summary>
        /// <param name="command">The string command of git</param>
        /// <param name="userInput">User input --> if a git string command is with a user input connected</param>
        /// <param name="timeout">Timeout of the process in milliseconds (default: 0 = indefinite)</param>
        private static void Call_Git_Command(string command, string userInput, int timeout = 0)
        {
            try
            {
                using (Process git = new Process())
                {
                    git.StartInfo.FileName = FILE_NAME;
                    INPUT = git.StartInfo.Arguments = command + userInput;
                    git.StartInfo.UseShellExecute = false;
                    git.StartInfo.WorkingDirectory = WORKING_DIRECTORY;
                    git.StartInfo.CreateNoWindow = true;
                    git.StartInfo.RedirectStandardOutput = true;
                    git.StartInfo.StandardOutputEncoding = System.Text.Encoding.UTF8;
                    git.OutputDataReceived += process_OutputDataReceived;
                    git.StartInfo.RedirectStandardError = true;
                    git.StartInfo.StandardErrorEncoding = System.Text.Encoding.UTF8;
                    git.ErrorDataReceived += process_ErrorDataReceived;

                    git.Start();

                    git.BeginOutputReadLine();
                    git.BeginErrorReadLine();

                    if (timeout > 0)
                        git.WaitForExit(timeout);
                    else
                        git.WaitForExit();
                }
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogError(e.Message);
            }
        }

        /// <summary>
        /// Gives the Output calls of the process class
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            OUTPUT += "\n" + e.Data;

            if (OUTPUT != "\n")
                codeSuccessfull = true;
            else
                codeSuccessfull = false;

            if (e.Data == null)
                return;

            UnityEngine.Debug.Log(e.Data);
        }

        /// <summary>
        /// Gives the Error calls of the process class
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            ERROR += "\n" + e.Data;

            if (e.Data == null)
                return;

            UnityEngine.Debug.Log(e.Data);
        }

        /// <summary>
        /// This code is gonna call a command to gitbash --> (without a user input) => just readonly commands!
        /// </summary>
        /// <param name="command"></param>
        private static void Call_Git_Command(string command)
           => Call_Git_Command(command, string.Empty);

        /// <summary>
        /// Copies .gitignorefile dynamicly in your actual project path
        /// </summary>
        private static void CopyGitIgnoreFileToTheRightPath()
        {
            string gitIgnorePath = WORKING_DIRECTORY + "/Assets/Daniyal Dehghany/GitHubTool/TurboGit/Resources/GitIgnoreExample/GitIgnore_Example.txt";
            string placeGitIgnorePath = WORKING_DIRECTORY + "/.gitignore";
            // If a file exists, just overwrite it!
            File.Copy(gitIgnorePath, placeGitIgnorePath, true);
        }
    }
}
