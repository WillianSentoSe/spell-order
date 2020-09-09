using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SidescrollerPlayer))]
public class SidescrollerPlayerEditor : Editor
{
    private bool advancedProperties = false;

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();

        SidescrollerPlayer _target = (SidescrollerPlayer)target;
        float _space = 15;

        EditorGUILayout.LabelField("Habilities", EditorStyles.boldLabel);
        _target.wallJumpEnabled = EditorGUILayout.Toggle("Wall Jump", _target.wallJumpEnabled);
        _target.canPushObjects = EditorGUILayout.Toggle("Can Push Objects", _target.canPushObjects);
        _target.jumpCount = EditorGUILayout.IntField("Jump Count", _target.jumpCount);
        EditorGUILayout.Space(_space);

        EditorGUILayout.LabelField("Properties", EditorStyles.boldLabel);
        _target.moveSpeed = EditorGUILayout.FloatField("Move Speed", _target.moveSpeed);
        _target.jumpHeight = EditorGUILayout.FloatField("Jump Height", _target.jumpHeight);
        if (_target.wallJumpEnabled) _target.wallJumpForce = EditorGUILayout.FloatField("Wall Jump Force", _target.wallJumpForce);
        EditorGUILayout.Space(_space);

        advancedProperties = EditorGUILayout.Foldout(advancedProperties, "Advanced");

        if (advancedProperties)
        {
            _target.accelerationTime = EditorGUILayout.FloatField("Acceleration Time", _target.accelerationTime);
            _target.accelerationTimeOnAir = EditorGUILayout.FloatField("Acceleration Time on Air", _target.accelerationTimeOnAir);
            _target.waitingTimeForGround = EditorGUILayout.FloatField("Waiting Time for Ground", _target.waitingTimeForGround);
            _target.smoothFallEnabled = EditorGUILayout.Toggle("Smooth Fall Enabled", _target.smoothFallEnabled);
            if (_target.smoothFallEnabled) _target.maxSmoothFallVelocity = EditorGUILayout.FloatField("Max Smooth Fall Velocity", _target.maxSmoothFallVelocity);
            if (_target.wallJumpEnabled) _target.wallJumpTime = EditorGUILayout.FloatField("Wall Jump Time", _target.wallJumpTime);
            if (_target.wallJumpEnabled) _target.wallSlidingVelocity = EditorGUILayout.FloatField("Wall Sliding Velocity", _target.wallSlidingVelocity);
            if (_target.canPushObjects) _target.pushVelocityMultiplayer = EditorGUILayout.FloatField("Push Velocity Multiplier", _target.pushVelocityMultiplayer);
        }
    }
}
