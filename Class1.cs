using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThunderRoad;
using UnityEngine;

namespace Blips
{
    public class Entry : LevelModule
    {
        public static System.Random random = new System.Random();
        public static double healthReceived;
        public static double manaReceived;
        public static double focusReceived;
        public string chanceComment;
        public double healthChance;
        public double manaChance;
        public double focusChance;
        public double goldenChance;
        public string minMaxConstComment;
        public double minHealth;
        public double maxHealth;
        public double constHealth;
        public double minMana;
        public double maxMana;
        public double constMana;
        public double minFocus;
        public double maxFocus;
        public double constFocus;
        public string spawnComment;
        public override IEnumerator OnLoadCoroutine(Level level)
        {
            EventManager.onCreatureKill += EventManager_onCreatureKill;
            return base.OnLoadCoroutine(level);
        }

        private void EventManager_onCreatureKill(Creature creature, Player player, CollisionInstance collisionInstance, EventTime eventTime)
        {
            if (eventTime == EventTime.OnStart)
            {
                if (random.Next(1, 101) <= healthChance)
                {
                    Catalog.GetData<ItemData>("HealthBlip").SpawnAsync(item =>
                    {
                        if (constHealth != -1)
                        {
                            healthReceived = constHealth;
                        }
                        else
                        {
                            healthReceived = random.Next((int)minHealth, (int)maxHealth + 1);
                        }
                    }, new Vector3(creature.transform.position.x + random.Next(-1, 2), creature.transform.position.y + 1f, creature.transform.position.z + random.Next(-1, 2)));
                }
                if (random.Next(1, 101) <= manaChance)
                {
                    Catalog.GetData<ItemData>("ManaBlip").SpawnAsync(item =>
                    {
                        if (constMana != -1)
                        {
                            manaReceived = constMana;
                        }
                        else
                        {
                            manaReceived = random.Next((int)minMana, (int)maxMana + 1);
                        }
                    }, new Vector3(creature.transform.position.x + random.Next(-1, 2), creature.transform.position.y + 1f, creature.transform.position.z + random.Next(-1, 2)));
                }
                if (random.Next(1, 101) <= focusChance)
                {
                    Catalog.GetData<ItemData>("FocusBlip").SpawnAsync(item =>
                    {
                        if (constFocus != -1)
                        {
                            focusReceived = constFocus;
                        }
                        else
                        {
                            focusReceived = random.Next((int)minFocus, (int)maxFocus + 1);
                        }
                    }, new Vector3(creature.transform.position.x + random.Next(-1, 2), creature.transform.position.y + 1f, creature.transform.position.z + random.Next(-1, 2)));
                }
                if (random.Next(1, 101) <= goldenChance)
                {
                    Catalog.GetData<ItemData>("GoldenBlip").SpawnAsync(item => { }, new Vector3(creature.transform.position.x + random.Next(-1, 2), creature.transform.position.y + 1f, creature.transform.position.z + random.Next(-1, 2)));
                }
            }
        }
        public static void Receive(Item item, Creature creature)
        {
            if (!creature.isKilled || creature.currentHealth > 0)
            {
                if (item.itemId == "HealthBlip")
                {
                    creature.currentHealth += (float)healthReceived;
                }
                if (item.itemId == "ManaBlip")
                {
                    creature.mana.currentMana += (float)manaReceived;
                }
                if (item.itemId == "FocusBlip")
                {
                    creature.mana.currentFocus += (float)focusReceived;
                }
                if (item.itemId == "GoldenBlip")
                {
                    creature.currentHealth = creature.maxHealth;
                    creature.mana.currentMana = creature.mana.maxMana;
                    creature.mana.currentFocus = creature.mana.maxFocus;
                }
                item.Despawn();
            }
        }
    }
    public class BlipModule : ItemModule
    {
        public override void OnItemLoaded(Item item)
        {
            base.OnItemLoaded(item);
            item.gameObject.AddComponent<BlipComponent>();
        }
    }
    public class BlipComponent : MonoBehaviour
    {
        private Item item;
        public void Start()
        {
            item = GetComponent<Item>();
            item.OnGrabEvent += Item_OnGrabEvent;
        }
        private void Item_OnGrabEvent(Handle handle, RagdollHand ragdollHand)
        {
            Creature creature = ragdollHand.gameObject.GetComponentInParent<Creature>();
            Entry.Receive(item, creature);
        }
        public void OnCollisionEnter(Collision c)
        {
            GameObject target = c.collider.gameObject;
            if (target.GetComponentInParent<Creature>() || target.GetComponentInParent<RagdollPart>())
            {
                Entry.Receive(item, target.GetComponentInParent<Creature>());
            }
        }
    }
}
