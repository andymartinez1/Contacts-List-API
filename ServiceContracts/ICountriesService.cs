using ServiceContracts.DTO;

namespace ServiceContracts
{
    /// <summary>
    /// Inrterface for the CountriesService class representing the buisiness logic layer for country-related operations.
    /// </summary>
    public interface ICountriesService
    {
        /// <summary>
        /// Adds a country object to the list of countries.
        /// </summary>
        /// <param name="countryAddRequest">Country object to add</param>
        /// <returns>Returns the country object after adding it</returns>
        CountryResponse AddCountry(CountryAddRequest? countryAddRequest);
        
        /// <summary>
        /// Returns all countries from the list
        /// </summary>
        /// <returns>All countries</returns>
        List<CountryResponse> GetAllCountries();
    }
}
