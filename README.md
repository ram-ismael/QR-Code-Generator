# QuickQR

A lightweight and modern QR code generator built with **.NET** and **Avalonia UI**.

QuickQR aims to provide a simple desktop experience for creating QR codes quickly and efficiently, with a clean interface and a structure designed for future expansion.

> 🚧 **Project status:** Early development. The main application window is currently being built, and the QR generation features and UI elements are being implemented incrementally.

## Features

* Generate QR codes from user-provided content
* Clean and modern desktop interface
* Cross-platform desktop support through Avalonia UI
* Lightweight and easy to use
* Designed for future customization and additional QR-related features

## Tech Stack

* **C#**
* **.NET**
* **Avalonia UI**
* **XAML**

## Project Structure

The project follows a source-based structure to keep the application organized and maintainable.

```text
QuickQR/
├── src/
│   ├── ...
├── .editorconfig
├── .gitignore
├── QuickQR.csproj
└── README.md
```

The `src/` directory contains the application's source code, views, models, services, and other components.

## Getting Started

### Prerequisites

Make sure you have the **.NET SDK** installed.

You can verify your installation with:

```bash
dotnet --version
```

### Clone the repository

```bash
git clone https://github.com/<ram-ismael>/QuickQR.git
cd QuickQR
```

### Restore dependencies

```bash
dotnet restore
```

### Run the application

```bash
dotnet run
```

## Development

QuickQR is being developed with a focus on a clean separation between the UI and application logic.

The interface is built with **Avalonia XAML**, while the application logic is implemented in **C#**.

As development progresses, the project will include additional components for QR generation, customization, exporting, and other related functionality.

## Roadmap

* [x] Initialize Avalonia desktop application
* [x] Restructure project architecture
* [x] Create the main application window
* [ ] Add QR code generation
* [ ] Add input controls
* [ ] Add QR preview
* [ ] Add QR customization options
* [ ] Add image export
* [ ] Improve accessibility and UX
* [ ] Add additional QR formats and options
* [ ] Release the first stable version

## Contributing

Contributions are welcome.

To contribute:

1. Fork the repository.
2. Create a new branch:

```bash
git checkout -b feature/your-feature
```

3. Make your changes.
4. Commit your changes:

```bash
git commit -m "feat: add your feature"
```

5. Push your branch:

```bash
git push origin feature/your-feature
```

6. Open a Pull Request.

Please keep changes focused and follow the existing project structure and coding conventions.

## License

This project is licensed under the **MIT License**.

See the `LICENSE` file for more information.

## Author

**Ramadan Ismael**

Built with ❤️ using **C#**, **.NET**, and **Avalonia UI**.
