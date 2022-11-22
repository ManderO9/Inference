using System.Text;
using System.Text.RegularExpressions;

namespace Inference;

public class Program
{

    public static void Main(string[] args)
    {

        var playground = new Playground();

        var exo1Regles = "exo1Regles.txt";
        var exo1Faits = "exo1Faits.txt";


        var exo2Regles = "exo2Regles.txt";
        var exo2Faits = "exo2Faits.txt";

        while (true)
        {
            // Clear the screen
            Console.Clear();

            // Le path vers les règles
            var reglesPath = "";

            // Le path vers la base de faits
            var baseDeFaitsPath = "";


            // Lire le numéto de l'exo que l'utilisateur veut choisire
            Console.WriteLine("Choisir le numero de l'exo que vous voulez tester, entrer 1 ou 2:");
            var numeroExo = Console.ReadLine() ?? "";

            // Supprimer les blancs de la fin et debut du numéro
            numeroExo = numeroExo.Trim();

            // Si c'est l'exo 1
            if (numeroExo == "1")
            {
                reglesPath = exo1Regles;
                baseDeFaitsPath = exo1Faits;
            }
            // Si c'est l'exo 2
            else if (numeroExo == "2")
            {
                reglesPath = exo2Regles;
                baseDeFaitsPath = exo2Faits;
            }
            // Sinon
            else
            {
                // Afficher une erreur
                Console.WriteLine("vous avez entré un nombre incorrect, cliquer entrer pour rééssayer");
                Console.ReadLine();
                Console.Clear();
                continue;
            }

            // Lire la liste de regles du fichier
            playground.ListeDeRegles = File.ReadAllLines(reglesPath, Encoding.Latin1).Select(x => TraduireRegle(x)).ToList();

            // Lire la base de faits
            playground.BaseDeFaits = File.ReadAllText(baseDeFaitsPath).Split(" ").Select(x => new Fait(x)).ToList();


            // Afficher la liste des règles
            Console.WriteLine("Liste des règles: ");
            playground.ListeDeRegles.ForEach(x => Console.WriteLine(x));
            Console.WriteLine(playground.Separator);

            // Afficher la base de faits initiale
            Console.WriteLine(playground.PrintBaseDeFaits());
            Console.WriteLine(playground.Separator);

            // Demander le fait qu'on veut démontrer
            Console.WriteLine(Environment.NewLine + Environment.NewLine + Environment.NewLine
                + "Quel fait voulez vous démontrer ?");

            // Lire le fait entré par l'utilisateur
            var fait = new Fait(Console.ReadLine() ?? "");


            // Faire la démonstration avec le chainage arrière
            Console.WriteLine(playground.Separator);
            Console.WriteLine("Démonstration avec chainage arrière");

            // Avoir le résultat de la démonstration
            var result = playground.DemontrerChainageArriere(playground.BaseDeFaits, playground.ListeDeRegles, fait);

            // Si on a démontrer le fait...
            if (result)
                Console.WriteLine($"Le fait: {fait} a été démonstré");
            // Sinon...
            else
                Console.WriteLine($"Impossible de démonstrer le fait: {fait}");

            Console.WriteLine(playground.Separator);

            // Lire la liste de règles du fichier
            playground.ListeDeRegles = File.ReadAllLines(reglesPath, Encoding.Latin1).Select(x => TraduireRegle(x)).ToList();

            // Lire la base de faits
            playground.BaseDeFaits = File.ReadAllText(baseDeFaitsPath).Split(" ").Select(x => new Fait(x)).ToList();

            // Faire la démonstration avec le chainage avant
            Console.WriteLine("Démonstration avec chainage avant");
            playground.DemontrerChainageAvant(fait);


            Console.ReadLine();
        }
    }

    /// <summary>
    /// Takes a line from the file and translates that line to a rule
    /// </summary>
    /// <param name="input">The line to translate</param>
    /// <returns>A new rule representing the line passed in</returns>
    public static Regle TraduireRegle(string input)
    {
        // The name of the premisses group
        const string premissesGroup = "premisses";

        // The name of the action group
        const string actionsGroup = "actions";

        // The name of the rule number group
        const string numeroGroup = "numero";

        // The pattern to match
        var pattern = $"^Règle\\((?<{numeroGroup}>(\\d*))\\)\\s*:\\s*SI\\s*\\((?<{premissesGroup}>(\\s*[a-z]+\\s*,?\\s*)+)\\)\\s*,\\s*Alors\\s*\\((?<{actionsGroup}>(\\s*[a-z]+\\s*,?\\s*)+)\\)";

        // Create a regex
        var regex = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.ExplicitCapture);

        // Get the matches for the passed in rule
        var matches = regex.Matches(input);

        // Create the list of premisses
        var premisses = matches[0].Groups.GetValueOrDefault(premissesGroup)?.Value.Replace(" ", "").Split(",").Select(x => new Fait(x)).ToList();

        // Create the list of actions
        var actions = matches[0].Groups.GetValueOrDefault(actionsGroup)?.Value.Replace(" ", "").Split(",").Select(x => new Fait(x)).ToList();

        // Get the number of the rule
        var number = matches[0].Groups.GetValueOrDefault(numeroGroup)?.Value;

        // The rule containing the premisses and the actions
        return new Regle() { Actions = actions ?? new(), Premisses = premisses ?? new(), Numero = number ?? "" };
    }

}