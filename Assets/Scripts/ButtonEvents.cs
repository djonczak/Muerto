using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Game.Menu 
{
    public class ButtonEvents : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private GameObject skullTop;
        [SerializeField] private GameObject skullBottom;
       private Button button;
        private Animator topAnimator;
        private Animator bottomAnimator;

        private const string StartKey = "Start";
        private const string StopKey = "Stop";

        private void Awake()
        {
            button = GetComponent<Button>();
        }

        private void Start()
        {
            topAnimator = skullTop.GetComponent<Animator>();
            bottomAnimator = skullBottom.GetComponent<Animator>();

            button.onClick.AddListener(PressedButton);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (button.IsInteractable())
            {
                topAnimator.SetTrigger(StartKey);
                bottomAnimator.SetTrigger(StartKey);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (button.IsInteractable())
            {
                topAnimator.SetTrigger(StopKey);
                bottomAnimator.SetTrigger(StopKey);
            }
        }

        private void PressedButton()
        {
            topAnimator.SetTrigger(StopKey);
            bottomAnimator.SetTrigger(StopKey);
        }
    }
}
