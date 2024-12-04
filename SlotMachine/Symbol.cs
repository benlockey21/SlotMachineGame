namespace SlotMachine;
public class Symbol
{
    public string Type { get; set; }
    public decimal Coefficient { get; set; }
    public int PercentageChance { get; set; }
    public bool IsWildcard { get; set; }
}
