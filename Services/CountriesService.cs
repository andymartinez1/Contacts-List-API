using Entities;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services;

public class CountriesService : ICountriesService
{
    private readonly List<Country> _countries;

    public CountriesService(bool initializeCountries = true)
    {
        _countries = new List<Country>();
        if (initializeCountries)
        {
            // Initialize with some default countries
            _countries.AddRange(
                new List<Country>()
                {
                    new Country
                    {
                        CountryID = Guid.Parse("4A656578-3623-4A33-977B-62969CBD4056"),
                        CountryName = "United States",
                    },
                    new Country
                    {
                        CountryID = Guid.Parse("E1230C55-DD31-4385-86CB-AA83A1B091F5"),
                        CountryName = "Canada",
                    },
                    new Country
                    {
                        CountryID = Guid.Parse("3534980C-3DED-49F7-BE69-F91BDEF89BA1"),
                        CountryName = "Mexico",
                    },
                    new Country
                    {
                        CountryID = Guid.Parse("AB33587A-1FC6-4FE2-BBFC-23E18694631B"),
                        CountryName = "United Kingdom",
                    },
                    new Country
                    {
                        CountryID = Guid.Parse("32A5FD37-80E7-4DDE-AF12-749C20E1E7E6"),
                        CountryName = "Germany",
                    },
                }
            );
        }
    }

    public CountryResponse AddCountry(CountryAddRequest? countryAddRequest)
    {
        // Validation: CountryAddRequest cannot be null
        if (countryAddRequest == null)
            throw new ArgumentNullException(nameof(CountryAddRequest));

        // Validation: CountryName cannot be null
        if (countryAddRequest.CountryName == null)
            throw new ArgumentException(nameof(countryAddRequest.CountryName));

        // Validation: CountryName cannot be duplicated
        if (_countries.Where(temp => temp.CountryName == countryAddRequest.CountryName).Count() > 0)
            throw new ArgumentException(
                $"Country name already exists: {countryAddRequest.CountryName}"
            );

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
        if (countryId == null)
            return null;

        Country? country_response_from_list = _countries.FirstOrDefault(temp =>
            temp.CountryID == countryId
        );

        if (country_response_from_list == null)
            return null;

        return country_response_from_list.ToCountryResponse();
    }
}
