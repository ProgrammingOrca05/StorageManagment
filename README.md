# SM-TPS — Store Management & Transaction Processing System

A dual-purpose desktop application built with C# and WPF for managing store inventory and processing customer transactions.

---

## Getting Started

### Requirements
- Windows OS
- Visual Studio 2022 (or later)
- .NET Desktop Development workload installed

### Running the Project
1. Clone the repository
2. Open `StorageManagement.sln` in Visual Studio
3. Press `Ctrl + F5` to build and run

> No database setup required. All data is stored in memory during runtime.

---

## Login Credentials

| Role    | Username | Password      |
|---------|----------|---------------|
| Cashier | cashier  | cashier_2014  |

---

## Features

### Module A — Inventory Management
- Add, edit, and delete products
- Each product has a Name, SKU, Price, and Stock Quantity
- Visual low stock alerts when quantity falls below threshold

### Module B — Sales Terminal
- Search products by name or SKU
- Add items to a live cart
- Real-time subtotal, VAT (14%), and total calculation
- Checkout confirmation dialog that deducts stock automatically

### Module C — Transaction History
- Searchable log of all completed transactions
- Click any transaction to view its full item breakdown

---

## Project Structure

```
StorageManagement/
├── Commands/           # RelayCommand
├── Data/               # DataStore (in-memory shared data)
├── Models/             # Product, Transaction, TransactionItem
├── ViewModels/         # One ViewModel per View
└── Views/
    ├── Windows/        # LoginWindow, MainWindow
    ├── UserControls/   # InventoryView, SalesTerminalView, TransactionHistoryView
    └── Dialogs/        # CheckoutDialog, TransactionDetailDialog
```

---

## Architecture

Built using strict **MVVM (Model-View-ViewModel)** architecture:
- Zero business logic in code-behind files
- All actions handled via `ICommand` / `RelayCommand`
- All data updates via `INotifyPropertyChanged` and `ObservableCollection<T>`

---

## Team

| Person | Responsibility |
|--------|---------------|
| Person 1 | Project setup, Login, Navigation |
| Person 2 | Inventory Module |
| Person 3 | Sales Terminal Module |
| Person 4 | Transaction History Module |