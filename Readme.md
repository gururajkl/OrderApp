# WPF Ordering Application

## Overview

This is a WPF desktop application built using the MVVM architecture that allows users to place an order for an item.

The application fetches available items asynchronously, allows users to input order details, validates inputs, and submits the order with a confirmation message.

---

## Features

* Fetch items asynchronously on application startup
* Select item from dropdown (ComboBox)
* Enter quantity (numeric only, between 1–100)
* Enter city (required, maximum 50 characters)
* Select state from predefined list (ComboBox)
* Input validation using IDataErrorInfo
* Loading indicator during API calls
* Confirmation message after order submission
* Disable "Place Order" button during submission

---

## Tech Stack

* .NET 10 (WPF)
* MVVM Architecture
* Dependency Injection (Microsoft.Extensions.DependencyInjection)
* ICommand (RelayCommand)
* IDataErrorInfo for validation
* Async/Await for API handling
* xUnit & Moq for unit testing

---

## Project Structure

* `/Models` → Data models (Item, Order)
* `/ViewModels` → MainViewModel (logic, validation, commands) and BaseViewModel (Which wraps INotifyPropertyChanged interface)
* `/Views` → MainWindow, Loader (UI)
* `/Services` → ApiService, MessageService
* `/Commands` → RelayCommand implementation
* `/Tests` → Unit tests for ViewModel logic

---

## How to Run

1. Clone the repository
2. Open the solution in Visual Studio 2026
3. Build the solution
4. Run the application (F5)

---

## API / Mock Service

The application uses a mock API service:

* `GetItemsAsync()` → Returns a list of items
* `SubmitOrderAsync()` → Simulates order submission

Both methods simulate network delay using `Task.Delay`.

---

## Validation

Validation is implemented using `IDataErrorInfo`.

### Rules:

* **Item to order**

  * Required
  * Item to order must be selected from the combo box to proceed with the order.

* **Quantity**

  * Required
  * Must be between 1 and 100
  * Numeric input only (restricted at UI level)

* **City**

  * Required
  * Maximum 50 characters

* **State**

  * Must be selected from predefined list

Validation errors are shown visually in the UI.

---

## Design Decisions

* Used MVVM pattern for clear separation of concerns.
* Used Dependency Injection to improve testability and flexibility.
* Used `IDataErrorInfo` for input validations.
* Implemented Loader as a reusable UserControl with overlay behavior.
* Used async/await for all API interactions.
* Avoided business logic in code-behind (only UI behavior handled).
* Structured ViewModel logic to be testable by abstracting dependencies (API and Message services), enabling effective unit testing using xUnit and Moq.

---

## Unit Tests

Unit tests are implemented using **xUnit** and **Moq**.

### Covered scenarios:

* Command enable/disable logic
* Validation rules
* API interaction (SubmitOrderAsync)
* Data loading (GetItemsAsync)

---

## Assumptions

* API is mocked (no real backend integration required)
* No database or persistence layer required
* State list is predefined in ViewModel
* Single order flow (no order history required)
