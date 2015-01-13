using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ClassLib.DataStructures
{
    /// <summary>
    /// Wyliczenie możliych ról użytkownika
    /// </summary>
    [DataContract]
    [Flags]
    public enum UserRole
    {
        [EnumMember]
        Client = 0x001,
        [EnumMember]
        Waiter = 0x010,
        [EnumMember]
        Manager = 0x100,
    }

    /// <summary>
    /// Stan Zamówienia
    /// </summary>
    [DataContract]
    [Flags]
    public enum OrderState
    {
        /// <summary>
        /// Złożone
        /// </summary>
        [EnumMember]
        Placed,
        /// <summary>
        /// Zaakceptowane
        /// </summary>
        [EnumMember]
        Accepted,
        /// <summary>
        /// Zrealizowane
        /// </summary>
        [EnumMember]
        Realized,
        /// <summary>
        /// Nie zrealizowane
        /// </summary>
        [EnumMember]
        NotRealized,
    }

    /// <summary>
    ///Klasa pomocnicza do reprezentująca wartości pieniężne
    /// </summary>
    [ComplexType]
    [DataContract]
    public class Money : IEquatable<Money>
    {
        [DataMember]
        public float Amount { get; set; }
        [DataMember]
        public string Currency { get; set; }

        /// <summary>
        /// Metoda porównująca wszystkie właściwości klasy (oprócz Id)
        /// </summary>
        public bool Equals(Money other)
        {
            return this.Amount.Equals(other.Amount)
                && this.Currency.Equals(other.Currency);
        }
    }
}
