using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TrafficLightPole
{
    public GameObject poleObject;
    public Light greenLight;
    public Light yellowLight;
    public Light redLight;
    public bool isGreen = false; // Flag to track if the green light is on
    public bool isYellow = false; // Flag to track if the yellow light is on
    public bool isRed = false; // Flag to track if the red light is on
}

public class fourtintersectioncontroller : MonoBehaviour
{
    public float greenLightDuration = 5f;
    public float yellowLightDuration = 2f;
    public float redLightDuration = 5f;

    public TrafficLightPole[] trafficLightPoles; // Array to hold all traffic light poles
    public GameObject[] signals; // Array of signals corresponding to each pole

    private int currentPoleIndex = 0; // Index of the current active traffic light pole
    private float timer = 0f; // Timer to track the duration of each light

    private void Start()
    {
        // Initialize all traffic light poles
        for (int i = 0; i < trafficLightPoles.Length; i++)
        {
            // Set the initial state of each pole
            bool isGreen = i == currentPoleIndex;
            bool isRed = !isGreen;

            SwitchLights(i, isGreen, false, isRed);

            // Deactivate the signal for this pole
            if (signals.Length > i && signals[i] != null)
            {
                signals[i].SetActive(false);
            }
        }

        // Activate the signal for the initial green pole
        if (signals.Length > currentPoleIndex && signals[currentPoleIndex] != null)
        {
            signals[currentPoleIndex].SetActive(true);
        }
    }

    private void Update()
    {
        // Update the timer
        timer += Time.deltaTime;

        // Handle light switching when the timer exceeds the duration
        if (timer >= greenLightDuration && trafficLightPoles[currentPoleIndex].isGreen)
        {
            SwitchLights(currentPoleIndex, false, true, false);
        }
        else if (timer >= yellowLightDuration && trafficLightPoles[currentPoleIndex].isYellow)
        {
            SwitchLights(currentPoleIndex, false, false, true);
        }
        else if (timer >= redLightDuration && trafficLightPoles[currentPoleIndex].isRed)
        {
            // Immediately switch to the next pole
            SwitchToNextPole();
        }
        else if (timer >= 0.1f && trafficLightPoles[currentPoleIndex].isRed)
        {
            // If the current pole is in its red phase and a small delay has passed,
            // immediately switch to the next pole
            SwitchToNextPole();
        }
    }

    private void SwitchToNextPole()
    {
        // Move to the next pole
        currentPoleIndex = (currentPoleIndex + 1) % trafficLightPoles.Length;

        // Reset the timer
        timer = 0f;

        // Switch its lights to green immediately
        SwitchLights(currentPoleIndex, true, false, false);

        // Deactivate all signals
        DeactivateAllSignals();

        // Find the pole that is currently showing the red light
        int redLightPoleIndex = -1;
        for (int i = 0; i < trafficLightPoles.Length; i++)
        {
            if (trafficLightPoles[i].isRed)
            {
                redLightPoleIndex = i;
                break;
            }
        }

        // Activate the signal for the pole that is currently showing the red light
        if (redLightPoleIndex != -1 && signals.Length > redLightPoleIndex && signals[redLightPoleIndex] != null)
        {
            signals[redLightPoleIndex].SetActive(true);
        }
    }


    private void DeactivateAllSignals()
    {
        foreach (GameObject signal in signals)
        {
            if (signal != null)
            {
                signal.SetActive(false);
            }
        }
    }

    private void SwitchLights(int poleIndex, bool green, bool yellow, bool red)
    {
        // Reset the timer
        timer = 0f;

        // Switch lights for the specified pole
        TrafficLightPole currentPole = trafficLightPoles[poleIndex];
        currentPole.isGreen = green;
        currentPole.isYellow = yellow;
        currentPole.isRed = red;

        // Update the lights
        currentPole.greenLight.color = green ? Color.green : Color.black;
        currentPole.yellowLight.color = yellow ? Color.yellow : Color.black;
        currentPole.redLight.color = red ? Color.red : Color.black;
    }
}
