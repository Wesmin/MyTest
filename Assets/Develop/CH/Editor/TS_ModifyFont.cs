using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class TS_ModifyFont : EditorWindow
{
    private static TS_ModifyFont _window;
    public Font ModifyFontText;//字体
    Transform[] childs = null;//子物体
    bool isThisObject = true;
    private GUIStyle defaultStyle = new GUIStyle();//定义 GUIStyle  
    [MenuItem("Tools/【TS】替换字体")]
    public static void WindowGUI()
    {
        Rect wr = new Rect (0, 0, 300, 200);//窗口大小
        _window = (TS_ModifyFont)GetWindowWithRect (typeof (TS_ModifyFont),wr, true, "【TSTools】");
        _window.Show ();

    }
    void OnGUI()
    {

        //文本
        defaultStyle.alignment = TextAnchor.MiddleCenter; //字体对齐方式
        defaultStyle.normal.textColor = Color.yellow; //字体颜色
        defaultStyle.fontSize = 15;//字体大小
        defaultStyle.fontStyle = FontStyle.Bold;//字体格式
        EditorGUILayout.LabelField("【一键替换字体】", defaultStyle); //绘制GUI组件，使用 defaultStyle
              
        //文本
        defaultStyle.alignment = TextAnchor.MiddleLeft;
        defaultStyle.normal.textColor = Color.cyan;
        defaultStyle.fontSize = 12;
        defaultStyle.fontStyle = FontStyle.Normal;
        childs = Selection.transforms;      
        if (childs.Length==0)
        {
            EditorGUILayout.LabelField("修改当前选中UI下的全部字体，需修改保存", defaultStyle);
            isThisObject = false;
        }
        else
        {
            string str = "";
            foreach (var item in childs)
            {
                str += " '" + item.name + "' ";
            }
            EditorGUILayout.LabelField("修改 " + str + " 下的全部字体", defaultStyle);
            isThisObject = true;
        }
        //字体
        ModifyFontText = (Font)EditorGUILayout.ObjectField("用于替换的字体", ModifyFontText, typeof(Font), true);
        //按钮
        GUI.color = Color.yellow;
        GUILayout.BeginHorizontal();
        if (GUILayout.Button(new GUIContent("替换字体", EditorGUIUtility.FindTexture("PlayButton"))))
        {
            modifyFont();
            GameObject go = new GameObject();
            EditorUtility.SetDirty(go);
            DestroyImmediate(go);//刷新场景更新字体
        }
        GUILayout.EndHorizontal();
    }

    /// <summary>
    /// 替换方法
    /// </summary>
    public void modifyFont()
    {
        if (ModifyFontText)
        {
            if (isThisObject)
            {
                foreach (Transform transforms in childs)
                {
                    foreach (Transform childs in transforms.GetComponentsInChildren<Transform>(true))
                    {
                        modifyText (childs.gameObject);
                    }
                }
            }
            else
            {
                foreach (GameObject item in GetAllSceneObjectsWithInactive ())
                {
                    modifyText (item);
                }
            }
        }
    } 
    /// <summary>
    /// 替换字体
    /// </summary>
    /// <param name="obj"></param>
    void modifyText(GameObject obj)
    {
        if (obj.GetComponent<Text> ())
        {
            obj.GetComponent<Text> ().font = ModifyFontText;
            Debug.Log(string.Format("<color=red>{0}</color>", obj.name) + "的字体已替换成" + string.Format("<color=yellow>{0}</color>", ModifyFontText.name));

        }
        if (obj.GetComponent<TextMesh> ())
        {
            obj.GetComponent<TextMesh> ().font = ModifyFontText;
            Debug.Log(string.Format("<color=red>{0}</color>", obj.name) + "的字体已替换成" + string.Format("<color=yellow>{0}</color>", ModifyFontText.name));

        }
    }

    private static List<GameObject> GetAllSceneObjectsWithInactive()
    {
        var allTransforms = Resources.FindObjectsOfTypeAll (typeof (Transform));
        var previousSelection = Selection.objects;
        Selection.objects = allTransforms.Cast<Transform> ()
            .Where (x => x != null)
            .Select (x => x.gameObject)
            .Cast<UnityEngine.Object> ().ToArray ();

        var selectedTransforms = Selection.GetTransforms (SelectionMode.Editable | SelectionMode.ExcludePrefab);
        Selection.objects = previousSelection;

        return selectedTransforms.Select (tr => tr.gameObject).ToList ();
    }

}
