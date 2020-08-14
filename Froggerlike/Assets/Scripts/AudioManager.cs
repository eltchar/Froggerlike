using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

	public static AudioManager instance;
	public Sound[] sounds;
	public float BGMVolume = 1f;
	public float SFXVolume = 1f;

	void Awake()
	{
		//singleton pattern
		if (instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
		//setting up audio settings
		foreach (Sound s in sounds)
		{
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.clip;
			s.source.loop = s.loop;
		}
	}
	private void Start()
	{
		Play("MainThemeBGM");
	}

	public void Play(string sound)
	{

		//finding desired song then playing it
		Sound s = Array.Find(sounds, item => item.name == sound);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}

		s.source.volume = s.volume;
		s.source.pitch = s.pitch;
		s.source.Play();
	}
	public void Pause(string sound)
	{
		//finding desired song then pause it
		Sound s = Array.Find(sounds, item => item.name == sound);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}

		s.source.volume = s.volume;
		s.source.pitch = s.pitch;
		s.source.Pause();
	}

	public void ChangeSFXVolume(float volume)
	{
		//changing volume of all sound files with SFX in name
		foreach (var s in sounds)
		{
			if (s.name.Contains("SFX"))
			{
				s.volume = volume;
				s.source.volume = s.volume;
			}
		}
		SFXVolume = volume;

	}
	public void ChangeBGMVolume(float volume)
	{
		//changing volume of all sound files with NGM in name
		foreach (var s in sounds)
		{
			if (s.name.Contains("BGM"))
			{
				s.volume = volume;
				s.source.volume = s.volume;
			}
		}
		BGMVolume = volume;
	}

}
