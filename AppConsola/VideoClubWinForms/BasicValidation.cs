
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Windows.Forms;

namespace VideoClubWinForms;

public static class BasicValidation
{
    public static void Check(Dictionary<PropertyInfo, Control> inputs)
    {
        foreach (var (prop, control) in inputs)
        {
            if (control is CheckBox)
                continue;

            var value = control.Text.Trim();

            // Validar campos obligatorios
            if (value.Length == 0)
                throw new InvalidOperationException(
                    $"Completá el campo {prop.Name}.");

            // Validar números enteros
            if (prop.PropertyType == typeof(int))
            {
                if (!int.TryParse(
                    value,
                    NumberStyles.Integer,
                    CultureInfo.CurrentCulture,
                    out var number))
                {
                    throw new InvalidOperationException(
                        $"{prop.Name} debe ser un número entero.");
                }

                if (number < 0)
                    throw new InvalidOperationException(
                        $"{prop.Name} no puede ser negativo.");

                if (prop.Name.Equals(
                    "Dni",
                    StringComparison.OrdinalIgnoreCase)
                    && number == 0)
                {
                    throw new InvalidOperationException(
                        "El DNI debe ser mayor que cero.");
                }
            }

            // Validar números decimales
            if (prop.PropertyType == typeof(decimal))
            {
                if (!decimal.TryParse(
                    value,
                    NumberStyles.Number,
                    CultureInfo.CurrentCulture,
                    out var number))
                {
                    throw new InvalidOperationException(
                        $"{prop.Name} debe ser un número válido.");
                }

                if (number < 0)
                    throw new InvalidOperationException(
                        $"{prop.Name} no puede ser negativo.");

                if (prop.Name.Equals(
                    "Descuento",
                    StringComparison.OrdinalIgnoreCase)
                    && number > 100)
                {
                    throw new InvalidOperationException(
                        "El descuento no puede superar el 100%.");
                }
            }
        }
    }
}
