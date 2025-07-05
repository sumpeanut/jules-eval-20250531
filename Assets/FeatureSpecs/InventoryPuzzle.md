# Inventory Puzzle Feature Specification

## Overview
Implement a grid-based inventory system where collected items such as seeds and produce occupy space with varying shapes. The player must fit items within a limited inventory grid, turning inventory management into a puzzle.

---

## Core Requirements

### 1. Inventory Grid
- Provide a 2D grid with configurable width and height.
- Each cell can either be empty or occupied by an item.

### 2. Inventory Item Data
- Items define a set of local grid coordinates that describe their shape.
- Items can span multiple cells and may have irregular forms.

### 3. Item Placement Logic
- Validate placements against grid bounds and existing items.
- Support removing items from the grid.

### 4. Puzzle Mechanics
- Ensure that items cannot overlap when placed.
- (Optional) Allow rotation of item shapes for more complex puzzles.

### 5. UI/UX
- Display the inventory grid and placed items.
- Provide feedback when placements are invalid.

### 6. Testing
- Create EditMode tests for grid initialization and placement rules.
- Test overlap checks and removal behaviour.

---

## Implementation Tasks
- [x] Create `InventoryGrid` class to manage cell data.
- [x] Create `InventoryItemData` ScriptableObject to describe item shapes.
- [x] Create `InventoryItem` component representing an item in the inventory.
- [x] Implement placement and removal logic in `InventoryGrid`.
- [x] Write EditMode tests validating placement success and failure cases.

---

*Update this file as the feature evolves.*
