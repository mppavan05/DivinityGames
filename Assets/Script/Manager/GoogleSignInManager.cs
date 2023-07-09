using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using UnityEngine.UI;
using Google;
using System;
using System.Threading.Tasks;
using WebSocketSharp;

public class GoogleSignInManager : MonoBehaviour
{
    public string infoText;
    public string webClientId = "<your client id here>";

    private FirebaseAuth auth;
    private FirebaseUser user;
    private GoogleSignInConfiguration configuration;

    public string userName;
    public string userEmail;

    private void Awake()
    {
        configuration = new GoogleSignInConfiguration { WebClientId = webClientId, RequestEmail = true, RequestIdToken = true };
        CheckFirebaseDependencies();
    }

    private void Update()
    {
        if (user != null && userEmail.IsNullOrEmpty())
        {
            userEmail = user.Email;
            userName = user.DisplayName;

            print("User Email : " + userEmail);
            print("User Name : " + userName);

            LoginManager.Instance.googleUserEmail = userEmail;
            LoginManager.Instance.googleUserName = userName;
            //LoginManager.Instance.email_Main(user.f);

        }
    }

    private void CheckFirebaseDependencies()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                if (task.Result == DependencyStatus.Available)
                    auth = FirebaseAuth.DefaultInstance;
                else
                    AddToInformation("Could not resolve all Firebase dependencies: " + task.Result.ToString());
            }
            else
            {
                AddToInformation("Dependency check was not completed. Error : " + task.Exception.Message);
            }
        });
    }

    public void SignInWithGoogle()
    {

        SoundManager.Instance.ButtonClick();
        if (Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.WindowsEditor)
        {
            string imageUrl = "https://lh3.googleusercontent.com/ogw/AOh-ky3pVzuI2Z1jwSVkU2JGgCT22_4YtGST8d9h3uI=s64-c-mo";
            //string imageUrl = "https://panel.dukeplay.com/assets/img/profile-picture/1120.png";
            PlayerPrefs.SetString("ProfileURL", LoginManager.Instance.testImageUrl);

            //LoginManager.Instance.googleUserEmail = "greejeshgajera709@gmail.com";
            //LoginManager.Instance.googleUserName = "GG GG1";
            //LoginManager.Instance.email_Main("112345654321", LoginManager.Instance.googleUserName, imageUrl);

            LoginManager.Instance.googleUserEmail = LoginManager.Instance.testEmail;
            LoginManager.Instance.googleUserName = LoginManager.Instance.testUserName;
            LoginManager.Instance.email_Main(LoginManager.Instance.testFirebaseToken, LoginManager.Instance.googleUserName, LoginManager.Instance.testImageUrl);

        }
        else
        {
            print("Sign in Button Click");
            OnSignIn();
        }

    }
    public void SignOutFromGoogle() { OnSignOut(); }

    private void OnSignIn()
    {
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;
        GoogleSignIn.Configuration.RequestEmail = true;
        AddToInformation("Calling SignIn");

        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(OnAuthenticationFinished);
    }

    private void OnSignOut()
    {
        AddToInformation("Calling SignOut");
        GoogleSignIn.DefaultInstance.SignOut();
    }

    public void OnDisconnect()
    {
        AddToInformation("Calling Disconnect");
        GoogleSignIn.DefaultInstance.Disconnect();
    }
    internal void OnAuthenticationFinished(Task<GoogleSignInUser> task)
    {
        if (task.IsFaulted)
        {
            using (IEnumerator<Exception> enumerator = task.Exception.InnerExceptions.GetEnumerator())
            {
                if (enumerator.MoveNext())
                {
                    GoogleSignIn.SignInException error = (GoogleSignIn.SignInException)enumerator.Current;
                    AddToInformation("Got Error: " + error.Status + " " + error.Message);
                }
                else
                {
                    AddToInformation("Got Unexpected Exception?!?" + task.Exception);
                }
            }
        }
        else if (task.IsCanceled)
        {
            AddToInformation("Canceled");
        }
        else
        {
            //AddToInformation("Welcome: " + task.Result.DisplayName + "!");
            //AddToInformation("Email = " + task.Result.Email);
            //AddToInformation("Google ID Token = " + task.Result.IdToken);
            //AddToInformation("Email = " + task.Result.Email);


            //SignInWithGoogleOnFirebase(task.Result.IdToken);
            //if (task.Result.Email != null)
            //{
            //    user = auth.CurrentUser;
            //    print("Enter The Login Google");
            //    //GGG
            //    LoginManager.Instance.googleUserEmail = user.Email.ToString();
            //    LoginManager.Instance.googleUserName = user.DisplayName.ToString();

            //    //print("Login Data : " + LoginManager.Instance.googleUserEmail);
            //    LoginManager.Instance.email_Main();
            //    //OnSignIn();
            //}

            print("Google Sign-In successed");

            print("IdToken: " + task.Result.IdToken);
            string firebaseToken = task.Result.IdToken;
            print("ImageUrl: " + task.Result.ImageUrl.ToString());

            PlayerPrefs.SetString("ProfileURL", task.Result.ImageUrl.ToString());
            //miniloading.SetActive(true);
            //Set imageUrl
            //imageUrl = task.Result.ImageUrl.ToString();

            //Start Firebase Auth
            Firebase.Auth.Credential credential =
                Firebase.Auth.GoogleAuthProvider.GetCredential(task.Result.IdToken, null);
            auth.SignInWithCredentialAsync(credential).ContinueWith(t =>
            {
                if (t.IsCanceled)
                {
                    print("SignInWithCredentialAsync was canceled.");
                    return;
                }

                if (t.IsFaulted)
                {
                    print("SignInWithCredentialAsync encountered an error: " + t.Exception);
                    return;
                }



                user = auth.CurrentUser;
                LoginManager.Instance.googleUserEmail = user.Email.ToString();
                LoginManager.Instance.googleUserName = user.DisplayName.ToString();

                LoginManager.Instance.email_Main(firebaseToken, LoginManager.Instance.googleUserName, PlayerPrefs.GetString("ProfileURL"));


                //DataManager.Instance.GetProfileImage(PlayerPrefs.GetString("ProfileURL"));
                Debug.Log("LoadImage(CheckImageUrl)");
            });
        }
    }

    private void SignInWithGoogleOnFirebase(string idToken)
    {
        Credential credential = GoogleAuthProvider.GetCredential(idToken, null);

        auth.SignInWithCredentialAsync(credential).ContinueWith(task =>
        {
            AggregateException ex = task.Exception;
            if (ex != null)
            {
                if (ex.InnerExceptions[0] is FirebaseException inner && (inner.ErrorCode != 0))
                    AddToInformation("\nError code = " + inner.ErrorCode + " Message = " + inner.Message);
            }
            else
            {
                AddToInformation("Sign In Successful.");
            }
        });
    }

    public void OnSignInSilently()
    {
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;
        AddToInformation("Calling SignIn Silently");

        GoogleSignIn.DefaultInstance.SignInSilently().ContinueWith(OnAuthenticationFinished);
    }

    public void OnGamesSignIn()
    {
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = true;
        GoogleSignIn.Configuration.RequestIdToken = false;

        AddToInformation("Calling Games SignIn");

        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(OnAuthenticationFinished);
    }

    private void AddToInformation(string str)
    {
        infoText += "\n" + str;
        Debug.Log("Info : " + str);
    }
}