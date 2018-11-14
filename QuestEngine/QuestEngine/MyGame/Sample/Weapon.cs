// <auto-generated>
//  automatically generated by the FlatBuffers compiler, do not modify
// </auto-generated>

namespace MyGame.Sample
{

using global::System;
using global::FlatBuffers;

public struct Weapon : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static Weapon GetRootAsWeapon(ByteBuffer _bb) { return GetRootAsWeapon(_bb, new Weapon()); }
  public static Weapon GetRootAsWeapon(ByteBuffer _bb, Weapon obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p.bb_pos = _i; __p.bb = _bb; }
  public Weapon __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public string Name { get { int o = __p.__offset(4); return o != 0 ? __p.__string(o + __p.bb_pos) : null; } }
  public ArraySegment<byte>? GetNameBytes() { return __p.__vector_as_arraysegment(4); }
  public short Damage { get { int o = __p.__offset(6); return o != 0 ? __p.bb.GetShort(o + __p.bb_pos) : (short)0; } }

  public static Offset<Weapon> CreateWeapon(FlatBufferBuilder builder,
      StringOffset nameOffset = default(StringOffset),
      short damage = 0) {
    builder.StartObject(2);
    Weapon.AddName(builder, nameOffset);
    Weapon.AddDamage(builder, damage);
    return Weapon.EndWeapon(builder);
  }

  public static void StartWeapon(FlatBufferBuilder builder) { builder.StartObject(2); }
  public static void AddName(FlatBufferBuilder builder, StringOffset nameOffset) { builder.AddOffset(0, nameOffset.Value, 0); }
  public static void AddDamage(FlatBufferBuilder builder, short damage) { builder.AddShort(1, damage, 0); }
  public static Offset<Weapon> EndWeapon(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<Weapon>(o);
  }
};


}
