using System;
using DG.Tweening;
using UnityEngine;

namespace Ui
{
    public class LoadingScreenUiView : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private Transform sliderTransform;
        [SerializeField] private Transform sliderLeftCorner;
        [SerializeField] private Transform sliderRightCorner;

        private Sequence sliderTravelSequence;
        
        public void Show(float fadeIn, float sliderTravelTime, float sliderTravelInterval)
        {
            canvasGroup.alpha = 0f;
            canvasGroup.DOFade(1f, fadeIn);
            
            sliderTransform.position = sliderLeftCorner.position;
            
            sliderTravelSequence?.Kill();
            sliderTravelSequence = DOTween.Sequence();
            sliderTravelSequence.Append(sliderTransform.DOMove(sliderRightCorner.position, sliderTravelTime));
            sliderTravelSequence.AppendInterval(sliderTravelInterval).OnComplete(delegate
            {
                sliderTransform.position = sliderLeftCorner.position;
            });
            sliderTravelSequence.SetLoops(-1);

        }

        public void Hide(float fadeOut, Action onCompleteCallback)
        {
            canvasGroup.DOFade(0f, fadeOut).OnComplete(delegate
            {
                onCompleteCallback?.Invoke();
            });
        }
    }
}
