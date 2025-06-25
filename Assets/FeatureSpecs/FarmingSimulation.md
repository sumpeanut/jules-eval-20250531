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
- [x] Design grid and cell data structures.
- [x] Implement seed placement logic.
- [x] Create plant growth system with resource checks.
- [x] Develop resource management UI and logic.
- [x] Implement hybridization detection and resolution.
- [x] Add visual feedback for plant states and events.
- [x] Write comprehensive tests for all systems.

---

## Next Steps: Grid & User Interaction

### Grid Visualization & Management
- Add a `FarmGrid` MonoBehaviour to a GameObject in the scene to instantiate and manage the visual grid.
- Create a cell prefab (sprite, button, or UI element) to represent each grid cell visually (empty, planted, harvested).

### User Interaction
- Implement a seed selection UI for players to choose which plant to place.
- Handle cell clicks/taps:
  - If empty: show seed selection and allow planting.
  - If a plant is present and mature: allow harvesting (remove plant, trigger reward/animation).
- Ensure resource management UI can be shown/hidden for the selected cell.

### Planting & Harvesting Logic
- On seed selection and cell click, instantiate a `PlantBase` prefab with the correct `PlantData` at that cell.
- Update the cell's state to "planted."
- When a plant is mature and clicked, remove it and optionally trigger a reward or animation.

### Scene Setup
- Add a GameObject for the grid manager (`FarmGrid` and a new `FarmGridView` script).
- Add a Canvas for UI (seed selection, resource management, etc.).
- Create and assign prefabs for grid cells and plants.

### Optional: Object Pooling
- Use object pooling for plant and cell prefabs for mobile performance.

---

*Update this file as the feature evolves.*
