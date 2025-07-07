using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using BudzetDomowyMinimal.Models;
using BudzetDomowyMinimal.Services;

namespace BudzetDomowyMinimal.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        private const string DataFile = "transactions.json";

        [ObservableProperty]
        private ObservableCollection<Transaction> transactions;

        [ObservableProperty]
        private string newDescription = string.Empty;

        [ObservableProperty]
        private string newAmount = string.Empty;

        [ObservableProperty]
        private Transaction? selectedTransaction;

        public MainWindowViewModel()
        {
            Transactions = new ObservableCollection<Transaction>();
            LoadTransactions();
            foreach (var t in Transactions)
            {
                t.OnChanged = SaveTransactions;
            }
            Transactions.CollectionChanged += Transactions_CollectionChanged;
        }

        public decimal Saldo => Transactions?.Sum(t => t.Amount) ?? 0;

        [RelayCommand]
        private void AddTransaction()
        {
            if (string.IsNullOrWhiteSpace(NewDescription) || string.IsNullOrWhiteSpace(NewAmount))
                return;

            if (decimal.TryParse(NewAmount, out decimal amount))
            {
                var transaction = new Transaction
                {
                    Date = DateTime.Now,
                    Description = NewDescription,
                    Amount = amount
                };
                transaction.OnChanged = SaveTransactions;
                Transactions.Add(transaction);
                
                NewDescription = string.Empty;
                NewAmount = string.Empty;
                
                OnPropertyChanged(nameof(Saldo));
            }
        }

        [RelayCommand]
        private void RemoveTransaction()
        {
            if (SelectedTransaction != null)
            {
                Transactions.Remove(SelectedTransaction);
                OnPropertyChanged(nameof(Saldo));
            }
        }

        private void Transactions_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (Transaction item in e.NewItems)
                {
                    item.OnChanged = SaveTransactions;
                }
            }
            SaveTransactions();
            OnPropertyChanged(nameof(Saldo));
        }

        private void SaveTransactions()
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                File.WriteAllText(DataFile, JsonSerializer.Serialize(Transactions, options));
            }
            catch (Exception ex)
            {
                File.AppendAllText("error.log", $"[{DateTime.Now}] Błąd zapisu: {ex}\n");
            }
        }

        private void LoadTransactions()
        {
            try
            {
                if (File.Exists(DataFile))
                {
                    var loaded = JsonSerializer.Deserialize<ObservableCollection<Transaction>>(File.ReadAllText(DataFile));
                    if (loaded != null)
                    {
                        Transactions.Clear();
                        foreach (var t in loaded)
                            Transactions.Add(t);
                    }
                }
                else
                {
                    Transactions.Add(new Transaction { Date = DateTime.Now, Description = "Zakupy", Amount = -50 });
                    Transactions.Add(new Transaction { Date = DateTime.Now, Description = "Wypłata", Amount = 3000 });
                }
            }
            catch (Exception ex)
            {
                File.AppendAllText("error.log", $"[{DateTime.Now}] Błąd odczytu: {ex}\n");
            }
        }
    }
}
