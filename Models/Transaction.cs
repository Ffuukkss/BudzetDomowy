using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BudzetDomowyMinimal.Models;

public class Transaction : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    private DateTime _date;
    public DateTime Date
    {
        get => _date;
        set { _date = value; OnPropertyChanged(); OnChanged?.Invoke(); }
    }

    private string _description = string.Empty;
    public string Description
    {
        get => _description;
        set { _description = value; OnPropertyChanged(); OnChanged?.Invoke(); }
    }

    private decimal _amount;
    public decimal Amount
    {
        get => _amount;
        set { _amount = value; OnPropertyChanged(); OnChanged?.Invoke(); }
    }

    public string DateString => Date.ToString("yyyy-MM-dd");

    public Action? OnChanged { get; set; }

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
} 