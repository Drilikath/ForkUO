using System;
using Server.Items;

namespace Server.Spells.Mystic
{
    public class HealingStoneSpell : MysticSpell
    {
        private static readonly SpellInfo m_Info = new SpellInfo(
            "Healing Stone", "Kal In Mani",
            230,
            9022,
            Reagent.Bone,
            Reagent.Garlic,
            Reagent.Ginseng,
            Reagent.SpidersSilk);
        public HealingStoneSpell(Mobile caster, Item scroll)
            : base(caster, scroll, m_Info)
        {
        }

        // Conjures a Healing Stone that will instantly heal the Caster when used.
        public override int RequiredMana
        {
            get
            {
                return 4;
            }
        }
        public override double RequiredSkill
        {
            get
            {
                return 0;
            }
        }
        public override void OnCast()
        {
            if (this.Caster.Backpack != null)
            {
                Item[] stones = this.Caster.Backpack.FindItemsByType(typeof(HealingStone));

                for (int i = 0; i < stones.Length; i++)
                    stones[i].Delete();

                int amount = (int)(this.Caster.Skills[this.DamageSkill].Value / 10);
                this.Caster.PlaySound(0x651);
                this.Caster.Backpack.DropItem(new HealingStone(this.Caster, amount));
                this.Caster.SendLocalizedMessage(1080115); // A Healing Stone appears in your backpack.
            }
        }
    }
}
/*




*/