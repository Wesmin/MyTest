using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CubeTimeTravel : MonoBehaviour
{


    /// <summary>
    /// 生成虚影的父物体
    /// </summary>
    public Transform shadowParent;
    /// <summary>
    /// 生成虚影的预制体
    /// </summary>
    public GameObject shadowPrefab;
    /// <summary>
    /// 存储生成的虚影
    /// </summary>
    public List<GameObject> shadowObjects = new List<GameObject>(); 


    /// <summary>
    /// 计算移动距离
    /// </summary>
    private float moveDistance = 0f;
    /// <summary>
    /// 上一次的位置
    /// </summary>
    private Vector3 lastPosition;


    /// <summary>
    /// 控制移动的速度
    /// </summary>
    public float moveSpeed = 5f;


   
    /// <summary>
    /// 是否开启倒流
    /// </summary>
    private bool isRewinding = false;
    /// <summary>
    /// 倒流速度
    /// </summary>
    public float RewindSpeed = 20f;



    /// <summary>
    /// 时间暂停音效
    /// </summary>
    public AudioSource TimePauseAudio;
    /// <summary>
    /// 是否时间暂停
    /// </summary>
    private bool isTimePause = false;
    /// <summary>
    /// 时间暂停时的启用遮罩画面
    /// </summary>
    public GameObject TimePausePanel;


    /// <summary>
    /// 结束音效
    /// </summary>
    public AudioSource GameOverAudio;
    /// <summary>
    /// 结束画面
    /// </summary>
    public GameObject GameOverPanel;



    /// <summary>
    /// 背景音乐
    /// </summary>
    public AudioSource BGM;


    /// <summary>
    /// 随机生成预制体的父物体
    /// </summary>
    public Transform prefabParent;

    /// <summary>
    /// 生成的预制体列表
    /// </summary>
    public List<GameObject> generatedObjects = new List<GameObject>();

    /// <summary>
    /// 得分
    /// </summary>
    int ScoreNum = 0;

    /// <summary>
    /// GUI提示
    /// </summary>
    void OnGUI()
    {
        GUIStyle fontStyle = new GUIStyle();
        fontStyle.alignment = TextAnchor.UpperLeft;
        fontStyle.fontSize = 40;

        fontStyle.normal.textColor = Color.white;

        GUI.Label(new Rect(20, 20, 50, 60), $"得分：{ScoreNum}", fontStyle);
    }


    void Start()
    {
        foreach (Transform temp in prefabParent)
        {
            generatedObjects.Add(temp.gameObject);
        }
    }

    void Update()
    {
        //如果未开启倒流
        if (!isRewinding)
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            Vector3 movement = new Vector3(horizontal, 0, vertical) * moveSpeed * Time.deltaTime;

            if (movement.magnitude > 0)
            {
                // 时间未暂停，且移动超过2个单位，生成一个虚影
                if (!isTimePause && moveDistance >= 1f) 
                {
                    CreateShadow();
                    moveDistance = 0f;
                }

                transform.Translate(movement);
                lastPosition = transform.position;
                moveDistance += movement.magnitude;
            }
        }

        // 开启倒流
        if (Input.GetKeyDown(KeyCode.Space) && shadowObjects.Count > 0)
        {
            StartCoroutine(RewindMovement());
        }

        // 时间暂停控制
        if (Input.GetKeyDown(KeyCode.Q))
        {
            isTimePause = !isTimePause;
            TimePausePanel.SetActive(isTimePause);
            transform.GetComponent<Rigidbody>().isKinematic = isTimePause;

            BGM.pitch = isTimePause ? 0.5f : 1f;
            TimePauseAudio.Stop();
            TimePauseAudio.Play();
        }
    }

    /// <summary>
    /// 生成虚影
    /// </summary>
    private void CreateShadow()
    {
        GameObject shadow = Instantiate(shadowPrefab, transform.position, Quaternion.identity);
        shadow.transform.parent = shadowParent;
        shadowObjects.Add(shadow);
    }

    /// <summary>
    /// 倒流逻辑
    /// </summary>
    private IEnumerator RewindMovement()
    {
        isRewinding = true;
        transform.GetComponent<Rigidbody>().isKinematic = true;
        BGM.pitch = 3f;

        while (shadowObjects.Count > 0)
        {
            GameObject lastShadow = shadowObjects[shadowObjects.Count - 1];
            Vector3 targetPosition = lastShadow.transform.position;
            shadowObjects.RemoveAt(shadowObjects.Count - 1);

            // 平滑移动到虚影位置
            while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime * RewindSpeed);
                yield return new WaitForSeconds(0.02f);
            }

            // 销毁虚影
            Destroy(lastShadow);
        }

        transform.GetComponent<Rigidbody>().isKinematic = false;
        isRewinding = false;

        BGM.Stop();

        GameOverPanel.SetActive(true);
        GameOverAudio.Play();
    }



    /// <summary>
    /// 当Cube与预制体发生碰撞时，销毁预制体
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        if (isRewinding)
        {
            if (generatedObjects.Contains(other.gameObject))
            {
                // 销毁触碰到的预制体
                Destroy(other.gameObject);
                generatedObjects.Remove(other.gameObject);
                AudioSource audio = transform.GetComponent<AudioSource>();
                audio.Stop();
                audio.Play();
                ScoreNum++;
            }
        }

    }
}
