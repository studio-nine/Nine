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
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion


namespace Isles.Graphics.Cameras
{
    public class TopDownEditorCamera : ICamera
    {
        public Input Input { get; private set; }
        public Game Game { get; private set; }

        public float Yaw { get; set; }
        public float Pitch { get; set; }
        public float Radius { get; set; }
        public float MinRadius { get; set; }
        public float MaxRadius { get; set; }
        public float Sensitivity { get; set; }


        private Vector3 target = Vector3.Zero;
        private Point startPoint = Point.Zero;


        public Matrix View
        {
            get
            {
                Vector3 eye;

                eye.Z = (float)Math.Sin(Pitch) * Radius;
                eye.Y = (float)Math.Cos(Pitch) * Radius;
                eye.X = (float)Math.Cos(Yaw) * eye.Y;
                eye.Y = (float)Math.Sin(Yaw) * eye.Y;

                eye += target;

                return Matrix.CreateLookAt(eye, target, Vector3.UnitZ);
            }
        }

        public Matrix Projection
        {
            get
            {
                return Matrix.CreatePerspectiveFieldOfView(
                    MathHelper.PiOver4, Game.GraphicsDevice.Viewport.AspectRatio, MinRadius / 10, MaxRadius * 2);
            }
        }

        public TopDownEditorCamera(Game game) : this(game, 50, MathHelper.PiOver2 * 0.6f) { }

        public TopDownEditorCamera(Game game, float radius, float pitch)
        {
            Game = game;
            Game.Components.Add(Input = new Input(game));

            Sensitivity = 1.0f;

            Yaw = -MathHelper.PiOver2;
            Radius = radius;
            MinRadius = 1f;
            MaxRadius = 500f;
            Pitch = pitch;

            Input.MouseDown += new EventHandler<MouseEventArgs>(Input_MouseDown);
            Input.MouseMove += new EventHandler<MouseEventArgs>(Input_MouseMove);
            Input.Wheel += new EventHandler<MouseEventArgs>(Input_Wheel);
        }

        void Input_Wheel(object sender, MouseEventArgs e)
        {
            Radius -= e.WheelDelta * (MaxRadius - MinRadius) * 0.0001f * Sensitivity;

            if (Radius < MinRadius)
                Radius = MinRadius;
            else if (Radius > MaxRadius)
                Radius = MaxRadius;
        }

        void Input_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButton.Right || e.Button == MouseButton.Middle)
            {
                startPoint.X = e.X;
                startPoint.Y = e.Y;
            }
        }

        void Input_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.IsRightButtonDown)
            {
                float dx = e.X - startPoint.X;
                float dy = e.Y - startPoint.Y;

                startPoint.X = e.X;
                startPoint.Y = e.Y;

                target.X -= ((float)Math.Cos(Yaw) * dy - (float)Math.Sin(Yaw) * dx) * 0.1f;
                target.Y -= ((float)Math.Sin(Yaw) * dy + (float)Math.Cos(Yaw) * dx) * 0.1f;
            }
            else if (e.IsMiddleButtonDown)
            {
                float dx = e.X - startPoint.X;

                startPoint.X = e.X;

                Yaw -= dx * 0.002f * MathHelper.Pi;
            }
        }
    }
}