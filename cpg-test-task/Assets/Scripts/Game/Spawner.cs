using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Spawner : GridItem
    {
        [SerializeField]
        private List<GameObject> spawnedItems;

        public override void Setup(GridGame grid, Vector2Int gridPosition)
        {
            base.Setup(grid, gridPosition);
            transform.position = GetGrid().GridToWorldPosition(GetGridPosition());
        }

        public void SpawnItem()
        {
            var nearestEmptyCell = GetGrid().FindNearestEmptyCell(GetGridPosition());
            if (nearestEmptyCell != GetGridPosition())
            {
                var spawnedItem = spawnedItems[Random.Range(0, spawnedItems.Count)];
                var position = transform.position;
                var newItemPosition = new Vector3(
                    position.x,
                    position.y,
                    position.z + 0.05f
                );
                var newItem = Instantiate(spawnedItem, newItemPosition, transform.rotation);
                GetGrid().CellAt(nearestEmptyCell).SetItem(newItem.GetComponent<Item>());
            }
        }

        public override bool IsAdjacent()
        {
            return false;
        }
    }
}