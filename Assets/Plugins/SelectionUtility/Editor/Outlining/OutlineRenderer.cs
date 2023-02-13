namespace Nementic.SelectionUtility
{
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Rendering;
    
    /// <summary>
    /// Renders a selection outline for a list of renderer components for a given camera.
    /// </summary>
    /// <remarks>
    /// This class is based heavily on implementation details provided by Unity's employee
    /// Tim Cooper in this forum thread: https://forum.unity.com/threads/selection-outline.429292/#post-2776318
    /// </remarks>
    internal class OutlineRenderer
    {
        public float blurRadius = 1f;
        public Color outlineColor = new Color(1f, 0.62f, 0.25f, 0.09f);
        public CameraEvent cameraEvent = CameraEvent.BeforeImageEffects;
        
        private readonly Camera camera;
        private readonly CommandBuffer commandBuffer;
        private readonly Material outlineMaterial;
        
        // IDs for temporary render textures.
        private readonly int outlineID;
        private readonly int blurredID;
        private readonly int temporaryID;
        private readonly int depthID;
        private readonly int idID;
        
        /// <summary>
        /// Each collection of renderer components belongs to a different selection root.
        /// </summary>
        private readonly List<Renderer[]> targets = new List<Renderer[]>();

        private int rtWidth;
        private int rtHeight;
        
        public OutlineRenderer(Camera camera)
        {
            this.camera = camera;
            
            commandBuffer = new CommandBuffer { name = "SelectionUtility Outline" };
            outlineMaterial = new Material(Shader.Find("Hidden/UnityOutline"));
            
            depthID = Shader.PropertyToID("_DepthRT");
            outlineID = Shader.PropertyToID("_OutlineRT");
            blurredID = Shader.PropertyToID("_BlurredRT");
            temporaryID = Shader.PropertyToID("_TemporaryRT");
            idID = Shader.PropertyToID("_idRT");
            
            this.camera.AddCommandBuffer(cameraEvent, commandBuffer);
            
            Camera.onPreRender -= OnPreRenderCallback;
            Camera.onPreRender += OnPreRenderCallback;
        }
        
        private void OnPreRenderCallback(Camera camera)
        {
            if (camera != this.camera) 
                return;
            
            if (camera.pixelWidth != rtWidth || 
                camera.pixelHeight != rtHeight)
            {
                RefreshCommandBuffer(); 
            }
        }
        
        public void AddTargets(Renderer[] renderers)
        {
            targets.Add(renderers);
            RefreshCommandBuffer();
        }
        
        private void RefreshCommandBuffer()
        {
            commandBuffer.Clear();

            if (targets.Count == 0)
                return;
            
            rtWidth = camera.pixelWidth;
            rtHeight = camera.pixelHeight;
            
            CreateRenderInstructions();

            commandBuffer.ReleaseTemporaryRT(blurredID);
            commandBuffer.ReleaseTemporaryRT(outlineID);
            commandBuffer.ReleaseTemporaryRT(temporaryID);
            commandBuffer.ReleaseTemporaryRT(depthID);
        }

        private void CreateRenderInstructions()
        {
            commandBuffer.GetTemporaryRT(depthID, rtWidth, rtHeight, 0, FilterMode.Bilinear,
                RenderTextureFormat.ARGB32);
            commandBuffer.SetRenderTarget(depthID, BuiltinRenderTextureType.CurrentActive);
            commandBuffer.ClearRenderTarget(false, true, Color.clear);

            // Render selected objects into a mask buffer, with different colors
            // for visible vs occluded ones (using existing Z buffer for testing).
            RenderTargetsToMaskBuffer();

            // Prepass on object ID to discover edges between roots.
            commandBuffer.GetTemporaryRT(
                idID, rtWidth, rtHeight, 0, FilterMode.Bilinear, RenderTextureFormat.ARGB32);
            
            commandBuffer.Blit(depthID, idID, outlineMaterial, 3);

            // Blur mask in two separable passes, keeping the mask channels.
            commandBuffer.GetTemporaryRT(
                temporaryID, rtWidth, rtHeight, 0, FilterMode.Bilinear, RenderTextureFormat.ARGB32);
            
            commandBuffer.GetTemporaryRT(
                blurredID, rtWidth, rtHeight, 0, FilterMode.Bilinear, RenderTextureFormat.ARGB32);

            commandBuffer.Blit(idID, blurredID);

            commandBuffer.SetGlobalVector("_BlurDirection", new Vector2(blurRadius, 0));
            commandBuffer.Blit(blurredID, temporaryID, outlineMaterial, 2);
            commandBuffer.SetGlobalVector("_BlurDirection", new Vector2(0, blurRadius));
            commandBuffer.Blit(temporaryID, blurredID, outlineMaterial, 2);

            // Blend outline over existing scene image.
            commandBuffer.SetGlobalColor("_OutlineColor", outlineColor);
            commandBuffer.Blit(blurredID, BuiltinRenderTextureType.CameraTarget, outlineMaterial, 4);
        }

        private void RenderTargetsToMaskBuffer()
        {
            if (outlineMaterial == null)
                return;
            
            float objectID = 0f;
            foreach (var selectionRoot in targets)
            {
                objectID += 0.25f;
                commandBuffer.SetGlobalFloat("_ObjectId", objectID);

                foreach (var renderer in selectionRoot)
                {
                    if (renderer == null)
                        continue;

                    commandBuffer.DrawRenderer(renderer, outlineMaterial, 0, 1);
                    commandBuffer.DrawRenderer(renderer, outlineMaterial, 0, 0);
                }
            }
        }

        public void Clear()
        {
            if (commandBuffer == null || camera == null)
                return;
            
            camera.RemoveCommandBuffer(cameraEvent, commandBuffer);
            commandBuffer.Clear();
        }
    }
}