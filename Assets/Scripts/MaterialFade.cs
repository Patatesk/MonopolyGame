using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


namespace PK
{
    public class MaterialFade : MonoBehaviour
    {
        [SerializeField] private Renderer rendereFade;
        private MeshRenderer _renderer;

        private Sequence fadeFlicker;
        private void Awake()
        {
            _renderer = GetComponent<MeshRenderer>();
            fadeFlicker = DOTween.Sequence();
            fadeFlicker.Pause();
            fadeFlicker.Append(rendereFade.material.DOFade(1f,.5f));
            fadeFlicker.Append(rendereFade.material.DOFade(0f,.5f));
            fadeFlicker.SetLoops(-1);
        }
        [ContextMenu("Deneme")]
        public void StartFade()
        {
            _renderer.enabled = true;
            fadeFlicker.Play();
        }
        public void StopFade()
        {
            _renderer.enabled = false;
            fadeFlicker.Pause();
            rendereFade.material.DOFade(0, 0);
        }
    }
}
