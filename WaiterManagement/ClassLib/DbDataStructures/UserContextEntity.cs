using System;
using ClassLib.DataStructures;

namespace ClassLib.DbDataStructures
{
	/// <summary>
    /// Kontekst bazowy wszystkich użytkowników systemu. Jego klasy pochodne są zwracane po udanym zalogowaniu się do systemu
    /// </summary>
    public class UserContextEntity : DbEntity, IEquatable<UserContextEntity>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Login { get; set; }
        public UserRole Role { get; set; }

        /// <summary>
        /// Metoda porównująca wszystkie właściwości klasy (oprócz Id)
        /// </summary>
        public bool Equals(UserContextEntity other)
        {
            return this.FirstName.Equals(other.FirstName)
                && this.LastName.Equals(other.LastName)
                && this.Login.Equals(other.Login)
                && this.Role.HasFlag(other.Role);
        }

        public void CopyData(UserContext userContextTransferObject)
        {
            this.FirstName = userContextTransferObject.FirstName;
            this.LastName = userContextTransferObject.LastName;
            this.Login = userContextTransferObject.Login;
            this.Role = userContextTransferObject.Role;
        }
    }
}
