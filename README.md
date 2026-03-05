# 🛡️ Valhalla Defense: Norse Mythology Tower Defense

A modular and scalable 2D Tower Defense game developed in Unity, themed around the defense of Valhalla against legendary creatures. This project was built to demonstrate advanced **Object-Oriented Programming (OOP)** principles and robust software architecture in game development.

---

## 🧠 Architectural Highlights

The project focuses on high sustainability and code reusability through hierarchical class structures:

- ** Strict OOP Principles:** Implementation of **Inheritance, Polymorphism, and Encapsulation**.
- **Abstract Base Classes:** Universal attributes and methods defined in `Enemy` and `Tower` abstract classes.
- **Design Patterns:** Use of the **Singleton Pattern** for global state management via `MissionControl`, `WaveSpawner`, and `BuildManager`.
- **Data-Driven Logging:** A custom timestamped logging system (`GameLogger`) to verify mathematical calculations (armor reduction, net damage, etc.).

---

## ⚔️ Game Entities

### Defense Towers 
- **Freya (Archer):** High precision, single-target physical damage.
- **Odin (Mage):** Runic Area-of-Effect (AoE) magical damage.
- **Ymir (Ice):** Frost-based attacks that inflict a slowing effect on enemies.

### Enemy Units 
- **Draugr (Standard):** Balanced speed and health.
- **Hrimthurs (Tank):** High armor and health, emphasizing the "Tank" role.
- **Valkyrie (Flying):** Airborne units that bypass certain ground obstacles.

---

## 🛠 Tech Stack
- **Engine:** Unity 2022.3 LTS.
- **Language:** C#.
- **Art:** 2D Stylized Norse Mythology Assets.

---

## 📊 Simulation Logic
The game includes a detailed verification system where every interaction is logged with a unique ID for both towers and enemies. This ensures that the simulation engine's backend logic matches the visual output.
