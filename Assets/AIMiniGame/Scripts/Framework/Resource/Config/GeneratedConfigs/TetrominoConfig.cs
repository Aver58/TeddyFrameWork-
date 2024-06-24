using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TetrominoConfig : BaseConfig {
    public string id;
    public int[,] shape;
    public Color color;

    public override void Parse(string[] values, string[] headers) {
        for (int i = 0; i < headers.Length; i++) {
            var header = headers[i].Replace("\r", "");
            switch (header) {
                case "id":
                    id = values[i];
                    break;
                case "shape":
                    var rows = values[i].Split('|');
                    shape = new int[4, 4];
                    for (int r = 0; r < rows.Length; r++) {
                        var cols = rows[r].Split(';');
                        for (int c = 0; c < cols.Length; c++) {
                            shape[r, c] = int.Parse(cols[c]);
                        }
                    }
                    break;
                case "color":
                    ColorUtility.TryParseHtmlString(values[i], out color);
                    break;
            }
        }
    }

    private static Dictionary<string, TetrominoConfig> cachedConfigs;
    public static TetrominoConfig Get(string key) {
        if (cachedConfigs == null) {
            cachedConfigs = ConfigManager.Instance.LoadConfig<TetrominoConfig>("Tetromino.csv");
        }

        TetrominoConfig config = null;
        if (cachedConfigs != null) {
            cachedConfigs.TryGetValue(key, out config);
        }

        return config;
    }

    public static List<string> GetKeys() {
        if (cachedConfigs == null) {
            cachedConfigs = ConfigManager.Instance.LoadConfig<TetrominoConfig>("Tetromino.csv");
        }

        return cachedConfigs != null ? new List<string>(cachedConfigs.Keys) : new List<string>();
    }
}