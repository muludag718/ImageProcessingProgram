# PixelCraft - A High-Performance Image Processing Application

[![C#](https://img.shields.io/badge/C%23-11-blueviolet)](https://learn.microsoft.com/en-us/dotnet/csharp/)
[![.NET](https://img.shields.io/badge/.NET-8.0-blue)](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
[![Platform](https://img.shields.io/badge/Platform-Windows-0078D6)](https://www.microsoft.com/en-us/windows)
[![License](https://img.shields.io/badge/License-MIT-green)](LICENSE)

An advanced image processing application built with C# and Windows Forms, focusing on a high-performance, memory-efficient, and extensible architecture. This project goes beyond a simple editor to demonstrate modern .NET features for computationally intensive tasks.

---

## 📸 Screenshot

***(Important: Replace the image below with a real screenshot of your application!)***

![Screenshot of PixelCraft](https://i.imgur.com/x5zUa2p.png) 
*A preview of the user interface, showing the original and processed images side-by-side.*

---

## ✨ Features

* **Load and Save:** Open and save images in common formats (JPG, PNG, BMP).
* **Non-Destructive Workflow:** The original image is always preserved. Each filter operation is applied to a fresh copy of the original.
* **Core Filters:** Includes foundational filters like:
    * Invert Colors (Negative)
    * Grayscale Conversion (Luminosity Method)
* **Parameterized Filters:** A flexible system for filters that require user input (e.g., Brightness) using a dynamically generated UI.
* **Extensible Filter System:** Automatically discovers and loads any new filter class added to the project at runtime.
* **Unlimited Undo/Redo:** A robust history system to step backward and forward through operations.

---

## 🛠️ Tech Stack & Core Architecture

This project was built to showcase modern C#/.NET 8 capabilities for high-performance computing.

* **Language:** C# 12
* **Framework:** .NET 8
* **UI:** Windows Forms

### Architectural Highlights:

* **High-Performance Memory Management:**
    * Utilizes `Span<T>` and `Memory<T>` for direct, zero-copy memory manipulation of pixel data.
    * Employs `ArrayPool` via `IMemoryOwner<T>` to rent and return large memory buffers. This drastically reduces pressure on the Garbage Collector (GC), preventing UI freezes and stutters during intensive operations like Undo/Redo and convolution.

* **Parallel Processing:**
    * Leverages the Task Parallel Library (`Parallel.For`) to distribute image processing tasks across all available CPU cores, significantly speeding up filter application on large images.

* **Extensible "Plugin" Architecture:**
    * A set of core interfaces (`IPixel`, `IOperation`, `IFilter`, `IRowProcessor`) define a clear contract for all components.
    * **Dynamic Filter Discovery:** The application uses **C# Reflection** to scan its own assembly at startup, find all classes that implement `IFilter`, and automatically build the UI menu. Adding a new filter is as simple as adding a new file.

* **Lazy Evaluation Engine:**
    * Operations are not executed immediately. They are added to a queue via a fluent API (`.Apply(filter)`).
    * The entire chain of operations is executed only once when the result is needed (e.g., displaying or saving the image), optimizing performance.

* **Modern C# Idioms:**
    * Generic (`<TPixel>`) data structures and algorithms for type safety and flexibility.
    * Use of `struct` for filters and pixel types for performance.
    * `explicit operator` overloads for intuitive type casting between `AdvancedBitmap` and `System.Drawing.Bitmap`.
    * `readonly` fields and target-typed `new()` expressions for cleaner, safer code.

---

## 🚀 Getting Started

### Prerequisites

* [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
* [Visual Studio 2022](https://visualstudio.microsoft.com/) (or another compatible IDE)

### Installation

1.  Clone the repository to your local machine:
    ```bash
    git clone [https://github.com/YOUR_USERNAME/YOUR_REPOSITORY_NAME.git](https://github.com/YOUR_USERNAME/YOUR_REPOSITORY_NAME.git)
    ```
2.  Open the `.sln` solution file in Visual Studio.
3.  Build the solution (this will restore NuGet packages if any were added).
4.  Run the project by pressing `F5`.

---

## 📖 How to Use

1.  Launch the application.
2.  Go to **File > Open...** to load an image. The original will appear on the left.
3.  Go to the **Filters** menu and select a category, then a filter.
4.  If the filter has parameters (e.g., "Brightness..."), a settings dialog will appear. Adjust the values and click "OK".
5.  The processed image will appear on the right.
6.  Go to **File > Save As...** to save the processed image.

---

## 🛣️ Future Roadmap

This project provides a solid foundation for more advanced features, including:

* [ ] Implementing multi-image operations (e.g., adding or blending two images).
* [ ] Implementing whole-image algorithms that are not row-parallel (e.g., Histogram-based operations, Rotation).
* [ ] Refactoring the `Convolve` engine into a separate, reusable `ImageProcessor` class.
* [ ] Adding a live preview option for parameterized filters.
* [ ] Implementing support for more pixel formats.

---

## 📄 License

This project is licensed under the MIT License. See the `LICENSE` file for details.

---

## 👤 Author

**YOUR NAME**

* GitHub: `[github.com/muludag718](https://github.com/muludag718)`
