namespace ASP.MongoDb.API.Repository
{
  using ASP.MongoDb.API.Entities;
  using ASP.MongoDb.API.DTOs;
  using Microsoft.Extensions.Options;
  using MongoDB.Driver;
  using MongoDB.Bson;

  public class PropertyRepository : Repository<Property>, IPropertyRepository
  {
    private readonly IMongoCollection<Owner> _ownerCollection;
    private readonly IMongoCollection<PropertyImage> _imageCollection;
    private readonly IMongoCollection<PropertyPlace> _placeCollection;

    public PropertyRepository(IOptions<Settings.MongoDbSettings> mongoDbSettings)
        : base(mongoDbSettings)
    {
      var client = new MongoClient(mongoDbSettings.Value.ConnectionString);
      var database = client.GetDatabase(mongoDbSettings.Value.DatabaseName);

      _ownerCollection = database.GetCollection<Owner>("Owner");
      _imageCollection = database.GetCollection<PropertyImage>("PropertyImage");
      _placeCollection = database.GetCollection<PropertyPlace>("PropertyPlace");
    }

    public async Task<PropertySearchResultRaw> SearchPropertiesAsync(PropertyFilterDto filter)
    {
      var filterBuilder = Builders<Property>.Filter;
      var filters = new List<FilterDefinition<Property>>();

      // Name filter
      if (!string.IsNullOrEmpty(filter.Name))
      {
        filters.Add(filterBuilder.Regex(x => x.Name, new BsonRegularExpression(filter.Name, "i")));
      }

      // Address filter
      if (!string.IsNullOrEmpty(filter.Address))
      {
        filters.Add(filterBuilder.Regex(x => x.Address, new BsonRegularExpression(filter.Address, "i")));
      }

      // Price range filter
      if (filter.MinPrice.HasValue)
      {
        filters.Add(filterBuilder.Gte(x => x.Price, filter.MinPrice.Value));
      }

      if (filter.MaxPrice.HasValue)
      {
        filters.Add(filterBuilder.Lte(x => x.Price, filter.MaxPrice.Value));
      }

      // Year filter
      if (filter.Year.HasValue)
      {
        filters.Add(filterBuilder.Eq(x => x.Year, filter.Year.Value));
      }

      // Combine all filters
      var combinedFilter = filters.Count > 0
          ? filterBuilder.And(filters)
          : filterBuilder.Empty;

      // Count total documents
      var totalCount = await _collection.CountDocumentsAsync(combinedFilter);

      // Calculate pagination
      var skip = (filter.Page - 1) * filter.PageSize;
      var totalPages = (int)Math.Ceiling((double)totalCount / filter.PageSize);

      // Sorting
      var sortDefinition = GetSortDefinition(filter.SortBy, filter.SortDirection);

      // Execute query with pagination
      var properties = await _collection
          .Find(combinedFilter)
          .Sort(sortDefinition)
          .Skip(skip)
          .Limit(filter.PageSize)
          .ToListAsync();

      // Get related data for each property
      var propertiesWithDetails = await GetPropertiesWithDetailsAsync(properties);

      // Filter by location if specified (since location is in separate collection)
      if (!string.IsNullOrEmpty(filter.City) || !string.IsNullOrEmpty(filter.State) || !string.IsNullOrEmpty(filter.Country))
      {
        propertiesWithDetails = await FilterByLocationAsync(propertiesWithDetails, filter.City, filter.State, filter.Country);
        totalCount = propertiesWithDetails.Count; // Recalculate total after location filter
        totalPages = (int)Math.Ceiling((double)totalCount / filter.PageSize);
      }

      // Filter by owner name if specified
      if (!string.IsNullOrEmpty(filter.OwnerName))
      {
        propertiesWithDetails = propertiesWithDetails.Where(p =>
            p.Owner != null &&
            (p.Owner.Name.Contains(filter.OwnerName, StringComparison.OrdinalIgnoreCase) ||
             p.Owner.LastName.Contains(filter.OwnerName, StringComparison.OrdinalIgnoreCase) ||
             p.Owner.FullName.Contains(filter.OwnerName, StringComparison.OrdinalIgnoreCase)))
            .ToList();

        totalCount = propertiesWithDetails.Count;
        totalPages = (int)Math.Ceiling((double)totalCount / filter.PageSize);
      }

      // Convert to DTOs
      var propertyDtos = propertiesWithDetails;

      return new PropertySearchResultRaw
      {
        Properties = propertyDtos,
        TotalCount = (int)totalCount,
        Page = filter.Page,
        PageSize = filter.PageSize,
        TotalPages = totalPages,
        HasNextPage = filter.Page < totalPages,
        HasPreviousPage = filter.Page > 1
      };
    }

    public async Task<Property?> GetPropertyWithDetailsAsync(string id)
    {
      var property = await GetByIdAsync(id);
      if (property == null) return null;

      // Load related data
      var properties = await GetPropertiesWithDetailsAsync(new List<Property> { property });
      return properties.FirstOrDefault();
    }

    public async Task<List<Property>> GetPropertiesByOwnerAsync(string ownerId)
    {
      var filter = Builders<Property>.Filter.Eq(x => x.IdOwner, ownerId);
      var properties = await _collection.Find(filter).ToListAsync();
      return await GetPropertiesWithDetailsAsync(properties);
    }

    public async Task<List<Property>> GetPropertiesByPriceRangeAsync(decimal minPrice, decimal maxPrice)
    {
      var filter = Builders<Property>.Filter.And(
          Builders<Property>.Filter.Gte(x => x.Price, minPrice),
          Builders<Property>.Filter.Lte(x => x.Price, maxPrice)
      );

      var properties = await _collection.Find(filter).ToListAsync();
      return await GetPropertiesWithDetailsAsync(properties);
    }

    public async Task<List<Property>> GetPropertiesByLocationAsync(string city, string? state = null, string? country = null)
    {
      var placeFilterBuilder = Builders<PropertyPlace>.Filter;
      var placeFilters = new List<FilterDefinition<PropertyPlace>>();

      if (!string.IsNullOrEmpty(city))
      {
        placeFilters.Add(placeFilterBuilder.And(
            placeFilterBuilder.Eq(x => x.Name, "City"),
            placeFilterBuilder.Regex(x => x.Value, new BsonRegularExpression(city, "i"))
        ));
      }

      if (!string.IsNullOrEmpty(state))
      {
        placeFilters.Add(placeFilterBuilder.And(
            placeFilterBuilder.Eq(x => x.Name, "State"),
            placeFilterBuilder.Regex(x => x.Value, new BsonRegularExpression(state, "i"))
        ));
      }

      if (!string.IsNullOrEmpty(country))
      {
        placeFilters.Add(placeFilterBuilder.And(
            placeFilterBuilder.Eq(x => x.Name, "Country"),
            placeFilterBuilder.Regex(x => x.Value, new BsonRegularExpression(country, "i"))
        ));
      }

      if (placeFilters.Count == 0) return new List<Property>();

      var placeCombinedFilter = placeFilterBuilder.Or(placeFilters);
      var matchingPlaces = await _placeCollection.Find(placeCombinedFilter).ToListAsync();
      var propertyIds = matchingPlaces.Select(p => p.IdProperty).Distinct().ToList();

      if (!propertyIds.Any()) return new List<Property>();

      var propertyFilter = Builders<Property>.Filter.In(x => x.Id, propertyIds);
      var properties = await _collection.Find(propertyFilter).ToListAsync();

      return await GetPropertiesWithDetailsAsync(properties);
    }

    public async Task<bool> ExistsByCodigoInternoAsync(string codigoInterno)
    {
      var filter = Builders<Property>.Filter.Eq(x => x.CodigoInternal, codigoInterno);
      var count = await _collection.CountDocumentsAsync(filter);
      return count > 0;
    }

    #region Private Helper Methods

    private async Task<List<Property>> GetPropertiesWithDetailsAsync(List<Property> properties)
    {
      if (!properties.Any()) return properties;

      var propertyIds = properties.Select(p => p.Id).ToList();
      var ownerIds = properties.Select(p => p.IdOwner).Distinct().ToList();

      // Load owners
      var ownerFilter = Builders<Owner>.Filter.In(x => x.Id, ownerIds);
      var owners = await _ownerCollection.Find(ownerFilter).ToListAsync();
      var ownerDict = owners.ToDictionary(o => o.Id, o => o);

      // Load images
      var imageFilter = Builders<PropertyImage>.Filter.In(x => x.IdProperty, propertyIds);
      var images = await _imageCollection.Find(imageFilter).ToListAsync();
      var imageDict = images.GroupBy(i => i.IdProperty).ToDictionary(g => g.Key, g => g.ToList());

      // Load places
      var placeFilter = Builders<PropertyPlace>.Filter.In(x => x.IdProperty, propertyIds);
      var places = await _placeCollection.Find(placeFilter).ToListAsync();
      var placeDict = places.GroupBy(p => p.IdProperty).ToDictionary(g => g.Key, g => g.ToList());

      // Assign related data to properties
      foreach (var property in properties)
      {
        if (ownerDict.TryGetValue(property.IdOwner, out var owner))
        {
          property.Owner = owner;
        }

        if (imageDict.TryGetValue(property.Id, out var propertyImages))
        {
          property.Images = propertyImages;
        }

        if (placeDict.TryGetValue(property.Id, out var propertyPlaces))
        {
          property.Places = propertyPlaces;
        }
      }

      return properties;
    }

    private async Task<List<Property>> FilterByLocationAsync(List<Property> properties, string? city, string? state, string? country)
    {
      return properties.Where(p =>
      {
        var places = p.Places ?? new List<PropertyPlace>();

        var cityMatch = string.IsNullOrEmpty(city) || places.Any(pl =>
                  pl.Name.Equals("City", StringComparison.OrdinalIgnoreCase) &&
                  pl.Value.Contains(city, StringComparison.OrdinalIgnoreCase));

        var stateMatch = string.IsNullOrEmpty(state) || places.Any(pl =>
                  pl.Name.Equals("State", StringComparison.OrdinalIgnoreCase) &&
                  pl.Value.Contains(state, StringComparison.OrdinalIgnoreCase));

        var countryMatch = string.IsNullOrEmpty(country) || places.Any(pl =>
                  pl.Name.Equals("Country", StringComparison.OrdinalIgnoreCase) &&
                  pl.Value.Contains(country, StringComparison.OrdinalIgnoreCase));

        return cityMatch && stateMatch && countryMatch;
      }).ToList();
    }

    private SortDefinition<Property> GetSortDefinition(string? sortBy, string? sortDirection)
    {
      var sortBuilder = Builders<Property>.Sort;
      var isDescending = sortDirection?.ToLowerInvariant() == "desc";

      return sortBy?.ToLowerInvariant() switch
      {
        "name" => isDescending ? sortBuilder.Descending(x => x.Name) : sortBuilder.Ascending(x => x.Name),
        "price" => isDescending ? sortBuilder.Descending(x => x.Price) : sortBuilder.Ascending(x => x.Price),
        "address" => isDescending ? sortBuilder.Descending(x => x.Address) : sortBuilder.Ascending(x => x.Address),
        "createdat" => isDescending ? sortBuilder.Descending(x => x.CreatedAt) : sortBuilder.Ascending(x => x.CreatedAt),
        _ => sortBuilder.Descending(x => x.CreatedAt) // Default sort
      };
    }

    #endregion
  }
}