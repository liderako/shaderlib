using System;
using DG.Tweening;
using UnityEngine;

namespace redmoongames.shaderlib
{
    public class DissolveComponent : MonoBehaviour
    {
        [SerializeField] private Renderer render;
        [SerializeField] private Ease ease;
        [SerializeField] private float duration;

        [Range(0, 1)] [SerializeField] private float defaultValue; 
        
        private float valueDissolve;
        
        private static readonly int isFill = Shader.PropertyToID("_isFill");
        private static readonly int fillAmount = Shader.PropertyToID("_FillAmount");

        private void Start()
        {
            if (defaultValue == 0)
            {
                render.material.SetFloat(fillAmount, 0);
                render.material.SetInt(isFill, 0);
            }
            else
            {
                render.material.SetFloat(fillAmount, defaultValue);
                render.material.SetInt(isFill, 1);
            }
        }

        public void Appear()
        {
            int fill = render.material.GetInt(isFill);
            if (fill == 0)
            {
                render.material.SetInt(isFill, 1);
            }
            Tween tween = Dissolve(1, 0);
            tween.OnComplete((() =>
            {
                render.material.SetInt(isFill, 0);
            }));
        }
        
        public void Disappear()
        {
            render.material.SetInt(isFill, 1);
            Dissolve(0, 1);
        }

        private Tween Dissolve(float startValue, float endValue)
        {
            valueDissolve = startValue;
            render.material.SetFloat(fillAmount, valueDissolve);
            Tween tween = DOTween.To(() => valueDissolve, x => valueDissolve = x, endValue, duration)
                .SetEase(ease).OnUpdate((() =>
                {
                    render.material.SetFloat(fillAmount, valueDissolve);
                }));
            return tween;
        }
    }
}