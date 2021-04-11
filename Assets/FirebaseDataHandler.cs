using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
public class FirebaseDataHandler : MonoBehaviour
{
    // Start is called before the first frame update
    public static FirebaseDataHandler singleton;
    [SerializeField] Text usernameField;
    [SerializeField] InputField passwordField;
    private const string API_KEY = "AIzaSyCvBSSphzkWA6GFOxg8YeDfcUHONENMG-M", DB_URL = "https://sprofile-8854f-default-rtdb.firebaseio.com";
    private string identityEndpoint, storedUserName, storedEmail, storedUID;
    private bool withAuth = false;
    private JSONObject character = null;
    private void Start() {
        singleton = this;
        identityEndpoint = "https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key=" + API_KEY;
    }

    [ContextMenu("Test reverseLookup")]
    void TestOmz()
    {
        StartCoroutine(RequestUserData("omz"));
    }

    public void GetPlayerInfoFromUI() {
        string userName = usernameField.text;
        if (withAuth && userName.Equals(storedUserName)) {
            StartCoroutine(RequestWithAuth());
        } else {
            StartCoroutine(RequestUserData(userName));
        }
    }

    IEnumerator RequestUserData(string _username) {
        UnityWebRequest www = UnityWebRequest.Get(DB_URL + "/reverseLookup/" + _username + ".json");
        yield return www.SendWebRequest();
        print(www.downloadHandler.text);
        string uid = "";
        try {
            JSONObject json = new JSONObject(www.downloadHandler.text);
            uid = json.GetField("uid").ToString();
            uid = uid.Substring(1, uid.Length - 2);
            storedUID = uid;
            storedEmail = json.GetField("email").ToString();
            storedEmail = storedEmail.Substring(1, storedEmail.Length -2);
            storedUserName = _username;
        } catch (System.Exception ex) {
            GameObject.Find("Subheading").GetComponent<Text>().text = "Profile does not exist";
            yield break;
        }

        UnityWebRequest profileRequest = UnityWebRequest.Get(DB_URL + "/users/" + uid + "/profileData.json");
        yield return profileRequest.SendWebRequest();
        
        JSONObject profileJson = new JSONObject(profileRequest.downloadHandler.text);

        if (profileJson.HasField("error")) {
            GameObject.Find("Subheading").GetComponent<Text>().text = "Profile is private";
            UIManager.singleton.PlayAnimation("loginExpand");
            withAuth = true;
        } else {
            GameObject.Find("Subheading").GetComponent<Text>().text = "";
            character = profileJson;
            GameManager.singleton.PrepareGame();
        }

    }

    IEnumerator RequestWithAuth() {
        WWWForm formData = new WWWForm();
        formData.AddField("email", storedEmail);
        formData.AddField("password", passwordField.text);
        print(passwordField.text);
        formData.AddField("returnSecureToken", "true");
        UnityWebRequest www = UnityWebRequest.Post(identityEndpoint, formData);
        yield return www.SendWebRequest();
        JSONObject response = new JSONObject(www.downloadHandler.text);
        if(response.GetField("idToken") != null) {
            string idToken = response.GetField("idToken").ToString();
            idToken = idToken.Substring(1, idToken.Length - 2);
            UnityWebRequest profileRequest = UnityWebRequest.Get(DB_URL + "/users/" + storedUID + ".json" + "?auth=" + idToken);
            yield return profileRequest.SendWebRequest();
            character = new JSONObject(profileRequest.downloadHandler.text);
            GameManager.singleton.PrepareGame();
        }
    }

    public string GetCosmetic(string category) {
        return category + '_' + (int.Parse(character.GetField(category).ToString()) + 1);
    }
}
