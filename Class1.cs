﻿using System.Collections;
using ThunderRoad;
using UnityEngine;

namespace Blips
{
    public class Entry : LevelModule
    {
        System.Random random = new System.Random();
        public string chanceComment;
        public double healthChance;
        public double manaChance;
        public double focusChance;
        public double goldenChance;
        public string minMaxConstComment;
        public override IEnumerator OnLoadCoroutine()
        {
            EventManager.onCreatureKill += EventManager_onCreatureKill;
            return base.OnLoadCoroutine();
        }

        private void EventManager_onCreatureKill(Creature creature, Player player, CollisionInstance collisionInstance, EventTime eventTime)
        {
            if (eventTime == EventTime.OnEnd)
            {
                if (random.Next(1, 101) <= healthChance)
                {
                    Catalog.GetData<ItemData>("HealthBlip").SpawnAsync(null, new Vector3(creature.transform.position.x + random.Next(-1, 2), creature.transform.position.y + 1f, creature.transform.position.z + random.Next(-1, 2)));
                }
                if (random.Next(1, 101) <= manaChance)
                {
                    Catalog.GetData<ItemData>("ManaBlip").SpawnAsync(null, new Vector3(creature.transform.position.x + random.Next(-1, 2), creature.transform.position.y + 1f, creature.transform.position.z + random.Next(-1, 2)));
                }
                if (random.Next(1, 101) <= focusChance)
                {
                    Catalog.GetData<ItemData>("FocusBlip").SpawnAsync(null, new Vector3(creature.transform.position.x + random.Next(-1, 2), creature.transform.position.y + 1f, creature.transform.position.z + random.Next(-1, 2)));
                }
                if (random.Next(1, 101) <= goldenChance)
                {
                    Catalog.GetData<ItemData>("GoldenBlip").SpawnAsync(null, new Vector3(creature.transform.position.x + random.Next(-1, 2), creature.transform.position.y + 1f, creature.transform.position.z + random.Next(-1, 2)));
                }
            }
        }
    }
    public class HealthModule : ItemModule
    {
        public double minHealth;
        public double maxHealth;
        public double constHealth;
        public override void OnItemLoaded(Item item)
        {
            base.OnItemLoaded(item);
            item.gameObject.AddComponent<HealthComponent>().Setup(minHealth, maxHealth, constHealth);
        }
    }
    public class HealthComponent : MonoBehaviour
    {
        Item item;
        double minHealth;
        double maxHealth;
        double constHealth;
        System.Random random = new System.Random();
        double healthReceived;
        public void Start()
        {
            item = GetComponent<Item>();
            item.OnGrabEvent += Item_OnGrabEvent;
            if (constHealth != -1)
            {
                healthReceived = constHealth;
            }
            else
            {
                healthReceived = random.Next((int)minHealth, (int)maxHealth + 1);
            }
        }
        public void Setup(double min, double max, double constant)
        {
            minHealth = min;
            maxHealth = max;
            constHealth = constant;
        }
        public void OnCollisionEnter(Collision c)
        {
            if (c.collider.GetComponentInParent<Creature>() != null && c.collider.GetComponentInParent<Creature>() is Creature creature && !creature.isKilled)
            {
                if (creature == Player.local.creature)
                {
                    CameraEffects.DoTimedEffect(Color.magenta, CameraEffects.TimedEffect.Flash, 1f);
                }
                creature.Heal((float)healthReceived, creature);
                item.Despawn();
            }
        }
        private void Item_OnGrabEvent(Handle handle, RagdollHand ragdollHand)
        {
            if (ragdollHand.creature == Player.local.creature)
            {
                CameraEffects.DoTimedEffect(Color.magenta, CameraEffects.TimedEffect.Flash, 1f);
            }
            ragdollHand.creature.Heal((float)healthReceived, ragdollHand.creature);
            item.Despawn();
        }
    }
    public class ManaModule : ItemModule
    {
        public double minMana;
        public double maxMana;
        public double constMana;
        public override void OnItemLoaded(Item item)
        {
            base.OnItemLoaded(item);
            item.gameObject.AddComponent<ManaComponent>().Setup(minMana, maxMana, constMana);
        }
    }
    public class ManaComponent : MonoBehaviour
    {
        Item item;
        double minMana;
        double maxMana;
        double constMana;
        System.Random random = new System.Random();
        double manaReceived;
        public void Start()
        {
            item = GetComponent<Item>();
            item.OnGrabEvent += Item_OnGrabEvent;
            if (constMana != -1)
            {
                manaReceived = constMana;
            }
            else
            {
                manaReceived = random.Next((int)minMana, (int)maxMana + 1);
            }
        }
        public void Setup(double min, double max, double constant)
        {
            minMana = min;
            maxMana = max;
            constMana = constant;
        }
        public void OnCollisionEnter(Collision c)
        {
            if (c.collider.GetComponentInParent<Creature>() != null && c.collider.GetComponentInParent<Creature>() is Creature creature && !creature.isKilled)
            {
                if (creature == Player.local.creature)
                {
                    CameraEffects.DoTimedEffect(Color.blue, CameraEffects.TimedEffect.Flash, 1f);
                }
                creature.mana.currentMana += (float)manaReceived;
                item.Despawn();
            }
        }
        private void Item_OnGrabEvent(Handle handle, RagdollHand ragdollHand)
        {
            if (ragdollHand.creature == Player.local.creature)
            {
                CameraEffects.DoTimedEffect(Color.blue, CameraEffects.TimedEffect.Flash, 1f);
            }
            ragdollHand.creature.mana.currentMana += (float)manaReceived;
            item.Despawn();
        }
    }
    public class FocusModule : ItemModule
    {
        public double minFocus;
        public double maxFocus;
        public double constFocus;
        public override void OnItemLoaded(Item item)
        {
            base.OnItemLoaded(item);
            item.gameObject.AddComponent<FocusComponent>().Setup(minFocus, maxFocus, constFocus);
        }
    }
    public class FocusComponent : MonoBehaviour
    {
        Item item;
        double minFocus;
        double maxFocus;
        double constFocus;
        System.Random random = new System.Random();
        double focusReceived;
        public void Start()
        {
            item = GetComponent<Item>();
            item.OnGrabEvent += Item_OnGrabEvent;
            if (constFocus != -1)
            {
                focusReceived = constFocus;
            }
            else
            {
                focusReceived = random.Next((int)minFocus, (int)maxFocus + 1);
            }
        }
        public void Setup(double min, double max, double constant)
        {
            minFocus = min;
            maxFocus = max;
            constFocus = constant;
        }
        public void OnCollisionEnter(Collision c)
        {
            if (c.collider.GetComponentInParent<Creature>() != null && c.collider.GetComponentInParent<Creature>() is Creature creature && !creature.isKilled)
            {
                if (creature == Player.local.creature)
                {
                    CameraEffects.DoTimedEffect(Color.green, CameraEffects.TimedEffect.Flash, 1f);
                }
                creature.mana.currentFocus += (float)focusReceived;
                item.Despawn();
            }
        }
        private void Item_OnGrabEvent(Handle handle, RagdollHand ragdollHand)
        {
            if (ragdollHand.creature == Player.local.creature)
            {
                CameraEffects.DoTimedEffect(Color.green, CameraEffects.TimedEffect.Flash, 1f);
            }
            ragdollHand.creature.mana.currentFocus += (float)focusReceived;
            item.Despawn();
        }
    }
    public class GoldModule : ItemModule
    {
        public override void OnItemLoaded(Item item)
        {
            base.OnItemLoaded(item);
            item.gameObject.AddComponent<GoldComponent>();
        }
    }
    public class GoldComponent : MonoBehaviour
    {
        Item item;
        public void Start()
        {
            item = GetComponent<Item>();
            item.OnGrabEvent += Item_OnGrabEvent;
        }
        public void OnCollisionEnter(Collision c)
        {
            if (c.collider.GetComponentInParent<Creature>() != null && c.collider.GetComponentInParent<Creature>() is Creature creature && !creature.isKilled)
            {
                if (creature == Player.local.creature)
                {
                    CameraEffects.DoTimedEffect(Color.yellow, CameraEffects.TimedEffect.Flash, 1f);
                }
                item.Despawn();
            }
        }
        private void Item_OnGrabEvent(Handle handle, RagdollHand ragdollHand)
        {
            if (ragdollHand.creature == Player.local.creature)
            {
                CameraEffects.DoTimedEffect(Color.yellow, CameraEffects.TimedEffect.Flash, 1f);
            }
            ragdollHand.creature.Heal(Mathf.Max(0, ragdollHand.creature.maxHealth - ragdollHand.creature.currentHealth), ragdollHand.creature);
            ragdollHand.creature.mana.currentMana += Mathf.Max(0, ragdollHand.creature.mana.maxMana - ragdollHand.creature.mana.currentMana);
            ragdollHand.creature.mana.currentFocus += Mathf.Max(0, ragdollHand.creature.mana.maxFocus - ragdollHand.creature.mana.currentFocus);
            item.Despawn();
        }
    }
}
