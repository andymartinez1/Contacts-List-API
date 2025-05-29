using Entities;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services;

public class CountriesService : ICountriesService
{
    private readonly List<Country> _countries;

    public CountriesService()
    {
        _countries = new List<Country>();
    }

    public CountryResponse AddCountry(CountryAddRequest? countryAddRequest)
    {
        // Validation: CountryAddRequest cannot be null
        if (countryAddRequest == null) throw new ArgumentNullException(nameof(CountryAddRequest));

        // Validation: CountryName cannot be null
        if (countryAddRequest.CountryName == null) throw new ArgumentException(nameof(countryAddRequest.CountryName));

        // Validation: CountryName cannot be duplicated
        if (_countries.Where(temp => temp.CountryName == countryAddRequest.CountryName).Count() > 0)
            throw new ArgumentException($"Country name already exists: {countryAddRequest.CountryName}");

        // Convert object from CountryAddRequest to Country type
        var country = countryAddRequest.ToCountry();

        // Generate CountryID
        country.CountryID = Guid.NewGuid();

        // Add country object into countries list
        _countries.Add(country);

        return country.ToCountryResponse();
    }

    public List<CountryResponse> GetAllCountries()
    {
        return _countries.Select(country => country.ToCountryResponse()).ToList();
    }

    public CountryResponse? GetCountryById(Guid? countryId)
    {
        if (countryId == null) return null;

        Country? country_response_from_list = _countries.FirstOrDefault(temp => temp.CountryID == countryId);

        if (country_response_from_list == null) return null;

        return country_response_from_list.ToCountryResponse();
    }
}