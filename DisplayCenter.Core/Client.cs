namespace DisplayCenter.Core
{
    public readonly struct Client
    {
        public int SolutionId { get; }
        public byte Status { get; }

        public static Client Empty => new Client(int.MinValue, byte.MaxValue);

        public bool IsEmpty => Equals(Empty);

        public bool Equals(Client other)
        {
            return SolutionId == other.SolutionId && Status == other.Status;
        }

        public Client(int solutionId, byte status)
        {
            SolutionId = solutionId;
            Status = status;
        }

        public override string ToString()
        {
            return $"solution id: {SolutionId};status: {Status}";
        }
    }
}
