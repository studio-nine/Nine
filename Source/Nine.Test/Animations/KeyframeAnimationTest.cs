﻿namespace Nine.Animations.Test
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    ///This is a test class for KeyframeAnimationTest and is intended
    ///to contain all KeyframeAnimationTest Unit Tests
    ///</summary>
    [TestClass()]
    public class KeyframeAnimationTest
    {
        class TestAnimation : KeyframeAnimation
        {
            public TestAnimation()
            {
                Repeat = 1;
                FramesPerSecond = 1;
                TotalFrames = 5;
            }

            protected override void OnSeek(int startFrame, int endFrame, float percentage)
            {
                if (!(startFrame >= 0 && startFrame < TotalFrames))
                    throw new Exception();
                Assert.IsTrue(percentage >= 0 && percentage <= 1);
                Assert.IsTrue(startFrame >= 0 && startFrame < TotalFrames);
                Assert.IsTrue(endFrame >= 0 && endFrame < TotalFrames);
            }
        }

        [TestMethod()]
        public void DurationTest()
        {
            TestAnimation animation = new TestAnimation();
            Assert.AreEqual<TimeSpan>(TimeSpan.FromSeconds(5), animation.Duration);
        }

        [TestMethod()]
        public void IsPlayingTest()
        {
            TestAnimation animation = new TestAnimation();
            Assert.AreNotEqual<AnimationState>(AnimationState.Playing, animation.State);

            ((IAnimation)animation).OnStarted();
            Assert.AreEqual<AnimationState>(AnimationState.Playing, animation.State);

            animation.Update(5);
            Assert.AreNotEqual<AnimationState>(AnimationState.Playing, animation.State);
        }

        [TestMethod()]
        public void DirectionTest()
        {
            TestAnimation animation = new TestAnimation();
            
            animation.StartupDirection = AnimationDirection.Backward;
            ((IAnimation)animation).OnStarted();
            animation.Update(2);

            Assert.AreEqual<AnimationState>(AnimationState.Playing, animation.State);
            Assert.AreEqual<TimeSpan>(TimeSpan.FromSeconds(3), animation.Position);
        }

        [TestMethod()]
        public void RepeatTest()
        {
            TestAnimation animation = new TestAnimation();

            animation.Repeat = 2.5f;
            ((IAnimation)animation).OnStarted();

            animation.Update(12.4f);
            Assert.AreEqual<AnimationState>(AnimationState.Playing, animation.State);

            animation.Update(0.1f);
            Assert.AreNotEqual<AnimationState>(AnimationState.Playing, animation.State);
        }

        [TestMethod()]
        public void AutoReverseTest()
        {
            TestAnimation animation = new TestAnimation();

            animation.Repeat = 2.25f;
            animation.AutoReverse = true;
            ((IAnimation)animation).OnStarted();

            animation.Update(6);
            Assert.AreEqual<AnimationState>(AnimationState.Playing, animation.State);
            Assert.AreEqual<TimeSpan>(TimeSpan.FromSeconds(4), animation.Position);

            animation.Update(5);
            Assert.AreEqual<TimeSpan>(TimeSpan.FromSeconds(1), animation.Position);
            Assert.AreEqual<AnimationState>(AnimationState.Playing, animation.State);

            animation.Update(1.5f);
            Assert.AreNotEqual<AnimationState>(AnimationState.Playing, animation.State);
        }

        [TestMethod()]
        public void SeekTest()
        {
            TestAnimation animation = new TestAnimation();

            ((IAnimation)animation).OnStarted();

            animation.Seek(TimeSpan.FromSeconds(4.5f));
            Assert.AreEqual<AnimationState>(AnimationState.Playing, animation.State);
            Assert.AreEqual<TimeSpan>(TimeSpan.FromSeconds(4.5), animation.Position);

            animation.Seek(2);
            Assert.AreEqual<AnimationState>(AnimationState.Playing, animation.State);
            Assert.AreEqual<TimeSpan>(TimeSpan.FromSeconds(2), animation.Position);

            animation.Update(3);
            Assert.AreNotEqual<AnimationState>(AnimationState.Playing, animation.State);

            animation.StartupDirection = AnimationDirection.Backward;
            ((IAnimation)animation).OnStarted();
            animation.Seek(1);
            Assert.AreEqual<AnimationState>(AnimationState.Playing, animation.State);

            animation.Update(1.001f);
            Assert.AreNotEqual<AnimationState>(AnimationState.Playing, animation.State);
        }

        [TestMethod()]
        public void SpeedTest()
        {
            TestAnimation animation = new TestAnimation();

            animation.Speed = 2;
            animation.Repeat = 2.25f;
            animation.AutoReverse = true;
            ((IAnimation)animation).OnStarted();

            animation.Update(3);
            Assert.AreEqual<AnimationState>(AnimationState.Playing, animation.State);
            Assert.AreEqual<TimeSpan>(TimeSpan.FromSeconds(4), animation.Position);

            animation.Update(2.5f);
            Assert.AreEqual<TimeSpan>(TimeSpan.FromSeconds(1), animation.Position);
            Assert.AreEqual<AnimationState>(AnimationState.Playing, animation.State);

            animation.Update(0.75f);
            Assert.AreNotEqual<AnimationState>(AnimationState.Playing, animation.State);
        }

        [TestMethod()]
        public void EnterFrameTest()
        {
            TestAnimation animation = new TestAnimation();

            int index = 0;

            animation.EnterFrame += (o, e) =>
                {
                    System.Diagnostics.Trace.WriteLine(e.Frame);
                    if (index < animation.TotalFrames)
                        Assert.AreEqual(index, e.Frame);
                    index++;
                };
            ((IAnimation)animation).OnStarted();

            for (int i = 0; i < 20; ++i)
                animation.Update(0.25f);

            Assert.AreNotEqual<AnimationState>(AnimationState.Playing, animation.State);
            Assert.AreEqual(6, index);
        }

        [TestMethod]
        public void KeyframeEndingTest()
        {
            TestAnimation animation = new TestAnimation();
            animation.Repeat = 9999;
            animation.Ending = KeyframeEnding.Clamp;
            ((IAnimation)animation).OnStarted();

            for (int i = 0; i < 5; ++i)
            {
                animation.Update(1);
            }
            Assert.AreEqual(0, animation.CurrentFrame);

            animation.Ending = KeyframeEnding.Discard;
            ((IAnimation)animation).OnStarted();

            for (int i = 0; i < 5; ++i)
            {
                animation.Update(1);
            }
            Assert.AreEqual(1, animation.CurrentFrame);

            animation.Ending = KeyframeEnding.Discard;
            animation.AutoReverse = true;
            ((IAnimation)animation).OnStarted();

            for (int i = 0; i < 50; ++i)
            {
                animation.Update(0.5f);
                System.Diagnostics.Trace.WriteLine(animation.CurrentFrame);
            }
            Assert.AreEqual(1, animation.CurrentFrame);
        }

        [TestMethod]
        public void BeginFrameEndFrameTest()
        {
            TestAnimation animation = new TestAnimation();
            animation.BeginFrame = 1;
            animation.EndFrame = 3;

            int frameCount = 0;

            animation.EnterFrame += (o, e) =>
            {
                frameCount++;
            };
            ((IAnimation)animation).OnStarted();

            for (int i = 0; i < 200; ++i)
                animation.Update(0.25f);

            Assert.AreEqual(AnimationState.Stopped, animation.State);
            Assert.AreEqual(3, frameCount);
        }
    }
}
