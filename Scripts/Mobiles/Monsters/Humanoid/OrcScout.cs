using System;
using Server.Items;
using Server.Misc;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("an orcish corpse")]
    public class OrcScout : BaseCreature
    {
        [Constructable]
        public OrcScout()
            : base(AIType.AI_OrcScout, FightMode.Closest, 10, 7, 0.2, 0.4)
        {
            this.Name = "an orc scout";
            this.Body = 0xB5;
            this.BaseSoundID = 0x45A;

            this.SetStr(96, 120);
            this.SetDex(101, 130);
            this.SetInt(36, 60);

            this.SetHits(58, 72);
            this.SetMana(30, 60);

            this.SetDamage(5, 7);

            this.SetDamageType(ResistanceType.Physical, 100);

            this.SetResistance(ResistanceType.Physical, 25, 35);
            this.SetResistance(ResistanceType.Fire, 30, 40);
            this.SetResistance(ResistanceType.Cold, 15, 25);
            this.SetResistance(ResistanceType.Poison, 15, 20);
            this.SetResistance(ResistanceType.Energy, 25, 30);

            this.SetSkill(SkillName.MagicResist, 50.1, 75.0);
            this.SetSkill(SkillName.Tactics, 55.1, 80.0);

            this.SetSkill(SkillName.Fencing, 50.1, 70.0);
            this.SetSkill(SkillName.Archery, 80.1, 120.0);
            this.SetSkill(SkillName.Parry, 40.1, 60.0);
            this.SetSkill(SkillName.Healing, 80.1, 100.0);
            this.SetSkill(SkillName.Anatomy, 50.1, 90.0);
            this.SetSkill(SkillName.DetectHidden, 100.1, 120.0);
            this.SetSkill(SkillName.Hiding, 100.0, 120.0);
            this.SetSkill(SkillName.Stealth, 80.1, 120.0);

            this.Fame = 1500;
            this.Karma = -1500;

            this.PackItem(new Apple(Utility.RandomMinMax(3, 5)));
            this.PackItem(new Arrow(Utility.RandomMinMax(60, 70)));
            this.PackItem(new Bandage(Utility.RandomMinMax(1, 15)));

            if (0.1 > Utility.RandomDouble())
                this.AddItem(new OrcishBow());
            else
                this.AddItem(new Bow());
        }

        public OrcScout(Serial serial)
            : base(serial)
        {
        }

        public override InhumanSpeech SpeechType
        {
            get
            {
                return InhumanSpeech.Orc;
            }
        }
        public override OppositionGroup OppositionGroup
        {
            get
            {
                return OppositionGroup.SavagesAndOrcs;
            }
        }
        public override bool CanHeal
        {
            get
            {
                return true;
            }
        }
        public override bool CanRummageCorpses
        {
            get
            {
                return true;
            }
        }
        public override int Meat
        {
            get
            {
                return 1;
            }
        }
        public override void GenerateLoot()
        {
            this.AddLoot(LootPack.Rich);
        }

        public override bool IsEnemy(Mobile m)
        {
            if (m.Player && m.FindItemOnLayer(Layer.Helm) is OrcishKinMask)
                return false;

            return base.IsEnemy(m);
        }

        public override void AggressiveAction(Mobile aggressor, bool criminal)
        {
            base.AggressiveAction(aggressor, criminal);

            Item item = aggressor.FindItemOnLayer(Layer.Helm);

            if (item is OrcishKinMask)
            {
                AOS.Damage(aggressor, 50, 0, 100, 0, 0, 0);
                item.Delete();
                aggressor.FixedParticles(0x36BD, 20, 10, 5044, EffectLayer.Head);
                aggressor.PlaySound(0x307);
            }
        }

        public override void OnThink()
        {
            if (Utility.RandomDouble() < 0.2)
                this.TryToDetectHidden();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }

        private Mobile FindTarget()
        {
            foreach (Mobile m in this.GetMobilesInRange(10))
            {
                if (m.Player && m.Hidden && m.IsPlayer())
                {
                    return m;
                }
            }

            return null;
        }

        private void TryToDetectHidden()
        {
            Mobile m = this.FindTarget();

            if (m != null)
            {
                if (DateTime.Now >= this.NextSkillTime && this.UseSkill(SkillName.DetectHidden))
                {
                    Target targ = this.Target;

                    if (targ != null)
                        targ.Invoke(this, this);

                    Effects.PlaySound(this.Location, this.Map, 0x340);
                }
            }
        }
    }
}