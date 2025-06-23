using ServiceContracts;
using ServiceContracts.DTO;
using Services;

namespace CRUD_Tests;

public class CountriesServiceTest
{
    private readonly ICountriesService _countriesService;

    public CountriesServiceTest()
    {
        _countriesService = new CountriesService(false);
    }

    #region AddCountry

    // When CountryAddRequest is null, it should throw an ArgumentNullException
    [Fact]
    public void AddCountry_NullCountry()
    {
        // Arrange
        CountryAddRequest? request = null;

        // Assert
        Assert.Throws<ArgumentNullException>(() =>
        {
            // Act
            _countriesService.AddCountry(request);
        });
    }

    // When CountryName is null, it should throw an ArgumentNullException
    [Fact]
    public void AddCountry_CountryNameIsNull()
    {
        // Arrange
        var request = new CountryAddRequest { CountryName = null };

        // Assert
        Assert.Throws<ArgumentException>(() =>
        {
            // Act
            _countriesService.AddCountry(request);
        });
    }

    // When CountryName is duplicate, it should throw an ArgumentException
    [Fact]
    public void AddCountry_DuplicateCountryName()
    {
        // Arrange
        var request1 = new CountryAddRequest { CountryName = "USA" };
        var request2 = new CountryAddRequest { CountryName = "USA" };

        // Assert
        Assert.Throws<ArgumentException>(() =>
        {
            // Act
            _countriesService.AddCountry(request1);
            _countriesService.AddCountry(request2);
        });
    }

    // When CountryName is proper, it should Add the country to the list of countries and return the country object
    [Fact]
    public void AddCountry_ProperCountryDetails()
    {
        // Arrange
        var request = new CountryAddRequest { CountryName = "Japan" };

        // Act
        var response = _countriesService.AddCountry(request);
        var countries_from_GetAllCountries = _countriesService.GetAllCountries();

        // Assert
        Assert.True(response.CountryID != Guid.Empty);
        Assert.Contains(response, countries_from_GetAllCountries);
    }

    #endregion

    #region GetAllCountries

    // The list of countries should be empty by default
    [Fact]
    public void GetAllCountries_EmptyCountryList()
    {
        // Act
        var actual_country_response_list = _countriesService.GetAllCountries();

        // Assert
        Assert.Empty(actual_country_response_list);
    }

    [Fact]
    public void GetAllCountries_AddFewCountries()
    {
        // Arrange
        var country_request_list = new List<CountryAddRequest>
        {
            new() { CountryName = "USA" },
            new() { CountryName = "Japan" },
        };

        // Act
        var country_list_from_add_country = new List<CountryResponse>();
        foreach (var country_request in country_request_list)
            country_list_from_add_country.Add(_countriesService.AddCountry(country_request));

        var actualCountryResponseList = _countriesService.GetAllCountries();

        // Read each element from country_list_from_add_country
        foreach (var expected_country in country_list_from_add_country)
            Assert.Contains(expected_country, actualCountryResponseList);
    }

    #endregion

    #region GetCountryById

    // A null value should return null
    [Fact]
    public void GetCountryById_NullCountryId()
    {
        // Arrange
        Guid? countryId = null;

        // Act
        CountryResponse? country_response_from_get_method = _countriesService.GetCountryById(
            countryId
        );

        // Assert
        Assert.Null(country_response_from_get_method);
    }

    // If a valid country id is supplied, it should return the matching country details
    [Fact]
    public void GetCountryById_ValidCountryId()
    {
        // Arrange
        CountryAddRequest? country_add_request = new CountryAddRequest() { CountryName = "China" };
        CountryResponse country_response_from_add = _countriesService.AddCountry(
            country_add_request
        );

        // Act
        CountryResponse? country_response_from_get = _countriesService.GetCountryById(
            country_response_from_add.CountryID
        );

        // Assert
        Assert.Equal(country_response_from_add, country_response_from_get);
    }

    #endregion
}
