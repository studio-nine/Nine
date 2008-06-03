﻿//-----------------------------------------------------------------------------
//  Isles v1.0
//  
//  Copyright 2008 (c) Nightin Games. All Rights Reserved.
//-----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Isles.Graphics;
using Isles.Engine;

namespace Isles.UI
{
    #region Panel
    /// <summary>
    /// Basic Panel
    /// </summary>
    public class Panel : UIElement
    {

        /// <summary>
        /// All UI elements in this panel
        /// </summary>
        /// FIXME: This is not a good design practise...
        protected BroadcastList<IUIElement, List<IUIElement>> elements = new
                  BroadcastList<IUIElement, List<IUIElement>>();


        /// <summary>
        /// Gets all ui elements contained in this panel
        /// </summary>
        public IEnumerable<IUIElement> Elements
        {
            get { return elements; }
        }

        Rectangle effectiveRegion;

        /// <summary>
        /// Gets or sets the region that takes effect
        /// under reference resolution.
        /// </summary>
        public Rectangle EffectiveRegion
        {
            get { return effectiveRegion; }
            set { effectiveRegion = value; }
        }


        Rectangle actualEffectiveRegion;
        public Rectangle ActualEffectiveRegion
        {
            get
            {
                if (IsDirty)
                {
                    actualEffectiveRegion = GetRelativeRectangle(effectiveRegion);
                }
                return actualEffectiveRegion;
            }
        }


        public override Rectangle DestinationRectangle
        {
            get
            {
                if (IsDirty)
                {
                    ResetDestinationRectangle();
                }
                return base.DestinationRectangle;
            }
        }

        public override void ResetDestinationRectangle()
        {
            base.ResetDestinationRectangle();
            actualEffectiveRegion = GetRelativeRectangle(effectiveRegion);
        }

        /// <summary>
        /// Create a panel
        /// </summary>
        /// <param name="area"></param>
        public Panel(Rectangle area)
            : base(area)
        {
            effectiveRegion = Area;
        }

        /// <summary>
        /// Adds an UI element to the panel
        /// </summary>
        /// <param name="element"></param>
        public virtual void Add(IUIElement element)
        {
            element.Parent = this;
            elements.Add(element);
        }

        /// <summary>
        /// Removes an UI elment from the panel
        /// </summary>
        /// <param name="element"></param>
        public virtual void Remove(IUIElement element)
        {
            element.Parent = null;
            elements.Remove(element);
        }

        public virtual void Clear()
        {
            foreach (IUIElement element in elements)
                element.Parent = null;

            elements.Clear();
        }


        protected override void OnEnableStateChanged()
        {
            foreach (IUIElement element in elements)
                element.Enabled = Enabled;
        }

        /// <summary>
        /// Update all UI elements
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            elements.Update();

            foreach (IUIElement element in elements)
                element.Update(gameTime);
        }

        /// <summary>
        /// Draw all UI elements
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime, SpriteBatch sprite)
        {
            if (Visible)
            {
                if (Texture != null)
                    sprite.Draw(Texture, DestinationRectangle, SourceRectangle, Color.White);

                foreach (IUIElement element in elements)
                    element.Draw(gameTime, sprite);
            }
        }

        public override EventResult HandleEvent(EventType type, object sender, object tag)
        {
            if (Visible && Enabled)
            {
                Input input = sender as Input;

                foreach (IUIElement element in elements)
                    if (element.Enabled &&
                        element.HandleEvent(type, sender, tag) == EventResult.Handled)
                        return EventResult.Handled;

                // Block mouse events
                if ((type == EventType.LeftButtonDown || type == EventType.RightButtonDown ||
                     type == EventType.DoubleClick || type == EventType.MiddleButtonDown) &&
                     input.MouseInBox(ActualEffectiveRegion))
                {
                    return EventResult.Handled;
                }
            }

            return EventResult.Unhandled;
        }


    }
    #endregion

    #region ScrollPanel
    /// <summary>
    /// Game scroll panel
    /// </summary>
    public class ScrollPanel : Panel
    {
        /// <summary>
        /// Index of the left most UIElement shown currently
        /// </summary>
        int current;

        /// <summary>
        /// Max number of UIElement visible
        /// </summary>
        int max;

        int buttonWidth, scrollButtonWidth, buttonHeight;

        public Button Left;
        public Button Right;

        public ScrollPanel(Rectangle area, int buttonWidth, int scrollButtonWidth)
            : base(area)
        {
            this.buttonWidth = buttonWidth;
            this.buttonHeight = DestinationRectangle.Height;
            this.scrollButtonWidth = scrollButtonWidth;

            current = 0;

            Left = new Button(new Rectangle(
                0, 0, scrollButtonWidth, buttonHeight));

            Right = new Button(new Rectangle(
                scrollButtonWidth, 0, scrollButtonWidth, buttonHeight));

            Left.Parent = Right.Parent = this;
            Left.Enabled = Right.Enabled = false;
            Left.Anchor = Right.Anchor = Anchor.BottomLeft;
            Left.ScaleMode = Right.ScaleMode = ScaleMode.ScaleY;

            Left.Click += new EventHandler(LeftScroll_Click);
            Right.Click += new EventHandler(RightScroll_Click);
        }

        void RightScroll_Click(object sender, EventArgs e)
        {
            if (Enabled)
            {
                if (current < elements.Count - max)
                {
                    current++;
                    Left.Enabled = true;

                    if (current == elements.Count - max)
                        Right.Enabled = false;
                }
            }
        }

        void LeftScroll_Click(object sender, EventArgs e)
        {
            if (Enabled)
            {
                if (current > 0)
                {
                    current--;
                    Right.Enabled = true;

                    if (current == 0)
                        Left.Enabled = false;
                }
            }
        }

        public override void Add(IUIElement element)
        {
            UIElement e = element as UIElement;

            // Scroll panel works only with UIElement
            if (e == null)
                throw new ArgumentException();

            // Reset element area
            Rectangle rect = new Rectangle(
                scrollButtonWidth + (elements.Count - current) * buttonWidth,
                0, buttonWidth, buttonHeight);

            e.Area = rect;
            e.Anchor = Anchor.BottomLeft;
            e.ScaleMode = ScaleMode.ScaleY;

            if (elements.Count < current + max)
                Right.Enabled = false;
            else
                Right.Enabled = true;

            rect.X += buttonWidth;
            rect.Width = scrollButtonWidth;

            Right.Area = rect;

            base.Add(element);
        }

        public override void Remove(IUIElement element)
        {
            base.Remove(element);

            throw new NotImplementedException();
        }

        public override void Clear()
        {
            current = 0;
            max = (DestinationRectangle.Width - scrollButtonWidth * 2) / buttonWidth;

            Left.Enabled = Right.Enabled = false;

            Right.Area = new Rectangle(
                scrollButtonWidth, 0, scrollButtonWidth, buttonHeight);

            base.Clear();
        }

        public override void Update(GameTime gameTime)
        {
            if (!Visible)
                return;

            Left.Update(gameTime);
            Right.Update(gameTime);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch sprite)
        {
            if (!Visible)
                return;

            Rectangle rect;

            rect.X = scrollButtonWidth;
            rect.Y = 0;
            rect.Width = buttonWidth;
            rect.Height = buttonHeight;

            int n = current + max - 1;
            if (n > elements.Count)
                n = elements.Count;

            for (int i = 0; i < elements.Count; i++)
            {
                if (i >= current && i < n)
                {
                    // Reset element area
                    elements.Elements[i].Visible = true;
                    elements.Elements[i].Area = rect;
                    rect.X += buttonWidth;
                }
                else
                {
                    elements.Elements[i].Visible = false;
                }
            }

            rect.Width /= 2;
            Right.Area = rect;

            Left.Draw(gameTime, sprite);
            Right.Draw(gameTime, sprite);

            base.Draw(gameTime, sprite);
        }
    }
    #endregion

    #region TextField
    public class TextField : Panel
    {
        String text;

        /// <summary>
        /// Gets or sets the text to be displayed
        /// </summary>
        public String Text
        {
            get { return text; }
            set
            {
                text = value;

                if (text == null)
                {
                    formatedText = null;
                    return;
                }

                // Format the input text based on text field size and font size
                formatedText = Graphics2D.FormatString(text, DestinationRectangle.Width,
                                                             DestinationRectangle.Height,
                                                             fontSize, Graphics2D.Font);
                if (centered)
                {
                    lines = formatedText.Split(new char[] { '\n' },
                                               StringSplitOptions.RemoveEmptyEntries);
                }
            }
        }

        String formatedText;

        /// <summary>
        /// Gets the formatted text
        /// </summary>
        public String FormatedText
        {
            get { return formatedText; }
        }


        /// <summary>
        /// Color of the text
        /// </summary>
        Color color;
        public Color Color
        {
            get { return color; }
            set { color = value; }
        }


        bool centered = false;
        /// <summary>
        /// Gets or sets whether the text is centered
        /// </summary>
        public bool Centered
        {
            get { return centered; }
            set 
            { 
                centered = value;

                if (centered)
                {
                    lines = formatedText.Split(new char[] { '\n' },
                                               StringSplitOptions.RemoveEmptyEntries);
                }
            }
        }

        float fontSize = 13f / 23;

        /// <summary>
        /// Gets or sets the font size
        /// </summary>
        public float FontSize
        {
            get { return fontSize; }
            set { fontSize = value; }

        }

        public int RealHeight
        {
            get { return (int)(Graphics2D.Font.MeasureString(formatedText).Y * fontSize); }
        }

        // Whether the text is shadowed
        bool shadowed = false;

        public bool Shadowed
        {
            get { return shadowed; }
            set { shadowed = value; }
        }

        /// <summary>
        /// Color for shadow
        /// </summary>
        private Color shadowColor = Color.Black;

        public Color ShadowColor
        {
            get { return shadowColor; }
            set { shadowColor = value; }
        }
	
        /// <summary>
        /// Constructors
        /// </summary>
        /// <param name="text"></param>
        /// <param name="area"></param>
        public TextField(String text, float fontSize, Color color, Rectangle area)
            : base(area)
        {
            this.color = color;
            this.fontSize = fontSize;
            this.EffectiveRegion = Rectangle.Empty;
            this.Text = text;   // Note this upper case Text
        }

        /// <summary>
        /// Constructors
        /// </summary>
        /// <param name="text"></param>
        /// <param name="area"></param>
        public TextField(String text, float fontSize, Color color, Rectangle area, Color shadowColor)
            : base(area)
        {
            shadowed = true;
            this.shadowColor = shadowColor;
            this.color = color;
            this.fontSize = fontSize;
            this.Text = text;
            this.EffectiveRegion = Rectangle.Empty;
        }

        /// <summary>
        /// Ignore event
        /// </summary>
        public override EventResult HandleEvent(EventType type, object sender, object tag)
        {
            return EventResult.Unhandled;
        }

        String[] lines;

        public override Rectangle DestinationRectangle
        {
            get
            {
                if (IsDirty && text != null)
                {
                    formatedText = Graphics2D.FormatString(text,
                                    base.DestinationRectangle.Width,
                                    base.DestinationRectangle.Height, fontSize,
                                    Graphics2D.Font);
                }
                return base.DestinationRectangle;
            }
        }

        /// <summary>
        /// Draw
        /// </summary>
        public override void Draw(GameTime gameTime, SpriteBatch sprite)
        {
            if (text == null)
                return;

            int width = DestinationRectangle.Width;
            
            if (Centered)
            {
                Vector2 size = Graphics2D.Font.MeasureString(formatedText) * fontSize;
                float heightOffset = (DestinationRectangle.Height - size.Y) / 2 + DestinationRectangle.Top;
                for (int i = 0; i < lines.Length; i++)
                {
                    size = Graphics2D.Font.MeasureString(lines[i]) * fontSize;
                    sprite.DrawString(  Graphics2D.Font, lines[i],
                                        new Vector2((DestinationRectangle.Width - size.X) / 2 + 
                                                    DestinationRectangle.Left, heightOffset),
                                        color, 0, Vector2.Zero, fontSize,
                                        SpriteEffects.None, 0);
                    if (shadowed)
                        sprite.DrawString(Graphics2D.Font, lines[i],
                                        new Vector2((DestinationRectangle.Width - size.X) / 2 + 
                                                    1 + DestinationRectangle.Left, heightOffset + 1),
                                         shadowColor, 0, Vector2.Zero, fontSize,
                                        SpriteEffects.None, 0);
                    heightOffset += size.Y;
                }
            }
            else
            {
                sprite.DrawString( Graphics2D.Font, formatedText, 
                                   new Vector2(DestinationRectangle.X, DestinationRectangle.Y),
                                   color, 0, Vector2.Zero, fontSize,
                                   SpriteEffects.None, 0);
                if (shadowed)
                    sprite.DrawString(  Graphics2D.Font, formatedText,
                                        new Vector2(DestinationRectangle.X + 1, DestinationRectangle.Y + 1),
                                        shadowColor, 0, Vector2.Zero, fontSize,
                                        SpriteEffects.None, 0);
            }
            base.Draw(gameTime, sprite);
        }
    }
    #endregion

    #region TextBox
    public class TextBox : TextField
    {
        public int MaxCharactors
        {
            get { return maxCharactors; }
            set { maxCharactors = value; }
        }

        int maxCharactors = 20;

        public TextBox(float fontSize, Color color, Rectangle area)
            : base("", fontSize, color, area)
        {
        }

        bool flash = false;
        double flashElapsedTime = 0;

        public override void Draw(GameTime gameTime, SpriteBatch sprite)
        {
            flashElapsedTime += gameTime.ElapsedGameTime.TotalSeconds;
            if (flashElapsedTime >= 0.5)
            {
                flashElapsedTime = 0;
                flash = !flash;
            }

            if (flash && Text.Length < maxCharactors)
            {
                Text = Text + "_";
                base.Draw(gameTime, sprite);
                Text = Text.Remove(Text.Length - 1);
            }
            else 
                base.Draw(gameTime, sprite);
        }

        public override EventResult HandleEvent(EventType type, object sender, object tag)
        {
            if (type == EventType.KeyDown && tag is Keys? && sender is Input)
            {
                Input input = sender as Input;
                Keys key = (tag as Keys?).Value;

                // Delete a charactor
                if (key == Keys.Back && Text.Length > 0)
                {
                    Text = Text.Remove(Text.Length - 1);
                    return EventResult.Handled;
                }

                // New charactor
                bool upperCase = input.Keyboard.IsKeyDown(Keys.CapsLock);//input.IsShiftPressed;

                char inputChar = Input.KeyToChar(key, upperCase);

                if (Text.Length < maxCharactors &&
                   (inputChar != ' ' || (inputChar == ' ' && key == Keys.Space)))
                {
                    Text += inputChar;
                }
                
                return EventResult.Handled;
            }

            return EventResult.Unhandled;
        }
    }
    #endregion
}