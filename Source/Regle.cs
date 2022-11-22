namespace Inference;

public class Regle
{
    /// <summary>
    /// Une liste de faits qui forme les premisses de la regle, avec un "et" logique entre ces faits
    /// </summary>
    public List<Fait> Premisses { get; set; } = new();

    /// <summary>
    /// La conclusion de la règle
    /// </summary>
    public List<Fait> Actions { get; set; } = new();

    /// <summary>
    /// Le numero de la regle
    /// </summary>
    public string Numero { get; set; } = string.Empty;

    public override string ToString()
    {
        // Règle(i): SI
        var output = $"Règle({Numero}): SI ";

        // A et B et C
        Premisses.ForEach(c => output += c + " et ");

        output = output.Substring(0, output.Length - 4);

        // Alors
        output += $" ALORS ";

        // X, Y, Z
        Actions.ForEach(c => output += c + ", ");

        output = output.Substring(0, output.Length - 2);

        // Règle(i): Si A et B et C Alors X, Y
        return output;
    }
}
