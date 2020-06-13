using System.Collections;
using System.Collections.Generic;

using LitJson;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class DetectObj : MonoBehaviour
{
    #region Fields
    //public GameObject Maincanvas, LoadingCanvas;
    public Text outputText;
    public GameObject headerCav;
    public GameObject Maincanvas, LoadingCanvas, Panel;
    JsonData jsonvale;
    //Output from server 
    string output;
    //[SerializeField] ScanProperties scanProperties;
    [SerializeField] InventoryObjectHolder inventory;
    [SerializeField] ProfileInfoContainer profile;
    [SerializeField] AchievementHolder achievement;
    [SerializeField] Sprite objectDetectSp;
    [SerializeField] Image outlineImg;
    [SerializeField] Button scanBtn;
    [SerializeField] Button nextBtn;
    DetectObjectData detectObjectData = new DetectObjectData ();
    ScanProperties scanProperties;
    #endregion Fields

    #region Methods
    public void Detect ()
    {
        AudioManager.Instance?.Play("UIAction", "UI");
        outputText.text = "Please Wait";
        StartCoroutine (TakePicture ());
    }
    private void Start()
    {
        scanProperties = ScanPropertiesHolder.Instance.GetPropertie();
        outputText.text = $"Point the camera frame at the { scanProperties.detectionObjectName} then tap SCAN to detect.";
        outlineImg.sprite = scanProperties.outlineSp;
        outlineImg.SetNativeSize();
    }

    //responsible of return string of response output of a JSON String
    string processJson (string response)
    {
        //jsonvale = JsonMapper.ToObject(response);
        //string ObjectDetectedFormResponse;

        //ObjectDetectedFormResponse = jsonvale[0]["message"].ToString();
        //return ObjectDetectedFormResponse;

        jsonvale = JsonMapper.ToObject (response);
        string ObjectDetectedFormResponse;
        Debug.Log (jsonvale);

        ObjectDetectedFormResponse = jsonvale ["detected"].ToString ();
        return ObjectDetectedFormResponse;
    }
    // upload image to the server to get detected
    IEnumerator UploadPNG ()
    {
        // hide mainCanvas and show loading Canvas
        Maincanvas.SetActive (false);
        LoadingCanvas.SetActive (true);
        // We should only read the screen after all rendering is complete
        yield return new WaitForEndOfFrame ();
        string path = Application.persistentDataPath + "/Screen-Capture" + ".png";
        // Create a texture the size of the screen, RGB24 format
        int width = Screen.width;
        int height = Screen.height;
        var tex = new Texture2D (width , height , TextureFormat.RGB24 , false);

        // Read screen contents into the texture
        tex.ReadPixels (new Rect (0 , 0 , width , height) , 0 , 0);
        tex.Apply ();


        // Encode texture into PNG
        byte [] bytes = tex.EncodeToPNG ();
        //optional step either save to image or not
        //System.IO.File.WriteAllBytes (path , bytes);
        Destroy (tex);
        Debug.Log (bytes);

        // Create a Web Form
        WWWForm form = new WWWForm ();
        form.AddField ("score" , "0.8");
        //form.AddField ("objectToDetect" , scanProperties.detectionObjectName);
        form.AddField ("objectToDetect" , "tree");
        form.AddBinaryData ("scannedImg" , bytes , "screenShot.png" , "image/png");

        // Upload to a cgi script
        using ( var w = UnityEngine.Networking.UnityWebRequest.Post ("https://exploreagora.herokuapp.com/player/vision/detect" , form) )
        {
            yield return w.SendWebRequest ();
            //if error 
            if ( w.isNetworkError || w.isHttpError )
            {
                Debug.Log (w.error);
                Debug.Log (w.error);
                Debug.Log ("Error");
                outputText.text = w.downloadHandler.text + " " + w.error;
            }
            //if Done
            else
            {
                Debug.Log ("Finished Uploading Screenshot");
                Debug.Log (w.downloadHandler.text);
                output = processJson (w.downloadHandler.text);
                //outputText.text = output;
                //For Example
                if ( output.ToLower () == "true" )
                {
                    print (output + " true   " + ScanPropertiesHolder.Instance.DetectionObjectName.ToLower ());
                    outputText.text = "Found";
                   
                    //if (ScanPropertiesHolder.Instance.ShouldContinueToExperience)
                    //    Panel.SetActive(true);
                    //else
                    //    SceneManager.LoadScene("first Scene");

                }
                else
                {
                    outputText.text = ScanPropertiesHolder.Instance.DetectionObjectName + " not found";
                    print (output + "  Not found  " + ScanPropertiesHolder.Instance.DetectionObjectName.ToLower ());
                }
            }
            // hide loading Canvas and show mainCanvas
            LoadingCanvas.SetActive (false);
            Maincanvas.SetActive (true);
        }
    }
    public IEnumerator TakePicture()
    {
        Maincanvas.SetActive(false);
        yield return new WaitForEndOfFrame();
        //string path = Application.persistentDataPath + "/Screen-Capture" + ".png";
        //string path2 = Application.persistentDataPath + "/Screen-Captur00" + ".png";
        // Create a texture the size of the screen, RGB24 format
        int width = Screen.width;
        int height = Screen.height;
        var tex = new Texture2D(width, height, TextureFormat.RGB24, false);
        // Read screen contents into the texture
        tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        tex.Apply();
        byte[] bytes = tex.EncodeToPNG();
        //System.IO.File.WriteAllBytes(path, bytes);

        var newTex = ScaleTexture(tex, 512, (height/width)*512);

        byte[] bytesss = newTex.EncodeToPNG();
        //System.IO.File.WriteAllBytes(path2, bytesss);

        // Encode texture into PNG
        bytes = tex.EncodeToPNG();
        detectObjectData.bytes = bytesss;
        //detectObjectData.score = "0.69";
        detectObjectData.detectionObjectName = ScanPropertiesHolder.Instance.DetectionObjectName.ToLower();
        detectObjectData.objectsToDetect = new ObjectsToDetect();
        detectObjectData.objectsToDetect.objects = scanProperties.objectInfos.ToArray();
        NetworkManager.Instance.DetectObject(detectObjectData, OnSuscees, OnFailed);
        Destroy(tex);
        Destroy(newTex);
        LoadingCanvas.SetActive(true);
    }
    private Texture2D ScaleTexture(Texture2D source, int targetWidth, int targetHeight)
    {
        Texture2D result = new Texture2D(targetWidth, targetHeight, source.format, true);
        Color[] rpixels = result.GetPixels(0);
        float incX = ((float)1 / source.width) * ((float)source.width / targetWidth);
        float incY = ((float)1 / source.height) * ((float)source.height / targetHeight);
        for (int px = 0; px < rpixels.Length; px++)
        {
            rpixels[px] = source.GetPixelBilinear(incX * ((float)px % targetWidth),
            incY * ((float)Mathf.Floor(px / targetWidth)));
        }
        result.SetPixels(rpixels, 0);
        result.Apply();
        return result;
    }
    private void OnSuscees (NetworkParameters obj)
    {
        DetectObjectResponse detectObjectResponse = (DetectObjectResponse)obj.responseData;
        print (detectObjectResponse.detected);
        LoadingCanvas.SetActive (false);
        Maincanvas.SetActive (true);

        if ( detectObjectResponse.detected.ToLower () == "true" )
        {
            print (output + " true   " + ScanPropertiesHolder.Instance.DetectionObjectName.ToLower ());
            outputText.text = $"{ ScanPropertiesHolder.Instance.DetectionObjectName} found";
            outlineImg.sprite = objectDetectSp;
            outlineImg.SetNativeSize();
            scanBtn.gameObject.SetActive(false);
            int counter = inventory.GetScanedCounter(ScanPropertiesHolder.Instance.DetectionObjectName);
            if (counter == -1)
            {
                counter = 0;
                achievement.UpdateCurrent();
                Sprite badge = achievement.GetBadge();
                if (badge != null) {
                    AchievementManager.Instance.AddBadge(badge);
                }
                AchievementManager.Instance.AddScannedObject(scanProperties.detectionObjectSp, ScanPropertiesHolder.Instance.DetectionObjectName);
            }
            AchievementManager.Instance.AddScore(ScorePointsUtility.ScanObject);
            profile.points += ScorePointsUtility.ScanObject;
            counter++;
            inventory.SetObject(ScanPropertiesHolder.Instance.DetectionObjectName, counter);
            UpdateProfile();
            if (ScanPropertiesHolder.Instance.ShouldContinueToExperience)
                Panel.SetActive(true);
            else {
                if (AppManager.Instance.boardingPhases != OnBoardingPhases.None)
                {
                    AppManager.Instance.currentBoardingIndex = 1;
                    AppManager.Instance.isCurrentLevelDone[0] = true;
                    AppManager.Instance.isCurrentLevelPrizeDone[0] = true;
                    AppManager.Instance.saveOnBoardingProgress();
                    //SceneLoader.Instance.LoadExperience("UI-UX");
                }
                nextBtn.gameObject.SetActive(true);
                //Invoke(nameof(GoBack), 6);
            }
        }
        else
        {
            outputText.text = ScanPropertiesHolder.Instance.DetectionObjectName + " Not Found";
            print (output + "  Not found  " + ScanPropertiesHolder.Instance.DetectionObjectName.ToLower ());
        }
    }
    public void GoBack() { SceneLoader.Instance.LoadExperience("UI-UX"); }
    public void UpdateProfile()
    {

        ProfileData ss = new ProfileData();
        if (inventory.ScanedObjects.Count > 0)
        {
            ss.scannedObjects = new ScannedObjects();
            ss.scannedObjects.scannedObjects = new List<ScannedObject>();
            foreach (var i in inventory.ScanedObjects)
            {
                ss.scannedObjects.scannedObjects.Add(new ScannedObject(i.Key, i.Value));
            }
        }
        ss.firstName = profile.fName;
        ss.lastName = profile.lName;
        ss.nickName = profile.nickname;
        ss.country = profile.country;
        ss.birthDate = profile.DOB.dateTime.ToString();
        ss.avatarId = profile.profileImgIndex.ToString();
        ss.email = profile.email;
        ss.gender = profile.gender.ToString();
        ss.keys = profile.keys;
        ss.dailyStreaks = profile.streaks;
        ss.points = profile.points;
        ss.powerStones = profile.stones;
        if (profile.Achievements.Count > 0)
        {
            ss.achievementsData = new AchievementsData();
            ss.achievementsData.achievements = profile.Achievements;
        }
        NetworkManager.Instance.UpdateProfile(ss, OnUpdateProfileSuccess, OnUpdateProfileFailed);
    }
    private void OnUpdateProfileSuccess(NetworkParameters obj)
    {
    }
    private void OnUpdateProfileFailed(NetworkParameters obj)
    {
    }
    private void OnFailed (NetworkParameters obj)
    {
        print (obj.err.message);
        outputText.text = "Poor Internet connection";
        LoadingCanvas.SetActive (false);
        Maincanvas.SetActive (true);
    }
    public void TapSound()
    {
        AudioManager.Instance?.Play("UIAction", "UI");
    }
    #endregion Methods
}