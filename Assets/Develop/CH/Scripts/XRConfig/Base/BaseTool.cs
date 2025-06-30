using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System;
using TMPro;
//using HighlightingSystem;
using UnityEngine.Video;
using System.Timers;
using System.Threading.Tasks;

namespace TS.Tools 
{
    public class BaseTool : MonoBehaviour
    {
        public static BaseTool Instance;

        void Awake()
        {
            Instance = this;
        }
        private void Start()
        {
           
        }


        /// <summary>
        /// 查找物体
        /// </summary>
        public GameObject ModelRoot(string str)
        {
            return transform.FindInAllChild(str).gameObject;
        }
        /// <summary>
        /// 动画播放
        /// </summary>
        public void AnimationPlay(Transform temp, bool play, string clipName = "")
        {
            Animation animation = temp.GetComponent<Animation>();

            if (clipName == "")
            {
                clipName = animation.clip.name;
            }
            // 获取动画状态
            AnimationState animState = animation[clipName];

            if (play)
            {
                // 设置动画为正播
                animState.speed = 1f;
                animState.time = 0; // 从动画的结尾开始播放
            }
            else
            {
                // 设置动画为倒放
                animState.speed = -1f;
                animState.time = animState.length; // 从动画的结尾开始播放
            }

            // 播放动画
            animation.Play(clipName);
        }


        /////////////基础适用方法/////////////

        #region 场景类
        /// <summary>
        /// 当前场景名称
        /// </summary>
        [HideInInspector]
        public string CurrentSceneName;
        /// <summary>
        /// 上级场景名称
        /// </summary>
        [HideInInspector]
        public string UpperSceneName;
        /// <summary>
        /// 获取场景名称
        /// </summary>
        public void GetScene()
        {
            CurrentSceneName = SceneManager.GetActiveScene().name;
        }
        /// <summary>
        /// 卸载场景
        /// </summary>
        /// <param name="sceneName"></param>
        public void CustomUnloadScene(string sceneName)
        {
            SceneManager.UnloadSceneAsync(sceneName);
        }
        /// <summary>
        /// 打开或关闭某个叠加场景的某个根物体
        /// </summary>
        /// <param name="sceneName">叠加场景名</param>
        /// <param name="objectName">根物体名</param>
        public void SceneSetActive(string sceneName, string objectName, bool isActive = true)
        {
            Scene targetScene = SceneManager.GetSceneByName(sceneName);
            if (targetScene.IsValid())
            {
                GameObject[] objectsInScene = targetScene.GetRootGameObjects();

                foreach (GameObject obj in objectsInScene)
                {
                    // 在当前物体及其所有子物体中查找指定名称的物体并设置活跃状态
                    GameObject childObject = FindChildRecursive(obj, objectName);
                    if (childObject != null)
                    {
                        childObject.SetActive(isActive);
                        return; // 如果找到就退出循环
                    }
                }
            }
            else
            {
                Debug.LogWarning("没有找到该场景");
            }
        }
        private GameObject FindChildRecursive(GameObject parent, string childName)
        {
            if (parent.name == childName)
            {
                return parent;
            }
            foreach (Transform child in parent.transform)
            {
                GameObject foundChild = FindChildRecursive(child.gameObject, childName);
                if (foundChild != null)
                {
                    return foundChild;
                }
            }
            return null;
        }
        /// <summary>
        /// 加载场景
        /// </summary>
        /// <param name="sceneName">场景名字</param>
        public void LoadScene(string sceneName)
        {
            StartCoroutine(LoadScenes(sceneName));
        }


        /// <summary>
        /// 加载方法
        /// </summary>
        private bool isLoading = false;
        /// <summary>
        /// 单独叠加一个场景
        /// </summary>
        /// <param name="sceneName">模型场景名字</param>
        /// <param name="onLoadComplete">加载完成后回调</param>
        public void CustomAddScene(string sceneName)
        {
            if (!isLoading)
            {
                StartCoroutine(LoadScenes(sceneName, LoadSceneMode.Additive));
            }
            else
            {
                Debug.LogError("场景正在加载或者卸载,逻辑有问题");
            }
        }
        private IEnumerator LoadScenes(string sceneName, LoadSceneMode sceneMode = LoadSceneMode.Single)
        {
            isLoading = true;
            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneName, sceneMode);
            yield return SceneManager.GetSceneByName(sceneName).isLoaded;
            while (!loadOperation.isDone)
            {
                yield return null;
            }

            SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
            isLoading = false;
        }

        #endregion

        #region 数值转时间
        /// <summary>
        /// 数值转时间
        /// </summary>
        public static string TimeFormat(float time)
        {
            //秒数取整
            int seconds = (int)time;
            //一小时为3600秒 秒数对3600取整即为小时
            int hour = seconds / 3600;
            //一分钟为60秒 秒数对3600取余再对60取整即为分钟
            int minute = seconds % 3600 / 60;
            //对3600取余再对60取余即为秒数
            seconds = seconds % 3600 % 60;
            //返回00:00:00时间格式
            return string.Format("{0:D2}:{1:D2}:{2:D2}", hour, minute, seconds);
        }
        #endregion

        #region 获取指定时间的时间戳
        /// <summary>
        /// 获取指定时间的时间戳
        /// </summary>
        /// <param name="time">时间</param>
        /// <returns>Unix时间戳（毫秒）</returns>
        public static long GetCurrentTimeStamp(DateTime time)
        {
            // 将传入的时间转换为 Unix 时间戳
            long timestamp = new DateTimeOffset(time).ToUnixTimeMilliseconds();
            return timestamp;
        }
        #endregion

        #region 延迟方法

        #region 延迟方法一：Timer延迟
        /// <summary>
        /// Timer延迟
        /// </summary>
        public void TimerDelay(float delaySeconds, Action action)
        {
            // 创建Timer实例
            timer = new Timer(delaySeconds * 1000); // Timer以毫秒为单位，因此乘以1000转换为秒

            // 注册回调方法
            timer.Elapsed += (sender, e) => OnTimerDelay(action);

            // 开始计时
            timer.Start();
        }
        private Timer timer;
        private void OnTimerDelay(Action action)
        {
            // 在此处编写延迟回调的逻辑
            action?.Invoke();

            // 停止计时器
            timer.Stop();
            timer.Dispose(); // 释放计时器资源
        }
        #endregion

        #region 延迟方法二：Task延迟
        /// <summary>
        /// Task延迟
        /// </summary>
        public async void TaskDelay(float delaySeconds, Action action)
        {
            await OnTaskDelay(delaySeconds, action);
        }
        private async Task OnTaskDelay(float delaySeconds, Action action)
        {
            await Task.Delay(TimeSpan.FromSeconds(delaySeconds));
            action?.Invoke();
        }
        #endregion

        #region 延迟方法三：协程延迟
        /// <summary>
        /// 协程延迟
        /// </summary>
        public void CoroutineDelay(float delaySeconds, Action action)
        {
            StartCoroutine(OnCoroutineDelay(delaySeconds, action));
        }
        IEnumerator OnCoroutineDelay(float delaySeconds, Action action)
        {
            yield return new WaitForSeconds(delaySeconds);
            action?.Invoke();
        }
        #endregion

        #region 延迟方法四：DOTween延迟
        /// <summary>
        /// 【DOTween延迟】用于执行延迟方法。（参数：延迟时间、执行方法）
        /// </summary>
        /// <param name="delayTime">延时的时间</param>
        public void Delay(float delayTime, Action action = null)
        {
            float timer = 0;
            DOTween.To(() => timer, x => timer = x, 1, delayTime).OnStepComplete(() => action?.Invoke());
        }
        #endregion

        #endregion

        #region DOTween插件（内含功能：②值变 ③渐变 ④位变 ⑤展缩）  
        /// <summary>
        /// 【DOTween值变】可用于显示屏数值的变化。（参数：数值文本、数值格式、开始值、结束值、持续时间、DoTweenEase曲线）
        /// </summary>
        /// <param name="NumText">数值文本</param>
        /// <param name="NumFormat">数值格式："{0:000.00}"</param>
        /// <param name="StartNum">开始值</param>
        /// <param name="EndNum">结束值</param>
        /// <param name="duration">持续时间</param>
        /// <param name="easeType">DoTweenEase曲线</param>
        public void DoNum(Text NumText, string NumFormat, int StartNum, int EndNum, float duration, Ease easeType)
        {
            DOTween.To(() => StartNum,
                x => NumText.text = string.Format(NumFormat, Mathf.Floor(x)),
                EndNum, duration).SetEase(easeType).SetUpdate(true); //不受Time.Scale影响
        }

        /// <summary>
        /// 【DOTween渐变】可用于跳场景时镜头画面渐入渐出。（参数：物体、透明度范围[0,1]、渐变所需时间）
        /// </summary>
        /// <param name="obj">物体</param>
        /// <param name="Transparent">透明度范围[0,1]</param>
        /// <param name="FadeTime">渐变所需时间</param>
        public void DoFade(GameObject obj, float Transparent, float FadeTime)
        {
            if (!obj.GetComponent<CanvasGroup>()) obj.AddComponent<CanvasGroup>();
            obj.GetComponent<CanvasGroup>().DOFade(Transparent, FadeTime);
        }

        /// <summary>
        /// 【DOTween位变】对象A变换到对象B，包含移动、旋转、缩放。（参数：对象、目标、移动时间）
        /// </summary>
        /// <param name="transA">对象</param>
        /// <param name="targetB">目标</param>
        /// <param name="time">移动时间</param>
        public void DoMove(Transform transA, Transform targetB, float time = 1f)
        {
            transA.DOMove(targetB.position, time);
            transA.DORotate(targetB.eulerAngles, time);
            transA.DOScale(targetB.localScale, time);
        }

        /// <summary>
        /// 【DOTween展UI】快速展开/缩小UI。（参数：对象、展开/缩小、X轴/XYZ轴）
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="bo"></param>
        public void DoPanel(Transform transform, bool isShow = true, float time = 0.3f)
        {
            float scale = isShow ? 1f : 0f;
            transform.DOScale(scale, time);
        }
        #endregion

        #region 协程流程方法及示例写法
        private bool isok;
        public bool Isok { get => isok; set => isok = value; }

        /// <summary>
        /// 返回布尔状态
        /// </summary>
        public bool BoolState() { return isok; }
        /// <summary>
        /// 改变布尔状态
        /// </summary>
        public void ChangeBoolState() { isok = true; }

        //协程中等待交互写法
        //yield return new WaitUntil(BaseTool.Instance.BoolState); BaseTool.Instance.Isok = false;
        #endregion

        ///////////// WEBGL /////////////

        #region 屏幕缩放控制 
        /// <summary>
        /// 屏幕缩放控制
        /// </summary>
        public void FullScreen()
        {
            Screen.fullScreen = !Screen.fullScreen;
        }
        #endregion


        ///////////// XRVR /////////////

        #region 退出程序 
        /// <summary>
        /// 退出程序 
        /// </summary>
        public void QuitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
        #endregion

        #region HighLight插件5.0版本

        ///// <summary>
        ///// 【TS高亮】快速添加、去除高亮方法。（参数：物体、添加高亮/去除高亮）
        ///// </summary>
        ///// <param name="obj"></param>
        ///// <param name="isHighlight"></param>
        //public void HighlightOBJ(GameObject obj, bool isHighlight)
        //{
        //    if (isHighlight)
        //    {
        //        if (obj.GetComponent<Highlighter>() == null)
        //        {
        //            obj.AddComponent<Highlighter>();
        //            obj.GetComponent<Highlighter>().tween = true;
        //            SetHighlighterColor(obj);
        //        }
        //    }
        //    else
        //    {
        //        if (obj.GetComponent<Highlighter>() != null)
        //        {
        //            Highlighter fc = obj.GetComponent<Highlighter>();
        //            Destroy(fc);
        //        }
        //    }
        //}

        ///// <summary>
        ///// 设置颜色  具体可写辅助类重载
        ///// </summary>
        //private void SetHighlighterColor(GameObject obj)
        //{
        //    Gradient gradient = new Gradient();
        //    // 创建渐变的颜色和时间值
        //    GradientColorKey[] colorKeys = new GradientColorKey[2];
        //    colorKeys[0].color = Color.cyan;
        //    colorKeys[0].time = 0f;
        //    colorKeys[1].color = Color.red;
        //    colorKeys[1].time = 1f;

        //    // 创建渐变的透明度和时间值
        //    GradientAlphaKey[] alphaKeys = new GradientAlphaKey[2];
        //    alphaKeys[0].alpha = 1f;
        //    alphaKeys[0].time = 0f;
        //    alphaKeys[1].alpha = 1f;
        //    alphaKeys[1].time = 1f;

        //    // 将颜色和透明度设置到渐变对象中
        //    gradient.SetKeys(colorKeys, alphaKeys);
        //    obj.GetComponent<Highlighter>().tweenGradient = gradient;
        //}

        #endregion

        #region HighLight插件4.0版本
        ///// <summary>
        ///// 快速给物体添加高亮显示
        ///// 参数已经设置好
        ///// 当前可用
        ///// </summary>
        ///// <param name="obj">需要高亮的物体</param>
        //public void HighlightShow(GameObject obj)
        //{
        //    if (obj.GetComponent<FlashingController>() == null)
        //    {
        //        obj.AddComponent<FlashingController>();
        //        obj.GetComponent<FlashingController>().ShowHight();
        //    }
        //    else
        //    {
        //        obj.GetComponent<FlashingController>().ShowHight();
        //    }
        //}
        ///// <summary>
        ///// 去除物体的高亮显示
        ///// 当前可用
        ///// </summary>
        ///// <param name="obj">需要高亮的物体</param>
        //public void HighlightHide(GameObject obj)
        //{
        //    if (obj.GetComponent<FlashingController>() != null)
        //    {
        //        obj.GetComponent<FlashingController>().CloseHight();
        //    }
        //}
        #endregion

        #region 玩家类
        ///// <summary>
        ///// Player传送
        ///// </summary>
        ///// <param name="stepName">黑屏显示的文本</param>
        ///// <param name="posName">传送的位置，该位置在ModelRoot下</param>
        ///// <param name="isWalkZone">如果传送后，玩家不能移动，则将此设为false</param>
        //public void PlayerTransitions(string stepName, string posName, Action action, Action action2 = null)
        //{
        //    GameObject go = LC_PCVR_Manager.instance.HeiMu_VR.transform.parent.Find("BlackFade").gameObject;
        //    Transform player = LC_PCVR_Manager.instance.WholeCamera_VR.transform;
        //    Transform pos = GameManager.instance.ModelRoot.transform.FindInAllChild(posName);
        //    TMP_Text info = go.FindInAllChild("Info").GetComponent<TMP_Text>();
        //    info.text = stepName;
        //    //黑幕等待
        //    DoFade(go, 1F, 1f);
        //    GameManager.instance.DOTweenToTest(1f, () =>
        //    {
        //        action2?.Invoke();
        //        GameManager.instance.DOTweenToTest(1f, () =>
        //        {
        //            //更改位置
        //            player.parent = pos;
        //            player.transform.localPosition = Vector3.zero;
        //            player.transform.localEulerAngles = Vector3.zero;
        //            //回调
        //            action?.Invoke();
        //            //黑幕等待
        //            DoFade(go, 0, 1f);
        //            GameManager.instance.DOTweenToTest(1f, () =>
        //            {
        //                OperationFlowManager.Instance.MoveToNextStep();
        //            });
        //        });
        //    });
        //}
        ///// <summary>
        ///// Player直接传送
        ///// </summary>
        ///// <param name="posName"></param>
        //public void PlayerPosSet(string posName)
        //{
        //    Transform player = LC_PCVR_Manager.instance.WholeCamera_VR.transform;
        //    Transform pos = GameManager.instance.ModelRoot.transform.FindInAllChild(posName);
        //    //更改位置
        //    player.parent = pos;
        //    player.transform.localPosition = Vector3.zero;
        //    player.transform.localEulerAngles = Vector3.zero;
        //}


        ///// <summary>
        ///// 黑场
        ///// </summary>
        ///// <param name="action"></param>
        //public void DarkSecrets(Action action = null)
        //{
        //    GameObject go = LC_PCVR_Manager.instance.HeiMu_VR.transform.parent.Find("BlackFade").gameObject;
        //    //黑幕等待
        //    DoFade(go, 1, 1f);
        //    GameManager.instance.DOTweenToTest(2f, () =>
        //    {
        //        //回调
        //        action?.Invoke();
        //        //黑幕等待
        //        DoFade(go, 0, 1f);
        //        GameManager.instance.DOTweenToTest(1f, () =>
        //        {
        //            OperationFlowManager.Instance.MoveToNextStep();
        //        });
        //    });
        //}
        #endregion
    }
}