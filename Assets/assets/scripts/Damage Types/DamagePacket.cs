public class DamagePacket
{
    public int amount;

    public DamageType damageType;

    public CharacterState source;

    public CharacterState target;

    public bool offensiveRollDamage = false;
}   