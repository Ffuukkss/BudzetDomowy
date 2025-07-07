using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using BudzetDomowyMinimal.Models;

namespace BudzetDomowyMinimal.Services
{
    public static class DataService
    {
        private static readonly string _folder =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                         "BudzetDomowy");

        private const string _fileName = "transactions.json";

        private static string FilePath => Path.Combine(_folder, _fileName);

        public static void Save(IEnumerable<Transaction> transactions)
        {
            try
            {
                Directory.CreateDirectory(_folder);
                var json = JsonSerializer.Serialize(transactions, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(FilePath, json);
            }
            catch (Exception ex)
            {
                File.AppendAllText("error.log", $"[{DateTime.Now}] Błąd zapisu DataService: {ex}\n");
            }
        }

        public static List<Transaction> Load()
        {
            try
            {
                if (File.Exists(FilePath))
                    return JsonSerializer.Deserialize<List<Transaction>>(File.ReadAllText(FilePath)) ?? new();
            }
            catch (Exception ex)
            {
                File.AppendAllText("error.log", $"[{DateTime.Now}] Błąd odczytu DataService: {ex}\n");
            }

            return new();
        }
    }
}
