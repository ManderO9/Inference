using System.Diagnostics.CodeAnalysis;

namespace Inference
{
    public struct Fait
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="nom">The name of the fact</param>
        public Fait(string nom)
        {
            // Set name as upper case
            Nom = nom.ToUpper();
        }

        public string Nom { get; set; }

        public override string ToString() => Nom;

        #region Equals Override

        /// <summary>
        /// Override equals method so it compares the name of the facts for equality
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals([NotNullWhen(true)] object? obj) => obj == null ? false : ((Fait)obj).Nom == Nom;


        public override int GetHashCode() => throw new NotImplementedException();

        public static bool operator ==(Fait left, Fait right) => left.Equals(right);

        public static bool operator !=(Fait left, Fait right) => !(left == right);

        #endregion
    }
}
