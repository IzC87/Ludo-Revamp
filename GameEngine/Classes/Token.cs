using System.Windows.Shapes;

namespace GameEngine.Classes
{
	public class Token
	{
		public int TokenID { get; set; }
		public int? Position { get; set; }
		public Ellipse Ellipse { get; set; }
		public int PlayerNumber { get; set; }
		public int TokenNumber { get; set; }
		public int MovedSteps { get; set; }
		public bool HasFinished { get; set; }
		public bool IsOnFinishLine { get; set; }
	}
}
