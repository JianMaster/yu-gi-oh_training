public struct AttackContext {
    public Player attacker;
    public Player opponent;
    public Card_Monster attackMonster;
    public Card_Monster targetMonster;
    public bool isDirectAttack;
    public Player getDamagePlayer;
    public int damage;
}