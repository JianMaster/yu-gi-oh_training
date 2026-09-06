
public abstract class CardBase {
    public int Belong { get; set; }
    public string ID { get; protected set; }
    public string Name { get; protected set; }
    public CardType CardType { get; protected set; }
    public CardFace Face { get; protected set; }

    public void Destroy() {
        
    }

}
