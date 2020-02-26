﻿// Copyright (c) XRTK. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections.Generic;
using UnityEngine;
using XRTK.EventDatum.Input;
using XRTK.Providers.Controllers.Hands;

namespace XRTK.SDK.UX.Controllers.Hands
{
    /// <summary>
    /// Hand controller visualizer visuailzing hand joints.
    /// </summary>
    public class HandControllerJointsVisualizer : BaseHandControllerVisualizer
    {
        private readonly Dictionary<TrackedHandJoint, GameObject> jointVisualizations = new Dictionary<TrackedHandJoint, GameObject>();

        [SerializeField]
        [Tooltip("The joint prefab to use.")]
        private GameObject jointPrefab = null;

        [SerializeField]
        [Tooltip("The joint prefab to use for palm.")]
        private GameObject palmPrefab = null;

        [SerializeField]
        [Tooltip("The joint prefab to use for the index tip (point of interaction.")]
        private GameObject fingertipPrefab = null;

        [SerializeField]
        [Tooltip("Material tint color for index fingertip.")]
        private Color indexFingertipColor = Color.cyan;

        public override void OnInputChanged(InputEventData<HandData> eventData)
        {
            base.OnInputChanged(eventData);

            if (eventData.Handedness != Controller.ControllerHandedness)
            {
                return;
            }

            for (int i = 0; i < BaseHandController.JointCount; i++)
            {
                CreateJointVisualizerIfNotExists((TrackedHandJoint)i);
            }
        }

        private void CreateJointVisualizerIfNotExists(TrackedHandJoint handJoint)
        {
            if (jointVisualizations.TryGetValue(handJoint, out _))
            {
                return;
            }

            GameObject prefab = jointPrefab;
            if (handJoint == TrackedHandJoint.Palm)
            {
                prefab = palmPrefab;
            }
            else if (handJoint == TrackedHandJoint.IndexTip || handJoint == TrackedHandJoint.MiddleTip
                || handJoint == TrackedHandJoint.PinkyTip || handJoint == TrackedHandJoint.RingTip || handJoint == TrackedHandJoint.ThumbTip)
            {
                prefab = fingertipPrefab;
            }

            if (prefab != null)
            {
                GameObject jointVisualization = Instantiate(prefab, GetOrCreateJointTransform(handJoint));
                if (handJoint == TrackedHandJoint.IndexTip)
                {
                    Renderer indexJointRenderer = jointVisualization.GetComponent<Renderer>();
                    Material indexMaterial = indexJointRenderer.material;
                    indexMaterial.color = indexFingertipColor;
                    indexJointRenderer.material = indexMaterial;
                }

                jointVisualizations.Add(handJoint, jointVisualization);
            }
        }
    }
}