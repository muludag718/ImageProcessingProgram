# PixelCraft - A High-Performance Image Processing Platform

[![C#](https://img.shields.io/badge/C%23-12-blueviolet)](https://learn.microsoft.com/en-us/dotnet/csharp/)
[![.NET](https://img.shields.io/badge/.NET-8.0-blue)](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
[![Platform](https://img.shields.io/badge/Platform-Windows-0078D6)](https://www.microsoft.com/en-us/windows)
[![License](https://img.shields.io/badge/License-MIT-green)](LICENSE)

PixelCraft is an advanced image processing application and framework built with C# and Windows Forms. It's designed from the ground up to showcase a high-performance, memory-efficient, and highly extensible architecture for complex computational tasks. This project serves as a practical example of modern .NET features applied to the domain of computer vision and image manipulation.

---

## ✨ Features

### Core Functionality
* **Load and Save:** Open and save images in common formats (JPG, PNG, BMP).
* **Non-Destructive Workflow:** The original image is always preserved. Each filter operation is applied to a fresh copy.
* **Region of Interest (ROI):** Apply any filter to a user-selected rectangular area of the image for precise editing and fast previews.
* **Robust Undo/Redo Engine:** The backend is fully equipped with an efficient Undo/Redo history system.

### Advanced Architectural Features
* **Dynamic Plugin System:** The application uses C# Reflection to automatically discover and load any filter class at runtime. Just add a new file, and it appears in the menu!
* **Parameterized Filters:** A powerful system that allows filters to request parameters from the user. The UI is generated automatically based on the filter's definition.
* **Multi-Image Operations:** The architecture supports filters that take two images as input (e.g., for blending or addition).

### Included Filters
* **Color & Tone:**
    * Invert (Negative)
    * Grayscale (Luminosity Method)
    * Sepia
    * Brightness (Parameterized)
    * Contrast (Parameterized)
    * Auto-Contrast (Two-pass global analysis)
* **Detail:**
    * Sharpen (Convolution-based)
    * Gaussian Blur (Parameterized & Convolution-based)
* **Binary Operations:**
    * Add Images

---

## 🛠️ Tech Stack & Core Architecture

This project was built to showcase modern C#/.NET 8 capabilities for high-performance computing.

* **Language:** C# 12
* **Framework:** .NET 8
* **UI:** Windows Forms

### Architectural Highlights:

* **High-Performance Memory Management:**
    * Utilizes `Span<T>` and `Memory<T>` for direct, zero-copy memory manipulation of pixel data.
    * Employs `ArrayPool` via `IMemoryOwner<T>` to rent and return large memory buffers, drastically reducing Garbage Collector (GC) pressure and preventing UI freezes.

* **Parallel Processing:**
    * Leverages the Task Parallel Library (`Parallel.For`) to distribute image processing tasks across all available CPU cores, significantly speeding up filter application on large images.

* **Extensible "Plugin" Architecture:**
    * A set of core interfaces (`IPixel`, `IOperation`, `IFilter`, `IRowProcessor`, `IBinaryImageOperation`) define clear contracts for all components.
    * **Dynamic Filter Discovery:** The application uses **C# Reflection** to scan its own assembly at startup, find all classes implementing `IFilter`, and automatically build the UI menu.

* **Versatile and Decoupled Filter Design:**
    * **Context Object Pattern:** A `ProcessContext` object is used to pass all necessary data (source image, secondary image, ROI, etc.) to operations. This makes filters stateless and highly extensible.
    * The architecture supports multiple algorithm types:
        1.  Simple, row-parallelizable filters (`IRowProcessor`).
        2.  Complex, matrix-based filters using the centralized `Convolve` engine.
        3.  Multi-pass, whole-image analysis filters that bypass the row processor for global calculations.

* **Dynamic Settings UI:**
    * The system uses the `PropertyGrid` control in combination with C# **Attributes** (`[DisplayName]`, `[Description]`, etc.) on "Settings" classes to automatically generate a rich settings UI for any filter that requires parameters.

* **Separation of Concerns:**
    * UI logic is cleanly separated from backend processing. For example, the `SelectionManager` class encapsulates all mouse handling and drawing for ROI selection, keeping the main form's code clean.

---

## 🚀 Getting Started

### Prerequisites

* [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
* [Visual Studio 2022](https://visualstudio.microsoft.com/)

### Installation

1.  Clone the repository:
    ```bash
    git clone [https://github.com/muludag718/ImageProcessingProgram.git](https://github.com/muludag718/ImageProcessingProgram.git)
    ```
2.  Open the `.sln` solution file in Visual Studio.
3.  Build and run the project by pressing `F5`.

---

## 🛣️ Future Roadmap

This project provides a solid foundation for more advanced features from our development plan:

* [ ] **UI/UX:**
    * Implement the UI for the Undo/Redo feature.
    * Add a live preview for parameterized filters.
    * Implement Zoom & Pan functionality.
* [ ] **Geometric Transformations:** Add Resize, Crop, and Rotate operations.
* [ ] **Professional Architecture:**
    * Implement a full Layer System with blending modes.
    * Refactor core engines like `Convolve` into a separate `ImageProcessor` class.
* [ ] **Advanced Algorithms:**
    * Explore Frequency Domain processing with FFT.
    * Integrate GPU acceleration via `ComputeSharp`.
    * Experiment with AI/ML filters using ONNX Runtime.

---

## 📄 License

This project is licensed under the MIT License. See the `LICENSE` file for details.

---

## 👤 Author

**Muludag**

* GitHub: `[github.com/muludag718]([https://github.com/muludag718])`
