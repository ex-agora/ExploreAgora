﻿using System.Collections;
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
    public GameObject Maincanvas, LoadingCanvas, Panel;
    JsonData jsonvale;
    //Output from server 
    string output;
    [SerializeField] ScanProperties scanProperties;
    [SerializeField] InventoryObjectHolder inventory;
    DetectObjectData detectObjectData = new DetectObjectData ();
    #endregion Fields

    #region Methods
    public void Detect ()
    {
        StartCoroutine (TakePicture ());
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
                    print (output + " true   " + scanProperties.detectionObjectName.ToLower ());
                    outputText.text = "Found";
                   
                    if (scanProperties.ShouldContinueToExperience)
                        Panel.SetActive(true);
                    else
                        SceneManager.LoadScene("first Scene");

                }
                else
                {
                    outputText.text = scanProperties.detectionObjectName + " Not Found";
                    print (output + "  Not found  " + scanProperties.detectionObjectName.ToLower ());
                }
            }
            // hide loading Canvas and show mainCanvas
            LoadingCanvas.SetActive (false);
            Maincanvas.SetActive (true);
        }
    }
    public IEnumerator TakePicture ()
    {
        Maincanvas.SetActive(false);
        LoadingCanvas.SetActive(true);
        yield return new WaitForEndOfFrame ();
        string path = Application.persistentDataPath + "/Screen-Capture" + ".png";
        // Create a texture the size of the screen, RGB24 format
        int width = Screen.width;
        int height = Screen.height;
        var tex = new Texture2D (width , height , TextureFormat.RGB24 , false);
        // Read screen contents into the texture
        tex.ReadPixels (new Rect (0 , 0 , width , height) , 0 , 0);
        tex.Apply ();
        byte [] bytes = tex.EncodeToPNG ();

        // Encode texture into PNG
        bytes = tex.EncodeToPNG ();
        detectObjectData.bytes = bytes;
        detectObjectData.score = "0.8";
        detectObjectData.detectionObjectName = scanProperties.detectionObjectName.ToLower();
        NetworkManager.Instance.DetectObject (detectObjectData , OnSuscees , OnFailed);
        Destroy (tex);
    }
    private void OnSuscees (NetworkParameters obj)
    {
        DetectObjectResponse detectObjectResponse = (DetectObjectResponse)obj.responseData;
        print (detectObjectResponse.detected);
        LoadingCanvas.SetActive (false);
        Maincanvas.SetActive (true);

        if ( detectObjectResponse.detected.ToLower () == "true" )
        {
            print (output + " true   " + scanProperties.detectionObjectName.ToLower ());
            outputText.text = "Found";
            int counter = inventory.GetScanedCounter(scanProperties.detectionObjectName);
            if (counter == -1)
                Debug.Log("Not Found Object at Inventory");
            counter++;
            inventory.SetObject(scanProperties.detectionObjectName, counter);
            //Network update
            if (scanProperties.ShouldContinueToExperience)
                Panel.SetActive(true);
            else
                SceneLoader.Instance.LoadExperience("UI-UX");
        }
        else
        {
            outputText.text = scanProperties.detectionObjectName + " Not Found";
            print (output + "  Not found  " + scanProperties.detectionObjectName.ToLower ());
        }
    }
    private void OnFailed (NetworkParameters obj)
    {
        print (obj.err.message);
        LoadingCanvas.SetActive (false);
        Maincanvas.SetActive (true);
    }
    #endregion Methods
}