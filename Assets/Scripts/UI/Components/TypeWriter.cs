﻿using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace UI.Components {
	[Serializable]
	[RequireComponent( typeof( TMP_Text ) )]
	public class TypeWriter : MonoBehaviour {
		public TMP_Text m_textUI = null;
		private string m_parsedText;
		private Action m_onComplete;
		private Tween m_tween;

		private void Reset() {
			m_textUI = GetComponent<TMP_Text>();
		}
		
		private void OnDestroy() {
			if ( m_tween != null ) {
				m_tween.Kill();
			}

			m_tween = null;
			m_onComplete = null;
		}
		
		public void Play(string text, float speed, Action onComplete) {
			m_textUI.text = text;
			m_onComplete = onComplete;

			m_textUI.ForceMeshUpdate();

			m_parsedText = m_textUI.GetParsedText();

			var length = m_parsedText.Length;
			var duration = 1 / speed * length;

			OnUpdate( 0 );

			if ( m_tween != null )
			{
				m_tween.Kill();
			}

			m_tween = DOTween
				.To( value => OnUpdate( value ), 0, 1, duration )
				.SetEase( Ease.Linear )
				.OnComplete( () => OnComplete() )
			;
		}
		
		public void Skip( bool withCallbacks = true ) {
			if ( m_tween != null ) {
				m_tween.Kill();
			}

			m_tween = null;
			OnUpdate( 1 );
			if ( !withCallbacks ) return;

			if ( m_onComplete != null ) {
				m_onComplete.Invoke();
			}

			m_onComplete = null;
		}
		
		public void Pause() {
			if ( m_tween != null ) {
				m_tween.Pause();
			}
		}
		
		public void Resume() {
			if ( m_tween != null ) {
				m_tween.Play();
			}
		}

		private void OnUpdate( float value ) {
			if (m_parsedText == null) return;
			var current = Mathf.Lerp( 0, m_parsedText.Length, value );
			var count = Mathf.FloorToInt( current );

			m_textUI.maxVisibleCharacters = count;
		}

		private void OnComplete() {
			m_tween = null;

			if ( m_onComplete != null ) {
				m_onComplete.Invoke();
			}

			m_onComplete = null;
		}
	}
}