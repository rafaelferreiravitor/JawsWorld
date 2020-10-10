﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.SceneManagement
{
    public class Fader : MonoBehaviour
    {

        public CanvasGroup canvasGroup;

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public void FadeOutImmediately()
        {
            canvasGroup.alpha = 1;
        }

        public IEnumerator FadeOutIn()
        {
            yield return FadeOut(3f);
            yield return FadeIn(1f);

        }
        
        public IEnumerator FadeOut(float time)
        {
            while (canvasGroup.alpha < 1f)
            {
                canvasGroup.alpha += Time.deltaTime / time;
                yield return null;
            }
            
        }

        public IEnumerator FadeIn(float time)
        {
            canvasGroup.alpha = 1;
            while (canvasGroup.alpha > 0f)
            {
                canvasGroup.alpha -= Time.deltaTime / time;
                yield return null;
            }
            
        }
    }
}