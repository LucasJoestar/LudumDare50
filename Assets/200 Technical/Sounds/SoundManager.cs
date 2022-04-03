// ===== Enhanced Editor - https://github.com/LucasJoestar/LudumDare50 ===== //
//
// ============================================================================ //

using DG.Tweening;
using EnhancedEditor;
using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;
using Range = EnhancedEditor.RangeAttribute;

namespace LudumDare50
{
	public class SoundManager : Singleton<SoundManager>
    {
        #region Global Members
        [Section("SoundManager")]
        [SerializeField] private AudioMixer mixer = null;
        [SerializeField] private AudioSource sourcePrefab = null;

        private Queue<AudioSource> sources = new Queue<AudioSource>();
        #endregion

        public void PlayClip(AudioClip _clip, float _volumeScale = 1.0f)
        {
            AudioSource _source;
            if (sources.Count > 0)
                _source = sources.Dequeue();
            else
            {
                _source = Instantiate(sourcePrefab, Vector3.zero, Quaternion.identity);
                _source.outputAudioMixerGroup = mixer.outputAudioMixerGroup;
            }
            _source.PlayOneShot(_clip, _volumeScale);
            Sequence _s = DOTween.Sequence();
            _s.AppendInterval(_clip.length);
            _s.AppendCallback(() => sources.Enqueue(_source));
        }

        public void SetAttennuationValue(float _value)
        {
            float _dB = Mathf.Lerp(-30f, 0f, _value);
            mixer.SetFloat("Volume", _dB);
        }
    }
}
