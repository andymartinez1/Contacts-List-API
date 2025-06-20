using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace ServiceContracts;

/// <summary>
/// Represents the service contract for managing persons.
/// </summary>
public interface IPersonsService
{
    /// <summary>
    /// Adds a new person to the system.
    /// </summary>
    /// <param name="personAddRequest"></param>
    /// <returns>Returns the person details with newly generated PersonID</returns>
    PersonResponse AddPerson(PersonAddRequest? personAddRequest);

    /// <summary>
    /// Returns a person by their unique PersonID.
    /// </summary>
    /// <param name="personID"></param>
    /// <returns>Returns matching person object</returns>
    PersonResponse? GetPersonByPersonId(Guid? personID);

    /// <summary>
    /// Returns a list of persons filtered by the specified search criteria.
    /// </summary>
    /// <param name="searchBy">Search field</param>
    /// <param name="searchString">Search string</param>
    /// <returns>Returns all matching persons</returns>
    List<PersonResponse> GetFilteredPersons(string searchBy, string? searchString);

    /// <summary>
    /// Returns a list of persons sorted by the specified field and order.
    /// </summary>
    /// <param name="allPersons">List of persons to sort</param>
    /// <param name="sortBy">Sort criteria</param>
    /// <param name="sortOrder">Ascending or descending</param>
    /// <returns>Sorted list of all persons</returns>
    List<PersonResponse> GetSortedPersons(
        List<PersonResponse> allPersons,
        string sortBy,
        SortOrderOptions sortOrder
    );

    /// <summary>
    /// Returns all persons in the system.
    /// </summary>
    /// <returns>List of persons of PersonResponse object typs</returns>
    List<PersonResponse> GetAllPersons();

    /// <summary>
    /// Updates an existing person's details.
    /// </summary>
    /// <param name="personUpdateRequest"></param>
    /// <returns>Updated person response object</returns>
    PersonResponse UpdatePerson(PersonUpdateRequest personUpdateRequest);
}
