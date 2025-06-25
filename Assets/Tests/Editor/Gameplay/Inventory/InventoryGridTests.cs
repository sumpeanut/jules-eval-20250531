using NUnit.Framework;
using UnityEngine;
using FarmingGame.Gameplay.Inventory.Logic;
using FarmingGame.Gameplay.Inventory; // For InventoryItem
using FarmingGame.Data.Inventory; // For ItemData and ItemShapeType

public class InventoryGridTests
{
    private ItemData _item1x1;
    private ItemData _item1x2;
    private ItemData _item2x1;
    private ItemData _item2x2;

    [SetUp]
    public void SetUp()
    {
        // Create dummy ItemData ScriptableObjects for testing
        // In a real test environment, these might be loaded or mocked.
        // For EditMode tests, ScriptableObject.CreateInstance is fine.
        _item1x1 = ScriptableObject.CreateInstance<ItemData>();
        _item1x1.itemName = "Test Item 1x1";
        _item1x1.itemShape = ItemShapeType._1x1;

        _item1x2 = ScriptableObject.CreateInstance<ItemData>();
        _item1x2.itemName = "Test Item 1x2";
        _item1x2.itemShape = ItemShapeType._1x2;

        _item2x1 = ScriptableObject.CreateInstance<ItemData>();
        _item2x1.itemName = "Test Item 2x1";
        _item2x1.itemShape = ItemShapeType._2x1;

        _item2x2 = ScriptableObject.CreateInstance<ItemData>();
        _item2x2.itemName = "Test Item 2x2";
        _item2x2.itemShape = ItemShapeType._2x2;
    }

    [TearDown]
    public void TearDown()
    {
        // Clean up ScriptableObject instances
        Object.DestroyImmediate(_item1x1);
        Object.DestroyImmediate(_item1x2);
        Object.DestroyImmediate(_item2x1);
        Object.DestroyImmediate(_item2x2);
    }

    [Test]
    public void InventoryGrid_Constructor_InitializesCorrectly()
    {
        var grid = new InventoryGrid(10, 5);
        Assert.AreEqual(new Vector2Int(10, 5), grid.GridSize);
        for (int x = 0; x < 10; x++)
        {
            for (int y = 0; y < 5; y++)
            {
                Assert.IsNull(grid.GetItemAtCell(new Vector2Int(x, y)), $"Cell ({x},{y}) should be empty.");
            }
        }
    }

    [Test]
    public void CanPlaceItem_EmptyGrid_ReturnsTrueForValidPlacement()
    {
        var grid = new InventoryGrid(5, 5);
        Assert.IsTrue(grid.CanPlaceItem(_item1x1, Vector2Int.zero, 0), "Should place 1x1 at (0,0)");
        Assert.IsTrue(grid.CanPlaceItem(_item2x2, new Vector2Int(1,1), 0), "Should place 2x2 at (1,1)");
    }

    [Test]
    public void CanPlaceItem_OutOfBounds_ReturnsFalse()
    {
        var grid = new InventoryGrid(3, 3);
        Assert.IsFalse(grid.CanPlaceItem(_item2x2, new Vector2Int(2,0), 0), "2x2 should not fit at (2,0) in 3x3 grid (out of x bounds)");
        Assert.IsFalse(grid.CanPlaceItem(_item2x2, new Vector2Int(0,2), 0), "2x2 should not fit at (0,2) in 3x3 grid (out of y bounds)");
        Assert.IsFalse(grid.CanPlaceItem(_item1x1, new Vector2Int(3,0), 0), "1x1 should not fit at (3,0) in 3x3 grid");
    }

    [Test]
    public void PlaceItem_And_GetItemAtCell_ReturnsCorrectItem()
    {
        var grid = new InventoryGrid(5, 5);
        var inventoryItem = new InventoryItem(_item1x1);

        grid.PlaceItem(inventoryItem, Vector2Int.zero, 0);

        Assert.AreEqual(inventoryItem, grid.GetItemAtCell(Vector2Int.zero));
    }

    [Test]
    public void PlaceItem_MultiCellItem_OccupiesAllCells()
    {
        var grid = new InventoryGrid(5, 5);
        var item = new InventoryItem(_item2x2);
        var position = new Vector2Int(1, 1);
        grid.PlaceItem(item, position, 0);

        Assert.AreEqual(item, grid.GetItemAtCell(new Vector2Int(1, 1)), "Cell (1,1) should be item");
        Assert.AreEqual(item, grid.GetItemAtCell(new Vector2Int(2, 1)), "Cell (2,1) should be item");
        Assert.AreEqual(item, grid.GetItemAtCell(new Vector2Int(1, 2)), "Cell (1,2) should be item");
        Assert.AreEqual(item, grid.GetItemAtCell(new Vector2Int(2, 2)), "Cell (2,2) should be item");
        Assert.IsNull(grid.GetItemAtCell(new Vector2Int(0,0)), "Cell (0,0) should be empty");
    }

    [Test]
    public void CanPlaceItem_CollisionWithOtherItem_ReturnsFalse()
    {
        var grid = new InventoryGrid(5, 5);
        var itemA = new InventoryItem(_item1x1);
        grid.PlaceItem(itemA, Vector2Int.zero, 0);

        Assert.IsFalse(grid.CanPlaceItem(_item1x1, Vector2Int.zero, 0), "Cannot place on occupied cell");
        Assert.IsFalse(grid.CanPlaceItem(_item2x2, Vector2Int.zero, 0), "Cannot place 2x2 overlapping itemA");
    }

    [Test]
    public void RemoveItem_ClearsCellsAndItemCanBeReplaced()
    {
        var grid = new InventoryGrid(5, 5);
        var item = new InventoryItem(_item1x1);
        var position = new Vector2Int(0, 0);

        grid.PlaceItem(item, position, 0);
        Assert.AreEqual(item, grid.GetItemAtCell(position));

        grid.RemoveItem(item);
        Assert.IsNull(grid.GetItemAtCell(position), "Cell should be empty after removing item.");
        Assert.IsTrue(grid.CanPlaceItem(_item1x1, position, 0), "Should be able to place item after removing previous one.");
    }

    [Test]
    public void RemoveItem_MultiCellItem_ClearsAllItsCells()
    {
        var grid = new InventoryGrid(5, 5);
        var item = new InventoryItem(_item2x2);
        var position = new Vector2Int(1,1);
        grid.PlaceItem(item, position, 0);

        grid.RemoveItem(item);

        Assert.IsNull(grid.GetItemAtCell(new Vector2Int(1,1)));
        Assert.IsNull(grid.GetItemAtCell(new Vector2Int(2,1)));
        Assert.IsNull(grid.GetItemAtCell(new Vector2Int(1,2)));
        Assert.IsNull(grid.GetItemAtCell(new Vector2Int(2,2)));
    }


    [Test]
    public void CanPlaceItem_WithRotation_CorrectlyChecksPlacement()
    {
        var grid = new InventoryGrid(3, 3);
        // _item1x2 is 1 wide, 2 tall by default
        // Rotated 90 deg (_item1x2, rot=1) becomes 2 wide, 1 tall
        Assert.IsTrue(grid.CanPlaceItem(_item1x2, Vector2Int.zero, 1), "1x2 rotated to 2x1 should fit at (0,0)");
        Assert.AreEqual(2, InventoryShapeUtility.GetDimensions(_item1x2.itemShape, 1).x, "Width of 1x2 rotated should be 2");
        Assert.AreEqual(1, InventoryShapeUtility.GetDimensions(_item1x2.itemShape, 1).y, "Height of 1x2 rotated should be 1");

        Assert.IsFalse(grid.CanPlaceItem(_item1x2, new Vector2Int(2,0), 1), "1x2 rotated to 2x1 should NOT fit at (2,0) in 3x3 (1 cell too wide)");
    }

    [Test]
    public void PlaceItem_WithRotation_OccupiesCorrectCells()
    {
        var grid = new InventoryGrid(3,3);
        var item = new InventoryItem(_item1x2); // 1 wide, 2 tall

        // Rotate item by 90 deg (becomes 2 wide, 1 tall)
        grid.PlaceItem(item, Vector2Int.zero, 1);

        Assert.AreEqual(item, grid.GetItemAtCell(new Vector2Int(0,0)));
        Assert.AreEqual(item, grid.GetItemAtCell(new Vector2Int(1,0))); // Due to rotation
        Assert.IsNull(grid.GetItemAtCell(new Vector2Int(0,1))); // Original second cell (0,1) should be empty
    }

    [Test]
    public void CanPlaceItem_IgnoreItem_AllowsCheckingForMove()
    {
        var grid = new InventoryGrid(3,3);
        var itemToMove = new InventoryItem(_item1x2);
        grid.PlaceItem(itemToMove, Vector2Int.zero, 0); // Placed at (0,0) and (0,1)

        // Try to "move" it to (1,0) without rotation. This should be possible.
        // If we don't ignore itemToMove, (0,0) would conflict with itself if shape is large.
        // The CanPlaceItem is for the new position.
        Assert.IsTrue(grid.CanPlaceItem(itemToMove.itemData, new Vector2Int(1,0), 0, itemToMove), "Should be able to place at (1,0) ignoring self.");

        // Place another item
        var blockerItem = new InventoryItem(_item1x1);
        grid.PlaceItem(blockerItem, new Vector2Int(1,0), 0);

        // Now, trying to move itemToMove to (1,0) should fail because blockerItem is there.
        Assert.IsFalse(grid.CanPlaceItem(itemToMove.itemData, new Vector2Int(1,0), 0, itemToMove), "Should not be able to place due to blockerItem.");
    }
}
