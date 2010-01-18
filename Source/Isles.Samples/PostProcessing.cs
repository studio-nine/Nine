﻿#region Copyright 2009 (c) Nightin Games
//=============================================================================
//
//  Copyright 2009 (c) Nightin Games. All Rights Reserved.
//
//=============================================================================
#endregion


#region Using Directives
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Formatters.Soap;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Isles.Components;
using Isles.Graphics;
using Isles.Graphics.Cameras;
using Isles.Graphics.Primitives;
using Isles.Graphics.Landscape;
using Isles.Graphics.Models;
using Isles.Graphics.Filters;
#endregion


namespace Isles.Samples
{
    [SampleClass]
    public class PostProcessingGame : BasicModelViewerGame
    {
        public FilterCollection PostEffects { get; set; }

        SpriteBatch sprite;
        Texture2D texture;


        protected override void LoadContent()
        {
            // Chainning post processing effects
            PostEffects = new FilterCollection();

            //PostEffects.Add(new BloomFilter());
            PostEffects.Add(new BlurFilter());
            //PostEffects.Add(new BlurFilter());
            //PostEffects.Add(new SaturationFilter());

            PostEffects[0].RenderTargetScale = 0.1f;


            sprite = new SpriteBatch(GraphicsDevice);
            texture = Content.Load<Texture2D>("Textures/glacier");

            
            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            GraphicsDevice.Clear(Color.Black);


            sprite.Begin();
            sprite.Draw(texture, new Rectangle(
                0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
            sprite.End();


            // Draw post chain
            PostEffects.Draw(GraphicsDevice, null, GraphicsDevice.Viewport.TitleSafeArea);
        }
    
        [SampleMethod(Startup=false)]
        public static void Test()
        {
            using (PostProcessingGame game = new PostProcessingGame())
            {
                game.Run();
            }
        }
    }
}