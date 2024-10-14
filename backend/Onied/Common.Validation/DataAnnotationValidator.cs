﻿using System.ComponentModel.DataAnnotations;

namespace Common.Validation;

public class DataAnnotationValidator
{
    /// <summary>
    /// Validates model with attributes from <c>System.ComponentModel.DataAnnotations</c>
    /// </summary>
    /// <param name="model">model to validate</param>
    /// <param name="results">validation results generated by checking</param>
    /// <returns>
    /// <list>
    /// <item>True - if model is valid</item>
    /// <item>False - if model is invalid, see more problems in results</item>
    /// </list>
    /// </returns>
    /// <exception cref="ArgumentNullException"></exception>
    public bool TryValidate<T>(T model, out List<ValidationResult> results)
    {
        if (model == null) throw new ArgumentNullException(nameof(model));

        var context = new ValidationContext(model);
        results = [];
        var result = Validator.TryValidateObject(model, context, results, true);
        return result;
    }
}
