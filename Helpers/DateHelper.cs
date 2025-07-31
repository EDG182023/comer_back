using System;
using System.Globalization;

namespace TarifarioBackend.Helpers
{
    public static class DateHelper
    {
        public static DateTime? SafeDate(object? value)
        {
            if (value == null || value == DBNull.Value)
            {
                return null;
            }

            if (value is DateTime dt)
            {
                return dt;
            }

            if (value is string s)
            {
                if (DateTime.TryParse(s, out DateTime parsedDate))
                {
                    return parsedDate;
                }
                // Try parsing with specific formats if default fails
                if (DateTime.TryParseExact(s, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate))
                {
                    return parsedDate;
                }
                if (DateTime.TryParseExact(s, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate))
                {
                    return parsedDate;
                }
                if (DateTime.TryParseExact(s, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate))
                {
                    return parsedDate;
                }
                if (DateTime.TryParseExact(s, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate))
                {
                    return parsedDate;
                }
            }

            return null;
        }

        public static DateTime GetCurrentBogotaTime()
        {
            // Define la zona horaria de Bogotá (Colombia)
            TimeZoneInfo bogotaTimeZone;
            try
            {
                bogotaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("America/Bogota"); // Para sistemas Linux/macOS
            }
            catch (TimeZoneNotFoundException)
            {
                bogotaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SA Pacific Standard Time"); // Para sistemas Windows
            }
            catch (Exception ex)
            {
                // Fallback en caso de que ninguna de las anteriores funcione
                Console.WriteLine($"Error al obtener la zona horaria de Bogotá: {ex.Message}. Usando UTC.");
                return DateTime.UtcNow;
            }

            // Obtiene la hora UTC actual
            DateTime utcNow = DateTime.UtcNow;

            // Convierte la hora UTC a la hora de Bogotá
            return TimeZoneInfo.ConvertTimeFromUtc(utcNow, bogotaTimeZone);
        }

        public static DateTime StartOfDay(this DateTime date)
        {
            return date.Date;
        }

        public static DateTime EndOfDay(this DateTime date)
        {
            return date.Date.AddDays(1).AddTicks(-1);
        }

        public static DateTime StartOfMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1);
        }

        public static DateTime EndOfMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month), 23, 59, 59, 999);
        }

        public static DateTime StartOfYear(this DateTime date)
        {
            return new DateTime(date.Year, 1, 1);
        }

        public static DateTime EndOfYear(this DateTime date)
        {
            return new DateTime(date.Year, 12, 31, 23, 59, 59, 999);
        }
    }
}
