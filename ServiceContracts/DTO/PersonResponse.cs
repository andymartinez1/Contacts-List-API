using Entities;
using ServiceContracts.Enums;

namespace ServiceContracts.DTO;

/// <summary>
/// DTO class used as a return type of most methods of Persons Service
/// </summary>
public class PersonResponse
{
    public Guid PersonID { get; set; }
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
        if (obj == null)
            return false;

        if (obj.GetType() != typeof(PersonResponse))
            return false;

        PersonResponse person = (PersonResponse)obj;
        return PersonID == person.PersonID
            && PersonName == person.PersonName
            && Email == person.Email
            && DateOfBirth == person.DateOfBirth
            && Gender == person.Gender
            && CountryID == person.CountryID
            && Address == person.Address
            && ReceiveNewsletter == person.ReceiveNewsletter;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    // Overriding ToString method to provide a string representation of the PersonResponse object for testing and debugging purposes
    public override string ToString()
    {
        return $"Person ID: {PersonID}, Name: {PersonName}, Email: {Email}, Date of Birth: {DateOfBirth}, Gender: {Gender}, Country ID: {CountryID}, Country Name: {Country}, Address: {Address}, Receive News Letter: {ReceiveNewsletter}";
    }

    public PersonUpdateRequest ToPersonUpdateRequest()
    {
        return new PersonUpdateRequest()
        {
            PersonId = PersonID,
            PersonName = PersonName,
            Email = Email,
            DateOfBirth = DateOfBirth,
            Gender = (GenderOptions)Enum.Parse(typeof(GenderOptions), Gender, true),
            CountryID = CountryID,
            Address = Address,
            ReceiveNewsletter = ReceiveNewsletter,
        };
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
            PersonID = person.PersonId,
            PersonName = person.PersonName,
            Email = person.Email,
            DateOfBirth = person.DateOfBirth,
            Gender = person.Gender,
            CountryID = person.CountryID,
            Address = person.Address,
            ReceiveNewsletter = person.ReceiveNewsletter,
            Age =
                (person.DateOfBirth != null)
                    ? Math.Round((DateTime.Now - person.DateOfBirth.Value).TotalDays / 365.25)
                    : null,
        };
    }
}
