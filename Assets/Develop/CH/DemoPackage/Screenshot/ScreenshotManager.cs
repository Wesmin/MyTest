using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 截图工具，Index需增加window.unityInstance = gameInstance;
/// </summary>
public class ScreenshotManager : MonoBehaviour
{
    // 摄像机
    public Camera targetCamera;
    // 长宽
    public int screenshotWidth = 1920;
    public int screenshotHeight = 1080;
    // 图片存放list
    public List<Texture2D> capturedImages = new List<Texture2D>();
    


   
    private Button ScreenshotBtn;//截图
    private Button ImageDownloadBtn;//下载
    private Button ImageUploadBtn;//上传
    private RawImage previewImage;//图片

    [DllImport("__Internal")]
    private static extern void ImageDownloader(string str, string fn);

    [DllImport("__Internal")]
    private static extern void UploadImage(); // JS中定义文件上传

    void Start()
    {
        //初始化适配
        if (targetCamera == null)
        {
            targetCamera = Camera.main;
        }
        ScreenshotBtn = transform.Find("ScreenshotBtn").GetComponent<Button>();
        ScreenshotBtn.onClick.AddListener(OnClick_Screenshot);
        ImageDownloadBtn = transform.Find("ImageDownloadBtn").GetComponent<Button>();
        ImageDownloadBtn.onClick.AddListener(() => OnClick_DownloadImage(capturedImages[0]));//默认下载截图第一张
        ImageUploadBtn = transform.Find("ImageUploadBtn").GetComponent<Button>();
        ImageUploadBtn.onClick.AddListener(OnClick_UploadImage);
        previewImage = transform.Find("RawImage").GetComponent<RawImage>();
    }

    /// <summary>
    /// 截图
    /// </summary>
    public void OnClick_Screenshot()
    {
        StartCoroutine(CaptureCamera());
        IEnumerator CaptureCamera()
        {
            yield return new WaitForEndOfFrame();

            RenderTexture rt = new RenderTexture(screenshotWidth, screenshotHeight, 24);
            targetCamera.targetTexture = rt;
            Texture2D screenShot = new Texture2D(screenshotWidth, screenshotHeight, TextureFormat.RGB24, false);

            targetCamera.Render();
            RenderTexture.active = rt;
            screenShot.ReadPixels(new Rect(0, 0, screenshotWidth, screenshotHeight), 0, 0);
            screenShot.Apply();

            targetCamera.targetTexture = null;
            RenderTexture.active = null;
            Destroy(rt);

            capturedImages.Add(screenShot);
            Debug.Log("截图已添加到列表，当前总数：" + capturedImages.Count);
        }
    }

  
    /// <summary>
    /// 下载图片
    /// </summary>
    /// <param name="texture"></param>
    public void OnClick_DownloadImage(Texture2D texture)
    {
#if !UNITY_EDITOR && UNITY_WEBGL
        if (texture != null)
        {
            string filename = DateTime.Now.ToString("yyyyMMddHHmmss") + ".jpg";
            byte[] bytes = texture.EncodeToJPG();
            ImageDownloader(Convert.ToBase64String(bytes), filename);
        }
#endif
    }

    /// <summary>
    /// 上传图片
    /// </summary>
    public void OnClick_UploadImage()
    {
#if !UNITY_EDITOR && UNITY_WEBGL
        UploadImage(); // 调用 JS
#endif
    }

    /// <summary>
    /// JS 调用此函数传图
    /// </summary>
    public void OnImageLoadedFromJS(string base64)
    {
        byte[] imageBytes = Convert.FromBase64String(base64);
        Texture2D tex = new Texture2D(2, 2);
        tex.LoadImage(imageBytes);
        if (previewImage != null)
        {
            previewImage.texture = tex;
        }
    }
}
