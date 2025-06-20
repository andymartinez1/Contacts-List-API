using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services;
using Xunit.Abstractions;

namespace CRUD_Tests;

public class PersonsServiceTest
{
    private readonly IPersonsService _personsService;
    private readonly ICountriesService _countriesService;
    private readonly ITestOutputHelper _testOutputHelper;

    public PersonsServiceTest(ITestOutputHelper testOutputHelper)
    {
        // Initialize the services
        _personsService = new PersonsService();
        _countriesService = new CountriesService();
        _testOutputHelper = testOutputHelper;
    }

    #region AddPerson

    // When PersonAddRequest is null, it should throw an ArgumentNullException
    [Fact]
    public void AddPerson_NullPerson()
    {
        // Arrange
        PersonAddRequest? personAddRequest = null;

        // Assert
        Assert.Throws<ArgumentNullException>(() =>
        {
            // Act
            _personsService.AddPerson(personAddRequest);
        });
    }

    // When person name is null or empty, it should throw an ArgumentException
    [Fact]
    public void AddPerson_PersonNameIsNull()
    {
        // Arrange
        PersonAddRequest? personAddRequest = new() { PersonName = null };

        // Assert
        Assert.Throws<ArgumentException>(() =>
        {
            // Act
            _personsService.AddPerson(personAddRequest);
        });
    }

    // When proper details provided, it should insert the person and return the details with PersonID
    [Fact]
    public void AddPerson_ProperPersonDetails()
    {
        // Arrange
        PersonAddRequest? personAddRequest = new()
        {
            PersonName = "John Doe",
            Email = "jdoe@example.com",
            Address = "123 Main St",
            CountryID = new Guid(),
            Gender = GenderOptions.Other,
            DateOfBirth = DateTime.Parse("1990-01-01"),
            ReceiveNewsletter = true,
        };

        // Act
        var person_response_from_add = _personsService.AddPerson(personAddRequest);

        List<PersonResponse> person_list = _personsService.GetAllPersons();

        // Assert
        Assert.True(person_response_from_add.PersonID != Guid.Empty);

        Assert.Contains(person_response_from_add, person_list);
    }

    #endregion

    #region GetPersonByPersonId

    // When PersonID is null, it should return null as PersonResponse
    [Fact]
    public void GetPersonByPersonId_NullPersonID()
    {
        // Arrange
        Guid? personId = null;

        // Act
        PersonResponse? person_response_from_get = _personsService.GetPersonByPersonId(personId);

        // Assert
        Assert.Null(person_response_from_get);
    }

    // When valud PersonID is provided, it should return the matching person details
    [Fact]
    public void GetPersonByPersonId_ValidPersonID()
    {
        // Arrange
        CountryAddRequest country_request = new CountryAddRequest() { CountryName = "China" };
        CountryResponse country_response = _countriesService.AddCountry(country_request);

        // Act
        PersonAddRequest person_request = new PersonAddRequest()
        {
            PersonName = "Jane Doe",
            Email = "test@example.com",
            Address = "456 Elm St",
            CountryID = country_response.CountryID,
            DateOfBirth = DateTime.Parse("1990-01-01"),
            Gender = GenderOptions.Female,
            ReceiveNewsletter = true,
        };
        PersonResponse person_response_from_add = _personsService.AddPerson(person_request);

        PersonResponse? person_response_from_get = _personsService.GetPersonByPersonId(
            person_response_from_add.PersonID
        );

        // Assert
        Assert.Equal(person_response_from_add, person_response_from_get);
    }

    #endregion

    #region GetAllPersons

    // When there are no persons in the system, it should return an empty list
    [Fact]
    public void GetAllPersons_EmptyList()
    {
        // Act
        List<PersonResponse> persons_from_get = _personsService.GetAllPersons();

        // Assert
        Assert.Empty(persons_from_get);
    }

    // Add a few persons then GetAllPersons() should return a list of those persons
    [Fact]
    public void GetAllPersons_AddFewPersons()
    {
        // Arrange
        CountryAddRequest country_request_1 = new CountryAddRequest() { CountryName = "India" };
        CountryAddRequest country_request_2 = new CountryAddRequest() { CountryName = "China" };
        CountryAddRequest country_request_3 = new CountryAddRequest() { CountryName = "Japan" };

        CountryResponse country_response_1 = _countriesService.AddCountry(country_request_1);
        CountryResponse country_response_2 = _countriesService.AddCountry(country_request_2);
        CountryResponse country_response_3 = _countriesService.AddCountry(country_request_3);

        PersonAddRequest person_request_1 = new PersonAddRequest()
        {
            PersonName = "Jane Doe",
            Email = "test@example.com",
            Address = "456 Elm St",
            CountryID = country_response_1.CountryID,
            DateOfBirth = DateTime.Parse("1990-01-01"),
            Gender = GenderOptions.Female,
            ReceiveNewsletter = true,
        };

        PersonAddRequest person_request_2 = new PersonAddRequest()
        {
            PersonName = "John Smith",
            Email = "jdoe@example.com",
            Address = "123 Main St",
            CountryID = country_response_2.CountryID,
            DateOfBirth = DateTime.Parse("1985-05-15"),
            Gender = GenderOptions.Male,
            ReceiveNewsletter = false,
        };

        PersonAddRequest person_request_3 = new PersonAddRequest()
        {
            PersonName = "Alice Johnson",
            Email = "ajohnson@example.com",
            Address = "789 Oak St",
            CountryID = country_response_3.CountryID,
            DateOfBirth = DateTime.Parse("1992-07-20"),
            Gender = GenderOptions.Other,
            ReceiveNewsletter = true,
        };

        List<PersonAddRequest> persons_requests = new List<PersonAddRequest>()
        {
            person_request_1,
            person_request_2,
            person_request_3,
        };

        List<PersonResponse> persons_response_list_from_add = new List<PersonResponse>();

        foreach (PersonAddRequest person_request in persons_requests)
        {
            PersonResponse person_response = _personsService.AddPerson(person_request);
            persons_response_list_from_add.Add(person_response);
        }

        // Print persons_response_list_from_add for debugging
        _testOutputHelper.WriteLine("Expected: ");
        foreach (PersonResponse person_response_from_add in persons_response_list_from_add)
        {
            _testOutputHelper.WriteLine(person_response_from_add.ToString());
        }

        // Act
        List<PersonResponse> persons_list_from_get = _personsService.GetAllPersons();

        // Print persons_response_list_from_get for debugging
        _testOutputHelper.WriteLine("Actual: ");
        foreach (PersonResponse person_response_from_get in persons_list_from_get)
        {
            _testOutputHelper.WriteLine(person_response_from_get.ToString());
        }

        // Assert
        foreach (PersonResponse person_response_from_add in persons_response_list_from_add)
        {
            Assert.Contains(person_response_from_add, persons_list_from_get);
        }
    }

    #endregion

    #region GetFilteredPersons

    // If the search field is empty, it should return all persons
    [Fact]
    public void GetFilteredPersons_EmptySearchField()
    {
        // Arrange
        CountryAddRequest country_request_1 = new CountryAddRequest() { CountryName = "India" };
        CountryAddRequest country_request_2 = new CountryAddRequest() { CountryName = "China" };
        CountryAddRequest country_request_3 = new CountryAddRequest() { CountryName = "Japan" };

        CountryResponse country_response_1 = _countriesService.AddCountry(country_request_1);
        CountryResponse country_response_2 = _countriesService.AddCountry(country_request_2);
        CountryResponse country_response_3 = _countriesService.AddCountry(country_request_3);

        PersonAddRequest person_request_1 = new PersonAddRequest()
        {
            PersonName = "Jane Doe",
            Email = "test@example.com",
            Address = "456 Elm St",
            CountryID = country_response_1.CountryID,
            DateOfBirth = DateTime.Parse("1990-01-01"),
            Gender = GenderOptions.Female,
            ReceiveNewsletter = true,
        };

        PersonAddRequest person_request_2 = new PersonAddRequest()
        {
            PersonName = "John Smith",
            Email = "jdoe@example.com",
            Address = "123 Main St",
            CountryID = country_response_2.CountryID,
            DateOfBirth = DateTime.Parse("1985-05-15"),
            Gender = GenderOptions.Male,
            ReceiveNewsletter = false,
        };

        PersonAddRequest person_request_3 = new PersonAddRequest()
        {
            PersonName = "Alice Johnson",
            Email = "ajohnson@example.com",
            Address = "789 Oak St",
            CountryID = country_response_3.CountryID,
            DateOfBirth = DateTime.Parse("1992-07-20"),
            Gender = GenderOptions.Other,
            ReceiveNewsletter = true,
        };

        List<PersonAddRequest> persons_requests = new List<PersonAddRequest>()
        {
            person_request_1,
            person_request_2,
            person_request_3,
        };

        List<PersonResponse> persons_response_list_from_add = new List<PersonResponse>();

        foreach (PersonAddRequest person_request in persons_requests)
        {
            PersonResponse person_response = _personsService.AddPerson(person_request);
            persons_response_list_from_add.Add(person_response);
        }

        // Print persons_response_list_from_add for debugging
        _testOutputHelper.WriteLine("Expected: ");
        foreach (PersonResponse person_response_from_add in persons_response_list_from_add)
        {
            _testOutputHelper.WriteLine(person_response_from_add.ToString());
        }

        // Act
        List<PersonResponse> persons_list_from_search = _personsService.GetFilteredPersons(
            nameof(Person.PersonName),
            string.Empty
        );

        // Print persons_response_list_from_get for debugging
        _testOutputHelper.WriteLine("Actual: ");
        foreach (PersonResponse person_response_from_get in persons_list_from_search)
        {
            _testOutputHelper.WriteLine(person_response_from_get.ToString());
        }

        // Assert
        foreach (PersonResponse person_response_from_add in persons_response_list_from_add)
        {
            Assert.Contains(person_response_from_add, persons_list_from_search);
        }
    }

    // Add a few persons then search. It should return the matching persons based on the search criteria
    [Fact]
    public void GetFilteredPersons_SearchByPersonName()
    {
        // Arrange
        CountryAddRequest country_request_1 = new CountryAddRequest() { CountryName = "India" };
        CountryAddRequest country_request_2 = new CountryAddRequest() { CountryName = "China" };
        CountryAddRequest country_request_3 = new CountryAddRequest() { CountryName = "Japan" };

        CountryResponse country_response_1 = _countriesService.AddCountry(country_request_1);
        CountryResponse country_response_2 = _countriesService.AddCountry(country_request_2);
        CountryResponse country_response_3 = _countriesService.AddCountry(country_request_3);

        PersonAddRequest person_request_1 = new PersonAddRequest()
        {
            PersonName = "Jane Doe",
            Email = "test@example.com",
            Address = "456 Elm St",
            CountryID = country_response_1.CountryID,
            DateOfBirth = DateTime.Parse("1990-01-01"),
            Gender = GenderOptions.Female,
            ReceiveNewsletter = true,
        };

        PersonAddRequest person_request_2 = new PersonAddRequest()
        {
            PersonName = "John Smith",
            Email = "jdoe@example.com",
            Address = "123 Main St",
            CountryID = country_response_2.CountryID,
            DateOfBirth = DateTime.Parse("1985-05-15"),
            Gender = GenderOptions.Male,
            ReceiveNewsletter = false,
        };

        PersonAddRequest person_request_3 = new PersonAddRequest()
        {
            PersonName = "Alice Johnson",
            Email = "ajohnson@example.com",
            Address = "789 Oak St",
            CountryID = country_response_3.CountryID,
            DateOfBirth = DateTime.Parse("1992-07-20"),
            Gender = GenderOptions.Other,
            ReceiveNewsletter = true,
        };

        List<PersonAddRequest> persons_requests = new List<PersonAddRequest>()
        {
            person_request_1,
            person_request_2,
            person_request_3,
        };

        List<PersonResponse> persons_response_list_from_add = new List<PersonResponse>();

        foreach (PersonAddRequest person_request in persons_requests)
        {
            PersonResponse person_response = _personsService.AddPerson(person_request);
            persons_response_list_from_add.Add(person_response);
        }

        // Print persons_response_list_from_add for debugging
        _testOutputHelper.WriteLine("Expected: ");
        foreach (PersonResponse person_response_from_add in persons_response_list_from_add)
        {
            _testOutputHelper.WriteLine(person_response_from_add.ToString());
        }

        // Act
        List<PersonResponse> persons_list_from_search = _personsService.GetFilteredPersons(
            nameof(Person.PersonName),
            "ja"
        );

        // Print persons_response_list_from_get for debugging
        _testOutputHelper.WriteLine("Actual: ");
        foreach (PersonResponse person_response_from_get in persons_list_from_search)
        {
            _testOutputHelper.WriteLine(person_response_from_get.ToString());
        }

        // Assert
        foreach (PersonResponse person_response_from_add in persons_response_list_from_add)
        {
            if (person_response_from_add.PersonName != null)
            {
                // Check if the person's name contains "ja" (case-insensitive)
                if (
                    person_response_from_add.PersonName.Contains(
                        "ja",
                        StringComparison.OrdinalIgnoreCase
                    )
                )
                {
                    Assert.Contains(person_response_from_add, persons_list_from_search);
                }
            }
        }
    }

    #endregion
}
