using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text.Json;
using TreasureBackEnd.Model;
using TreasureBackEnd.IServices;
using TreasureBackEnd.Model;
using TreasureBackEnd.Repository;

namespace TreasureBackEnd.Services
{
    public class TreasureService : ITreasureService
    {

        private readonly TreasureContext _treasureContext;

        public TreasureService(TreasureContext treasureContext)
        {
            _treasureContext = treasureContext;
        }

        private double CalculateDistance(int x1, int y1, int x2, int y2)
        {
            return Math.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));
        }

        private async Task SaveResultToDatabase(TreasureInput input, double fuel)
        {
            var record = new TreasureRecord
            {
                Rows = input.N,
                Columns = input.M,
                MaxChest = input.P,
                MatrixData = JsonSerializer.Serialize(input.Matrix),
                FuelCost = fuel
            };


            await _treasureContext.TreasureRecords.AddAsync(record);
            await _treasureContext.SaveChangesAsync();
        }
        public async Task<Double> CalculateMinimumFuel(TreasureInput input)
        {

            ValidateTreasure(input);
            int n = input.N; // rows
            int m = input.M; // columns
            int p = input.P; // highest level of treasure
            int[][] grid = input.Matrix;  // the extracted matrix itself

            var chestPositions = new Dictionary<int, List<(int x, int y)>>();
            // mapping the treasure
            //            {
            //  1: [(0, 0)],
            //  2: [(1, 1), (2, 2)],
            //  3: [(4, 4)]
            //}

            for (int i = 0; i < n; i++)
                for (int j = 0; j < m; j++)
                {
                    int chestNumber = grid[i][j];
                    if (!chestPositions.ContainsKey(chestNumber))
                        chestPositions[chestNumber] = new List<(int, int)>();
                    chestPositions[chestNumber].Add((i, j));
                }

            // Will throw error if not add this 
            if (p == 1)
            {
                var pos = chestPositions[1][0];
                double temp = Math.Round(CalculateDistance(0, 0, pos.x, pos.y), 5);
                await SaveResultToDatabase(input, temp);
                return temp;
            }


            double[,] dimension = new double[n, m];
            for (int i = 0; i < n; i++)
                for (int j = 0; j < m; j++)
                    dimension[i, j] = double.MaxValue; // add the value is maximum and cannot be lower than anything (to safeguard) 

            dimension[0, 0] = 0;

            // start at chest 1
            foreach (var (x, y) in chestPositions[1])
                dimension[x, y] = CalculateDistance(0, 0, x, y);

            // move to chest 2 through p
            for (int chest = 2; chest <= p; chest++)
            {
                if (!chestPositions.ContainsKey(chest - 1) || !chestPositions.ContainsKey(chest)) continue;

                foreach (var (nx, ny) in chestPositions[chest])
                {
                    dimension[nx, ny] = chestPositions[chest - 1]
                        .Select(pos => dimension[pos.x, pos.y] + CalculateDistance(pos.x, pos.y, nx, ny))
                        .Min();
                }
            }
            // last position, get the lowest distance to take the last chest ( treasure) all the wway from the beginning
            var finalPos = chestPositions[p][0];
            double result = Math.Round(dimension[finalPos.x, finalPos.y], 5);

            await SaveResultToDatabase(input, result);
            return result;
        }
        public async Task<List<TreasureRecord>> GetHistory()
        {
            return await _treasureContext.TreasureRecords
                                         .OrderByDescending(t => t.Id)
                                         .Take(5)
                                         .ToListAsync();
        }

        public void ValidateTreasure(TreasureInput input)
        {
            int n = input.N;
            int m = input.M;
            int p = input.P;
            int[][] matrix = input.Matrix;

            if (n < 1 || n > 500) throw new ArgumentException("n must be between 1 and 500");
            if (m < 1 || m > 500) throw new ArgumentException("m must be between 1 and 500");
            if (p < 1 || p > n * m) throw new ArgumentException("p must be between 1 and n * m");

            if (matrix.Length != n) throw new ArgumentException("Matrix row count does not match n");
            foreach (var row in matrix)
            {
                if (row.Length != m) throw new ArgumentException("Matrix column count does not match m");
            }

            var treasureCounts = new int[p + 1]; // index 0 unused



            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    int value = matrix[i][j];
                    if (value < 1 || value > p)
                    {
                        throw new ArgumentException($"Invalid treasure number at ({i},{j}): {value} (must be 1 <= a[i][j] <= p)");
                    }
                    treasureCounts[value]++;
                }
            }

            for (int i = 1; i <= p; i++)
            {
                if (treasureCounts[i] == 0)
                {
                    throw new ArgumentException($"Treasure number {i} does not appear in the matrix");
                }
            }

            if (treasureCounts[p] != 1)
            {
                throw new ArgumentException($"Treasure number {p} must appear exactly once (found {treasureCounts[p]})");
            }
        }


    }
}

