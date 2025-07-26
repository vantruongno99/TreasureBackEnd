namespace TreasureBackEnd.Model
{

        public class TreasureRecord
        {
            public int Id { get; set; }
            public int Rows { get; set; }
            public int Columns { get; set; }
            public int MaxChest { get; set; }
            public string MatrixData { get; set; } // JSON string
            public double FuelCost { get; set; }
            public DateTime CreatedAt { get; set; } = DateTime.Now;
        } 
}
