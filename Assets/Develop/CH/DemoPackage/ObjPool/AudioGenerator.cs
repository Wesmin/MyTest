using System.Collections;
using UnityEngine;

public class AudioGenerator : MonoBehaviour
{
    public ObjectPool objectPool; // 对象池引用

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            GenerateAudio();
        }
    }
    void OnGUI()
    {
        GUIStyle fontStyle = new GUIStyle();
        fontStyle.alignment = TextAnchor.UpperLeft;
        fontStyle.fontSize = 40;
        fontStyle.fontStyle = FontStyle.Bold;
        fontStyle.normal.textColor = new Color(50 / 255f, 50 / 255f, 50 / 255f, 1);
        GUI.Label(new Rect(20, 20, 50, 50), "Q键生成对象，最大数量上限取决于速度", fontStyle);
        GUI.Label(new Rect(20, 70, 50, 50), "该功能主要用于优化，如射击类游戏，生成的子弹", fontStyle);
    }
    void GenerateAudio()
    {
        GameObject audioObject = objectPool.GetObjectFromPool();

        if (audioObject != null)
        {
            // 放置位置
            audioObject.transform.parent = GameObject.Find("Cubes").transform;

            // 随机生成颜色
            Color randomColor = new Color(Random.value, Random.value, Random.value);
            // 将颜色应用到子物体的材质上
            Renderer renderer = audioObject.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = randomColor;
            }


            // 在生成的音频对象上找到 AudioSource 组件
            AudioSource audioSource = audioObject.GetComponent<AudioSource>();

            // 订阅音频播放完成的事件，以便在播放完成后返回对象池
            audioSource.Play();

            float audioDuration = audioSource.clip.length;
            //Invoke("ReturnAudioToPool", audioDuration);
            StartCoroutine(DelayTime(audioDuration, audioSource.gameObject));
        }
    }

    IEnumerator DelayTime(float delayTime,GameObject obj)
    {
        yield return new WaitForSeconds(delayTime);
        // 将音频对象放回对象池
        objectPool.ReturnObjectToPool(obj);
        obj.SetActive(false);
    }
}
