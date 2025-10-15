using Arcadian.System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Audio;

namespace Arcadian.Sound
{
    /// <summary>
    /// A ScriptableObject wrapper for easily playing sound effects via Addressables, supporting custom pitch, clip length, and mixer routing. Useful for managing reusable SFX assets that can be triggered anywhere without needing scene references or persistent AudioSources.
    /// </summary>
    [CreateAssetMenu(menuName = "Misc/Sound Effect", fileName = "New Sound Effect")]
    public class SoundEffect : ScriptableObject
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
            Addressables.InstantiateAsync(
                        ArcadianAssets.Config.SoundEffectInstancePath,
                        Vector3.zero,
                        Quaternion.identity)
                    .Completed +=
                handle =>
                {
                    // Get the instance
                    var soundEffectInstance = handle.Result.GetComponent<SoundEffectInstance>();

                    // Attach the Variables
                    soundEffectInstance.SetClip(Clip);
                    soundEffectInstance.SetMixerGroup(MixerGroup);

                    if (clipLength != null) soundEffectInstance.SetClipLength(clipLength.Value);
                    if (offsetPitch) soundEffectInstance.OffsetPitch();

                    // Play
                    soundEffectInstance.Play();
                };
        }
    }
}