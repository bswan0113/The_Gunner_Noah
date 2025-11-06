namespace Utils
{
    public enum CharacterState
    {
        Idle,
        AttackReady,
        EvadeReady,
        ParryReady,
        Attacking,
        Evading,
        Parrying,
    }

    public enum HitType
    {
        Hit,      // 일반 피격
        Miss,     // 헛스윙
        Parried,  // 상대가 패리함
        Evaded    // 상대가 회피함
    }
}