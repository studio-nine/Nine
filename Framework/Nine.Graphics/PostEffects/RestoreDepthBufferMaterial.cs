namespace Nine.Graphics.Materials
{
    using System.ComponentModel;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Nine.Graphics.Drawing;
    using Nine.Graphics;
    using DirectionalLight = Nine.Graphics.DirectionalLight;

    partial class RestoreDepthBufferMaterial
    {
        partial void ApplyGlobalParameters(DrawingContext context)
        {
            GraphicsDevice.Textures[0] = context.textures[TextureUsage.DepthBuffer];
            GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;
        }
    }
}