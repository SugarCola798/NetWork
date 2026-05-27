using System.Collections.Generic;

public static class BulletStrategyFactory
{
    public static IBulletMove CreateMove(BulletMoveType moveType)
    {
        switch (moveType)
        {
            case BulletMoveType.Homing:
                return new BulletHomingMove();
            case BulletMoveType.Bounce:
                return new BulletBounceMove();
            case BulletMoveType.Beam:
                return new BulletBeamMove();
            case BulletMoveType.Straight:
            default:
                return new BulletStraightMove();
        }
    }

    public static List<IBulletEffect> CreateEffects(IReadOnlyList<BulletEffectType> effectTypes)
    {
        var effects = new List<IBulletEffect>();
        if (effectTypes == null || effectTypes.Count == 0)
        {
            effects.Add(new BulletNormalEffect());
            return effects;
        }

        for (int i = 0; i < effectTypes.Count; i++)
        {
            effects.Add(CreateEffect(effectTypes[i]));
        }

        return effects;
    }

    private static IBulletEffect CreateEffect(BulletEffectType effectType)
    {
        switch (effectType)
        {
            case BulletEffectType.Burn:
                return new BulletBurnEffect();
            case BulletEffectType.Slow:
                return new BulletSlowEffect();
            case BulletEffectType.Knockback:
                return new BulletKnockbackEffect();
            case BulletEffectType.Splash:
                return new BulletSplashEffect();
            case BulletEffectType.Normal:
            default:
                return new BulletNormalEffect();
        }
    }
}
