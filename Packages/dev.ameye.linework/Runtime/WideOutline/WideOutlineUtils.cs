using System;
using UnityEngine;

namespace Linework.WideOutline
{
    [Serializable]
    public sealed class ShaderResources
    {
        public Shader clear;
        public Shader mask;
        public Shader silhouette;
        public Shader outline;

        public ShaderResources Load()
        {
            clear = Shader.Find(Linework.ShaderPath.Clear);
            mask = Shader.Find(ShaderPath.Mask);
            silhouette = Shader.Find(ShaderPath.Silhouette);
            outline = Shader.Find(ShaderPath.Outline);
            return this;
        }
    }
    
    static class ShaderPath
    {
        public const string Mask = "Hidden/Outlines/Wide Outline/Mask";
        public const string Silhouette = "Hidden/Outlines/Wide Outline/Silhouette";
        public const string Outline = "Hidden/Outlines/Wide Outline/Outline";
    }
    
    static class ShaderPass
    {
        public const int Silhouette = 0;
        public const int FloodInit = 1;
        public const int FloodJump = 2;
        public const int Outline = 3;
        public const int Sdf = 4;
    }
    
    static class ShaderPassName
    {
        public const string Mask = "Mask (Wide Outline)";
        public const string Silhouette = "Silhouette (Wide Outline)";
        public const string Flood = "Flood (Wide Outline)";
        public const string Outline = "Outline (Wide Outline)";
    }
    
    static class ShaderPropertyId
    {
        public static readonly int OutlineOccludedColor = Shader.PropertyToID("_OutlineOccludedColor");
        public static readonly int OutlineWidth = Shader.PropertyToID("_OutlineWidth");
        public static readonly int MinOutlineWidth = Shader.PropertyToID("_MinimumOutlineWidth");
        public static readonly int RenderScale = Shader.PropertyToID("_RenderScale");
        
        public static readonly int AxisWidthId = Shader.PropertyToID("_AxisWidth");
        public static readonly int SilhouetteBuffer = Shader.PropertyToID("_SilhouetteBuffer");
        public static readonly int SilhouetteDepthBuffer = Shader.PropertyToID("_SilhouetteDepthBuffer");
    }
    
    static class ShaderFeature
    {
        public const string AlphaCutout = "ALPHA_CUTOUT";
        public const string CustomDepth = "CUSTOM_DEPTH";
    }
    
    static class Buffer
    {
        public const string Silhouette = "_SilhouetteBuffer";
        public const string SilhouetteDepth = "_SilhouetteDepthBuffer";
        public const string Ping = "_PingBuffer";
        public const string Pong = "_PongBuffer";
    }

    public enum DebugStage
    {
        None,
        Mask,
        Silhouette,
        Flood
    }

    public enum MaterialType
    {
        Basic,
        Custom
    }
    
    public enum Scaling
    {
        ConstantScreenSize,
        ScaleWithDistance
    }
    
    public enum WideOutlineOcclusion
    {
        Always,
        WhenOccluded,
        WhenNotOccluded,
        AsMask
    }
    
    public enum Pattern
    {
        Solid,
        Checkerboard,
        Dots,
        Stripes
    }
}