using UnityEngine;
using UnityEngine.Audio;

namespace Arcadian.Sound
{
    /// <summary>
    /// A ScriptableObject wrapper for easily playing sound effects via Addressables, supporting custom pitch, clip length, and mixer routing. Useful for managing reusable SFX assets that can be triggered anywhere without needing scene references or persistent AudioSources.
    /// </summary>
    [CreateAssetMenu(menuName = "Arcadian/Sound Effect", fileName = "New SFX")]
    public class SFX : ScriptableObject
    {
        [field: SerializeField] public AudioClip Clip { private set; get; }
        [field: SerializeField] public AudioMixerGroup MixerGroup { private set; get; }

        /// <summary>
        /// Play a sound effect.
        /// </summary>
        /// <param name="clipLength"></param>
        /// <param name="offsetPitch"></param>
        public void Play(float? clipLength = null, bool offsetPitch = false)
        {
            // Create the object
            var audioSource = new GameObject { name = $"SFX ({Clip.name})" }.AddComponent<AudioSource>();

            // Set the clip and the mixer group
            audioSource.clip = Clip;
            audioSource.outputAudioMixerGroup = MixerGroup;

            // Optional: Adjust pitch to change clip length
            if (clipLength.HasValue) audioSource.pitch = Clip.length / clipLength.Value;

            // Optional: Offset the pitch
            if (offsetPitch) audioSource.pitch += Random.Range(-0.1f, 0.1f);

            // Play and destroy once ended
            audioSource.Play();
            Destroy(audioSource.gameObject, Clip.length / audioSource.pitch);
        }
    }
}