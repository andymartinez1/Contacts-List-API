﻿using Entities;

namespace ServiceContracts.DTO;

/// <summary>
///     DTO class used as a return type for CountriesService methods.
/// </summary>
public class CountryResponse
{
    public Guid CountryID { get; set; }
    public string? CountryName { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj == null) return false;

        if (obj.GetType() != typeof(CountryResponse)) return false;

        var country_to_compare = (CountryResponse)obj;
        return CountryID == country_to_compare.CountryID && CountryName == country_to_compare.CountryName;
        ;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}

public static class CountryResponseExtensions
{
    public static CountryResponse ToCountryResponse(this Country country)
    {
        return new CountryResponse
        {
            CountryID = country.CountryID,
            CountryName = country.CountryName
        };
    }
}