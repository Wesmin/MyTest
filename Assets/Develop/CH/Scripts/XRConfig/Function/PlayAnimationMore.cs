using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TS.Tools
{
    public class PlayAnimationMore : MonoBehaviour
    {
        public List<string> animationNames = new List<string>();
        public float animationSpeed = 1;
        public UnityEvent callback;
        private int Index = 0;
        private Action action;
        AnimationEvent animationEvent = new AnimationEvent();
        public void OnValidate()
        {
            if (animationNames.Count == 0)
            {
                foreach (AnimationState state in GetComponent<Animation>())
                {
                    animationNames.Add(state.name);
                }
            }
        }
        public void Play(Action CallBack = null)
        {
            action = CallBack == null ? null : CallBack;
            Animation animation = GetComponent<Animation>();
            if (animation != null)
            {
                PlaySpecificAnimation(animation, animationNames.Count == 0 ? "" : animationNames[Index], animationSpeed);
                Index++;
            }
            Animator animator = GetComponent<Animator>();
            if (animator != null)
            {
                PlaySpecificAnimation(animator, animationNames.Count == 0 ? "" : animationNames[Index], animationSpeed);
                Index++;
            }
        }
        void PlaySpecificAnimation(Animation animation, string animationClipName, float speed)
        {
            if (animation == null)
                return;
            AnimationClip clip = null;
            foreach (AnimationState state in animation)
            {
                state.speed = speed;
                if (animationClipName == "")
                {
                    clip = state.clip;
                    break;
                }
                else if (state.name == animationClipName)
                {
                    clip = state.clip;
                    break;
                }
            }
            if (clip != null)
            {
                animation.clip = clip;
                animationEvent.time = clip.length; // 设置事件触发时间为动画长度
                animationEvent.functionName = "OnAnimationComplete"; // 回调方法名
                animation.clip.AddEvent(animationEvent);
                animation.Play();
            }
        }
        void PlaySpecificAnimation(Animator animator, string animationClipName, float speed)
        {
            if (animator != null)
            {
                animator.speed = speed;
                animator.Play(animationClipName, 0, 0);
                StartCoroutine(WaitForAnimation(animator, animationClipName));
            }
        }
        IEnumerator WaitForAnimation(Animator animator, string animationClipName)
        {
            while (!animator.GetCurrentAnimatorStateInfo(0).IsName(animationClipName))
            {
                yield return null;
            }

            while (animator.GetCurrentAnimatorStateInfo(0).IsName(animationClipName))
            {
                yield return null;
            }
            OnAnimationComplete();
        }
        // 动画播放结束后的回调方法
        void OnAnimationComplete()
        {
            action?.Invoke();
            callback?.Invoke();
        }
    }
}