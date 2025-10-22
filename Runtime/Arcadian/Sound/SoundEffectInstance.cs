using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

namespace Arcadian.Sound
{
    /// <summary>
    /// A lightweight, self-contained audio component for playing temporary sound effects with optional pitch variance and automatic clean up. Useful for one-shot SFX like explosions, button clicks, or footsteps without manually managing object lifetimes.
    /// </summary>
    [RequireComponent(typeof(AudioSource)), AddComponentMenu("Arcadian/Sound/Sound Effect Instance")]
    public class SoundEffectInstance : MonoBehaviour
    {
        private AudioSource _audioSource;
        
        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        /// <summary>
        /// Set the audio clip.
        /// </summary>
        /// <param name="audioClip">Clip the use.</param>
        public void SetClip(AudioClip audioClip)
        {
            _audioSource.clip = audioClip;
        }

        /// <summary>
        ///  Set the audio mixer group.
        /// </summary>
        /// <param name="audioMixerGroup">Audio mixer group.</param>
        public void SetMixerGroup(AudioMixerGroup audioMixerGroup)
        {
            _audioSource.outputAudioMixerGroup = audioMixerGroup;
        }

        /// <summary>
        /// Set the clip length.
        /// </summary>
        /// <param name="targetLength">Target length.</param>
        public void SetClipLength(float targetLength)
        {
            _audioSource.pitch = _audioSource.clip.length / targetLength;
        }

        /// <summary>
        /// Offset the pitch by a random amount.
        /// </summary>
        public void OffsetPitch()
        {
            _audioSource.pitch += Random.Range(-0.1f, 0.1f);
        }

        /// <summary>
        /// Play the sound effect.
        /// </summary>
        public void Play()
        {
            _audioSource.Play();

            StartCoroutine(LifetimeCheck());
        }

        private IEnumerator LifetimeCheck()
        {
            while (_audioSource.isPlaying)
            {
                yield return null;
            }
            
            // Clip has finished
            Destroy(_audioSource.gameObject);
        }
    }
}