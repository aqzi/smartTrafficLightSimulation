using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using UnityEngine;
using UnityEngine.Rendering;

//Some of the code in this file comes from:
//https://tutorialsforar.com/how-to-take-screenshots-within-an-app-using-unity/

public class TakeImage : MonoBehaviour
{
    public class ObjectToSend
    {
        public int roadNr { get; set; }
        public byte[] img { get; set; }
    }

    public Camera myCamera;
    public int captureWidth = 1920;
    public int captureHeight = 1080;
    public DecisionService decisionService;
    private string outputFolderScreenshot;
    private string outputFolderDataset;

    //variables below this point are needed for the screenshot
    private Rect rect;
    private RenderTexture renderTexture;
    private Texture2D screenShot;
    private bool isProcessing = false;
    private Road road;

    //Initialize Directory
    public void Start()
    {
        if(Settings.current == null) return;

        road = myCamera.transform.parent.gameObject.GetComponent<Road>();

        outputFolderScreenshot = Settings.current.path + "/image_data/" + Settings.current.datasetType.ToString().ToLower();
        if(!Directory.Exists(outputFolderScreenshot)) Directory.CreateDirectory(outputFolderScreenshot);

        outputFolderDataset = Settings.current.path + "/label_data";
        if(!Directory.Exists(outputFolderDataset)) Directory.CreateDirectory(outputFolderDataset);

        InvokeRepeating("TakeScreenShot", 3.0f, 3.0f); //take screenshot each 3 seconds
    }

    private string CreateFileName(int counter)
    {
        return string.Format("/{0}.{1}", counter, Settings.current.format.ToString().ToLower());;
    }

    private void CaptureScreenshot(int counter)
    {
        isProcessing = true;
        // create screenshot objects
        if (renderTexture == null)
        {
            // creates off-screen render texture to be rendered into
            rect = new Rect(0, 0, captureWidth, captureHeight);
            renderTexture = new RenderTexture(captureWidth, captureHeight, 24);
            screenShot = new Texture2D(captureWidth, captureHeight, TextureFormat.RGB24, false);
        }
        // get main camera and render its output into the off-screen render texture created above
        myCamera.targetTexture = renderTexture;
        myCamera.Render();
        // mark the render texture as active and read the current pixel data into the Texture2D
        RenderTexture.active = renderTexture;
        screenShot.ReadPixels(rect, 0, 0);
        TextureScale.Scale(screenShot, 250, 250);
        // reset the textures and remove the render texture from the Camera since were done reading the screen data
        myCamera.targetTexture = null;
        RenderTexture.active = null;
        // get our filename
        string filename = CreateFileName(counter);
        // get file header/data bytes for the specified image format
        byte[] fileHeader = null;
        byte[] fileData = null;
        //Set the format and encode based on it
        if (Settings.current.checkFormat(Settings.Format.RAW))
        {
            fileData = screenShot.GetRawTextureData();
        }
        else if (Settings.current.checkFormat(Settings.Format.PNG))
        {
            fileData = screenShot.EncodeToPNG();
        }
        else if (Settings.current.checkFormat(Settings.Format.JPG))
        {
            fileData = screenShot.EncodeToJPG();
        }
        else //For ppm files
        {
            // create a file header - ppm files
            string headerStr = string.Format("P6\n{0} {1}\n255\n", rect.width, rect.height);
            fileHeader = System.Text.Encoding.ASCII.GetBytes(headerStr);
            fileData = screenShot.GetRawTextureData();
        }

        if(Settings.current.generateData)
        {
            string folderPath = outputFolderScreenshot + filename;
            System.IO.File.WriteAllBytes(folderPath, fileData);

            Debug.Log(string.Format("Screenshot Saved {0}, size {1}", filename, fileData.Length));
        }

        if(Settings.current.mode == Settings.Mode.SMART) decisionService.sendImage(fileData);

        isProcessing = false;

        //Cleanup
        Destroy(renderTexture);
        renderTexture = null;
        screenShot = null;
    }

    public void TakeScreenShot()
    {
        int counter = Settings.current.getCounter();

        if (!isProcessing)
        {
            CaptureScreenshot(counter);
            updateDataset();
        }
        else
        {
            Debug.Log("Currently Processing");
        }
    }

    public void updateDataset()
    {
        if(road == null || !Settings.current.generateData) return;

        string folderPath = string.Format("{0}/{1}.{2}", outputFolderDataset, Settings.current.datasetType.ToString().ToLower(), Settings.Format.CSV.ToString().ToLower());
        TextWriter tw = new StreamWriter(folderPath, true);
        tw.WriteLine(string.Format("{0}, {1}",  road.getRoadNr(), road.getAmountOfCars()));
        tw.Close();
    }
}
