using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Rendering;

//Most of the code from this file comes from:
//https://tutorialsforar.com/how-to-take-screenshots-within-an-app-using-unity/

public class TakeImage : MonoBehaviour
{
    public enum Format { RAW, JPG, PNG, PPM, CSV };
    public enum DatasetType {TRAIN, TEST};

    public Camera myCamera;
    public int captureWidth = 1920;
    public int captureHeight = 1080;
    public Format format = Format.PNG;
    public bool generateData = true; //it enables/disables data generation
    public DatasetType datasetType = DatasetType.TRAIN; //Is the data to train a model or to test it?
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
        road = myCamera.transform.parent.gameObject.GetComponent<Road>();

        outputFolderScreenshot = Directory.GetCurrentDirectory() + "/Screenshots/" + datasetType.ToString().ToLower();
        if(!Directory.Exists(outputFolderScreenshot)) Directory.CreateDirectory(outputFolderScreenshot);

        outputFolderDataset = Directory.GetCurrentDirectory() + "/Dataset";
        if(!Directory.Exists(outputFolderDataset)) Directory.CreateDirectory(outputFolderDataset);

        InvokeRepeating("TakeScreenShot", 3.0f, 3.0f); //take screenshot each 3 seconds
    }

    private string CreateFileName(int width, int height)
    {
        //timestamp to append to the screenshot filename
        string timestamp = System.DateTime.Now.ToString("yyyyMMddTHHmmss");
        // use width, height, and timestamp for unique file 
        var filename = string.Format("/screen_{0}x{1}_{2}_{3}.{4}", width, height, timestamp, myCamera.name, format.ToString().ToLower());
        // return filename
        return filename;
    }

    private void CaptureScreenshot()
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
        // reset the textures and remove the render texture from the Camera since were done reading the screen data
        myCamera.targetTexture = null;
        RenderTexture.active = null;
        // get our filename
        string filename = CreateFileName((int)rect.width, (int)rect.height);
        // get file header/data bytes for the specified image format
        byte[] fileHeader = null;
        byte[] fileData = null;
        //Set the format and encode based on it
        if (format == Format.RAW)
        {
            fileData = screenShot.GetRawTextureData();
        }
        else if (format == Format.PNG)
        {
            fileData = screenShot.EncodeToPNG();
        }
        else if (format == Format.JPG)
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

        string folderPath = outputFolderScreenshot + filename;
        System.IO.File.WriteAllBytes(folderPath, fileData);

        Debug.Log(string.Format("Screenshot Saved {0}, size {1}", filename, fileData.Length));
        isProcessing = false;

        //Cleanup
        Destroy(renderTexture);
        renderTexture = null;
        screenShot = null;
    }

    public void TakeScreenShot()
    {
        if (!isProcessing)
        {
            CaptureScreenshot();
            updateDataset();
        }
        else
        {
            Debug.Log("Currently Processing");
        }
    }

    public void updateDataset()
    {
        if(road == null) return;

        string folderPath = string.Format("{0}/{1}.{2}", outputFolderDataset, datasetType.ToString().ToLower(), Format.CSV.ToString().ToLower());
        TextWriter tw = new StreamWriter(folderPath, true);
        tw.WriteLine(string.Format("{0}, {1}",  road.getRoadNr(), road.getAmountOfCars()));
        tw.Close();
    }
}
