#region Copyright 2009 (c) Nightin Games
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
using System.ComponentModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace Isles.Components
{
    /// <summary>
    /// Screenshot capturer grabbed from Racing game
    /// </summary>
    public class ScreenshotCapturer : GameComponent
    {
        #region Variables
        /// <summary>
        /// Internal screenshot number (will increase by one each screenshot)
        /// </summary>
        private int screenshotNum = 0;
        private bool pressedLastFrame = false;

        private string screenshotsDirectory;

        public string ScreenshotsDirectory 
        {
            get { return screenshotsDirectory; }

            set 
            {
                screenshotsDirectory = value; 
                screenshotNum = GetCurrentScreenshotNum(); 
            }
        }

        public Keys CaptureKey { get; set; }
        public event EventHandler Captured;
        public string LastScreenshotFile { get; private set; }
        public Texture2D LastScreenshot { get; private set; }

        #endregion

        #region Constructor
        public ScreenshotCapturer(Game game) : base(game)
        {
            if (game == null)
                throw new ArgumentNullException();

            ScreenshotsDirectory = "Screenshots";
            screenshotNum = GetCurrentScreenshotNum();
            CaptureKey = Keys.PrintScreen;
        }
        #endregion

        #region Make screenshot
        #region Screenshot name builder
        /// <summary>
        /// Screenshot name builder
        /// </summary>
        /// <param name="num">Num</param>
        /// <returns>String</returns>
        private string ScreenshotNameBuilder(int num)
        {
            return ScreenshotsDirectory + "/" +
                Game.Window.Title + " Screenshot " +
                num.ToString("0000") + ".png";
        }
        #endregion

        #region Get current screenshot num
        /// <summary>
        /// Get current screenshot num
        /// </summary>
        /// <returns>Int</returns>
        private int GetCurrentScreenshotNum()
        {
            // We must search for last screenshot we can found in list using own
            // fast filesearch
            int i = 0, j = 0, k = 0, l = -1;
            // First check if at least 1 screenshot exist
            if (File.Exists(ScreenshotNameBuilder(0)) == true)
            {
                // First scan for screenshot num/1000
                for (i = 1; i < 10; i++)
                {
                    if (File.Exists(ScreenshotNameBuilder(i * 1000)) == false)
                        break;
                }

                // This i*1000 does not exist, continue scan next level
                // screenshotnr/100
                i--;
                for (j = 1; j < 10; j++)
                {
                    if (File.Exists(ScreenshotNameBuilder(i * 1000 + j * 100)) == false)
                        break;
                }

                // This i*1000+j*100 does not exist, continue scan next level
                // screenshotnr/10
                j--;
                for (k = 1; k < 10; k++)
                {
                    if (File.Exists(ScreenshotNameBuilder(
                            i * 1000 + j * 100 + k * 10)) == false)
                        break;
                }

                // This i*1000+j*100+k*10 does not exist, continue scan next level
                // screenshotnr/1
                k--;
                for (l = 1; l < 10; l++)
                {
                    if (File.Exists(ScreenshotNameBuilder(
                            i * 1000 + j * 100 + k * 10 + l)) == false)
                        break;
                }

                // This i*1000+j*100+k*10+l does not exist, we have now last
                // screenshot nr!!!
                l--;
            }

            return i * 1000 + j * 100 + k * 10 + l;
        }
        #endregion


        /// <summary>
        /// Make screenshot
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "App should not crash from making a screenshot, " +
            "but exceptions are often thrown by this code, see inside.")]
        public Texture2D Capture(bool shouldSave)
        {
            try
            {

                //NOTE: This doesn't always work on all cards, especially if
                // desktop mode switches in fullscreen mode!
                screenshotNum++;

                // Make sure screenshots directory exists
                if (Directory.Exists(ScreenshotsDirectory) == false)
                    Directory.CreateDirectory(ScreenshotsDirectory);


                int width = Game.GraphicsDevice.PresentationParameters.BackBufferWidth;
                int height = Game.GraphicsDevice.PresentationParameters.BackBufferHeight;


                // Get data with help of the resolve method
                Color[] backbuffer = new Color[width * height];
                Game.GraphicsDevice.GetBackBufferData<Color>(backbuffer);

                LastScreenshot = new Texture2D(Game.GraphicsDevice, width, height);

                LastScreenshot.SetData<Color>(backbuffer);

                if (shouldSave)
                {
                    LastScreenshot.SaveAsPng(
                        new FileStream(LastScreenshotFile = ScreenshotNameBuilder(screenshotNum), FileMode.OpenOrCreate),
                        width, height);

                    if (Captured != null)
                        Captured(this, null);
                }

                return LastScreenshot;
            }
            catch (Exception ex)
            {
                if (LastScreenshot != null)
                    LastScreenshot.Dispose();
                return null;
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            bool pressed = Keyboard.GetState().IsKeyDown(CaptureKey);

            if (pressedLastFrame && !pressed)
            {
                pressedLastFrame = false;
                Capture(true);
            }

            pressedLastFrame = pressed;
        }
        #endregion
    }
}