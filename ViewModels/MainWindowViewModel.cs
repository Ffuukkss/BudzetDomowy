using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using BudzetDomowyMinimal.Models;

namespace BudzetDomowyMinimal.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private const string DataFile = "transactions.json";
    public ObservableCollection<Transaction> Transactions { get; } = new();

    [ObservableProperty]
    private string _newDescription = string.Empty;
    [ObservableProperty]
    private decimal _newAmount;
    [ObservableProperty]
    private DateTime _newTransactionDate = DateTime.Now;

    [ObservableProperty]
    private Transaction? _selectedTransaction;

    public string Greeting { get; } = "Welcome to Avalonia!";

    public decimal Saldo => Transactions.Sum(t => t.Amount);

    public MainWindowViewModel()
    {
        Transactions.CollectionChanged += (s, e) =>
        {
            OnPropertyChanged(nameof(Saldo));
            if (e.NewItems != null)
            {
                foreach (Transaction t in e.NewItems)
                {
                    t.PropertyChanged += Transaction_PropertyChanged;
                    t.OnChanged = SaveTransactions;
                }
            }
            if (e.OldItems != null)
            {
                foreach (Transaction t in e.OldItems)
                {
                    t.PropertyChanged -= Transaction_PropertyChanged;
                    t.OnChanged = null;
                }
            }
            SaveTransactions();
        };
        LoadTransactions();
        foreach (var t in Transactions)
        {
            t.PropertyChanged += Transaction_PropertyChanged;
            t.OnChanged = SaveTransactions;
        }
    }

    private void Transaction_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(Transaction.Amount))
            OnPropertyChanged(nameof(Saldo));
        SaveTransactions();
    }

    [RelayCommand]
    private void AddTransaction()
    {
        if (!string.IsNullOrWhiteSpace(NewDescription) && NewAmount != 0)
        {
            var newT = new Transaction
            {
                Date = DateTime.Now,
                Description = NewDescription,
                Amount = NewAmount
            };
            newT.OnChanged = SaveTransactions;
            Transactions.Add(newT);
            NewDescription = string.Empty;
            NewAmount = 0;
        }
    }

    [RelayCommand]
    private void RemoveTransaction()
    {
        if (SelectedTransaction != null)
        {
            Transactions.Remove(SelectedTransaction);
            SelectedTransaction = null;
        }
    }

    private void SaveTransactions()
    {
        try
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            File.WriteAllText(DataFile, JsonSerializer.Serialize(Transactions, options));
        }
        catch { /* Można dodać logowanie błędów */ }
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
        catch { /* Można dodać logowanie błędów */ }
    }
}
