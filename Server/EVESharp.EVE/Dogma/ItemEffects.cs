﻿using System;
using EVESharp.Database.Dogma;
using EVESharp.Database.Inventory.Attributes;
using EVESharp.EVE.Data.Inventory;
using EVESharp.EVE.Data.Inventory.Items.Dogma;
using EVESharp.EVE.Data.Inventory.Items.Types;
using EVESharp.EVE.Dogma.Interpreter.Opcodes;
using EVESharp.EVE.Exceptions;
using EVESharp.EVE.Exceptions.dogma;
using EVESharp.EVE.Notifications;
using EVESharp.EVE.Notifications.Inventory;
using EVESharp.EVE.Sessions;
using EVESharp.Types;
using Environment = EVESharp.EVE.Dogma.Interpreter.Environment;

namespace EVESharp.EVE.Dogma;

public class ItemEffects
{
    /// <summary>
    /// The item to apply effects to
    /// </summary>
    public ShipModule Module { get; }
    /// <summary>
    /// The session attached to this item effects
    /// </summary>
    public Session Session { get; }
    /// <summary>
    /// The item factory to handle the item
    /// </summary>
    private IItems ItemFactory { get; }
    /// <summary>
    /// The dogma notificator
    /// </summary>
    private IDogmaNotifications DogmaNotifications { get; }

    public ItemEffects (ShipModule item, IItems itemFactory, IDogmaNotifications dogmaNotifications, Session session)
    {
        this.Module             = item;
        this.ItemFactory        = itemFactory;
        this.Session            = session;
        this.DogmaNotifications = dogmaNotifications;

        foreach ((int effectID, Effect effect) in this.Module.Type.Effects)
            // create effects entry in the list
            this.Module.Effects [effectID] = new GodmaShipEffect
            {
                AffectedItem = this.Module,
                Effect       = effect,
                ShouldStart  = false,
                StartTime    = 0,
                Duration     = 0
            };

        if (this.Module.IsInModuleSlot () || this.Module.IsInRigSlot ())
        {
            // apply passive effects
            this.ApplyPassiveEffects (session);

            // special case, check for the isOnline attribute and put the module online if so
            if (this.Module.Attributes [AttributeTypes.isOnline] == 1)
                this.ApplyEffect ("online", session);
        }
    }

    public void ApplyEffect (string effectName, Session session = null)
    {
        // check if the module has the given effect in it's list
        if (this.Module.Type.EffectsByName.TryGetValue (effectName, out Effect effect) == false)
            throw new EffectNotActivatible (this.Module.Type);
        if (this.Module.Effects.TryGetEffect (effect.EffectID, out GodmaShipEffect godmaEffect) == false)
            throw new CustomError ("Cannot apply the given effect, our type has it but we dont");

        this.ApplyEffect (effect, godmaEffect, session);
    }

    private void ApplyEffect (Effect effect, GodmaShipEffect godmaEffect, Session session = null)
    {
        if (godmaEffect.ShouldStart)
            return;

        Ship      ship      = this.ItemFactory.GetItem <Ship> (this.Module.LocationID);
        Character character = this.ItemFactory.GetItem <Character> (this.Module.OwnerID);

        try
        {
            // create the environment for this run
            Environment env = new Environment
            {
                Character          = character,
                Self               = this.Module,
                Ship               = ship,
                Target             = null,
                Session            = session,
                ItemFactory        = this.ItemFactory,
                DogmaNotifications = this.DogmaNotifications,
            };

            Opcode opcode = new Interpreter.Interpreter (env).Run (effect.PreExpression.VMCode);

            if (opcode is OpcodeRunnable runnable)
                runnable.Execute ();
            else if (opcode is OpcodeWithBooleanOutput booleanOutput)
                booleanOutput.Execute ();
            else if (opcode is OpcodeWithDoubleOutput doubleOutput)
                doubleOutput.Execute ();
        }
        catch (System.Exception)
        {
            // notify the client about it
            // TODO: THIS MIGHT NEED MORE NOTIFICATIONS
            this.DogmaNotifications.QueueMultiEvent (session.EnsureCharacterIsSelected (), new OnGodmaShipEffect (godmaEffect));

            throw;
        }

        // ensure the module is saved
        this.Module.Persist ();

        PyDataType duration = 0;

        if (effect.DurationAttributeID is not null)
            duration = this.Module.Attributes [(int) effect.DurationAttributeID];

        // update things like duration, start, etc
        godmaEffect.StartTime   = DateTime.UtcNow.ToFileTimeUtc ();
        godmaEffect.ShouldStart = true;
        godmaEffect.Duration    = duration;

        // notify the client about it
        // TODO: THIS MIGHT NEED MORE NOTIFICATIONS
        // TODO: CHECK IF THIS MULTIEVENT IS RIGHT OR NOT
        this.DogmaNotifications.QueueMultiEvent (this.Module.OwnerID, new OnGodmaShipEffect (godmaEffect));

        if (effect.EffectID == (int) EffectsEnum.Online)
            this.ApplyOnlineEffects (session);
    }

    public void StopApplyingEffect (string effectName, Session session = null)
    {
        // check if the module has the given effect in it's list
        if (this.Module.Type.EffectsByName.TryGetValue (effectName, out Effect effect) == false)
            throw new EffectNotActivatible (this.Module.Type);
        if (this.Module.Effects.TryGetEffect (effect.EffectID, out GodmaShipEffect godmaEffect) == false)
            throw new CustomError ("Cannot apply the given effect, our type has it but we dont");

        this.StopApplyingEffect (effect, godmaEffect, session);
    }

    private void StopApplyingEffect (Effect effect, GodmaShipEffect godmaEffect, Session session = null)
    {
        // ensure the effect is being applied before doing anything
        if (godmaEffect.ShouldStart == false)
            return;

        Ship      ship      = this.ItemFactory.GetItem <Ship> (this.Module.LocationID);
        Character character = this.ItemFactory.GetItem <Character> (this.Module.OwnerID);

        // create the environment for this run
        Environment env = new Environment
        {
            Character          = character,
            Self               = this.Module,
            Ship               = ship,
            Target             = null,
            Session            = session,
            ItemFactory        = this.ItemFactory,
            DogmaNotifications = DogmaNotifications,
        };

        Opcode opcode = new Interpreter.Interpreter (env).Run (effect.PostExpression.VMCode);

        if (opcode is OpcodeRunnable runnable)
            runnable.Execute ();
        else if (opcode is OpcodeWithBooleanOutput booleanOutput)
            booleanOutput.Execute ();
        else if (opcode is OpcodeWithDoubleOutput doubleOutput)
            doubleOutput.Execute ();

        // ensure the module is saved
        this.Module.Persist ();

        // update things like duration, start, etc
        godmaEffect.StartTime   = 0;
        godmaEffect.ShouldStart = false;
        godmaEffect.Duration    = 0;

        // notify the client about it
        // TODO: THIS MIGHT NEED MORE NOTIFICATIONS
        this.DogmaNotifications.QueueMultiEvent (session.EnsureCharacterIsSelected (), new OnGodmaShipEffect (godmaEffect));

        // online effect, this requires some special processing as all the passive effects should also be applied
        if (effect.EffectID == (int) EffectsEnum.Online)
            this.StopApplyingOnlineEffects (session);
    }

    private void ApplyEffectsByCategory (EffectCategory category, Session session = null)
    {
        foreach ((int _, GodmaShipEffect effect) in this.Module.Effects)
            if (effect.Effect.EffectCategory == category && effect.ShouldStart == false)
                this.ApplyEffect (effect.Effect, effect, session);
    }

    private void StopApplyingEffectsByCategory (EffectCategory category, Session session = null)
    {
        foreach ((int _, GodmaShipEffect effect) in this.Module.Effects)
            if (effect.Effect.EffectCategory == category && effect.ShouldStart)
                this.StopApplyingEffect (effect.Effect, effect, session);
    }

    private void ApplyOnlineEffects (Session session = null)
    {
        this.ApplyEffectsByCategory (EffectCategory.Online, session);
    }

    private void StopApplyingOnlineEffects (Session session = null)
    {
        this.StopApplyingEffectsByCategory (EffectCategory.Online, session);
    }

    public void ApplyPassiveEffects (Session session = null)
    {
        this.ApplyEffectsByCategory (EffectCategory.Passive, session);
    }

    public void StopApplyingPassiveEffects (Session session = null)
    {
        this.StopApplyingEffectsByCategory (EffectCategory.Passive, session);
    }
}