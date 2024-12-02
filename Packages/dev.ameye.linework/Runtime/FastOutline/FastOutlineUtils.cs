using System;
using UnityEngine;

namespace Linework.FastOutline
{
    [Serializable]
    public sealed class ShaderResources
    {
        public Shader clear;
        public Shader mask;
        public Shader outline;

        public ShaderResources Load()
        {
            clear = Shader.Find(Linework.ShaderPath.Clear);
            mask = Shader.Find(ShaderPath.Mask);
            outline = Shader.Find(ShaderPath.Outline);
            return this;
        }
    }
    
    static class ShaderPath
    {
        public const string Mask = "Hidden/Outlines/Fast Outline/Mask";
        public const string Outline = "Hidden/Outlines/Fast Outline/Outline";
    }
    
    static class ShaderPassName
    {
        public const string Mask = "Mask (Fast Outline)";
        public const string Outline = "Outline (Fast Outline)";
    }

    static class ShaderPropertyId
    {
        public static readonly int OutlineColor = Shader.PropertyToID("_OutlineColor");
        public static readonly int OutlineOccludedColor = Shader.PropertyToID("_OutlineOccludedColor");
        public static readonly int OutlineWidth = Shader.PropertyToID("_OutlineWidth");
        public static readonly int MinOutlineWidth = Shader.PropertyToID("_MinimumOutlineWidth");
    }

    static class ShaderFeature
    {
        public const string ScaleWithDistance = "SCALE_WITH_DISTANCE";
        public const string Occlusion = "OCCLUSION";
    }

    public enum MaskingStrategy
    {
        Stencil,
        CullFrontFaces
    }

    public enum ExtrusionMethod
    {
        [InspectorName("Vertex Position (OS)")]
        ObjectSpaceVertexPosition = 0,
        [InspectorName("Normalized Vertex Position (OS)")]
        ObjectSpaceNormalizedVertexPosition = 1,
        [InspectorName("Normal Vector (OS)")]
        ObjectSpaceNormalVector = 2,
        [InspectorName("Vertex Color (OS)")]
        ObjectSpaceVertexColor = 3,
        [InspectorName("Normal Vector (CS)")]
        ClipSpaceNormalVector = 4,
        [InspectorName("Normal Vector (SS)")]
        ScreenSpaceNormalVector = 5,
        [InspectorName("Normal Vector (WS)")]
        WorldSpaceNormalVector = 6,
        [InspectorName("Baked Direction UV8")]
        BakedDirection = 7
    }

    public enum Scaling
    {
        ConstantScreenSize,
        ScaleWithDistance
    }
    
    public enum DebugStage
    {
        None,
        Mask
    }
}