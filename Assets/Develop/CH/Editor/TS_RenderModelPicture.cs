//C# Example
using UnityEditor;
using UnityEngine;


[ExecuteInEditMode]
public class TS_RenderModelPicture : EditorWindow
{

	int resWidth = Screen.width * 4;
	int resHeight = Screen.height * 4;

	public Camera myCamera;
	int scale = 1;

	string path = "";
	bool showPreview = true;
	RenderTexture renderTexture;

	bool isTransparent = false;

	[MenuItem("Tools/【TS】裁切截图")]
	public static void ShowWindow()
	{

		EditorWindow editorWindow = EditorWindow.GetWindow(typeof(TS_RenderModelPicture));
		editorWindow.autoRepaintOnSceneChange = true;
		editorWindow.Show();
		editorWindow.title = "【TS】一键裁切截图";

	}

	float lastTime;


	void OnGUI()
	{
		EditorGUILayout.LabelField("分辨率", EditorStyles.boldLabel);
		resWidth = EditorGUILayout.IntField("宽度", resWidth);
		resHeight = EditorGUILayout.IntField("高度", resHeight);

		EditorGUILayout.Space();


		//scale = EditorGUILayout.IntSlider("比例", scale, 1, 15);
		//EditorGUILayout.HelpBox("截图的默认模式是裁剪，所以选择合适的宽度和高度。比例是在不损失质量的情况下倍增或放大渲染的一个因素。", MessageType.None);


		EditorGUILayout.Space();


		GUILayout.Label("保存路径", EditorStyles.boldLabel);

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.TextField(path, GUILayout.ExpandWidth(false));
		if (GUILayout.Button("浏览", GUILayout.ExpandWidth(false)))
			path = EditorUtility.SaveFolderPanel("保存图片的路径", path, Application.dataPath);

		EditorGUILayout.EndHorizontal();

		EditorGUILayout.Space();



		//isTransparent = EditorGUILayout.Toggle(isTransparent,"Transparent Background");



		GUILayout.Label("选择相机", EditorStyles.boldLabel);


		myCamera = EditorGUILayout.ObjectField(myCamera, typeof(Camera), true, null) as Camera;


		if (myCamera == null)
		{
			myCamera = Camera.main;
		}

		isTransparent = EditorGUILayout.Toggle("透明的背景", isTransparent);


		EditorGUILayout.HelpBox("选择要捕捉渲染的相机。您可以使用透明度选项使背景透明。", MessageType.None);

		EditorGUILayout.Space();
		EditorGUILayout.BeginVertical();
		EditorGUILayout.LabelField("默认选项", EditorStyles.boldLabel);


		if (GUILayout.Button("设置屏幕大小"))
		{
			resHeight = (int)Handles.GetMainGameViewSize().y;
			resWidth = (int)Handles.GetMainGameViewSize().x;

		}


		if (GUILayout.Button("默认大小"))
		{
			resHeight = 1440;
			resWidth = 2560;
			scale = 1;
		}



		EditorGUILayout.EndVertical();

		EditorGUILayout.Space();
		EditorGUILayout.LabelField("截图分别率 " + resWidth * scale + " x " + resHeight * scale + " px", EditorStyles.boldLabel);

		if (GUILayout.Button("截图", GUILayout.MinHeight(60)))
		{
			if (path == "")
			{
				path = EditorUtility.SaveFolderPanel("保存图片的路径", path, Application.dataPath);
				Debug.Log("路径设置");
				TakeHiResShot();
			}
			else
			{
				TakeHiResShot();
			}
		}

		EditorGUILayout.Space();
		EditorGUILayout.BeginHorizontal();

		if (GUILayout.Button("打开最后一个截图", GUILayout.MaxWidth(160), GUILayout.MinHeight(40)))
		{
			if (lastScreenshot != "")
			{
				Application.OpenURL("file://" + lastScreenshot);
				Debug.Log("打开文件" + lastScreenshot);
			}
		}

		if (GUILayout.Button("打开文件夹", GUILayout.MaxWidth(160), GUILayout.MinHeight(40)))
		{

			Application.OpenURL("file://" + path);
		}

		//if (GUILayout.Button("更多的资产", GUILayout.MaxWidth(100), GUILayout.MinHeight(40)))
		//{
		//	Application.OpenURL("https://www.assetstore.unity3d.com/en/#!/publisher/5951");
		//}

		EditorGUILayout.EndHorizontal();


		if (takeHiResShot)
		{
			int resWidthN = resWidth * scale;
			int resHeightN = resHeight * scale;
			RenderTexture rt = new RenderTexture(resWidthN, resHeightN, 24);
			myCamera.targetTexture = rt;

			TextureFormat tFormat;
			if (isTransparent)
				tFormat = TextureFormat.ARGB32;
			else
				tFormat = TextureFormat.RGB24;


			Texture2D screenShot = new Texture2D(resWidthN, resHeightN, tFormat, false);
			myCamera.Render();
			RenderTexture.active = rt;
			screenShot.ReadPixels(new Rect(0, 0, resWidthN, resHeightN), 0, 0);
			myCamera.targetTexture = null;
			RenderTexture.active = null;
			byte[] bytes = screenShot.EncodeToPNG();
			string filename = ScreenShotName(resWidthN, resHeightN);

			System.IO.File.WriteAllBytes(filename, bytes);
			Debug.Log(string.Format("图片保存位置: {0}", filename));
			Application.OpenURL(filename);
			takeHiResShot = false;
		}

		EditorGUILayout.HelpBox("如果出现任何错误，请确保您有Unity Pro，因为插件需要Unity Pro才能工作。", MessageType.Info);
	}



	private bool takeHiResShot = false;
	public string lastScreenshot = "";


	public string ScreenShotName(int width, int height)
	{

		string strPath = "";

		strPath = string.Format("{0}/screen_{1}x{2}_{3}.png",
							 path,
							 width, height,
									   System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
		lastScreenshot = strPath;

		return strPath;
	}



	public void TakeHiResShot()
	{
		Debug.Log("采取截图");
		takeHiResShot = true;
	}

}
