using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Menu
{
    public class ImageCanvasConventer : MonoBehaviour
    {
        [SerializeField] private RuntimeAnimatorController controller;
        [SerializeField] private Sprite sprite;

        private Image image;
        private SpriteRenderer fakeRenderer;
        private Animator animator;

        private void Awake()
        {
            image = gameObject.GetComponent<Image>();
            fakeRenderer = gameObject.AddComponent<SpriteRenderer>();
            animator = gameObject.AddComponent<Animator>();
        }

        private void Start()
        {
            fakeRenderer.enabled = false;
            animator.runtimeAnimatorController = controller;
        }

        private void Update()
        {
            if (animator.runtimeAnimatorController)
            {
                sprite = fakeRenderer.sprite;
                image.sprite = fakeRenderer.sprite;
            }
        }
    }
}
