using System.Runtime.Serialization.Formatters;
using UnityEngine;
using UnityEngine.Audio;


namespace Crockhead.Unity
{
	/// <summary>
	/// 사운드.
	/// </summary>
	[RequireComponent(typeof(AudioSource))]
	public class Sound : MonoBehaviour
	{
		#region INSPECTOR
		[SerializeField] private AudioSource m_AudioSource;
		#endregion

		/// <summary>
		/// 재생 중인지 여부.
		/// </summary>
		private bool m_IsPlaying;

		/// <summary>
		/// 일시 정지 되었는지 여부.
		/// </summary>
		private bool m_IsPaused;

		/// <summary>
		/// 재생 중인지 여부 프로퍼티.
		/// </summary>
		public bool IsPlaying => m_IsPlaying;

		/// <summary>
		/// 일시 정지 중인지 여부.
		/// </summary>
		public bool IsPaused => m_IsPaused;

		/// <summary>
		/// 루프 여부.
		/// </summary>
		public bool IsLoop => m_AudioSource.loop;

		/// <summary>
		/// 현재 시간.
		/// </summary>
		public float Time => m_AudioSource.time;

		/// <summary>
		/// 재생 시간.
		/// </summary>
		public float Duration => m_AudioSource.clip.length;

		/// <summary>
		/// 생성됨.
		/// </summary>
		protected virtual void Awake()
		{
			//base.Awake();

			m_AudioSource = TransformHelper.GetOrAddComponent<AudioSource>(transform);
			m_IsPlaying = false;
			m_IsPaused = false;
		}

		/// <summary>
		/// 해제됨.
		/// </summary>
		protected virtual void OnDestroy()
		{
			//base.OnDestroy();
		}

		/// <summary>
		/// 재생.
		/// </summary>
		public void Play(AudioMixerGroup audioMixerGroup, AudioClip audioClip, bool loop)
		{
			if (m_IsPlaying)
				return;

			m_AudioSource.outputAudioMixerGroup = audioMixerGroup;
			m_AudioSource.clip = audioClip;
			m_AudioSource.loop = loop;
			m_AudioSource.Play();

			m_IsPlaying = true;
			m_IsPaused = false;
		}

		/// <summary>
		/// 정지.
		/// </summary>
		public void Stop()
		{
			if (!m_IsPlaying)
				return;

			m_AudioSource.Stop();
			m_AudioSource.clip = null;
			m_AudioSource.outputAudioMixerGroup = null;

			m_IsPlaying = false;
			m_IsPaused = false;
		}

		/// <summary>
		/// 일시 정지.
		/// </summary>
		public void Pause()
		{
			if (!m_IsPlaying)
				return;
			if (m_IsPaused)
				return;

			m_AudioSource.Pause();

			m_IsPaused = true;
		}

		/// <summary>
		/// 재개.
		/// </summary>
		public void Resume()
		{
			if (!m_IsPlaying)
				return;
			if (!m_IsPaused)
				return;

			m_AudioSource.UnPause();

			m_IsPaused = false;
		}
	}
}