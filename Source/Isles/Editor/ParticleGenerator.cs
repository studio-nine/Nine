using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Isles;
using Isles.Engine;
using Isles.Graphics;
using Isles.UI;

namespace Isles.Editor
{
    public partial class ParticleGenerator : Form
    {
        public GameWorld World
        {
            get
            {
                return this.world;
            }
            set
            {
                this.world = value;
            }
        }
        private GameWorld world;
        public ParticleGenerator(GameWorld world)
        {
            InitializeComponent();
            this.world = world;
        }

        private void Edit(ParticleEffect effect)
        {
            (world.Game as GameIsles).StartEditor(new ParticleEditor(effect));
        }

        private Building GetTestTarget()
        {
            return Player.LocalPlayer.GetObjects(
                                    Player.LocalPlayer.TownhallName).First.Value as Building;
        }

        //EffectFireball fireball;

        public class TestTarget : BaseEntity
        {
            IWorldObject center;
            float radius = 75;
            float angle = 0;

            public TestTarget(GameWorld world, IWorldObject center)
                :base(world)
            {
                this.center = center;
            }

            public override void Update(GameTime gameTime)
            {
                angle += 2.0f * (float)gameTime.ElapsedGameTime.TotalSeconds;

                Position = new Vector3(center.Position.X + radius * (float)Math.Cos(angle),
                                       center.Position.Y + radius * (float)Math.Sin(angle),
                                       center.Position.Z + 25);
            }

            public override void Draw(GameTime gameTime) { }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Building townHall = GetTestTarget();
            TestTarget target = new TestTarget(world, townHall);

            EffectPunishOfNature fireball = new EffectPunishOfNature(world, townHall.Position);

            //fireball.Projectile.Hit += new EventHandler(Projectile_Hit);

            Edit(fireball);

            world.Add(target);
            world.Add(fireball);
        }

        void Projectile_Hit(object sender, EventArgs e)
        {
            //world.Destroy(fireball);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //ParticleEffectMissile missile = this.World.Create("Missile") as ParticleEffectMissile;
            //Building townHall = GetTestTarget();
            //missile.Position = townHall.Position - new Vector3(40, 20, -10);
            //missile.Destination = townHall.Position + new Vector3(40,20,10);
            //missile.Launch();
            //this.world.Add(missile);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Building townHall = GetTestTarget();
            EffectFire fire = new EffectFire(world);

            fire.Position = townHall.TopCenter;

            Edit(fire);

            world.Add(fire);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Building townHall = GetTestTarget();
            TestTarget target = new TestTarget(world, townHall);

            EffectTest test = new EffectTest(world, target, townHall.Position);

            Edit(test);

            world.Add(test);
            world.Add(target);
        }

        private void button6_Click(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            Building townHall = GetTestTarget();
            EffectConstruct smoke = new EffectConstruct(
                world, townHall.Outline * 0.5f, townHall.Position.Z, townHall.Position.Z + 50);

            townHall.Model.Alpha = 0.3f;

            Edit(smoke);

            this.world.Add(smoke);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            //Building townHall = GetTestTarget();
            //Arrow arrow = this.world.Create("Arrow") as Arrow;

            //arrow.Position = townHall.Position - new Vector3(40, 0, 10);
            //arrow.Destination = townHall.Position + new Vector3(40, 0, 10);

            //arrow.Launch();

            //this.world.Add(arrow);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            //Building townHall = GetTestTarget();
            //RainEffect rain = this.world.Create("Rain") as RainEffect;

            //rain.Position = townHall.Position;
            //rain.RainOutline = townHall.Outline * 2;

            //this.world.Add(rain);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            //Building townHall = GetTestTarget();
            //Tree tree = StateHarvestLumber.FindAnotherTree(null, townHall.Position, world);
            //EffectGlow fire = new EffectGlow(world, tree);
            //    //world, townHall);
            //EffectStar star = new EffectStar(world, tree);

            //townHall.Model.Alpha = 0.5f;

            //Edit(fire);

            //world.Add(fire);
            //world.Add(star);   

            Building townHall = GetTestTarget();
            EffectExplosion explosion = new EffectExplosion(world, (townHall.TopCenter + townHall.Position) / 2);

            townHall.Model.Alpha = 0.5f;

            Edit(explosion);

            world.Add(explosion);
        }
    }
}