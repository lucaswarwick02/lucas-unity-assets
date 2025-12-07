using UnityEngine;
using UnityEngine.Audio;

namespace LucasWarwick02.UnityAssets
{
    /// <summary>
    /// A ScriptableObject wrapper for easily playing sound effects via Addressables, supporting custom pitch, clip length, and mixer routing. Useful for managing reusable SFX assets that can be triggered anywhere without needing scene references or persistent AudioSources.
    /// </summary>
    [CreateAssetMenu(menuName = "Lucas's Unity Assets/Sound Effect", fileName = "New SFX")]
    public class SFX : ScriptableObject
    {
        [field: SerializeField] public AudioClip Clip { private set; get; }
        [field: SerializeField] public AudioMixerGroup MixerGroup { private set; get; }
    }

    public static class SFXExtensions
    {
        /// <summary>
        /// Play a sound effect. If the object is null, then we exit early.
        /// We use an extension method to allow for an object to call this method even if
        /// the object has not been set, to avoid null errors.
        /// </summary>
        /// <param name="sfx">SFX scriptable object<./param>
        /// <param name="clipLength">Length to shorten the clip to.</param>
        /// <param name="offsetPitch">Whether or not to slightly offset the pitch.</param>
        public static void Play(this SFX sfx, float? clipLength = null, bool offsetPitch = false)
        {
            if (sfx == null) return;

            // Create the object
            var audioSource = new GameObject { name = $"SFX ({sfx.Clip.name})" }.AddComponent<AudioSource>();

            // Set the clip and the mixer group
            audioSource.clip = sfx.Clip;
            audioSource.outputAudioMixerGroup = sfx.MixerGroup;

            // Optional: Adjust pitch to change clip length
            if (clipLength.HasValue) audioSource.pitch = sfx.Clip.length / clipLength.Value;

            // Optional: Offset the pitch
            if (offsetPitch) audioSource.pitch += Random.Range(-0.1f, 0.1f);

            // Play and destroy once ended
            audioSource.Play();
            Object.Destroy(audioSource.gameObject, sfx.Clip.length / audioSource.pitch);
        }
    }
}