namespace WieFit.Common.DAL
{
    internal abstract class DAL
    {
        protected readonly string connectionString = @"Data Source=.;Initial Catalog=WieFit;Integrated Security=True;Encrypt=True;Trust Server Certificate=True;";

        public DAL() { }

    }
}
