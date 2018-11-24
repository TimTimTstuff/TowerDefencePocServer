namespace TStuff.Game.TowerDefense3d.lib.Enums
{
    public enum MobStatusEffect
    {
        //On Movement Calc
        Slowed, //Reduce speed by %
        Stun, //Set Speed to 0
        //On Damage calc
        ArmoreReduced, //Reduce armor by %
        //Own Calc
        Dot, // Do Damage over time
        Hot //Heals by %
        
    }
}