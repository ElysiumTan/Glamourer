﻿using Penumbra.GameData.Actors;
using System;
using FFXIVClientStructs.FFXIV.Client.Game.Character;
using FFXIVClientStructs.FFXIV.Client.Game.Object;
using Penumbra.GameData.Enums;
using Penumbra.GameData.Structs;

namespace Glamourer.Interop.Structs;

public readonly unsafe struct Actor : IEquatable<Actor>
{
    private Actor(nint address)
        => Address = address;

    public static readonly Actor Null = new(nint.Zero);

    public readonly nint Address;

    public GameObject* AsObject
        => (GameObject*)Address;

    public Character* AsCharacter
        => (Character*)Address;

    public bool Valid
        => Address != nint.Zero;

    public bool IsCharacter
        => Valid && AsObject->IsCharacter();

    public static implicit operator Actor(nint? pointer)
        => new(pointer ?? nint.Zero);

    public static implicit operator Actor(GameObject* pointer)
        => new((nint)pointer);

    public static implicit operator Actor(Character* pointer)
        => new((nint)pointer);

    public static implicit operator nint(Actor actor)
        => actor.Address;

    public ActorIdentifier GetIdentifier(ActorManager actors)
        => actors.FromObject(AsObject, out _, true, true, false);

    public bool Identifier(ActorManager actors, out ActorIdentifier ident)
    {
        if (Valid)
        {
            ident = GetIdentifier(actors);
            return ident.IsValid;
        }

        ident = ActorIdentifier.Invalid;
        return false;
    }

    public Model Model
        => Valid ? AsObject->DrawObject : null;

    public static implicit operator bool(Actor actor)
        => actor.Address != nint.Zero;

    public static bool operator true(Actor actor)
        => actor.Address != nint.Zero;

    public static bool operator false(Actor actor)
        => actor.Address == nint.Zero;

    public static bool operator !(Actor actor)
        => actor.Address == nint.Zero;

    public bool Equals(Actor other)
        => Address == other.Address;

    public override bool Equals(object? obj)
        => obj is Actor other && Equals(other);

    public override int GetHashCode()
        => Address.GetHashCode();

    public static bool operator ==(Actor lhs, Actor rhs)
        => lhs.Address == rhs.Address;

    public static bool operator !=(Actor lhs, Actor rhs)
        => lhs.Address != rhs.Address;

    /// <summary> Only valid for characters. </summary>
    public CharacterArmor GetArmor(EquipSlot slot)
        => ((CharacterArmor*)&AsCharacter->DrawData.Head)[slot.ToIndex()];

    public CharacterWeapon GetMainhand()
        => *(CharacterWeapon*)&AsCharacter->DrawData.MainHandModel;

    public CharacterWeapon GetOffhand()
        => *(CharacterWeapon*)&AsCharacter->DrawData.OffHandModel;
}
