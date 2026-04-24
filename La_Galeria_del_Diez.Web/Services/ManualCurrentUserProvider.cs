namespace La_Galeria_del_Diez.Web.Services
{
    public class ManualCurrentUserProvider : ICurrentUserProvider
    {
        public int CurrentUserId { get; set; } = 3;
    }
}
