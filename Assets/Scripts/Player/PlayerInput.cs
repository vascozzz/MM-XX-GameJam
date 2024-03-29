﻿using UnityEngine;
using System.Collections;
using InControl;

public class PlayerInput : MonoBehaviour
{
    private InputDevice inputDevice;
    private float horizontalLeniency = 0.1f;
    private float verticalLeniency = 0.8f;

    public void Initialize(int inputDeviceId)
    {
        if (InputManager.Devices.Count - 1 < inputDeviceId)
        {
            Debug.LogError("Not enough controllers connected.");
        }
        else
        {
            inputDevice = InputManager.Devices[inputDeviceId];
        }       
    }

    public float Horizontal()
    {
        return inputDevice != null ? GetInputRaw(inputDevice.Direction.X, horizontalLeniency) : 0;
    }

    public float Vertical()
    {
        return inputDevice != null ? GetInputRaw(inputDevice.Direction.Y, verticalLeniency) : 0;
    }

    public bool Jump()
    {
        return inputDevice != null ? inputDevice.Action1.WasPressed : false;
    }

    public bool Dash()
    {
        return inputDevice != null ? inputDevice.RightBumper.WasPressed : false;
    }

    public bool Red()
    {
        return inputDevice != null ? inputDevice.Action3.WasPressed : false;
    }

    public bool Green()
    {
        return inputDevice != null ? inputDevice.Action4.WasPressed : false;
    }

    public bool Yellow()
    {
        return inputDevice != null ? inputDevice.Action2.WasPressed : false;
    }

    private float GetInputRaw(float input, float leniency)
    {
        if (input > leniency)
        {
            return 1f;
        }
        else if (input < -leniency)
        {
            return -1f;
        }
        else
        {
            return 0f;
        }
    }
}
