#region Copyright 2009 (c) Engine Nine
//=============================================================================
//
//  Copyright 2009 (c) Engine Nine. All Rights Reserved.
//
//=============================================================================
#endregion

#region Using Directives
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace Nine.Components
{
    using Nine.Graphics;

    /// <summary>
    /// Frame rate profiler
    /// </summary>
    public class FrameRate : IDrawable
    {
        private int updateCount = 0;
        private int currentFrame = 0;
        private int counter = 0;
        private TimeSpan elapsedTimeSinceLastUpdate = TimeSpan.Zero;
        private float fps = 0;
        private float overallFps = 0;

        
        /// <summary>
        /// Gets the graphics device.
        /// </summary>
        public GraphicsDevice GraphicsDevice { get; private set; }

        /// <summary>
        /// Time needed to calculate FPS.
        /// </summary>
        public TimeSpan UpdateFrequency { get; set; }

        /// <summary>
        /// Gets or sets the sprite font used to draw FPS string
        /// </summary>
        public SpriteFont Font { get; set; }

        /// <summary>
        /// Gets or sets the color used to draw FPS string
        /// </summary>
        public Color Color { get; set; }

        /// <summary>
        /// Gets or set the frame rate position on the screen
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// Gets the total number of frames since profiler started
        /// </summary>
        public int CurrentFrame
        {
            get { return currentFrame; }
        }

        /// <summary>
        /// Gets the average frame rate up until now
        /// </summary>
        public float OverallFramesPerSecond
        {
            get { return overallFps; }
        }

        /// <summary>
        /// Gets the current Frame Per Second for the game
        /// </summary>
        public float FramesPerSecond
        {
            get { return fps; }
        }

        /// <summary>
        /// The main constructor for the class.
        /// </summary>
        public FrameRate(GraphicsDevice graphics, SpriteFont font)
        {
            this.Font = font;
            this.GraphicsDevice = graphics;
            this.UpdateFrequency = TimeSpan.FromSeconds(1);
            this.Color = Color.Yellow;
        }
        
        public void Draw(TimeSpan elapsedTime)
        {
            UpdateFPS(elapsedTime);

            // Draw FPS text
            if (Font != null && GraphicsDevice != null)
            {
                SpriteBatch spriteBatch = GraphicsResources<SpriteBatch>.GetInstance(GraphicsDevice);

                spriteBatch.Begin();
                spriteBatch.DrawString(Font, string.Format("FPS: {0:00.00}", fps), Position, Color);
                spriteBatch.End();

                GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            }
        }

        private void UpdateFPS(TimeSpan elapsedTime)
        {
            counter++;
            currentFrame++;

            elapsedTimeSinceLastUpdate += elapsedTime;

            if (elapsedTimeSinceLastUpdate >= UpdateFrequency)
            {
                fps = (float)(1000 * counter / elapsedTimeSinceLastUpdate.TotalMilliseconds);
                counter = 0;
                elapsedTimeSinceLastUpdate -= UpdateFrequency;

                overallFps = (overallFps * updateCount + fps) / (updateCount + 1);
                updateCount++;
            }
        }
    }
}