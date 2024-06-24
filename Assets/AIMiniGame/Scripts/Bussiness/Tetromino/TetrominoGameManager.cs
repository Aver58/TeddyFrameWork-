using System.Collections.Generic;
using UnityEngine;

public class TetrominoGameManager : MonoBehaviour {
    private int rows = 20;
    private int columns = 10;
    private float dropInterval = 1.0f;
    private float dropTimer = 0.0f;
    public Transform CanvasTransform;
    private Tetromino currentTetromino;
    private Vector2Int spawnPosition = new Vector2Int(5, 20);
    private HashSet<Vector2Int> occupiedCells = new HashSet<Vector2Int>();
    private List<GameObject> activeTetrominos = new List<GameObject>();

    void Start() {
        StartGame();
    }

    void Update() {
        dropTimer += Time.deltaTime;
        if (dropTimer >= dropInterval) {
            dropTimer = 0.0f;
            DropCurrentTetromino();
        }

        HandleInput();
    }

    void OnDestroy() {

    }

    private void StartGame() {
        Debug.Log("Game Start!");
        SpawnTetromino();
    }

    private void GameOver() {
        Debug.Log("Game Over!");
        ClearTetrominos();
        occupiedCells.Clear();
    }

    private void SpawnTetromino() {
        var tetrominoIds = TetrominoConfig.GetKeys();
        var randomId = tetrominoIds[Random.Range(0, tetrominoIds.Count)];
        SpawnTetromino(randomId, spawnPosition);
    }

    public void SpawnTetromino(string id, Vector2Int startPosition) {
        TetrominoConfig config = TetrominoConfig.Get(id);
        Debug.Log($"SpawnTetromino {id}");
        if (config != null) {
            ResourceManager.Instance.LoadResourceAsync<GameObject>("Assets/AIMiniGame/ToBundle/Prefabs/Tetromino/Tetromino.prefab", prefab => {
                if (prefab != null) {
                    var tetrominoGo = Instantiate(prefab, CanvasTransform);
                    var tetromino = tetrominoGo.GetComponent<Tetromino>();
                    tetromino.Initialize(config.shape, config.color, startPosition);
                    activeTetrominos.Add(tetrominoGo);

                    if (IsPositionOccupied(tetromino.position)) {
                        GameOver();
                    } else {
                        currentTetromino = tetromino;
                    }
                }
            });
        }
    }

    private void DropCurrentTetromino() {
        if (currentTetromino != null) {
            Vector2Int newPosition = currentTetromino.position + new Vector2Int(0, -1);
            if (IsValidPosition(newPosition, currentTetromino.cells)) {
                MoveTetromino(currentTetromino, new Vector2Int(0, -1));
            } else {
                AddOccupyCells(currentTetromino);
                ClearCompleteLines();
                SpawnTetromino();
            }
        }
    }

    public void MoveTetromino(Tetromino tetromino, Vector2Int direction) {
        tetromino.Move(direction);
    }

    private void AddOccupyCells(Tetromino tetromino) {
        foreach (Vector2Int cell in tetromino.cells) {
            occupiedCells.Add(tetromino.position + cell);
        }
    }

    private bool IsValidPosition(Vector2Int position, Vector2Int[] cells) {
        foreach (Vector2Int cell in cells) {
            Vector2Int newPos = position + cell;
            if (newPos.x < 0 || newPos.x >= columns || newPos.y < 0 || occupiedCells.Contains(newPos)) {
                return false;
            }
        }
        return true;
    }

    private bool IsPositionOccupied(Vector2Int position) {
        return occupiedCells.Contains(position);
    }

    private void HandleInput() {
        if (currentTetromino == null) return;

        if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            TryMoveCurrentTetromino(new Vector2Int(-1, 0));
        }
        if (Input.GetKeyDown(KeyCode.RightArrow)) {
            TryMoveCurrentTetromino(new Vector2Int(1, 0));
        }
        if (Input.GetKeyDown(KeyCode.DownArrow)) {
            DropCurrentTetromino();
        }
        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            RotateCurrentTetromino();
        }
    }

    private void TryMoveCurrentTetromino(Vector2Int direction) {
        Vector2Int newPosition = currentTetromino.position + direction;
        if (IsValidPosition(newPosition, currentTetromino.cells)) {
            MoveTetromino(currentTetromino, direction);
        }
    }

    public void ClearTetrominos() {
        foreach (var tetromino in activeTetrominos) {
            Destroy(tetromino);
        }

        activeTetrominos.Clear();
    }

    private void RotateCurrentTetromino() {
        currentTetromino.Rotate();
        if (!IsValidPosition(currentTetromino.position, currentTetromino.cells)) {
            currentTetromino.Rotate(); // Undo rotation
            currentTetromino.Rotate();
            currentTetromino.Rotate();
        }
    }

    private void ClearCompleteLines() {
        List<int> completeLines = new List<int>();
        for (int y = 0; y < rows; y++) {
            bool isComplete = true;
            for (int x = 0; x < columns; x++) {
                if (!occupiedCells.Contains(new Vector2Int(x, y))) {
                    isComplete = false;
                    break;
                }
            }
            if (isComplete) {
                completeLines.Add(y);
            }
        }

        foreach (int line in completeLines) {
            for (int x = 0; x < columns; x++) {
                occupiedCells.Remove(new Vector2Int(x, line));
            }
            List<Vector2Int> newOccupiedCells = new List<Vector2Int>();
            foreach (Vector2Int cell in occupiedCells) {
                if (cell.y > line) {
                    newOccupiedCells.Add(new Vector2Int(cell.x, cell.y - 1));
                } else {
                    newOccupiedCells.Add(cell);
                }
            }
            occupiedCells = new HashSet<Vector2Int>(newOccupiedCells);
        }
    }
}