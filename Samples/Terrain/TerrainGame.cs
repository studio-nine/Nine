#region Copyright 2009 - 2010 (c) Nightin Games
//=============================================================================
//
//  Copyright 2009 - 2010 (c) Nightin Games. All Rights Reserved.
//
//=============================================================================
#endregion


#region Using Directives
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Isles;
using Isles.Graphics;
using Isles.Graphics.Cameras;
using Isles.Graphics.Effects;
using Isles.Graphics.Landscape;
#endregion


namespace TerrainSample
{
    /// <summary>
    /// Demonstrates how to create a terrain based on a heightmap.
    /// </summary>
    public class TerrainGame : Microsoft.Xna.Framework.Game
    {
        TopDownEditorCamera camera;

        Terrain terrain;
        BasicEffect basicEffect;
        ScrollEffect scrollEffect;
        SplatterEffect splatterEffect;
        DecalEffect decalEffect;


        public TerrainGame()
        {
            GraphicsDeviceManager graphics = new GraphicsDeviceManager(this);

            graphics.PreferredBackBufferWidth = 900;
            graphics.PreferredBackBufferHeight = 600;

            Content.RootDirectory = "Content";

            IsMouseVisible = true;
        }


        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a topdown perspective editor camera to help us visualize the scene
            camera = new TopDownEditorCamera(this);


            // Create a terrain based on the terrain geometry loaded from file
            terrain = new Terrain(GraphicsDevice, Content.Load<TerrainGeometry>("RF1"), 8);

            // Uncomment next line to create a flat terrain
            //terrain = new Terrain(GraphicsDevice, 1, 128, 128, 8);


            // Initialize terrain effects
            basicEffect = new BasicEffect(GraphicsDevice, null);
            basicEffect.DirectionalLight0.Enabled = true;
            basicEffect.DirectionalLight0.DiffuseColor = Color.Yellow.ToVector3();
            basicEffect.DirectionalLight0.Direction = Vector3.Normalize(-Vector3.One);
            basicEffect.Texture = Content.Load<Texture2D>("grass");
            basicEffect.TextureEnabled = true;
            basicEffect.LightingEnabled = true;
            basicEffect.PreferPerPixelLighting = true;


            scrollEffect = new ScrollEffect(GraphicsDevice);
            scrollEffect.Texture = Content.Load<Texture2D>("clouds");
            scrollEffect.Alpha = 0.5f;
            scrollEffect.Direction = MathHelper.ToRadians(45);
            scrollEffect.TextureScale = Vector2.One * 10;
            scrollEffect.Speed = 0.2f;


            splatterEffect = new SplatterEffect(GraphicsDevice);
            splatterEffect.SplatterTexture = Content.Load<Texture2D>("splat");
            splatterEffect.Textures[0] = Content.Load<Texture2D>("grass");
            splatterEffect.SplatterTextureScale = new Vector2(
                1.0f * terrain.Geometry.TessellationU / terrain.PatchTessellation,
                1.0f * terrain.Geometry.TessellationV / terrain.PatchTessellation);


            decalEffect = new DecalEffect(GraphicsDevice);
            decalEffect.Texture = Content.Load<Texture2D>("checker");
            decalEffect.Position = Vector3.One * 10;
            decalEffect.Rotation = MathHelper.ToRadians(10);
            decalEffect.Scale = Vector2.One * 10;
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// </summary>
        protected override void Update(GameTime gameTime)
        {
            scrollEffect.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// Unproject a point on the screen to a ray in the 3D world
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public Ray Unproject(int x, int y, Matrix view, Matrix projection)
        {
            Ray ray;

            Matrix viewInverse = Matrix.Invert(view);
            Matrix viewProjectionInverse = Matrix.Invert(view * projection);

            Vector3 v;
            v.X = (((2.0f * x) / GraphicsDevice.Viewport.Width) - 1);
            v.Y = -(((2.0f * y) / GraphicsDevice.Viewport.Height) - 1);
            v.Z = 0.0f;

            ray.Position.X = viewInverse.M41;
            ray.Position.Y = viewInverse.M42;
            ray.Position.Z = viewInverse.M43;
            ray.Direction = Vector3.Normalize(
                Vector3.Transform(v, viewProjectionInverse) - ray.Position);

            return ray;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkSlateGray);


            // Toggle wireframe when W is pressed
            if (Keyboard.GetState().IsKeyDown(Keys.W))
                GraphicsDevice.RenderState.FillMode = FillMode.WireFrame;
            else
                GraphicsDevice.RenderState.FillMode = FillMode.Solid;


            // Enable alpha blending
            GraphicsDevice.RenderState.AlphaBlendEnable = true;
            GraphicsDevice.RenderState.AlphaSourceBlend = Blend.SourceAlpha;
            GraphicsDevice.RenderState.DestinationBlend = Blend.InverseSourceAlpha;

            
            // Terrain picking            
            //float? distance;
            //Ray ray = Unproject(Mouse.GetState().X, Mouse.GetState().Y, camera.View, camera.Projection);

            //terrain.Geometry.Pick(ray, out distance);

            //if (distance.HasValue)
            //    decalEffect.Position = ray.Position + ray.Direction * distance.Value;
            //terrain.Geometry.Pick(new Ray(new Vector3(0, 0, 100), -Vector3.UnitZ), out distance);

            // Draw the terrain
            BoundingFrustum frustum = new BoundingFrustum(camera.View * camera.Projection);

            foreach (TerrainPatch patch in terrain.Patches)
            {
                // Cull patches that are outside the view frustum
                if (frustum.Contains(patch.BoundingBox) != ContainmentType.Disjoint)
                {
                    // Setup matrices
                    basicEffect.World = patch.Transform;
                    basicEffect.View = camera.View;
                    basicEffect.Projection = camera.Projection;


                    scrollEffect.World = patch.Transform;
                    scrollEffect.View = camera.View;
                    scrollEffect.Projection = camera.Projection;


                    splatterEffect.World = patch.Transform;
                    splatterEffect.View = camera.View;
                    splatterEffect.Projection = camera.Projection;


                    decalEffect.World = patch.Transform;
                    decalEffect.View = camera.View;
                    decalEffect.Projection = camera.Projection;


                    // Draw each path with the specified effect
                    patch.Draw(basicEffect);
                    patch.Draw(splatterEffect);
                    patch.Draw(scrollEffect);

                    if (patch.BoundingBox.Intersects(decalEffect.BoundingSphere))
                    {
                        patch.Draw(decalEffect);

                        // Need to restore sampler states
                        GraphicsDevice.SamplerStates[0].AddressU =
                        GraphicsDevice.SamplerStates[0].AddressV = TextureAddressMode.Wrap;
                    }
                }
            }


            //GraphicsDevice.DrawSprite(splatterEffect.Textures[0], null, null, Color.White, null);

            base.Draw(gameTime);
        }
    }
}
