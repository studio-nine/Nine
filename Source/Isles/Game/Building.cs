//-----------------------------------------------------------------------------
//  Isles v1.0
//  
//  Copyright 2008 (c) Nightin Games. All Rights Reserved.
//-----------------------------------------------------------------------------

using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Isles.Graphics;
using Isles.Engine;
using Isles.UI;

namespace Isles
{
    #region Building
    /// <summary>
    /// Respresents a building in the game
    /// </summary>
    public class Building : GameObject, IPlaceable
    {
        #region Building State & Flag
        /// <summary>
        /// State of the building
        /// </summary>
        public enum BuildingState
        {
            Normal,
            PreConstruct,
            Wait,
            Constructing,
            Destroyed,
        }
        #endregion

        #region Fields
        /// <summary>
        /// Gets the building state
        /// </summary>
        public new BuildingState State
        {
            get { return state; }
        }

        protected BuildingState state = BuildingState.Normal;


        /// <summary>
        /// Gets or sets the time to construct this building
        /// </summary>
        public float ConstructionTime, ConstructionTimeElapsed;


        /// <summary>
        /// Gets or sets how much wood needed for the building
        /// </summary>
        public int Lumber;


        /// <summary>
        /// Gets or sets how much gold needed for the building
        /// </summary>
        public int Gold;

        /// <summary>
        /// Gets or sets how much food this building can provide
        /// </summary>
        public int Food;

        /// <summary>
        /// Gets or sets the number of builders
        /// </summary>
        public int BuilderCount
        {
            get { return builderCount; }
            set
            {
                System.Diagnostics.Debug.Assert(value >= 0);

                builderCount = value;
            }
        }

        const float CorporationTradeoff = 0.5f;

        int builderCount;

        /// <summary>
        /// Gets or sets the rotation on the xy plane
        /// </summary>
        public float RotationZ
        {
            get { return rotationZ; }
            set { rotationZ = value; }
        }

        float rotationZ;

        /// <summary>
        /// Path brush
        /// </summary>
        Vector2 obstructorSize;
        List<Point> pathGrids = new List<Point>();

        /// <summary>
        /// Gets the units that can be trained from this building
        /// </summary>
        public List<string> Units
        {
            get { return units; }
        }

        List<string> units = new List<string>();

        /// <summary>
        /// Gets the pending requests that are going to be handled
        /// </summary>
        public Queue<Spell> QueuedSpells
        {
            get { return pendingSpells; }
        }

        Queue<Spell> pendingSpells = new Queue<Spell>();

        /// <summary>
        /// Gets or sets the spawn point of this building
        /// </summary>
        public Vector3 SpawnPoint;
        public Worker Builder;


        /// <summary>
        /// Effects
        /// </summary>
        EffectConstruct construct;
        List<EffectFire> fire;
        List<Vector3> fireSpawnPoints;
        List<Vector3> fireSpawnPointsLeft;

        /// <summary>
        /// Gets the rally point model for all buildings
        /// </summary>
        public static GameModel RallyPointModel
        {
            get 
            {
                if (rallyPointModel == null)
                    rallyPointModel = new GameModel("Models/rally");
                return rallyPointModel;
            }
        }

        static GameModel rallyPointModel;

        /// <summary>
        /// Halo effect
        /// </summary>
        EffectHalo halo;
        string haloParticle;
        #endregion

        #region Methods
        /// <summary>
        /// Create a new building
        /// </summary>
        public Building(GameWorld world, string classID)
            : base(world, classID) { }

        public override void Deserialize(XmlElement xml)
        {
            int.TryParse(xml.GetAttribute("Lumber"), out Lumber);
            int.TryParse(xml.GetAttribute("Gold"), out Gold);
            int.TryParse(xml.GetAttribute("Food"), out Food);
            float.TryParse(xml.GetAttribute("ConstructionTime"), out ConstructionTime);

            string value;

            if ((value = xml.GetAttribute("ObstructorSize")) != "")
                obstructorSize = Helper.StringToVector2(value) / 2;

            if ((value = xml.GetAttribute("SpawnPoint")) != "")
                SpawnPoint = Helper.StringToVector3(value);

            if ((value = xml.GetAttribute("Units")) != "")
            {
                units = new List<string>(
                    value.Split(new char[] { ',', ' ', '\n', '\r' }));
                units.RemoveAll(delegate(string v) { return v.Length <= 0; });
            }

            if ((value = xml.GetAttribute("Halo")) != "")
                haloParticle = value;

            // Deserialize models after default attributes are assigned
            base.Deserialize(xml);

            // Reset sound die to explosion
            SoundDie = "Explosion";
        }

        protected override void UpdateOutline(Outline outline)
        {
            Vector2 position;
            position.X = Position.X;
            position.Y = Position.Y;

            outline.SetRectangle(-obstructorSize, obstructorSize, position, rotationZ);
        }

        public override void OnCreate()
        {
            if (state == BuildingState.Normal)
            {
                OnComplete();
                MarkGrids();
            }

            if (Owner != null)
            {
                Owner.MarkFutureObject(ClassID);
            }

            base.OnCreate();
        }

        protected override void OnCreateSpell(Spell spell)
        {
            SpellTraining training = spell as SpellTraining;
            if (training != null)
            {
                training.Enable = false;
                units.Add(training.Type);
            }
        }

        protected override void ShowSpells(GameUI ui)
        {
            if (Sound != null)
                Audios.Play(Sound, Audios.Channel.Building, this);

            base.ShowSpells(ui);
        }

        protected virtual void OnComplete()
        {
            if (Owner != null)
            {
                Owner.Add(this);
                Owner.FoodCapacity += Food;

                // Refresh ruined land if we are steamer
                if (Owner.Race == Race.Steamer && RuinedLand.Singleton != null)
                    RuinedLand.Singleton.Refresh();
            }

            foreach (Spell spell in Spells)
                spell.Enable = true;

            LocalPlayer local = Owner as LocalPlayer;

            if (local != null &&
                local.CurrentGroup != null && local.CurrentGroup.Contains(this))
            {
                Focused = true;
            }
        }

        private void MarkGrids()
        {
            pathGrids.AddRange(World.PathManager.EnumerateGridsInOutline(Outline));
            World.PathManager.Mark(pathGrids);
        }

        protected override void OnSelect(GameUI ui)
        {
            Tree.Pickable = true;
            base.OnSelect(ui);
        }

        protected override void OnDeselect(GameUI ui)
        {
            Tree.Pickable = false;
            base.OnDeselect(ui);
        }

        public static Building LastDestroyedBuilding;

        public override void OnDestroy()
        {
            LastDestroyedBuilding = this;
            base.OnDestroy();
        }

        protected override void OnDie()
        {
            if (pathGrids != null)
                World.PathManager.Unmark(pathGrids);

            if (Owner != null && state == BuildingState.Normal)
            {
                Owner.Remove(this);
                Owner.FoodCapacity -= Food;
                Owner.UnmarkFutureObject(ClassID);

                // Refresh ruined land if we are steamer
                if (Owner.Race == Race.Steamer && RuinedLand.Singleton != null)
                    RuinedLand.Singleton.Refresh();
            }

            // Create explosion
            if (ShouldDrawModel)
                new EffectExplosion(World, (TopCenter + Position) / 2);

            state = BuildingState.Destroyed;

            GameServer.Singleton.Destroy(this);
        }

        /// <summary>
        /// Gets whether a given type of unit can be trained from this building
        /// </summary>
        public bool CanTrain(string type)
        {
            if (state == BuildingState.Normal && units != null && units.Contains(type))
            {
                if (Owner.IsUnique(type) && Owner.IsFutureAvailable(type))
                    return false;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Trains a given type of unit
        /// </summary>
        public bool TrainUnit(string type)
        {
            if (CanTrain(type))
            {
                float gold, lumber, food;

                if ((gold = GameDefault.Singleton.GetGold(type)) > Owner.Gold)
                {
                    if (Owner is LocalPlayer)
                    {
                        Audios.Play("NotEnoughGold", Audios.Channel.Interface, null);
                        GameUI.Singleton.PushMessage("Insufficient Gold!", MessageType.Unavailable, Color.White);
                    }
                    return false;
                }
                else if ((lumber = GameDefault.Singleton.GetLumber(type)) > Owner.Lumber)
                {
                    if (Owner is LocalPlayer)
                    {
                        Audios.Play("NotEnoughLumber", Audios.Channel.Interface, null);
                        GameUI.Singleton.PushMessage("Insufficient Lumber!", MessageType.Unavailable, Color.White);
                    }
                    return false;
                }
                else if ((food = GameDefault.Singleton.GetFood(type)) > Owner.FoodCapacity - Owner.Food)
                {
                    if (Owner is LocalPlayer)
                    {
                        Audios.Play("MoreFarms", Audios.Channel.Interface, null);
                        GameUI.Singleton.PushMessage("Build More Farms!", MessageType.Unavailable, Color.White);
                    }
                    return false;
                }

                Owner.Gold -= gold;
                Owner.Lumber -= lumber;
                Owner.Food += food;
                Owner.MarkFutureObject(type);

                foreach (Spell s in Spells)
                {
                    SpellTraining train = s as SpellTraining;
                    if (train != null && train.Type == type)
                    {
                        train.Count++;
                        pendingSpells.Enqueue(train);
                        return true;
                    }
                }
            }

            return false;
        }

        public void CancelTraining()
        {
            // Removes the last spell
            if (pendingSpells.Count > 0)
            {
                Spell[] spells = pendingSpells.ToArray();
                pendingSpells.Clear();
                for (int i = 0; i < spells.Length - 1; i++)
                    pendingSpells.Enqueue(spells[i]);

                Spell removed = spells[spells.Length - 1];
                if (removed is SpellTraining)
                {
                    SpellTraining spell = removed as SpellTraining;
                    string type = spell.Type;
                    Owner.Gold += GameDefault.Singleton.GetGold(type);
                    Owner.Lumber += GameDefault.Singleton.GetLumber(type);
                    Owner.Food -= GameDefault.Singleton.GetFood(type);

                    if (Owner is ComputerPlayer)
                        (Owner as ComputerPlayer).UnmarkFutureObject(type);

                    if (--spell.Count <= 0)
                        spell.CoolDownElapsed = spell.CoolDown;
                }
            }
        }

        public void CancelTraining(Spell target)
        {
            // Removes the last specified spell
            if (pendingSpells.Count > 0)
            {
                Spell[] spells = pendingSpells.ToArray();
                pendingSpells.Clear();

                int removedIndex = -1;
                for (int i = spells.Length - 1; i >= 0; i--)
                    if (spells[i] == target)
                    {
                        removedIndex = i;
                        break;
                    }

                if (removedIndex < 0)
                    throw new ArgumentException();

                for (int i = 0; i < spells.Length; i++)
                    if (i != removedIndex)
                        pendingSpells.Enqueue(spells[i]);

                Spell removed = spells[removedIndex];
                if (removed is SpellTraining)
                {
                    SpellTraining spell = removed as SpellTraining;
                    string type = spell.Type;
                    Owner.Gold += GameDefault.Singleton.GetGold(type);
                    Owner.Lumber += GameDefault.Singleton.GetLumber(type);
                    Owner.Food -= GameDefault.Singleton.GetFood(type);

                    Owner.UnmarkFutureObject(type);

                    if (--spell.Count <= 0)
                        spell.CoolDownElapsed = spell.CoolDown;
                }
            }
        }

        public List<object> RallyPoints = new List<object>();

        public override void PerformAction(Entity entity, bool queueAction)
        {
            if (!queueAction)
                RallyPoints.Clear();

            RallyPoints.Add(entity);

            if (Owner is LocalPlayer)
                Audios.Play("Rally");
        }

        public override void PerformAction(Vector3 position, bool queueAction)
        {
            if (!queueAction)
                RallyPoints.Clear();

            RallyPoints.Add(position);

            if (Owner is LocalPlayer)
                Audios.Play("Rally");
        }
        #endregion

        #region Update & Draw
        protected override void DrawStatus()
        {
            if (state == BuildingState.Constructing)
            {
                GameUI.Singleton.DrawProgress(TopCenter, 5,
                                              (int)(SelectionAreaRadius * 10.0f),
                                              100 * ConstructionTimeElapsed / ConstructionTime,
                                              Color.Orange);
            }
        }

        protected override void DrawFogOfWar()
        {
            if (state != BuildingState.PreConstruct &&
                state != BuildingState.Constructing)
                base.DrawFogOfWar();
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            //if (ClassID == "Townhall")
            //    Health -= 2.0f;

            // Pre construct
            if (state == BuildingState.PreConstruct)
            {
                Model.Tint = IsLocationPlacable() ? Vector3.One : Vector3.UnitX;

                // Hide the building if our mouse is over the UI
                Visible = !GameUI.Singleton.Overlaps(World.Game.Input.MousePosition);
            }

            // Wait
            if (state == BuildingState.Wait)
            {
                if (CanPlace(false))
                {
                    RestoreStates();
                    MarkGrids();
                    state = BuildingState.Constructing;
                    BeginConstruct();
                }
                else if (waitTimer < 0)
                {
                    RestoreStates();
                    if (Owner is LocalPlayer)
                    {
                        Audios.Play("CannotBuild", Audios.Channel.Interface, null);
                        GameUI.Singleton.PushMessage("Can't Build There...", MessageType.Unavailable, Color.White);
                    }
                    CancelPlace();
                    OnDie();
                }

                waitTimer -= gameTime.ElapsedGameTime.TotalSeconds;
            }

            // Construct building
            if (state == BuildingState.Constructing && BuilderCount > 0)
            {
                if (construct == null)
                    construct = new EffectConstruct(World, Outline * 0.5f, Position.Z, Position.Z + 10);

                if (ShouldDrawModel)
                    construct.Update(gameTime);

                float elapsedSeconds = (float)(gameTime.ElapsedGameTime.TotalSeconds);
                ConstructionTimeElapsed += elapsedSeconds * (1 + (builderCount - 1) * CorporationTradeoff);

                if (ConstructionTimeElapsed > ConstructionTime)
                {
                    ConstructionTimeElapsed = ConstructionTime;

                    // Build complete
                    OnComplete();

                    Model.Alpha = 1.0f;
                    state = BuildingState.Normal;
                }
            }

            // Building onfire
            const float StartBurnPercentage = 0.7f;
            if (state == BuildingState.Normal && Health < MaximumHealth * StartBurnPercentage)
            {
                if (fire == null)
                {
                    fire = new List<EffectFire>();
                    
                    fireSpawnPoints = new List<Vector3>();
                    fireSpawnPointsLeft = new List<Vector3>();

                    int index = 1;
                    int bone = 0;

                    while ((bone = Model.GetBone("fire" + index)) >= 0)
                    {
                        index++;
                        fireSpawnPoints.Add(Model.GetBoneTransform(bone).Translation);
                    }

                    fireSpawnPointsLeft.AddRange(fireSpawnPoints);
                }

                // How many fire we should set
                int count = 1 + (int)(fireSpawnPoints.Count * (1 - Health / (MaximumHealth * StartBurnPercentage)));
                if (count > fireSpawnPoints.Count)
                    count = fireSpawnPoints.Count;

                if (count > fire.Count)
                {
                    // Setup a new fires
                    Vector3 position = fireSpawnPointsLeft[
                        Helper.Random.Next(fireSpawnPointsLeft.Count)];

                    fireSpawnPointsLeft.Remove(position);

                    fire.Add(new EffectFire(World, position));

                    Audios.Play("Damage", this);
                }
                else if (count < fire.Count)
                {
                    // Remove an existing fire
                    int index = Helper.Random.Next(fire.Count);
                    
                    Vector3 position = fire[index].Position;
                    
                    fire.RemoveAt(index);

                    fireSpawnPointsLeft.Add(position);
                }

                // Update fires
                if (ShouldDrawModel)
                {
                    foreach (EffectFire effect in fire)
                        effect.Update(gameTime);
                }
            }


            // Repair building
            if (state == BuildingState.Normal && Health < MaximumHealth && builderCount > 0)
            {
                float elapsedSeconds = (float)(gameTime.ElapsedGameTime.TotalSeconds);
                Health += 0.2f * MaximumHealth / ConstructionTime * elapsedSeconds *
                                (1 + (builderCount - 1) * CorporationTradeoff);
            }

            // Produce units & upgrades
            if (state == BuildingState.Normal && pendingSpells.Count > 0)
            {
                if (pendingSpells.Peek().Ready)
                    pendingSpells.Peek().Cast();

                // Create halo effect
                if (ShouldDrawModel && haloParticle != null)
                {
                    if (halo == null)
                        halo = new EffectHalo(World, TopCenter, 30, haloParticle);

                    halo.Update(gameTime);
                }
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            // Draw rally point
            if (Selected && ShouldDrawModel && IsAlive && Owner is LocalPlayer &&
                (state == BuildingState.Normal || state == BuildingState.Constructing) &&
                RallyPointModel != null && RallyPoints.Count > 0)
            {
                Vector3 position = Vector3.Zero;

                if (RallyPoints[0] is Entity)
                    position = (RallyPoints[0] as Entity).Position;
                else if (RallyPoints[0] is Vector3)
                    position = (Vector3)RallyPoints[0];

                RallyPointModel.Transform = Matrix.CreateTranslation(position);
                RallyPointModel.Draw(gameTime);
            }
        }

        public override EventResult HandleEvent(EventType type, object sender, object tag)
        {
            if (type == EventType.KeyDown && tag is Keys? && (tag as Keys?).Value == Keys.Escape)
            {
                // Press Esc to cancel build
                if (state == BuildingState.Constructing)
                {
                    CancelPlace();
                    OnDie();
                    return EventResult.Handled;
                }

                // Cancel last spell
                if (state == BuildingState.Normal)
                    CancelTraining();
            }

            return EventResult.Unhandled;
        }

        private void RestoreStates()
        {
            // Restore states
            foreach (KeyValuePair<Charactor, IState> pair in negotiables)
            {
                pair.Key.State = pair.Value;
            }
        }


        private void UpdateSmoke()
        {

        }

        #endregion

        #region IPlaceable Members
        public bool BeginPlace()
        {
            if (Owner != null)
            {
                if (Owner is LocalPlayer)
                {
                    if (Gold > Owner.Gold)
                    {
                        Audios.Play("NotEnoughGold", Audios.Channel.Interface, null);
                        GameUI.Singleton.PushMessage("Insufficient Gold!", MessageType.Unavailable, Color.White);
                        return false;
                    }

                    if (Lumber > Owner.Lumber)
                    {
                        Audios.Play("NotEnoughLumber", Audios.Channel.Interface, null);
                        GameUI.Singleton.PushMessage("Insufficient Lumber!", MessageType.Unavailable, Color.White);
                        return false;
                    }
                }

                Owner.Gold -= Gold;
                Owner.Lumber -= Lumber;
                Model.Alpha = 0.3f;
                state = BuildingState.PreConstruct;
            }

            return true;
        }

        List<Charactor> GetNegotiables()
        {
            List<Charactor> obstructors = new List<Charactor>();
            IEnumerable<IWorldObject> nearbyObjects;
            nearbyObjects = World.GetNearbyObjects(Position, Math.Max(obstructorSize.X,
                                                                      obstructorSize.Y));
            foreach (IWorldObject wo in nearbyObjects)
            {
                Charactor c = wo as Charactor;

                if (c != null && c.IsAlive && c.Owner == Owner)
                    obstructors.Add(c);
            }

            return obstructors;
        }

        public bool IsLocationPlacable()
        {
            if (Owner is LocalPlayer)
            {
                if (World.FogOfWar.Contains(Position.X, Position.Y))
                    return false;

                if (Owner.Race == Race.Steamer && ClassID != Owner.TownhallName &&
                    !RuinedLand.Singleton.Contains(Position.X, Position.Y))
                    return false;
                
                if (GameUI.Singleton.Overlaps(World.Game.Input.MousePosition))
                    return false;
            }

            // Exclude nearby units with the same owner
            List<Charactor> negotiables = GetNegotiables();

            foreach (Charactor c in negotiables)
                World.PathManager.Unmark(c);

            bool canPlace = CanPlace(true);

            foreach (Charactor c in negotiables)
                World.PathManager.Mark(c);

            return canPlace;
        }

        bool CanPlace(bool enlargeOutline)
        {
            const float RadiusSquared = 80 * 80;

            // Buildings can't be placed near the goldmine
            foreach (IWorldObject o in World.GetNearbyObjects(Position, 50))
            {
                Goldmine g = o as Goldmine;

                if (g != null)
                {
                    Vector2 v;
                    v.X = g.Position.X - Position.X;
                    v.Y = g.Position.Y - Position.Y;
                    if (v.LengthSquared() < RadiusSquared)
                        return false;
                }
            }

            return World.PathManager.CanBePlacedAt(
                   World.PathManager.EnumerateGridsInOutline(
                   enlargeOutline ? Outline + 5 : Outline), true);
        }

        double waitTimer = 0;
        List<KeyValuePair<Charactor, IState>> negotiables = new List<KeyValuePair<Charactor, IState>>();

        public void Place()
        {
            if (Owner is LocalPlayer)
                Audios.Play("Place");

            // Check if the building can be directly placed at the target
            if (!CanPlace(false))
            {
                // Tell the nearby charactors to avoid the building
                Vector2 position;
                position.X = Position.X;
                position.Y = Position.Y;
                List<Point> temp = new List<Point>();
                List<Point> grids = new List<Point>(World.PathManager.EnumerateGridsInOutline(Outline * 1.5f));
                World.PathManager.Mark(grids);

                negotiables.Clear();

                foreach (Charactor c in GetNegotiables())
                {
                    Vector2 target = World.PathManager.FindValidPosition(position, c.Brush);
                    negotiables.Add(new KeyValuePair<Charactor, IState>(c, c.State));
                    c.MoveTo(new Vector3(target, 0), false);

                    temp.Clear();
                    temp.AddRange(World.PathManager.Graph.EnumerateGridsInBrush(target, c.Brush));
                    grids.AddRange(temp);
                    World.PathManager.Mark(temp);
                }

                World.PathManager.Unmark(grids);

                // Reset wait timer
                waitTimer = 4;

                state = BuildingState.Wait;
            }
            else
            {
                MarkGrids();
                state = BuildingState.Constructing;
                BeginConstruct();
            }
        }

        private void BeginConstruct()
        {
            if (Builder != null)
            {
                IState state = new StateConstruct(World, Builder, this);
                if (World.Game.Input.IsShiftPressed)
                    Builder.QueuedStates.Enqueue(state);
                else
                    Builder.State = state;
            }
        }

        public void CancelPlace()
        {
            Owner.Gold += Gold;
            Owner.Lumber += Lumber;
        }
        #endregion
    }

    #endregion

    #region Tower
    public class Tower : Building
    {
        SpellCombat combat;
        GameObject currentTarget;

        public Tower(GameWorld world, string classID)
            : base(world, classID)
        {
            // Add a combat spell
            combat = new SpellCombat(world, this);
        }

        public override void TriggerAttack(Entity target)
        {
            Vector3 velocity = target.Position - Position;
            velocity.Z = 0;
            velocity.Z = (float)(velocity.Length() * Math.Tan(MathHelper.ToRadians(30)));
            velocity.Normalize();
            velocity *= 75;

            EffectFireball fireball = new EffectFireball(
                World, TopCenter - Vector3.UnitZ * 5, velocity, target,
                "Frost", "FrostExplosion");
            fireball.Projectile.Hit += new EventHandler(Hit);
            World.Add(fireball);

            if (ShouldDrawModel)
                Audios.Play("FireballCast", this);
        }           

        public override void PerformAction(Entity entity, bool queueAction)
        {
            currentTarget = entity as GameObject;
        }

        public override void Update(GameTime gameTime)
        {
            if (state == BuildingState.Normal && IsAlive)
            {
                // Trigger attack
                if (IsOpponent(currentTarget) &&
                    combat.CanAttakTarget(currentTarget) &&
                    combat.TargetWithinRange(currentTarget))
                    combat.Cast(currentTarget);
                else
                    currentTarget = StateAttack.FindAnotherTarget(World, this, null, combat);

                if (combat != null)
                    combat.Update(gameTime);
            }

            base.Update(gameTime);
        }

        void Hit(object sender, EventArgs e)
        {
            IProjectile projectile = sender as IProjectile;

            if (projectile != null && projectile.Target is GameObject)
            {
                GameObject target = projectile.Target as GameObject;

                if (target.ShouldDrawModel)
                    Audios.Play("FireballHit", target);

                target.Health -= ComputeHit(this, target);
            }
        }
    }
    #endregion

    #region Lumbermill
    public class Lumbermill : Building
    {
        public Lumbermill(GameWorld world, string classID)
            : base(world, classID) { }

        public void LiveOfNatureResearched()
        {
            foreach (Tree tree in EnumerateAffectedTrees())
                tree.EverGreen = true;
        }

        protected override void OnComplete()
        {
            if (Owner != null && Owner.IsAvailable("LiveOfNature"))
                LiveOfNatureResearched();

            base.OnComplete();
        }

        public override void OnDestroy()
        {
            if (affectedTrees != null)
            {
                foreach (Tree tree in EnumerateAffectedTrees())
                    tree.EverGreen = false;
            }
            base.OnDestroy();
        }

        IEnumerable<Tree> EnumerateAffectedTrees()
        {
            if (affectedTrees == null)
            {
                Vector2 v;
                const float EffectRadius = 50;

                affectedTrees = new List<Tree>();

                // Set nearby tree to evergreen
                foreach (IWorldObject wo in World.GetNearbyObjects(Position, EffectRadius))
                {
                    Tree tree = wo as Tree;

                    if (tree != null)
                    {

                        v.X = tree.Position.X - Position.X;
                        v.Y = tree.Position.Y - Position.Y;

                        if (v.LengthSquared() <= EffectRadius * EffectRadius)
                        {
                            affectedTrees.Add(tree);
                        }
                    }
                }
            }

            return affectedTrees;
        }

        List<Tree> affectedTrees;
    }
    #endregion
}