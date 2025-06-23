using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services.Helpers;

namespace Services;

public class PersonsService : IPersonsService
{
    private readonly List<Person> _persons;
    private readonly ICountriesService _countriesService;

    public PersonsService(bool initializePersons = true)
    {
        _persons = new List<Person>();
        _countriesService = new CountriesService();
        if (initializePersons)
        {
            // Initialize with some default persons
            _persons.AddRange(
                new List<Person>()
                {
                    new Person
                    {
                        PersonId = Guid.Parse("75227058-0432-4844-B037-DB2E422B2C30"),
                        PersonName = "Ive",
                        Email = "imacswayde0@tamu.edu",
                        Address = "79 Dottie Plaza",
                        CountryID = Guid.Parse("4A656578-3623-4A33-977B-62969CBD4056"),
                        DateOfBirth = DateTime.Parse("06/04/2012"),
                        Gender = "Male",
                        ReceiveNewsletter = true,
                    },
                    new Person
                    {
                        PersonId = Guid.Parse("D6DACD8D-A886-4C26-8596-D086172BB2FF"),
                        PersonName = "Cal",
                        Email = "cmckomb1@miitbeian.gov",
                        Address = "926 Jackson Drive",
                        CountryID = Guid.Parse("E1230C55-DD31-4385-86CB-AA83A1B091F5"),
                        DateOfBirth = DateTime.Parse("10/17/2009"),
                        Gender = "Female",
                        ReceiveNewsletter = false,
                    },
                    new Person
                    {
                        PersonId = Guid.Parse("EBBFE40A-2BF4-4EDC-B02C-CEF1E9B3B64D"),
                        PersonName = "Rodolphe",
                        Email = "rbasil2@weather.com",
                        Address = "926 Jackson Drive",
                        CountryID = Guid.Parse("3534980C-3DED-49F7-BE69-F91BDEF89BA1"),
                        DateOfBirth = DateTime.Parse("03/07/2000"),
                        Gender = "Other",
                        ReceiveNewsletter = false,
                    },
                    new Person
                    {
                        PersonId = Guid.Parse("FE20B6A0-DC50-4E3A-8FB5-DDDB0CF5EEC5"),
                        PersonName = "Harry",
                        Email = "hdurtnal3@accuweather.com",
                        Address = "45631 Sycamore Place",
                        CountryID = Guid.Parse("AB33587A-1FC6-4FE2-BBFC-23E18694631B"),
                        DateOfBirth = DateTime.Parse("08/23/2002"),
                        Gender = "Male",
                        ReceiveNewsletter = true,
                    },
                    new Person
                    {
                        PersonId = Guid.Parse("27546559-51AE-4816-899E-FBCB3D76CE89"),
                        PersonName = "Sunshine",
                        Email = "srude6@cisco.com",
                        Address = "5 Ruskin Point",
                        CountryID = Guid.Parse("32A5FD37-80E7-4DDE-AF12-749C20E1E7E6"),
                        DateOfBirth = DateTime.Parse("04/10/1998"),
                        Gender = "Female",
                        ReceiveNewsletter = true,
                    },
                }
            );
        }
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

    public List<PersonResponse> GetFilteredPersons(string searchBy, string? searchString)
    {
        List<PersonResponse> allPersons = GetAllPersons();
        List<PersonResponse> matchingPersons = allPersons;

        if (string.IsNullOrEmpty(searchBy) || string.IsNullOrEmpty(searchString))
            return matchingPersons;

        switch (searchBy)
        {
            case nameof(Person.PersonName):
                matchingPersons = allPersons
                    .Where(temp =>
                        (
                            string.IsNullOrEmpty(temp.PersonName)
                            || temp.PersonName.Contains(
                                searchString,
                                StringComparison.OrdinalIgnoreCase
                            )
                        )
                    )
                    .ToList();
                break;

            case nameof(Person.Email):
                matchingPersons = allPersons
                    .Where(temp =>
                        (
                            string.IsNullOrEmpty(temp.Email)
                            || temp.Email.Contains(searchString, StringComparison.OrdinalIgnoreCase)
                        )
                    )
                    .ToList();
                break;

            case nameof(Person.DateOfBirth):
                matchingPersons = allPersons
                    .Where(temp =>
                        (temp.DateOfBirth == null)
                        || temp.DateOfBirth.Value.ToString("MM-dd-yyyy")
                            .Contains(searchString, StringComparison.OrdinalIgnoreCase)
                    )
                    .ToList();
                break;

            case nameof(Person.Gender):
                matchingPersons = allPersons
                    .Where(temp =>
                        (
                            string.IsNullOrEmpty(temp.Gender)
                            || temp.Gender.Contains(
                                searchString,
                                StringComparison.OrdinalIgnoreCase
                            )
                        )
                    )
                    .ToList();
                break;

            case nameof(Person.CountryID):
                matchingPersons = allPersons
                    .Where(temp =>
                        (
                            string.IsNullOrEmpty(temp.Country)
                            || temp.Country.Contains(
                                searchString,
                                StringComparison.OrdinalIgnoreCase
                            )
                        )
                    )
                    .ToList();
                break;

            case nameof(Person.Address):
                matchingPersons = allPersons
                    .Where(temp =>
                        (
                            string.IsNullOrEmpty(temp.Address)
                            || temp.Address.Contains(
                                searchString,
                                StringComparison.OrdinalIgnoreCase
                            )
                        )
                    )
                    .ToList();
                break;

            default:
                matchingPersons = allPersons;
                break;
        }

        return matchingPersons;
    }

    public List<PersonResponse> GetSortedPersons(
        List<PersonResponse> allPersons,
        string sortBy,
        SortOrderOptions sortOrder
    )
    {
        if (string.IsNullOrEmpty(sortBy))
            return allPersons;

        List<PersonResponse> sortedPersons = (sortBy, sortOrder) switch
        {
            (nameof(PersonResponse.PersonName), SortOrderOptions.ASC) => allPersons
                .OrderBy(pr => pr.PersonName, StringComparer.OrdinalIgnoreCase)
                .ToList(),
            (nameof(PersonResponse.PersonName), SortOrderOptions.DESC) => allPersons
                .OrderByDescending(pr => pr.PersonName, StringComparer.OrdinalIgnoreCase)
                .ToList(),
            (nameof(PersonResponse.Email), SortOrderOptions.ASC) => allPersons
                .OrderBy(pr => pr.Email, StringComparer.OrdinalIgnoreCase)
                .ToList(),
            (nameof(PersonResponse.Email), SortOrderOptions.DESC) => allPersons
                .OrderByDescending(pr => pr.Email, StringComparer.OrdinalIgnoreCase)
                .ToList(),
            (nameof(PersonResponse.DateOfBirth), SortOrderOptions.ASC) => allPersons
                .OrderBy(pr => pr.DateOfBirth)
                .ToList(),
            (nameof(PersonResponse.DateOfBirth), SortOrderOptions.DESC) => allPersons
                .OrderByDescending(pr => pr.DateOfBirth)
                .ToList(),
            (nameof(PersonResponse.Age), SortOrderOptions.ASC) => allPersons
                .OrderBy(pr => pr.Age)
                .ToList(),
            (nameof(PersonResponse.Age), SortOrderOptions.DESC) => allPersons
                .OrderByDescending(pr => pr.Age)
                .ToList(),
            (nameof(PersonResponse.Gender), SortOrderOptions.ASC) => allPersons
                .OrderBy(pr => pr.Gender, StringComparer.OrdinalIgnoreCase)
                .ToList(),
            (nameof(PersonResponse.Gender), SortOrderOptions.DESC) => allPersons
                .OrderByDescending(pr => pr.Gender, StringComparer.OrdinalIgnoreCase)
                .ToList(),
            (nameof(PersonResponse.Country), SortOrderOptions.ASC) => allPersons
                .OrderBy(pr => pr.Country, StringComparer.OrdinalIgnoreCase)
                .ToList(),
            (nameof(PersonResponse.Country), SortOrderOptions.DESC) => allPersons
                .OrderByDescending(pr => pr.Country, StringComparer.OrdinalIgnoreCase)
                .ToList(),
            (nameof(PersonResponse.Address), SortOrderOptions.ASC) => allPersons
                .OrderBy(pr => pr.Address, StringComparer.OrdinalIgnoreCase)
                .ToList(),
            (nameof(PersonResponse.Address), SortOrderOptions.DESC) => allPersons
                .OrderByDescending(pr => pr.Address, StringComparer.OrdinalIgnoreCase)
                .ToList(),
            (nameof(PersonResponse.ReceiveNewsletter), SortOrderOptions.ASC) => allPersons
                .OrderBy(pr => pr.Address)
                .ToList(),
            (nameof(PersonResponse.ReceiveNewsletter), SortOrderOptions.DESC) => allPersons
                .OrderByDescending(pr => pr.Address)
                .ToList(),
            _ => allPersons,
        };
        return sortedPersons;
    }

    public List<PersonResponse> GetAllPersons()
    {
        return _persons.Select(p => p.ToPersonResponse()).ToList();
    }

    public PersonResponse UpdatePerson(PersonUpdateRequest personUpdateRequest)
    {
        if (personUpdateRequest == null)
            throw new ArgumentNullException(nameof(personUpdateRequest));

        // Validate the personUpdateRequest using Model Validation helper
        ValidationHelper.ModelValidation(personUpdateRequest);

        // Find the person by PersonId
        Person? matchingPerson = _persons.FirstOrDefault(p =>
            p.PersonId == personUpdateRequest.PersonId
        );

        if (matchingPerson == null)
        {
            throw new ArgumentException(
                $"Person with ID {personUpdateRequest.PersonId} does not exist."
            );
        }

        // Update the matching person's details
        matchingPerson.PersonName = personUpdateRequest.PersonName;
        matchingPerson.Email = personUpdateRequest.Email;
        matchingPerson.DateOfBirth = personUpdateRequest.DateOfBirth;
        matchingPerson.Gender = personUpdateRequest.Gender.ToString();
        matchingPerson.CountryID = personUpdateRequest.CountryID;
        matchingPerson.Address = personUpdateRequest.Address;
        matchingPerson.ReceiveNewsletter = personUpdateRequest.ReceiveNewsletter;

        // Convert the updated person to PersonResponse and return
        return matchingPerson.ToPersonResponse();
    }

    public bool DeletePerson(Guid? personID)
    {
        if (personID == null)
        {
            throw new ArgumentNullException(nameof(personID));
        }

        // Find the person by PersonId
        Person? matchingPerson = _persons.FirstOrDefault(p => p.PersonId == personID);

        if (matchingPerson == null)
            return false;

        // Remove the person from the list
        _persons.Remove(matchingPerson);

        return true;
    }
}
