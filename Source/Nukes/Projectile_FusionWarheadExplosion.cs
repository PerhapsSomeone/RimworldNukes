using System;
using Verse;
using UnityEngine;
using System.Collections.Generic;
using Verse.Sound;

namespace Nukes
{
    public class Projectile_FusionWarheadExplosion : Projectile
    {
        private int ticksToDetonation;
        private int explosionCount = 0;
        private int explosionsMax = 10;

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref ticksToDetonation, "ticksToDetonation", 0, false);
        }

        public override void Tick()
        {
            base.Tick();
            if (ticksToDetonation > 0)
            {
                ticksToDetonation--;
                if (ticksToDetonation <= 0)
                {
                    Explode();
                }
            }
        }

        protected override void Impact(Thing hitThing)
        {
            if (def.projectile.explosionDelay == 0)
            {
                Explode();
            }
            else
            {
                landed = true;
                ticksToDetonation = def.projectile.explosionDelay;
                GenExplosion.NotifyNearbyPawnsOfDangerousExplosive(this, def.projectile.damageDef, launcher.Faction);
            }
        }

        protected virtual void Explode()
        {
            Settings s = new Settings();
            if (explosionCount > explosionsMax)
            {
                return;
            }
            explosionCount++;

            Map map = base.Map;

            if (s.customSounds)
            {
                SoundDef sound = SoundDef.Named("FusionExplosionSoundEffect");
                sound.PlayOneShotOnCamera(map);
            }

            if (base.def.projectile.explosionEffect != null)
            {
                Effecter effecter = base.def.projectile.explosionEffect.Spawn();
                effecter.Trigger(new TargetInfo(base.Position, map, false), new TargetInfo(base.Position, map, false));
                effecter.Cleanup();
            }

            IntVec3 position = base.Position;
            Map map2 = map;
            float explosionRadius = base.def.projectile.explosionRadius;
            DamageDef damageDef = base.def.projectile.damageDef;
            Thing launcher = base.launcher;
            int damageAmount = base.DamageAmount;
            float armorPenetration = base.ArmorPenetration;
            SoundDef soundExplode = base.def.projectile.soundExplode;
            ThingDef equipmentDef = base.equipmentDef;
            ThingDef def = base.def;
            Thing thing = intendedTarget.Thing;
            ThingDef postExplosionSpawnThingDef = base.def.projectile.postExplosionSpawnThingDef;
            float postExplosionSpawnChance = base.def.projectile.postExplosionSpawnChance;
            int postExplosionSpawnThingCount = base.def.projectile.postExplosionSpawnThingCount;
            ThingDef preExplosionSpawnThingDef = base.def.projectile.preExplosionSpawnThingDef;


            if (s.radiationEnabled && explosionCount == 1)
            {
                foreach (Pawn pawn in map.mapPawns.AllPawns.ListFullCopy())
                {
                    if (pawn.Dead)
                    {
                        continue;
                    }

                    try
                    {
                        if (position.DistanceTo(pawn.Position) <= explosionRadius)
                        {
                            pawn.health.AddHediff(HediffDef.Named("LethalRadiationPoisoning"));
                        }
                        else if (s.radiationAirburst)
                        {
                            pawn.health.AddHediff(HediffDef.Named("LightRadiationPosisoning"));
                        }
                    }
                    catch (Exception e) { }
                }
            }



            if (explosionCount == 1)
            {
                IEnumerable<IntVec3> radius = GenRadial.RadialCellsAround(position, explosionRadius, true);
                foreach(IntVec3 pos in radius)
                {
                    var destroyableThings = map.thingGrid.ThingsAt(pos);
                    foreach (Thing destroy in destroyableThings)
                    {
                        destroy.Destroy(DestroyMode.Vanish);
                    }
                }
            }

            for (int i = 0; i < 3; i++) GenExplosion.DoExplosion(position, map2, explosionRadius, damageDef, launcher, damageAmount, armorPenetration, soundExplode, equipmentDef, def, thing, postExplosionSpawnThingDef, postExplosionSpawnChance, postExplosionSpawnThingCount, base.def.projectile.applyDamageToExplosionCellsNeighbors, preExplosionSpawnThingDef, base.def.projectile.preExplosionSpawnChance, base.def.projectile.preExplosionSpawnThingCount, base.def.projectile.explosionChanceToStartFire, base.def.projectile.explosionDamageFalloff);

            if(explosionCount > explosionsMax)
            {
                Destroy(DestroyMode.Vanish);
            }
        }
    }
}
