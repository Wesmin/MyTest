using UnityEngine;
namespace TS.Tools
{
    public class AnimationFrame : MonoBehaviour
    {
        Animation ani;
        public string animationClipName = "Take 001"; // 更换为你的动画剪辑的名称

        private void Awake()
        {
            ani = transform.GetComponent<Animation>();

            // 检查动画组件是否存在
            if (ani == null)
            {
                Debug.LogError("没有找到Animation");
                return;
            }

            // 检查动画剪辑是否存在
            if (ani[animationClipName] == null)
            {
                Debug.LogError("没有找到clip");
                return;
            }

            // 设置动画在开始时的帧数
            //SetAnimationTime(0);
        }

        public void SetAnimationTime(float time)
        {
            // 设置动画的播放时间
            ani[animationClipName].time = time;

            // 播放动画
            ani.Play(animationClipName);
        }

        // 在指定帧数范围内播放动画
        public void PlayRange(int startFrame, int endFrame)
        {
            // 将帧数转换为时间
            float startTime = startFrame / ani[animationClipName].clip.frameRate;
            float endTime = endFrame / ani[animationClipName].clip.frameRate;

            // 设置动画开始时间
            SetAnimationTime(startTime);

            // 在指定时间停止播放动画
            Invoke("StopAnimation", endTime - startTime);
        }

        // 新增方法：倒放指定帧数范围的动画
        public void PlayReverseRange(int startFrame, int endFrame)
        {
            // 将帧数转换为时间
            float startTime = endFrame / ani[animationClipName].clip.frameRate;
            float endTime = startFrame / ani[animationClipName].clip.frameRate;

            // 设置动画开始时间
            SetAnimationTime(startTime);

            // 设置动画倒放
            ani[animationClipName].speed = -1.0f;

            // 在指定时间停止倒放动画
            Invoke("StopAnimation", endTime - startTime);
        }

        // 停止动画
        public void StopAnimation()
        {
            // 重置动画速度
            ani[animationClipName].speed = 1.0f;

            // 停止动画
            ani.Stop();
        }
    }
}

