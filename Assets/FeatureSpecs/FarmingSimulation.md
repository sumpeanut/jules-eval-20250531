# Farming Simulation Feature Specification

## Overview
Implement a farming simulation system where players can place seeds in a grid, manage resources, and observe plant growth, including hybridization based on neighboring plants.

---

## Core Requirements

### 1. Grid System
- Implement a 2D grid to represent the farm plot.
- Each cell can hold a seed, plant, or be empty.
- Support for variable grid sizes.

### 2. Seed Placement
- Players can select and place seeds in grid cells.
- Seeds have types (e.g., carrot, tomato, etc.).
- Prevent placement in occupied cells.

### 3. Plant Growth
- Seeds progress through growth stages (seed → sprout → mature plant).
- Growth is time-based and can be influenced by resources and neighbors.
- Visual representation for each growth stage.

### 4. Resource Management
- Plants require water, soil quality, and sunlight.
- Each plant type has unique resource needs.
- Players can manage and allocate resources to grid cells.
- Resource levels affect growth speed and health.

### 5. Hybridization System
- Plants can influence neighboring plants.
- If compatible plants are adjacent, hybrids can form with unique traits.
- Define rules for hybridization (e.g., which plants can hybridize, resulting traits).

### 6. UI/UX
- Display grid, plant states, and resource levels.
- Allow players to select seeds, place them, and manage resources.
- Show feedback for growth, resource needs, and hybridization events.

### 7. Testing
- Write EditMode and PlayMode tests for all core systems.
- Ensure testability via dependency injection and modular design.

---

## Implementation Tasks
- [ ] Design grid and cell data structures.
- [ ] Implement seed placement logic.
- [ ] Create plant growth system with resource checks.
- [ ] Develop resource management UI and logic.
- [ ] Implement hybridization detection and resolution.
- [ ] Add visual feedback for plant states and events.
- [ ] Write comprehensive tests for all systems.

---

## Notes
- Optimize for mobile performance (object pooling, efficient updates).
- Use prefabs for plants and UI elements.
- Organize scripts under `Assets/Scripts/Gameplay/Farming/`.
- Document all public APIs and complex logic.

---

*Update this file as the feature evolves.*
