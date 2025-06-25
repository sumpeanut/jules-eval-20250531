using UnityEngine;
using System.Collections.Generic;
using FarmingGame.Data.Inventory; // To access ItemShapeType

namespace FarmingGame.Gameplay.Inventory.Logic
{
    public static class InventoryShapeUtility
    {
        /// <summary>
        /// Gets the relative cell coordinates occupied by an item shape at a given rotation.
        /// The origin (0,0) is typically the anchor point of the shape.
        /// Rotations are clockwise: 0 = default, 1 = 90 deg, 2 = 180 deg, 3 = 270 deg.
        /// </summary>
        public static List<Vector2Int> GetShapeCells(ItemShapeType shapeType, int rotation = 0)
        {
            List<Vector2Int> baseShape;
            switch (shapeType)
            {
                case ItemShapeType._1x1:
                    baseShape = new List<Vector2Int> { Vector2Int.zero };
                    break;
                case ItemShapeType._1x2:
                    baseShape = new List<Vector2Int> { Vector2Int.zero, new Vector2Int(0, 1) };
                    break;
                case ItemShapeType._2x1:
                    baseShape = new List<Vector2Int> { Vector2Int.zero, new Vector2Int(1, 0) };
                    break;
                case ItemShapeType._2x2:
                    baseShape = new List<Vector2Int> { Vector2Int.zero, new Vector2Int(1, 0), new Vector2Int(0, 1), new Vector2Int(1, 1) };
                    break;
                case ItemShapeType._1x3:
                    baseShape = new List<Vector2Int> { Vector2Int.zero, new Vector2Int(0, 1), new Vector2Int(0, 2) };
                    break;
                case ItemShapeType._3x1:
                    baseShape = new List<Vector2Int> { Vector2Int.zero, new Vector2Int(1, 0), new Vector2Int(2, 0) };
                    break;
                // For L and T shapes, define a base orientation (e.g., LShapeLeftUp)
                // Rotation logic will transform these base coordinates.
                case ItemShapeType.LShapeLeftUp: // Anchor at (0,0), extends to (-1,0) and (0,1)
                                                 // To keep coordinates positive for grid indexing, let's define anchor as bottom-most, left-most point of the bounding box
                                                 // For an L shape like:
                                                 // X O  (0,1) (1,1)
                                                 // X    (0,0)
                                                 // Anchor (0,0)
                    baseShape = new List<Vector2Int> { Vector2Int.zero, new Vector2Int(0,1), new Vector2Int(1,1) }; // Standard L shape
                    break;
                 case ItemShapeType.TShapeUp: // Anchor at the center bottom of the T's vertical bar
                                              // O X O
                                              //   X
                                              // (0,1) (1,1) (2,1)
                                              //       (1,0) - Anchor
                    baseShape = new List<Vector2Int> { new Vector2Int(1,0), new Vector2Int(0,1), new Vector2Int(1,1), new Vector2Int(2,1) };
                    break;
                // TODO: Define other LShape and TShape base forms if needed, or rely on rotation.
                // For simplicity, the current LShape and TShape enums might need refinement
                // or the rotation logic here will handle transforming a single base L/T shape.
                // Let's assume LShapeLeftUp is the "canonical" L shape and TShapeUp is the "canonical" T shape for now.
                case ItemShapeType.LShapeRightUp: // Rotated LShapeLeftUp
                case ItemShapeType.LShapeLeftDown:
                case ItemShapeType.LShapeRightDown:
                     baseShape = GetShapeCells(ItemShapeType.LShapeLeftUp, 0); // Get base and then rotate
                     break;
                case ItemShapeType.TShapeDown:
                case ItemShapeType.TShapeLeft:
                case ItemShapeType.TShapeRight:
                    baseShape = GetShapeCells(ItemShapeType.TShapeUp, 0); // Get base and then rotate
                    break;
                default:
                    baseShape = new List<Vector2Int> { Vector2Int.zero }; // Default to 1x1 for undefined shapes
                    break;
            }

            if (rotation == 0) return baseShape;

            List<Vector2Int> rotatedShape = new List<Vector2Int>();
            // Normalize rotation to 0-3 range (0, 90, 180, 270 clockwise)
            int normalizedRotation = (rotation % 4 + 4) % 4; // Handles negative rotations too

            foreach (var cell in baseShape)
            {
                int x = cell.x;
                int y = cell.y;
                int newX = x, newY = y;

                // Pivot is assumed to be (0,0) of the local shape coordinates.
                // For complex shapes, choosing a consistent pivot is crucial.
                // For shapes like L or T, the "bounding box" concept helps.
                // We might need to adjust coordinates after rotation if the pivot isn't (0,0) of the bounding box.

                // Simple rotation logic (clockwise):
                // 0 deg: (x, y) -> (x, y)
                // 90 deg: (x, y) -> (y, -x)  -- for typical math coordinates. Unity's Y is often inverted.
                // For a grid where (0,0) is top-left, and Y increases downwards:
                // 90 deg clockwise: (x,y) -> (y, MaxX-x) if pivot is (0,0) and MaxX is width-1 of original shape
                // Or more generally, for pivot (px, py):
                // x' = px + (x-px)cos(a) - (y-py)sin(a)
                // y' = py + (x-px)sin(a) + (y-py)cos(a)
                // For 90-degree increments, it's simpler:
                // 90 deg clockwise: (x,y) -> (pivot_y + y - pivot_y, pivot_x - (x - pivot_x)) -- this needs care with grid systems.

                // Let's use a common matrix rotation for 2D points (x,y) around origin (0,0):
                // 90 deg clockwise: (x,y) -> (y, -x)
                // 180 deg clockwise: (x,y) -> (-x, -y)
                // 270 deg clockwise: (x,y) -> (-y, x)
                // After rotation, we might need to translate the shape so all coordinates are non-negative
                // and the top-leftmost cell becomes the new local (0,0) or anchor.

                // For now, let's implement for the defined shapes, assuming (0,0) is a sensible pivot.
                // The L and T shapes might need special handling or a redefinition of their base shapes' origins.

                switch (normalizedRotation)
                {
                    case 1: // 90 degrees clockwise
                        newX = y; newY = -x;
                        break;
                    case 2: // 180 degrees
                        newX = -x; newY = -y;
                        break;
                    case 3: // 270 degrees clockwise
                        newX = -y; newY = x;
                        break;
                }
                rotatedShape.Add(new Vector2Int(newX, newY));
            }

            // After rotation, the coordinates might be negative or shifted.
            // We need to normalize them so the top-leftmost cell of the rotated shape's bounding box is effectively (0,0) for that shape.
            // This is crucial for consistent placement logic.
            if (normalizedRotation > 0 && rotatedShape.Count > 0)
            {
                int minX = int.MaxValue, minY = int.MaxValue;
                foreach(var cell in rotatedShape)
                {
                    if(cell.x < minX) minX = cell.x;
                    if(cell.y < minY) minY = cell.y;
                }

                // Translate all points so that the minX, minY becomes (0,0)
                // This ensures the rotated shape's local origin is its top-leftmost point.
                List<Vector2Int> normalizedAndTranslatedShape = new List<Vector2Int>();
                foreach(var cell in rotatedShape)
                {
                    normalizedAndTranslatedShape.Add(new Vector2Int(cell.x - minX, cell.y - minY));
                }
                return normalizedAndTranslatedShape;
            }

            return rotatedShape; // or baseShape if rotation was 0
        }

        /// <summary>
        /// Gets the dimensions (width, height) of a shape's bounding box after rotation.
        /// </summary>
        public static Vector2Int GetDimensions(ItemShapeType shapeType, int rotation = 0)
        {
            List<Vector2Int> cells = GetShapeCells(shapeType, rotation);
            if (cells == null || cells.Count == 0) return Vector2Int.zero;

            int minX = 0, maxX = 0, minY = 0, maxY = 0;
            // Since GetShapeCells now normalizes to have a (0,0) point, minX and minY will be 0
            // if the shape is not empty.

            foreach (var cell in cells)
            {
                // minX = Mathf.Min(minX, cell.x); // Will be 0 due to normalization
                // minY = Mathf.Min(minY, cell.y); // Will be 0 due to normalization
                maxX = Mathf.Max(maxX, cell.x);
                maxY = Mathf.Max(maxY, cell.y);
            }
            return new Vector2Int(maxX + 1, maxY + 1); // Dimensions are (maxCoord + 1)
        }
    }
}
