# Budżet Domowy Minimal

Aplikacja desktopowa do zarządzania budżetem domowym napisana w C# z wykorzystaniem frameworka Avalonia UI.

## Opis

Budżet Domowy Minimal to prosta aplikacja pozwalająca na:
- Dodawanie transakcji (przychodów i wydatków)
- Przeglądanie historii transakcji
- Wizualizację salda z kolorowym kodowaniem (zielony dla dodatniego, czerwony dla ujemnego)
- Zapisywanie danych w pliku JSON

## Technologie

- **.NET 9.0**
- **Avalonia UI** - cross-platform UI framework
- **CommunityToolkit.Mvvm** - MVVM pattern implementation
- **JSON** - format przechowywania danych

## Wymagania systemowe

- Windows 10/11
- .NET 9.0 Runtime

## Instalacja i uruchomienie

1. Sklonuj repozytorium:
```bash
git clone https://github.com/[twoja-nazwa-uzytkownika]/BudzetDomowyMinimal.git
```

2. Przejdź do katalogu projektu:
```bash
cd BudzetDomowyMinimal
```

3. Uruchom aplikację:
```bash
dotnet run
```

## Struktura projektu

```
BudzetDomowyMinimal/
├── Models/
│   └── Transaction.cs          # Model transakcji
├── Services/
│   └── DataService.cs          # Serwis do zarządzania danymi
├── ViewModels/
│   ├── ViewModelBase.cs        # Bazowa klasa ViewModel
│   ├── MainWindowViewModel.cs  # ViewModel głównego okna
│   └── AmountToColorConverter.cs # Konwerter kolorów
├── Views/
│   └── MainWindow.axaml        # Główne okno aplikacji
└── App.axaml                   # Konfiguracja aplikacji
```

## Funkcjonalności

### Dodawanie transakcji
- Wprowadź kwotę (dodatnią dla przychodów, ujemną dla wydatków)
- Dodaj opis transakcji
- Kliknij "Dodaj transakcję"

### Przeglądanie historii
- Lista wszystkich transakcji z datą, kwotą i opisem
- Kolorowe kodowanie kwot (zielony/czerwony)
- Aktualne saldo wyświetlane na górze

### Zapisywanie danych
- Dane automatycznie zapisywane w pliku `transactions.json`
- Aplikacja pamięta wszystkie transakcje między uruchomieniami

## Licencja

MIT License

## Autor

[Twoje imię i nazwisko] 