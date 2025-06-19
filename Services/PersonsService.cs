using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using Services.Helpers;

namespace Services;

public class PersonsService : IPersonsService
{
    private readonly List<Person> _persons;
    private readonly ICountriesService _countriesService;

    public PersonsService()
    {
        _persons = new List<Person>();
        _countriesService = new CountriesService();
    }

    private PersonResponse ConvertPersonToPersonResponse(Person person)
    {
        // Convert the Person to PersonResponse and return
        PersonResponse personResponse = person.ToPersonResponse();
        personResponse.Country = _countriesService.GetCountryById(person.CountryID)?.CountryName;
        return personResponse;
    }

    public PersonResponse AddPerson(PersonAddRequest? personAddRequest)
    {
        // Check if the personAddRequest is null
        if (personAddRequest == null)
        {
            throw new ArgumentNullException(
                nameof(personAddRequest),
                "Person add request cannot be null."
            );
        }

        // Validate the personAddRequest using Model Validation helper
        ValidationHelper.ModelValidation(personAddRequest);

        // Convert the PersonAddRequest to PersonResponse
        Person person = personAddRequest.ToPerson();

        // Generate a new PersonID
        person.PersonId = Guid.NewGuid();

        // Add the person to the list
        _persons.Add(person);

        return ConvertPersonToPersonResponse(person);
    }

    public PersonResponse? GetPersonByPersonId(Guid? personID)
    {
        if (personID == null)
            return null;

        Person? person = _persons.FirstOrDefault(p => p.PersonId == personID);

        if (person == null)
            return null;

        return person.ToPersonResponse();
    }

    public List<PersonResponse> GetAllPersons()
    {
        return _persons.Select(p => p.ToPersonResponse()).ToList();
    }
}
