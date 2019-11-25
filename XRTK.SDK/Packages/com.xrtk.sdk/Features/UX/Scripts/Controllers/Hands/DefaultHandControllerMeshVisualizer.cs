﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using UnityEngine;
using XRTK.EventDatum.Input;
using XRTK.Providers.Controllers.Hands;

namespace XRTK.SDK.UX.Controllers.Hands
{
    /// <summary>
    /// Default hand controller visualizer for hand meshes.
    /// </summary>
    [RequireComponent(typeof(DefaultMixedRealityControllerVisualizer))]
    public class DefaultHandControllerMeshVisualizer : BaseHandControllerVisualizer
    {
        private DefaultMixedRealityControllerVisualizer controllerVisualizer;
        private MeshFilter meshFilter;

        protected override void Start()
        {
            base.Start();
            controllerVisualizer = GetComponent<DefaultMixedRealityControllerVisualizer>();
        }

        protected override void OnDestroy()
        {
            ClearMesh();
            base.OnDestroy();
        }

        public override void OnHandDataUpdated(InputEventData<HandData> eventData)
        {
            if (eventData.Handedness != controllerVisualizer.Controller?.ControllerHandedness)
            {
                return;
            }

            if (Profile == null || !Profile.EnableHandMeshVisualization || eventData.InputData.Mesh == null)
            {
                ClearMesh();
                return;
            }

            HandMeshData handMeshData = eventData.InputData.Mesh;
            if (handMeshData.Empty)
            {
                return;
            }

            if (meshFilter == null && Profile?.HandMeshPrefab != null)
            {
                CreateMesh();
            }

            if (meshFilter != null)
            {
                Mesh mesh = meshFilter.mesh;

                mesh.vertices = handMeshData.Vertices;
                mesh.normals = handMeshData.Normals;
                mesh.triangles = handMeshData.Triangles;

                if (handMeshData.Uvs != null && handMeshData.Uvs.Length > 0)
                {
                    mesh.uv = handMeshData.Uvs;
                }

                meshFilter.transform.position = handMeshData.Position;
                meshFilter.transform.rotation = handMeshData.Rotation;
            }
        }

        private void ClearMesh()
        {
            if (meshFilter != null)
            {
                Destroy(meshFilter.gameObject);
            }
        }

        private void CreateMesh()
        {
            meshFilter = Instantiate(Profile.HandMeshPrefab).GetComponent<MeshFilter>();
        }
    }
}