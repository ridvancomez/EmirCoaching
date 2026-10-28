using System.Collections;
using System.Reflection;
using AwesomeAssertions;
using EmirCoaching.Business.Common;
using EmirCoaching.Business.Dtos;
using EmirCoaching.Business.Validation;

namespace EmirCoaching.Tests.Validators;

public class TurkishMessagesTests
{
    [Fact]
    public async Task Validator_UsesTurkishFieldNameInMessage()
    {
        // Arrange
        var request = new CreateBlogCategoryRequest { Name = "", Slug = "beslenme" };

        // Act
        var errors = await new CreateBlogCategoryRequestValidator().GetErrorsAsync(request);

        // Assert
        errors.Should().ContainSingle().Which.Should().Be("'Ad' boş olamaz.");
    }

    [Fact]
    public async Task Validator_ReturnsSingleMessagePerField()
    {
        // Arrange — boş slug hem NotEmpty hem Matches kuralını bozar; CascadeMode.Stop ile yalnızca ilki gelir
        var request = new CreateBlogCategoryRequest { Name = "Beslenme", Slug = "" };

        // Act
        var errors = await new CreateBlogCategoryRequestValidator().GetErrorsAsync(request);

        // Assert
        errors.Should().ContainSingle().Which.Should().Be("'URL adı' boş olamaz.");
    }

    // Her Request alanının FieldNames'te Türkçe karşılığı olmalı; yoksa mesajda property adı (ör. "PhoneNumber") görünür
    [Fact]
    public void FieldNames_CoversEveryRequestProperty()
    {
        // Arrange
        var requestTypes = typeof(CreateBlogCategoryRequest).Assembly.GetTypes()
            .Where(t => t.Namespace == typeof(CreateBlogCategoryRequest).Namespace && t.IsPublic && t.Name.EndsWith("Request"))
            .SelectMany(RequestTypesWithChildren)
            .Distinct()
            .ToList();
        requestTypes.Should().Contain([typeof(UpdatePackageRequest), typeof(PackageModuleItemRequest)]); // tarama boş geçmesin

        // Act
        var missing = requestTypes
            .SelectMany(t => t.GetProperties(BindingFlags.Public | BindingFlags.Instance).Select(p => $"{t.Name}.{p.Name}"))
            .Where(name => FieldNames.Find(name.Split('.')[1]) is null)
            .ToList();

        // Assert
        missing.Should().BeEmpty();
    }

    // Request'in içindeki alt listelerin (paket modülleri, ölçümler...) tipleri de kullanıcıdan gelen alandır
    private static IEnumerable<Type> RequestTypesWithChildren(Type type)
    {
        yield return type;

        foreach (var property in type.GetProperties())
        {
            var child = property.PropertyType.IsGenericType && typeof(IEnumerable).IsAssignableFrom(property.PropertyType)
                ? property.PropertyType.GetGenericArguments()[0]
                : property.PropertyType;

            if (child.Namespace == type.Namespace && child.IsClass && child != type)
                foreach (var nested in RequestTypesWithChildren(child))
                    yield return nested;
        }
    }
}
