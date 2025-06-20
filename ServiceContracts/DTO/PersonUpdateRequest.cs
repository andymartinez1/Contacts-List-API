using System.ComponentModel.DataAnnotations;
using Entities;
using ServiceContracts.Enums;

namespace ServiceContracts.DTO;

/// <summary>
/// Updating a Person DTO
/// </summary>
public class PersonUpdateRequest
{
    [Required(ErrorMessage = "Person ID is required.")]
    public Guid PersonId { get; set; }

    [Required(ErrorMessage = "Person name is required.")]
    public string? PersonName { get; set; }

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    public string? Email { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public GenderOptions? Gender { get; set; }

    public Guid? CountryID { get; set; }

    public string? Address { get; set; }

    public bool ReceiveNewsletter { get; set; }

    /// <summary>
    /// Converts the current object of PersonUpdateRequest into a new Person object
    /// </summary>
    /// <returns>Person object</returns>
    public Person ToPerson()
    {
        return new Person()
        {
            PersonId = PersonId,
            PersonName = PersonName,
            Email = Email,
            DateOfBirth = DateOfBirth,
            Gender = Gender.ToString(),
            CountryID = CountryID,
            Address = Address,
            ReceiveNewsletter = ReceiveNewsletter,
        };
    }
}
