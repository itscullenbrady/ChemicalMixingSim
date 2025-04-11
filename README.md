# Chemical Mixing Visualization Application  
**Exploring Chemistry Through Software Development**  
**Author:** Cullen Murphy-Brady  
**Date:** April 2025  

---

## Table of Contents  
1. [Project Overview](#project-overview)  
2. [System Architecture](#system-architecture)  
3. [Core Features](#core-features)  
4. [Technical Implementation](#technical-implementation)  
5. [Challenges and Solutions](#challenges-and-solutions)  
6. [Lessons Learned](#lessons-learned)  
7. [Future Directions](#future-directions)  
8. [How to Run the Application](#how-to-run-the-application)  
9. [Acknowledgments](#acknowledgments)  

---

## Project Overview  
The **Chemical Mixing Visualization Application** is a desktop tool built with WPF that simulates chemical reactions in a safe, virtual environment. It allows users to combine two predefined chemicals and explore their molecular structures and potential interactions.

**Key Highlights:**  
- **Platform:** Windows Presentation Foundation (WPF)  
- **Tech Stack:** C# .NET, SQLite, OpenBabel cheminformatics toolkit  
- **Purpose:**  
  - Safely explore chemical interactions  
  - Visualize molecular structures using SMILES notation  
  - Provide an interactive and educational experience  

This project represents a fusion of technical development and scientific curiosity, inspired by alchemy-themed games and a lifelong interest in chemistry.

---

## System Architecture  
**Platform:**  
- Windows Presentation Foundation (WPF) for robust and responsive desktop UI  

**Tech Stack:**  
- **C# .NET:** Core application logic  
- **SQLite:** Local database storing chemical and reaction data  
- **OpenBabel:** Used for molecular visualization and SMILES notation processing  

**Key Components:**  
- **Chemical Database Integration:**  
  - Stores 31 chemicals and associated metadata  
  - Includes a reaction lookup table  

- **Molecular Visualization:**  
  - Generates 2D molecular diagrams using OpenBabel  

- **Reaction Prediction:**  
  - Validates chemical combinations  
  - Displays descriptive outputs when reactions are found  

---

## Core Features  
1. **Dual Chemical Selection**  
   - Choose two chemicals from dropdown menus  

2. **Reaction Validation**  
   - Checks for a match in the reaction table  
   - Displays details if a reaction is found  

3. **Molecular Visualization**  
   - Generates molecular images of individual chemicals  

4. **Descriptive Info Panels**  
   - Shows scientific descriptions for chemicals and reactions  

---

## Technical Implementation  

### Database Integration  
```csharp
using (var connection = new SQLiteConnection(dbPath)) {
    connection.Open();
    string query = "SELECT Name FROM Chemicals";
    // Execute query and process results
}
```

### OpenBabel Integration  
- **SMILES Processing**: Converts chemical strings into diagrams  
- **Wrapper Pattern**: Executes the OpenBabel CLI with parameters  

```csharp
var startInfo = new ProcessStartInfo {
    FileName = OpenBabelPath,
    Arguments = $"-:\"{smiles}\" -O \"{imagePath}\" --gen2D -xu 500",
    UseShellExecute = false,
    RedirectStandardOutput = true,
    RedirectStandardError = true
};
```

### WPF Features  
- **Data Binding** for dynamic UI updates  
- **Grid Layout** for organized and intuitive interface  

---

## Challenges and Solutions  

### 1. OpenBabel Integration  
- **Challenge**: Sparse documentation, version conflicts  
- **Solution**:  
  - Tested multiple versions  
  - Implemented error handling and fallback logic  

### 2. SMILES Notation Parsing  
- **Challenge**: Ensuring visual accuracy  
- **Solution**: Extensive testing with varied chemical data  

### 3. Development Time Overrun  
- **Challenge**: Integration hurdles extended development  
- **Solution**: Researched community resources and adapted approach  

---

## Lessons Learned  
1. **Thoroughly Vet Dependencies**  
   - Assess compatibility and support before committing  

2. **Buffer for the Unexpected**  
   - Leave time for the unknowns in planning  

3. **Value of Community Knowledge**  
   - Forums, GitHub issues, and Stack Overflow can be life-savers  

---

## Future Directions  
1. **Web & Mobile Versions**  
   - Extend accessibility beyond desktop platforms  

2. **3D Molecular Visualization**  
   - Enhance the visual experience using advanced rendering  

3. **Expanded Database**  
   - Add more chemicals and data for deeper simulations  

4. **Gamification Elements**  
   - Add discovery-based goals to promote deeper learning  

---

## How to Run the Application  

### Prerequisites  
- Windows OS with .NET 9 installed  
- Visual Studio 2022  
- OpenBabel (v3.1.1 recommended)  

### Setup Instructions  
1. Clone or download this repository  
2. Open the solution in Visual Studio 2022  
3. Build the project to restore all dependencies  
4. Ensure `obabel.exe` is placed in the `OpenBabel-3.1.1` folder inside the project directory  
5. Run the application  

### Usage  
1. Select two chemicals from the dropdown menus  
2. View molecular diagrams and chemical info  
3. Check for and observe any resulting reactions  

---

## Acknowledgments  
- **OpenBabel Community** – For the powerful open-source toolkit  
- **Online Developer Forums** – For supporting problem-solving and troubleshooting  
- **Inspirations** – Alchemy-themed games and real-world chemistry curiosity  

---

