namespace BugVentureEngine.Models
{
	class ItemPercentage
	{
		public int ID { get; }
		public int Percentage { get; }

		public ItemPercentage(int id, int percentage)
		{
			ID = id;
			Percentage = percentage;
		}
	}
}
