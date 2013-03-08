#region License
/* The MIT License
 *
 * Copyright (c) 2011 Red Badger Consulting
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
*/
#endregion

namespace Nine.Graphics.UI
{
    using System;
    using System.Linq;

    using Nine.Graphics.UI.Graphics;
    using Nine.Graphics.UI.Input;
    using Nine.Graphics.UI.Media;
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     RootElement is the main host for all <see cref = "UIElement">UIElement</see>s, it manages the renderer, user input and is the target for Update/Draw calls.
    /// </summary>
    public class Window : UIElement, ISprite
    {
        private readonly IInputManager inputManager;
        private UIElement elementWithMouseCapture;

        /// <summary>
        ///     Initializes a new instance of the <see cref = "Window">RootElement</see> class.
        /// </summary>
        public Window() { }

        /// <summary>
        ///     Initializes a new instance of the <see cref = "Window">RootElement</see> class.
        /// </summary>
        /// <param name = "inputManager">An implementation of <see cref = "IInputManager">IInputManager</see> that can be used to respond to user input.</param>
        public Window(IInputManager inputManager)
        {
            if ((this.inputManager = inputManager) != null)
                this.inputManager.GestureSampled += g => OnNextGesture(g);
        }

        public IInputManager InputManager
        {
            get { return this.inputManager; }
        }

        /// <summary>
        ///     Gets or sets the viewport used by <see cref = "Window">RootElement</see> to layout its content.
        /// </summary>
        public Rectangle? Viewport { get; set; }

        public void Draw(DrawingContext context, Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            Rectangle viewport = this.Viewport.HasValue ? this.Viewport.Value : context.GraphicsDevice.Viewport.Bounds;
            BoundingRectangle bounds = new BoundingRectangle
            {
                X = viewport.X,
                Y = viewport.Y,
                Width = viewport.Width,
                Height = viewport.Height,
            };

            Measure(new Vector2(bounds.Width, bounds.Height));
            Arrange(bounds);


        }

        public bool CaptureMouse(UIElement element)
        {
            if (this.elementWithMouseCapture == null)
            {
                this.elementWithMouseCapture = element;
                return true;
            }
            return false;
        }

        public void ReleaseMouseCapture(UIElement element)
        {
            if (this.elementWithMouseCapture == element)
            {
                this.elementWithMouseCapture = null;
            }
        }

        protected override void OnNextGesture(Gesture gesture)
        {
            if (this.elementWithMouseCapture != null)
                this.elementWithMouseCapture.NotifyGesture(gesture);
            else
                NotifyGesture(this, gesture);
        }

        private static bool NotifyGesture(UIElement element, Gesture gesture)
        {
            var children = element.GetChildren();
            if (children != null)
            {
                var handled = false;
                for (int i = children.Count - 1; i >= 0; i++)
                {
                    if (NotifyGesture(children[i], gesture))
                    {
                        handled = true;
                        break;
                    }
                }

                if (!handled && element is IInputElement && element.HitTest(gesture.Vector2))
                {
                    element.NotifyGesture(gesture);
                    return true;
                }
            }
            return false;
        }

        #region ISprite
        Microsoft.Xna.Framework.Graphics.BlendState ISprite.BlendState
        {
            get { return null; }
        }

        Materials.Material ISprite.Material
        {
            get { return null; }
        }

        Microsoft.Xna.Framework.Graphics.SamplerState ISprite.SamplerState
        {
            get { return null; }
        }

        int ISprite.ZOrder
        {
            get { return 0; }
        }

        void ISprite.Draw(DrawingContext context, Materials.Material material)
        {
            throw new InvalidOperationException();
        }
        #endregion
    }
}