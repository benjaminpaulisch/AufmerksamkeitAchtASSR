using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPresentationController : MonoBehaviour
{
    public AudioClip leftEarClip;
    public AudioClip rightEarClip;

    private AudioSource leftEarSource;
    private AudioSource rightEarSource;

    private float toneDuration = 1f; // duration of each tone in seconds
    private float interToneInterval = 0.5f; // time between end of one tone and start of next tone in seconds

    void Start()
    {
        // Add AudioSources to the GameObject
        leftEarSource = gameObject.AddComponent<AudioSource>();
        rightEarSource = gameObject.AddComponent<AudioSource>();

        // Set the AudioClip for each AudioSource
        leftEarSource.clip = leftEarClip;
        rightEarSource.clip = rightEarClip;

        // Set the volume for each AudioSource
        leftEarSource.volume = 1f;
        rightEarSource.volume = 1f;
    }

    void Update()
    {
        // Calculate the presentation frequency for each ear
        float leftEarPresentationFrequency = 39f; // Hz
        float rightEarPresentationFrequency = 41f; // Hz

        // Calculate the number of samples per tone
        int numSamplesPerTone = (int)(leftEarSource.clip.frequency * toneDuration);

        // Calculate the number of samples per inter-tone interval
        int numSamplesPerInterToneInterval = (int)(leftEarSource.clip.frequency * interToneInterval);

        // Determine which ear to present the tone to based on the current time
        float time = Time.time;
        bool presentLeftEarTone = (time % (1f / leftEarPresentationFrequency)) < (1f / (2f * leftEarPresentationFrequency));

        // Determine which ear to present the tone to based on the current time
        bool presentRightEarTone = (time % (1f / rightEarPresentationFrequency)) < (1f / (2f * rightEarPresentationFrequency));

        // If it's time to present the left ear tone, play it
        if (presentLeftEarTone)
        {
            leftEarSource.PlayScheduled(AudioSettings.dspTime);
        }

        // If it's time to present the right ear tone, play it
        if (presentRightEarTone)
        {
            rightEarSource.PlayScheduled(AudioSettings.dspTime);
        }

        // Wait for the end of the tone and the inter-tone interval
        double nextPlayTime = AudioSettings.dspTime + toneDuration;
        if (presentLeftEarTone && presentRightEarTone)
        {
            nextPlayTime += interToneInterval;
        }

        // Schedule the next tone to be played
        leftEarSource.SetScheduledEndTime(nextPlayTime);
        rightEarSource.SetScheduledEndTime(nextPlayTime);
    }
}