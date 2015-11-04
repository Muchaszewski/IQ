using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using System.Reflection;

namespace Extensions
{
    [AddComponentMenu("Extensions/UI/ExtendedButton", 10)]
    public class ExtendedButton : Selectable, IPointerClickHandler, ISubmitHandler
    {
        protected bool _isPointerInside { get; set; }
        protected bool _isPointerDown { get; set; }
        protected AudioSource _audioSource { get; set; }

        [SerializeField]
        private bool _hasAudioClips;

        [FormerlySerializedAs("audioClipOnHoover"), SerializeField]
        private AudioClip _audioClipHoover;
        [FormerlySerializedAs("audioClipOnPress"), SerializeField]
        private AudioClip _audioClipPress;
        [FormerlySerializedAs("audioClipOnRelase"), SerializeField]
        private AudioClip _audioClipRelase;

        [FormerlySerializedAs("onClick"), SerializeField]
        private Button.ButtonClickedEvent m_OnClick = new Button.ButtonClickedEvent();
        public Button.ButtonClickedEvent onClick
        {
            get
            {
                return this.m_OnClick;
            }
            set
            {
                this.m_OnClick = value;
            }
        }

        public AudioClip AudioClipHoover
        {
            get { return _audioClipHoover; }
            set { _audioClipHoover = value; }
        }

        public AudioClip AudioClipPress
        {
            get { return _audioClipPress; }
            set { _audioClipPress = value; }
        }

        public AudioClip AudioClipRelase
        {
            get { return _audioClipRelase; }
            set { _audioClipRelase = value; }
        }

        public bool HasAudioClips
        {
            get { return _hasAudioClips; }
            set { _hasAudioClips = value; }
        }

        [Serializable]
        public class ButtonClickedEvent : UnityEvent
        {

        }

        protected ExtendedButton()
        {

        }

        protected override void OnEnable()
        {
            base.OnEnable();
            _audioSource = GetComponent<AudioSource>();
        }

        private void Press()
        {
            if (!this.IsActive() || !this.IsInteractable())
            {
                return;
            }
            this.m_OnClick.Invoke();
        }

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
            {
                return;
            }
            this.Press();
        }

        public virtual void OnSubmit(BaseEventData eventData)
        {
            this.Press();
            if (!this.IsActive() || !this.IsInteractable())
            {
                return;
            }
            this.DoStateTransition(Selectable.SelectionState.Pressed, false);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);

            DoStateTransition(SelectionState.Normal, false);
            _isPointerInside = false;
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
            _isPointerDown = false;
            if (_isPointerInside == false)
            {
                this.DoStateTransition(Selectable.SelectionState.Normal, true);
            }
            PlayRelaseSound();
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
            _isPointerInside = true;
            PlayHooverShound();
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            _isPointerDown = true;
            PlayPressSound();
        }

        public void PlayHooverShound()
        {
            if (!this.IsActive() || !this.IsInteractable())
            {
                return;
            }
            if (_audioSource != null)
            {
                if (_audioClipHoover != null)
                {
                    _audioSource.clip = _audioClipHoover;
                    _audioSource.Play();
                }
            }
        }

        public void PlayPressSound()
        {
            if (!this.IsActive() || !this.IsInteractable())
            {
                return;
            }
            if (_audioSource != null)
            {
                if (_audioClipHoover != null)
                {
                    _audioSource.clip = _audioClipPress;
                    _audioSource.Play();
                }
            }
        }

        public void PlayRelaseSound()
        {
            if (!this.IsActive() || !this.IsInteractable())
            {
                return;
            }
            if (_audioSource != null)
            {
                if (_audioClipHoover != null)
                {
                    _audioSource.clip = _audioClipRelase;
                    _audioSource.Play();
                }
            }
        }
    }
}