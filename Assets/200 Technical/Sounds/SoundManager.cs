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
        [SerializeField] private AudioSource sourceMusic = null;
        [SerializeField] private AudioClip musicMainMenu = null;
        [SerializeField] private AudioClip musicInGame = null;
        [SerializeField, Enhanced, Range(.1f, 1f)] private float fadeInDuration = 1.0f;
        [SerializeField, Enhanced, Range(.1f, 1f)] private float fadeOutDuration = 1.0f;

        private Queue<AudioSource> sources = new Queue<AudioSource>();
        private Sequence sequence = null;
        #endregion

        public void PlayClip(AudioClip _clip, float _volumeScale = 1.0f)
        {
            AudioSource _source;
            if (sources.Count > 0)
                _source = sources.Dequeue();
            else
                _source = Instantiate(sourcePrefab, Vector3.zero, Quaternion.identity);
            _source.PlayOneShot(_clip, _volumeScale);
            Sequence _s = DOTween.Sequence();
            {
                _s.AppendInterval(_clip.length).SetUpdate(true);
            }
            _s.onComplete += () => sources.Enqueue(_source);
        }

        public void SetAttennuationValue(float _value)
        {
            Debug.Log("Call Set Attenuation");
            if (_value == 0)
                mixer.SetFloat("Volume", -80);
            else { 
            float _dB = Mathf.Lerp(-30f, 0f, _value);
            mixer.SetFloat("Volume", _dB);
            }
        }

        public void SwitchMusicTo(MusicType _type)
        {
            if (sequence.IsActive()) sequence.Kill(true);
            sequence = DOTween.Sequence();
            sequence.Join(sourceMusic.DOFade(0, fadeOutDuration));
            switch (_type)
            {
                case MusicType.Menu:
                    sequence.AppendCallback(() => sourceMusic.clip = musicMainMenu);
                    break;
                case MusicType.InGame:
                    sequence.AppendCallback(() => sourceMusic.clip = musicInGame);
                    break;
            }
            sequence.Append(sourceMusic.DOFade(1, fadeInDuration));
            sequence.AppendCallback(sourceMusic.Play);
            sequence.Play();
        }

    }

    public enum MusicType
    {
        Menu, 
        InGame
    }
}
