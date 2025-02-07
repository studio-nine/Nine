﻿#region Copyright 2009 - 2011 (c) Engine Nine
//=============================================================================
//
//  Copyright 2009 - 2011 (c) Engine Nine. All Rights Reserved.
//
//=============================================================================
#endregion

#region Using Directives
using System;
using System.Text;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Diagnostics;
using System.ComponentModel.Composition;
using Nine.Studio;
using Nine.Studio.Controls;
using Nine.Studio.Visualizers;
using Nine.Studio.Extensibility;
using Nine.Components;
using Nine.Content.Graphics.ParticleEffects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace Nine.Graphics.ParticleEffects.Design
{
    [Export(typeof(IDocumentVisualizer))]
    public class ParticleEffectGameVisualizer : GameVisualizer<ParticleEffect>
    {
#if WINDOWS_PHONE
        private const int TargetFrameRate = 30;
        private const int BackBufferWidth = 800;
        private const int BackBufferHeight = 480;
#elif XBOX
        private const int TargetFrameRate = 60;
        private const int BackBufferWidth = 1280;
        private const int BackBufferHeight = 720;
#else
        private const int TargetFrameRate = 60;
        private const int BackBufferWidth = 900;
        private const int BackBufferHeight = 600;
#endif

        ModelViewerCamera camera;
        PrimitiveBatch primitiveBatch;
        
        [EditorBrowsable(EditorBrowsableState.Always)]
        public bool ShowWireframe { get; set; }

        public ParticleEffectGameVisualizer()
        {
            GraphicsDeviceManager graphics = new GraphicsDeviceManager(this);

            graphics.SynchronizeWithVerticalRetrace = false;
            graphics.PreferredBackBufferWidth = BackBufferWidth;
            graphics.PreferredBackBufferHeight = BackBufferHeight;

            TargetElapsedTime = TimeSpan.FromTicks(TimeSpan.TicksPerSecond / TargetFrameRate);

            Content.RootDirectory = "Content";

            IsMouseVisible = true;
            IsFixedTimeStep = false;
        }


        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            Components.Add(new FrameRate(GraphicsDevice, null));
            Components.Add(new InputComponent());

            camera = new ModelViewerCamera(GraphicsDevice);
            primitiveBatch = new PrimitiveBatch(GraphicsDevice, 4096);
            
            base.LoadContent();
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkSlateGray);

            GraphicsDevice.BlendState = BlendState.AlphaBlend;
            GraphicsDevice.DepthStencilState = DepthStencilState.None;

            BoundingFrustum frustum = new BoundingFrustum(
                Matrix.CreateLookAt(new Vector3(0, 15, 15), Vector3.Zero, Vector3.UnitZ) *
                Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, 1, 1, 10));

            primitiveBatch.Begin(PrimitiveSortMode.Deferred, camera.View, camera.Projection);
            {
                primitiveBatch.DrawSphere(new BoundingSphere(Vector3.UnitX * 4, 1), 24, null, Color.White);
                primitiveBatch.DrawGrid(1, 128, 128, null, Color.White * 0.25f);
                primitiveBatch.DrawGrid(8, 16, 16, null, Color.Black);
                primitiveBatch.DrawLine(new Vector3(5, 5, 0), new Vector3(5, 5, 5), Color.Blue);
                primitiveBatch.DrawConstrainedBillboard(null, new Vector3(5, -5, 0), new Vector3(5, -5, 5), 0.05f, null, null, Color.Yellow);
                primitiveBatch.DrawArrow(Vector3.Zero, Vector3.UnitZ * 2, null, Color.White);
                primitiveBatch.DrawBox(new BoundingBox(-Vector3.One, Vector3.One), null, Color.White);
                primitiveBatch.DrawSolidBox(new BoundingBox(-Vector3.One, Vector3.One), null, Color.Yellow * 0.2f);
                primitiveBatch.DrawCircle(Vector3.UnitX * 2, 1, 24, null, Color.Yellow);
                primitiveBatch.DrawSolidSphere(new BoundingSphere(Vector3.UnitX * 4, 1), 24, null, Color.Red * 0.2f);
                primitiveBatch.DrawAxis(Matrix.CreateTranslation(-4, 0, 0));
                primitiveBatch.DrawFrustum(frustum, null, Color.White);
                primitiveBatch.DrawSolidFrustum(frustum, null, Color.Pink * 0.5f);
                primitiveBatch.DrawCentrum(new Vector3(-5, -2, 0), 2, 1, 24, null, Color.WhiteSmoke * 0.5f);
                primitiveBatch.DrawSolidCentrum(new Vector3(-5, -2, 0), 2, 1, 24, null, Color.LawnGreen * 0.3f);
                primitiveBatch.DrawCylinder(new Vector3(-5, -6, 0), 2, 1, 24, null, Color.WhiteSmoke * 0.5f);
                primitiveBatch.DrawSolidCylinder(new Vector3(-5, -6, 0), 2, 1, 24, null, Color.Lavender * 0.3f);
            }
            primitiveBatch.End();

            base.Draw(gameTime);
        }
    }
}
