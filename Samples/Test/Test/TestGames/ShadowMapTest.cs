namespace Test
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;
    using Nine;
    using Nine.Components;
    using Nine.Graphics;
    using Nine.Graphics.Materials;
    using Nine.Graphics.Primitives;

    public class ShadowMapTest : ITestGame
    {
        public Scene CreateTestScene(GraphicsDevice graphics, ContentManager content)
        {
            var scene = new Scene();

            // NOTE: If you are targeting silverlight, make sure to set the TargetPlatform property
            //       of the content processor to Silverlight.
            var shadowMaterial = content.Load<Material>("Materials/Shadow");

            //((Nine.Graphics.Drawing.DrawingPass)scene.GetDrawingContext(graphics).MainPass.Passes[0]).MaterialUsage = MaterialUsage.Depth;

            //scene.Add(new Nine.Graphics.Cameras.BirdEyeCamera(graphics));
            scene.Add(new Surface(graphics, 1, 256, 256, 32) { Material = shadowMaterial });
            scene.Add(new Nine.Graphics.DirectionalLight(graphics) { Direction = new Vector3(-1, -1, -1), CastShadow = true });
            //scene.Add(new FullScreenQuad(graphics) { Material = new DebugMaterial(graphics) { SamplerState = SamplerState.PointClamp, TextureUsage = TextureUsage.ShadowMap } });
            //scene.Add(new Sprite(graphics) { Size = Vector2.One * 128, Material = new DebugMaterial(graphics) { SamplerState = SamplerState.PointClamp, TextureUsage = TextureUsage.ShadowMap } });
            scene.Add(new Sprite(graphics) { Size = Vector2.One * 128, Material = new BasicMaterial(graphics) { SamplerState = SamplerState.PointClamp, Texture = content.Load<Texture2D>("Textures/bitmap1") } });
            
            var size = 20;
            var step = 10;
            var random = new Random(0);
            for (var x = 0; x < size; x++)
                for (var z = 0; z < size; z++)
                {
                    scene.Add(new Cylinder(graphics)
                    {
                        CastShadow = true,
                        ReceiveShadow = true,
                        Material = shadowMaterial,
                        Transform = Matrix.CreateScale(1, random.Next(5, 15), 1) * Matrix.CreateTranslation(28 + x * step, random.Next(-2, 2), 28 + z * step)
                    });
                }

            return scene;
        }
    }
}
