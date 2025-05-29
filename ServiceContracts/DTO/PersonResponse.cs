using Entities;
using ServiceContracts.Enums;

namespace ServiceContracts.DTO;

/// <summary>
/// DTO class used as a return type of most methods of Persons Service
/// </summary>
public class PersonResponse
{
    public Guid PersonId { get; set; }
    public string? PersonName { get; set; }
    public string? Email { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Gender { get; set; }
    public Guid? CountryID { get; set; }
    public string? Country { get; set; }
    public string? Address { get; set; }
    public bool ReceiveNewsletter { get; set; }
    public double? Age { get; set; }

    /// <summary>
    /// Compares the current object data with the parameter object
    /// </summary>
    /// <param name="obj"></param>
    /// <returns>True if all person details are matched with the specified parameter object</returns>
    public override bool Equals(object? obj)
    {
        if (obj == null) return false;

        if (obj.GetType() != typeof(PersonResponse)) return false;

        PersonResponse person = (PersonResponse)obj;
        return PersonId == person.PersonId && PersonName == person.PersonName && Email == person.Email &&
               DateOfBirth == person.DateOfBirth && Gender == person.Gender && CountryID == person.CountryID &&
               Address == person.Address && ReceiveNewsletter == person.ReceiveNewsletter;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}

/// <summary>
/// An extenstion method to convert an object of Person class into PersonResponse class
/// </summary>
public static class PersonExtensions
{
    public static PersonResponse ToPersonResponse(this Person person)
    {
        return new PersonResponse()
        {
            PersonId = person.PersonId, PersonName = person.PersonName, Email = person.Email,
            DateOfBirth = person.DateOfBirth, Gender = person.Gender, CountryID = person.CountryID,
            Address = person.Address, ReceiveNewsletter = person.ReceiveNewsletter,
            Age = (person.DateOfBirth != null)
                ? Math.Round((DateTime.Now - person.DateOfBirth.Value).TotalDays / 365.25)
                : null
        };
    }
}