namespace Inference;

public class Playground
{
    public string Separator { get; set; } = "-------------------------------------------------------------------------";

    public List<Fait> BaseDeFaits { get; set; } = new List<Fait>();

    public List<Regle> ListeDeRegles { get; set; } = new();



    // Algorithme chainage avant:

    //ChainageAvant(BF, BR, F)
    //DEBUT
    //  TANT QUE(F /∈ BF) ET(∃ R ∈ BR, applicable(R))
    //  FAIRE
    //      choisir une regle applicable R
    //      BR = BR - R(d ́esactivation de R)
    //      BF = BF union concl(R) (déclenchement de la règle R, ajout de sa conclusion)
    //  FIN TANT QUE
    //  SI F ∈ BF ALORS F est établi
    //  SINON F n’est pas  ́etabli
    //FIN



    //ChainageAvant(BF, BR, F)
    // BF et BR sont déclarées dans cette classe (Playground)
    public void DemontrerChainageAvant(Fait fait)
    {
        // La valeur de incrementation/decrementation de l'index de règle dans la boucle
        var upValue = -1;

        // Index de la prochaine règle a tester
        var indexRegle = 0;

        //  TANT QUE(F /∈ BF) ET (∃ R ∈ BR, applicable(R))
        // Applicable(R) = la liste des règles contient au moins une règle où tous ses premisses sont dans la base des faits
        while (!BaseDeFaits.Contains(fait) && ListeDeRegles.Any(regle => regle.Premisses.All(premisse => BaseDeFaits.Contains(premisse))))
        {
            if (indexRegle >= ListeDeRegles.Count - 1)
                indexRegle = ListeDeRegles.Count - 1;

            // choisir une regle applicable R
            Regle regleApplicable;
            while (!ListeDeRegles[indexRegle].Premisses.All(premisse => BaseDeFaits.Contains(premisse)))
            {
                if (indexRegle == ListeDeRegles.Count - 1 || indexRegle == 0)
                    upValue = -upValue;

                indexRegle += upValue;
            }
            regleApplicable = ListeDeRegles[indexRegle];

            // Si la conclusion de la règle est déjà dans la base de fait
            if (regleApplicable.Actions.All(action => BaseDeFaits.Contains(action)))
            // ignoré cette règle
            {
                ListeDeRegles.Remove(regleApplicable);
                continue;
            }

            // BR = BR - R(désactivation de R)
            ListeDeRegles.Remove(regleApplicable);

            // BF = BF union concl(R) (déclenchement de la règle R, ajout de sa conclusion)
            BaseDeFaits.AddRange(regleApplicable.Actions);


            // Afficher la règle qu'on a démontré
            Console.WriteLine($"Règle démontré: {regleApplicable}");

            // Afficher la base de faits
            Console.WriteLine(PrintBaseDeFaits() + Environment.NewLine + Separator);
        }

        // SI F ∈ BF ALORS F est établi
        if (BaseDeFaits.Contains(fait))
            Console.WriteLine($"Le fait {fait} est établi");

        // SINON F n’est pas établi
        else Console.WriteLine($"Le fait {fait} n'est pas établi");
    }





    //=================================================================================================================================
    //ChainageArriere (BF, BR, F)
    //DEBUT
    //  SI (F ∈ BF) ALORS ChainageArriere ← OK
    //  SINON
    //  construire ER ensemble de règles R, telle que F ∈ conclusion(R)
    //  FAIRE
    //      valide ← VRAI
    //      R ← premier element de ER
    //      ER ← ER – R
    //      POUR TOUT Fr ∈ premisse(R) valide ← VRAI ET ChainageArriere (BF, BR, Fr)
    //      FIN POUR
    //  JUSQU’à (valide OU ER 6 = ∅ )
    //  SI valide ALORS BF = BF ∪ F
    //  ChainageArriere ← valide
    //FIN


    //ChainageArriere (BF, BR, F)
    public bool DemontrerChainageArriere(List<Fait> baseDeFaits, List<Regle> baseDeRegles, Fait fait)
    {
        //  SI (F ∈ BF) ALORS ChainageArriere ← OK
        if (baseDeFaits.Contains(fait))
            return true;

        //  SINON
        //  construire ER ensemble de règles R, telle que F ∈ conclusion(R)
        var ensembleRegles = baseDeRegles.Where(regle => regle.Actions.Contains(fait)).ToList();

        bool valide;

        //  FAIRE
        do
        {
            // valide ← VRAI
            valide = true;

            // R ← premier element de ER
            var regle = ensembleRegles.FirstOrDefault();

            // Si on a aucune règle pour démontrer le fait, on va le considérer comme faut
            if (regle is null)
            {
                valide = false;

                // Pas besoin d'executer le code suivant car on a aucune règle à démontrer
                break;
            }

            // ER ← ER – R
            ensembleRegles.Remove(regle);


            // Remarque: l'algorithme du cours contient une erreur: 
            // Cette règle est fausse:
            // POUR TOUT Fr ∈ premisse(R) valide ← VRAI ET ChainageArriere (BF, BR, Fr)

            // Forme correcte: 
            // POUR TOUT Fr ∈ premisse(R) valide ← valide ET ChainageArriere (BF, BR, Fr)



            // POUR TOUT Fr ∈ premisse(R) valide ← valide ET ChainageArriere (BF, BR, Fr)
            foreach (var faitRegle in regle.Premisses)
                valide = valide && DemontrerChainageArriere(baseDeFaits, baseDeRegles, faitRegle);
            // FIN POUR

            // Afficher la règle qu'on a démontrer
            if (valide)
                Console.WriteLine($"{PrintBaseDeFaits(baseDeFaits)}{Environment.NewLine}règle démonstré: {regle}");

            // JUSQU’à (valide OU ER != ∅ )
        } while (!valide && ensembleRegles.Count > 0);

        //  SI valide ALORS BF = BF ∪ F
        if (valide) baseDeFaits.Add(fait);

        // ChainageArriere ← valide
        return valide;
        // FIN
    }





    //================================================================================================================================
    public string PrintBaseDeFaits(List<Fait>? baseDeFaits = null)
    {
        var output = $"Base de faits: ";

        if (baseDeFaits is not null)
            baseDeFaits.ForEach(a => output += a + " ");
        else
            BaseDeFaits.ForEach(a => output += a + " ");

        return output;
    }
}
