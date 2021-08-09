using System;
using Verse;
using Verse.Sound;

namespace Nukes
{
    public class Projectile_LargeNuclearExplosion : Projectile
    {
        private int ticksToDetonation;
        private Boolean exploding = false;

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
            Explode();
        }

        protected virtual void Explode()
        {
            if(exploding)
            {
                return;
            }
            exploding = true;

            Map map = base.Map;
            
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



            GenExplosion.DoExplosion(position, map2, explosionRadius, damageDef, launcher, damageAmount, armorPenetration, soundExplode, equipmentDef, def, thing, postExplosionSpawnThingDef, postExplosionSpawnChance, postExplosionSpawnThingCount, base.def.projectile.applyDamageToExplosionCellsNeighbors, preExplosionSpawnThingDef, base.def.projectile.preExplosionSpawnChance, base.def.projectile.preExplosionSpawnThingCount, base.def.projectile.explosionChanceToStartFire, base.def.projectile.explosionDamageFalloff);


            Settings s = new Settings();

            if(s.customSounds)
            {
                SoundDef sound = SoundDef.Named("LoudNukeExplosionSound");
                sound.PlayOneShotOnCamera(map);
            }

            if (!s.radiationEnabled) return;

            foreach (Pawn pawn in map.mapPawns.AllPawns.ListFullCopy())
            {
                if (pawn.Dead)
                {
                    continue;
                }

                try
                {
                    if (position.DistanceTo(pawn.Position) < 7.5f * s.radiationLevel)
                    {
                        pawn.health.AddHediff(HediffDef.Named("LethalRadiationPoisoning"));
                    }
                    else if (position.DistanceTo(pawn.Position) < 25f * s.radiationLevel)
                    {
                        pawn.health.AddHediff(HediffDef.Named("HeavyRadiationPoisoning"));
                    }
                    else if (position.DistanceTo(pawn.Position) < 35f * s.radiationLevel)
                    {
                        pawn.health.AddHediff(HediffDef.Named("MediumRadiationPoisoning"));
                    }
                    else
                    {
                        if (s.radiationAirburst)
                        {
                            pawn.health.AddHediff(HediffDef.Named("LightRadiationPoisoning"));
                        }
                    }
                }
                catch (Exception e)
                {
                    Log.Message(e.ToString());
                }
            }

            Destroy(DestroyMode.Vanish);
        }
    }
}
