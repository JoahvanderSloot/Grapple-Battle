using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound {

	public string m_Name;

	public AudioClip m_Clip;

	[Range(0f, 1.5f)]
	public float m_Volume = .75f;
	[Range(0f, 1f)]
	public float m_VolumeVariance = .1f;

	[Range(.1f, 3f)]
	public float m_Pitch = 1f;
	[Range(0f, 1f)]
	public float m_PitchVariance = .1f;

	public bool m_Loop = false;

    public bool m_Music = false;

    public AudioMixerGroup m_MixerGroup;

	[HideInInspector]
	public AudioSource m_Source;

}
